package com.github.sejoslaw.dova;

import com.github.sejoslaw.dova.models.*;

import java.lang.reflect.*;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

public class ClassProcessor {
    public static void ProcessClass(Class<?> clazz, ClassDefinitionModel model) {
        GetClassDetails(clazz, model.ClassDetailsModel);
        GetBaseClass(clazz, model.BaseClassModel);
        GetInterfaces(clazz, model.InterfaceModels);
        GetConstructors(clazz, model.ConstructorModels);
        GetFields(clazz, model.FieldModels);
        GetMethods(clazz, model.MethodModels);
        GetInnerClasses(model.ModuleName, clazz, model.InnerClassModels);
    }

    private static void GetClassDetails(Class<?> clazz, ClassDetailsDefinitionModel model) {
        model.PackageName = clazz.getPackageName();
        model.ClassName = clazz.getSimpleName();
        model.IsEnum = clazz.isEnum();
        model.Modifiers = GetModifiers(clazz.getModifiers());
        model.IsInterface = Modifier.isInterface(clazz.getModifiers());
        model.IsAbstract = Modifier.isAbstract(clazz.getModifiers());

        GetTypeParameters(clazz.getTypeParameters(), model.TypeParameterModels);

        model.Signature = SignatureBuilder.Build(clazz);
    }

    private static void GetTypeParameters(TypeVariable<? extends Class<?>>[] typeParameters, Collection<TypeParameterModel> models) {
        for (var typeParameter : typeParameters) {
            var model = new TypeParameterModel();

            GetTypeParameter(typeParameter, model);

            models.add(model);
        }
    }

    private static void GetTypeParameters(Type type, Collection<TypeParameterModel> models) {
        if (type instanceof ParameterizedType) {
            var typeParameters = ((ParameterizedType) type).getActualTypeArguments();

            for (var typeParameter : typeParameters) {
                var model = new TypeParameterModel();

                GetTypeParameter(typeParameter, model);

                models.add(model);
            }
        }
    }

    private static void GetTypeParameter(TypeVariable<?> typeParameter, TypeParameterModel model) {
        model.VariableName = typeParameter.getName();
        model.TypeName = typeParameter.getTypeName();

        GetBounds(typeParameter.getBounds(), model.BoundModels);
    }

    private static void GetTypeParameter(Type type, TypeParameterModel model) {
        if (type instanceof TypeVariable<?>) {
            GetTypeParameter((TypeVariable<?>) type, model);
            return;
        } else if (type instanceof WildcardType) {
            GetTypeParameter((WildcardType) type, model);
            return;
        }

        model.TypeName = type.toString();
    }

    private static void GetTypeParameter(WildcardType type, TypeParameterModel model) {
        model.TypeName = type.toString();

        var bounds = new ArrayList<>(List.of(type.getUpperBounds()));
        bounds.addAll(List.of(type.getLowerBounds()));

        GetBounds(bounds.toArray(new Type[0]), model.BoundModels);
    }

    private static void GetBounds(Type[] bounds, Collection<BoundDefinitionModel> models) {
        for (var bound : bounds) {
            var model = new BoundDefinitionModel();

            model.Name = bound.getTypeName();
            model.Signature = SignatureBuilder.Build(bound);

            models.add(model);
        }
    }

    private static void GetBaseClass(Class<?> clazz, ClassElementDefinitionModel model) {
        var baseClass = clazz.getGenericSuperclass();

        if (baseClass == null) {
            return;
        }

        model.Name = baseClass.getTypeName();

        GetTypeParameters(baseClass, model.TypeParameterModels);

        model.Signature = SignatureBuilder.Build(baseClass);
    }

    private static void GetMethods(Class<?> clazz, Collection<ClassElementDefinitionModel> models) {
        for (var method : clazz.getDeclaredMethods()) {
            var model = new ClassElementDefinitionModel();

            model.Modifiers = GetModifiers(method.getModifiers());
            model.Name = method.getName();
            model.IsStatic = Modifier.isStatic(method.getModifiers());

            GetParameters(method.getParameters(), model.ParameterModels);

            var type = method.getGenericReturnType();

            model.ReturnType = type.getTypeName();

            GetTypeParameters(type, model.TypeParameterModels);

            model.Signature = SignatureBuilder.Build(method);

            model.HasParent = HasParentMethod(clazz, method);

            models.add(model);
        }
    }

    private static boolean HasParentMethod(Class<?> clazz, Method method) {
        try {
            var parentMethod = clazz.getSuperclass().getDeclaredMethod(method.getName(), method.getParameterTypes());
            return true;
        } catch (NoSuchMethodException | NullPointerException ex) {
            return false;
        }
    }

    private static void GetInterfaces(Class<?> clazz, Collection<ClassElementDefinitionModel> models) {
        for (var interfaceType : clazz.getGenericInterfaces()) {
            var model = new ClassElementDefinitionModel();

            model.Name = interfaceType.getTypeName();

            GetTypeParameters(interfaceType, model.TypeParameterModels);

            model.Signature = SignatureBuilder.Build(interfaceType);

            models.add(model);
        }
    }

    private static void GetFields(Class<?> clazz, Collection<ClassElementDefinitionModel> fieldModels) {
        for (var field : clazz.getDeclaredFields()) {
            var model = new ClassElementDefinitionModel();

            model.Modifiers = GetModifiers(field.getModifiers());
            model.Name = field.getName();
            model.IsStatic = Modifier.isStatic(field.getModifiers());

            var type = field.getGenericType();

            model.ReturnType = type.getTypeName();

            GetTypeParameters(type, model.TypeParameterModels);

            model.Signature = SignatureBuilder.Build(field);

            fieldModels.add(model);
        }
    }

    private static String GetModifiers(int mods) {
        return Modifier.toString(mods);
    }

    private static void GetConstructors(Class<?> clazz, Collection<ClassElementDefinitionModel> models) {
        for (var constructor : clazz.getDeclaredConstructors()) {
            var model = new ClassElementDefinitionModel();

            model.Modifiers = GetModifiers(constructor.getModifiers());

            GetParameters(constructor.getParameters(), model.ParameterModels);

            model.Signature = SignatureBuilder.Build(constructor);

            models.add(model);
        }
    }

    private static void GetParameters(Parameter[] parameters, Collection<ParameterDefinitionModel> models) {
        for (var parameter : parameters) {
            var model = new ParameterDefinitionModel();

            model.Name = parameter.getName();

            var type = parameter.getParameterizedType();

            model.Type = type.getTypeName();

            GetTypeParameters(type, model.TypeParameterModels);

            model.Signature = SignatureBuilder.Build(type);

            models.add(model);
        }
    }

    private static void GetInnerClasses(String moduleName, Class<?> clazz, Collection<ClassDefinitionModel> models) {
        for (Class<?> innerClass : clazz.getDeclaredClasses()) {
            var model = new ClassDefinitionModel();

            model.ModuleName = moduleName;

            ProcessClass(innerClass, model);

            models.add(model);
        }
    }
}
