package com.github.sejoslaw.dova.generators;

import java.io.File;
import java.util.Arrays;
import java.util.Objects;

public class DirectoryJarFileBasedDefinitionGenerator {
    public static void Run(String outputPath, String path) {
        var dir = new File(path);

        var jarFileNames = Arrays
                .stream(Objects.requireNonNull(dir.listFiles()))
                .filter(file -> file.isFile() && file.getAbsolutePath().endsWith(".jar"))
                .map(File::getAbsolutePath)
                .toArray(String[]::new);

        JarFileBasedDefinitionGenerator.Run(outputPath, jarFileNames);
    }
}
