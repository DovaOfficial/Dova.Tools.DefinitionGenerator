package com.github.sejoslaw.dova.generators;

import com.github.sejoslaw.dova.ClassProcessor;
import com.github.sejoslaw.dova.ModelWriter;
import com.github.sejoslaw.dova.models.ClassDefinitionModel;

import java.io.File;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLClassLoader;
import java.util.Arrays;
import java.util.Objects;
import java.util.jar.JarFile;

public class JarFileBasedDefinitionGenerator {
    public static void Run(String outputPath, String[] jarFilePaths) {
        var files = Arrays
                .stream(jarFilePaths)
                .map(File::new)
                .toList();

        var urls = files
                .stream()
                .map(file -> {
                    try {
                        return file.toURI().toURL();
                    } catch (MalformedURLException e) {
                        return null;
                    }
                })
                .filter(Objects::nonNull)
                .toList()
                .toArray(new URL[0]);

        var classLoader = new URLClassLoader(urls);

        for (var file : files) {
            try {
                ProcessJarFile(classLoader, outputPath, file);
            } catch (Exception ex) {
            }
        }
    }

    private static void ProcessJarFile(ClassLoader classLoader, String outputPath, File file) throws IOException {
        try (var jarFile = new JarFile(file)) {
            var threads = jarFile.stream()
                    .filter(jarFileEntry -> {
                        var name = jarFileEntry.getRealName();
                        return name.endsWith(".class")
                                && !name.contains("-")
                                && !name.contains("$");
                    })
                    .map(jarFileEntry -> {
                        var name = jarFileEntry.getRealName();
                        var classPath = name.replace(".class", "");
                        var className = classPath.replace("/", ".");

                        try {
                            var clazz = Class.forName(className, true, classLoader);

                            var thread = new Thread(() -> {
                                try {
                                    ProcessClass(clazz, outputPath, classPath);
                                } catch (Exception ex) {
                                }
                            });

                            thread.start();

                            return thread;
                        } catch (ClassNotFoundException e) {
                            return null;
                        }
                    })
                    .filter(Objects::nonNull)
                    .toList();

            for (var thread : threads) {
                try {
                    if (thread.isAlive()) {
                        thread.join();
                    }
                } catch (Exception ex) {
                    System.err.println("Error when joining thread: " + thread.getName() + ", :" + ex);
                }
            }
        }
    }

    private static void ProcessClass(Class<?> clazz, String outputPath, String classPath) {
        if (ModelWriter.ModelExists(outputPath, "", classPath)) {
            return;
        }

        var model = new ClassDefinitionModel();

        ClassProcessor.ProcessClass(clazz, model);

        var tempOutputPathFull = ModelWriter.GetPath(outputPath, "", classPath);

        ModelWriter.Write(tempOutputPathFull, model);
    }
}
