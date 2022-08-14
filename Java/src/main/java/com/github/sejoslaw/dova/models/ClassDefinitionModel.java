package com.github.sejoslaw.dova.models;

import java.util.ArrayList;
import java.util.Collection;

public class ClassDefinitionModel {
    public String ModuleName;
    public ClassDetailsDefinitionModel ClassDetailsModel = new ClassDetailsDefinitionModel();
    public ClassElementDefinitionModel BaseClassModel = new ClassElementDefinitionModel();
    public Collection<ClassElementDefinitionModel> InterfaceModels = new ArrayList<>();
    public Collection<ClassElementDefinitionModel> ConstructorModels = new ArrayList<>();
    public Collection<ClassElementDefinitionModel> FieldModels = new ArrayList<>();
    public Collection<ClassElementDefinitionModel> MethodModels = new ArrayList<>();
    public Collection<ClassDefinitionModel> InnerClassModels = new ArrayList<>();
}
