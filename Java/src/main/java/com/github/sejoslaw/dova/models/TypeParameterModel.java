package com.github.sejoslaw.dova.models;

import java.util.ArrayList;
import java.util.Collection;

public class TypeParameterModel {
    public String VariableName;
    public String TypeName;
    public Collection<BoundDefinitionModel> BoundModels = new ArrayList<>();
}
