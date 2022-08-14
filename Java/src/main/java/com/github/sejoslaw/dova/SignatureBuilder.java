package com.github.sejoslaw.dova;

import java.lang.reflect.*;

public class SignatureBuilder {
    public static String Build(Class<?> clazz) {
        return org.objectweb.asm.Type.getDescriptor(clazz);
    }

    public static String Build(Type type) {
        if (type instanceof ParameterizedType) {
            var rawType = ((ParameterizedType) type).getRawType();

            if (rawType instanceof Class<?>) {
                return Build(((Class<?>) rawType));
            }
        }

        if (type instanceof Class<?>) {
            return Build(((Class<?>) type));
        }

        return null;
    }

    public static String Build(Method method) {
        return org.objectweb.asm.Type.getMethodDescriptor(method);
    }

    public static String Build(Field field) {
        return org.objectweb.asm.Type.getDescriptor(field.getType());
    }

    public static String Build(Constructor<?> constructor) {
        return org.objectweb.asm.Type.getConstructorDescriptor(constructor);
    }
}
