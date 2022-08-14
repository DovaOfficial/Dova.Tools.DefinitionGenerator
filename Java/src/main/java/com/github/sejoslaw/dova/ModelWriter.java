package com.github.sejoslaw.dova;

import com.github.sejoslaw.dova.models.ClassDefinitionModel;
import org.codehaus.jackson.map.ObjectMapper;

import java.io.IOException;
import java.nio.file.FileSystems;
import java.nio.file.Files;
import java.nio.file.Path;

public class ModelWriter {
    public static String GetPath(String paths, String moduleName, String classPath) {
        return Path.of(paths, moduleName, classPath.replace('$', '_') + ".dova").toString();
    }

    public static boolean ModelExists(String outputPath, String moduleName, String classPath) {
        var pathStr = GetPath(outputPath, moduleName, classPath);
        var path = Path.of(pathStr);
        var file = path.toFile();

        return file.exists() && file.length() > 0;
    }

    public static void Write(String tempOutputPathFull, ClassDefinitionModel model) {
        var filePath = FileSystems.getDefault().getPath(tempOutputPathFull);
        var file = filePath.toFile();

        try {
            Files.deleteIfExists(filePath);

            if (!file.getParentFile().exists()) {
                file.getParentFile().mkdirs();
            }

            if (!file.exists()) {
                file.createNewFile();
            }
        } catch (IOException e) {
            throw new RuntimeException("Error occurred when preparing files for: " + filePath, e);
        }

        var mapper = new ObjectMapper();

        try {
            mapper.writeValue(file, model);
            Thread.sleep(10);
        } catch (Exception e) {
            throw new RuntimeException("Error occurred when writing JSON data to: " + filePath, e);
        }
    }
}
