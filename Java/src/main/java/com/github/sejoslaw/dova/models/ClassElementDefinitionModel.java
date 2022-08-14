package com.github.sejoslaw.dova.models;

import java.util.ArrayList;
import java.util.Collection;

public class ClassElementDefinitionModel {
    public String Modifiers;
    public String ReturnType;
    public String Name;
    public Collection<ParameterDefinitionModel> ParameterModels = new ArrayList<>();
    public Collection<TypeParameterModel> TypeParameterModels = new ArrayList<>();
    public String Signature;
    public boolean IsStatic;
    public boolean HasParent;
}
