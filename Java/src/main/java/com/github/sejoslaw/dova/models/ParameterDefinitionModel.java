package com.github.sejoslaw.dova.models;

import java.util.ArrayList;
import java.util.Collection;

public class ParameterDefinitionModel {
    public String Type;
    public String Name;
    public Collection<TypeParameterModel> TypeParameterModels = new ArrayList<>();
    public String Signature;
}
