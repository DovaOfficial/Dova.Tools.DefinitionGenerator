package com.github.sejoslaw.dova;

import com.github.sejoslaw.dova.generators.JarFileBasedDefinitionGenerator;
import com.github.sejoslaw.dova.generators.ModuleBasedDefinitionGenerator;

public class Main {
    public static void main(String[] args) {
        var generationType = args[0];
        var outputPath = args[1];

        switch (generationType) {
            case "jar":
                var jarFilePaths = args[2].split(",");
                JarFileBasedDefinitionGenerator.Run(outputPath, jarFilePaths);
                return;
            case "module":
                var modulePaths = args.length > 2 ? args[2] : "";
                ModuleBasedDefinitionGenerator.Run(outputPath, modulePaths);
                return;
        }
    }
}