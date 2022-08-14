package com.github.sejoslaw.dova.generators;

import com.github.sejoslaw.dova.ClassProcessor;
import com.github.sejoslaw.dova.ModelWriter;
import com.github.sejoslaw.dova.models.ClassDefinitionModel;

import java.io.IOException;
import java.lang.module.ModuleFinder;
import java.lang.module.ModuleReference;
import java.nio.file.Path;
import java.util.Comparator;
import java.util.List;
import java.util.Objects;
import java.util.Set;
import java.util.stream.Stream;

public class ModuleBasedDefinitionGenerator {
    public static void Run(String outputPath, String modulePaths) {
        var moduleReferences = GetModuleReferences(modulePaths)
                .stream()
                .sorted(Comparator.comparing(moduleReference -> moduleReference.descriptor().name()))
                .toList();

        try {
            var threads = moduleReferences
                    .parallelStream()
                    .map(moduleReference -> {
                        var moduleName = moduleReference.descriptor().name();

                        try {
                            return moduleReference
                                    .open()
                                    .list()
                                    .parallel()
                                    .filter(classPath -> classPath.endsWith(".class")
                                            && !classPath.contains("-")
                                            && !classPath.contains("$"))
                                    .map(classPath -> {
                                        var innerThreadName = moduleName + "---" + classPath;

                                        return new Thread(() -> {
                                            try {
                                                ProcessClass(moduleName, classPath, outputPath);
                                            } catch (Exception ex) {
                                                System.err.println("Error in module: '" + moduleName + "' in classpath: '" + classPath + "' :" + ex);
                                            }
                                        }, innerThreadName);
                                    })
                                    .toList();
                        } catch (IOException e) {
                            System.err.println("Error in module: '" + moduleName + "' :" + e);
                            return null;
                        }
                    })
                    .filter(Objects::nonNull)
                    .flatMap(List::stream)
                    .toList();

            for (var thread : threads) {
                thread.start();
                Thread.sleep(10);
            }

            for (var thread : threads) {
                try {
                    if (thread.isAlive()) {
                        thread.join();
                    }
                } catch (Exception ex) {
                    System.err.println("Error when joining thread: " + thread.getName() + ", :" + ex);
                }
            }
        } catch (Exception e) {
            System.err.println(e);
        }
    }

    private static Set<ModuleReference> GetModuleReferences(String modulePaths) {
        if (!modulePaths.equals("")) {
            var modulePathsStr = modulePaths.split(",");

            var modulesArray = Stream
                    .of(modulePathsStr)
                    .map(Path::of)
                    .toArray(Path[]::new);

            return ModuleFinder.of(modulesArray).findAll();
        }

        return ModuleFinder.ofSystem().findAll();
    }

    private static void ProcessClass(String moduleName, String classPath, String outputPath) throws ClassNotFoundException {
        if (ModelWriter.ModelExists(outputPath, moduleName, classPath)) {
            return;
        }

        var className = classPath
                .split("\\.")[0]
                .replace('/', '.');

        var clazz = Class.forName(className);
        var model = new ClassDefinitionModel();

        model.ModuleName = moduleName;

        ClassProcessor.ProcessClass(clazz, model);

        var tempOutputPathFull = ModelWriter.GetPath(outputPath, moduleName, classPath);

        ModelWriter.Write(tempOutputPathFull, model);
    }
}
