using Dova.Tools.DefinitionGenerator.Common;
using Xunit;

namespace Dova.Tools.Tests;

public class DefinitionCleanerTests
{
    [Theory]
    [InlineData("event;", "@event;")]
    [InlineData("test.event;", "test.@event;")]
    [InlineData("test.event.Test;", "test.@event.Test;")] // Technically this don't need to be changed but for the simplicity it is
    [InlineData("namespace com.test.event;", "namespace com.test.@event;")]
    public void Should_clean_namespace(string str, string validStr)
    {
        var cleaned = DefinitionCleaner.CleanKeywords(str) + ";";
        
        Assert.True(cleaned.Equals(validStr));
    }
    
    [Theory]
    [InlineData("T", "Dova.JDK.java.lang.Object")]
    [InlineData("T[]", "JavaArray<Dova.JDK.java.lang.Object>")]
    [InlineData("void", "void")]
    [InlineData("byte", "byte")]
    [InlineData("int", "int")]
    [InlineData("byte[]", "JavaArray<byte>")]
    [InlineData("byte[][]", "JavaArray<JavaArray<byte>>")]
    [InlineData("byte[][][]", "JavaArray<JavaArray<JavaArray<byte>>>")]
    [InlineData("java.lang.Byte", "Dova.JDK.java.lang.Byte")]
    [InlineData("java.lang.Byte[]", "JavaArray<Dova.JDK.java.lang.Byte>")]
    [InlineData("java.lang.Byte[][]", "JavaArray<JavaArray<Dova.JDK.java.lang.Byte>>")]
    [InlineData("java.lang.Byte[][][]", "JavaArray<JavaArray<JavaArray<Dova.JDK.java.lang.Byte>>>")]
    [InlineData("boolean", "bool")]
    [InlineData("java.lang.Class<?>", "Dova.JDK.java.lang.Class")]
    [InlineData("java.lang.Class<? extends java.util.PrintStream>", "Dova.JDK.java.lang.Class")]
    [InlineData("java.util.Vector<java.util.Vector<java.text.EntryPair>>", "Dova.JDK.java.util.Vector")]
    [InlineData("java.util.Vector<java.util.Vector<java.text.EntryPair>>[]", "JavaArray<Dova.JDK.java.util.Vector>")]
    [InlineData("java.util.Class<java.util.Class<java.lang.Object>[]>", "Dova.JDK.java.util.Class")]
    [InlineData("java.util.Class$InnerClass<java.util.Class<java.util.Class$InnerClass<?>>[]>[]", "JavaArray<Dova.JDK.java.util.Class.InnerClass>")]
    [InlineData("java.util.Spliterator$OfInt<java.util.Function<int>, java.util.List$ArrayList<?>, java.util.Class$InnerClass<java.lang.Integer>>", "Dova.JDK.java.util.Spliterator.OfInt")]
    [InlineData("java.util.Function2<java.util.List$ArrayList<int>, java.util.List$ArrayList<double>>", "Dova.JDK.java.util.Function2")]
    [InlineData("java.util.Class$InnerClass$SecondInner", "Dova.JDK.java.util.Class.InnerClass.SecondInner")]
    [InlineData("java.util.Set<java.util.Map.Entry<K, V>>", "Dova.JDK.java.util.Set")]
    [InlineData("java.util.Set<java.util.Map.Entry<K, V>, java.util.Map.Entry<K, V>>", "Dova.JDK.java.util.Set")]
    [InlineData("java.util.Set<java.util.Map.Entry<K, V>[], java.util.Map.Entry<K, V>>", "Dova.JDK.java.util.Set")]
    [InlineData("java.util.Set<java.util.Map.Entry<K, V>[], java.util.Map.Entry>", "Dova.JDK.java.util.Set")]
    [InlineData("java.util.Set<java.util.Map.Entry<K,V>[],java.util.Map.Entry>", "Dova.JDK.java.util.Set")]
    [InlineData("java.util.Set<int[], java.util.Map.Entry>", "Dova.JDK.java.util.Set")]
    [InlineData("java.util.Set<int[],java.util.Map.Entry>", "Dova.JDK.java.util.Set")]
    [InlineData("java.util.Set<int, java.util.Map.Entry, L[]>", "Dova.JDK.java.util.Set")]
    [InlineData("java.util.Set<int,java.util.Map.Entry,L[]>", "Dova.JDK.java.util.Set")]
    [InlineData("java.lang.invoke.ClassSpecializer<T, K, S>.SpeciesData", "Dova.JDK.java.lang.invoke.ClassSpecializer.SpeciesData")]
    [InlineData("javax.swing.JComboBox<E>.AccessibleJComboBox.EditorAccessibleContext", "Dova.JDK.javax.swing.JComboBox.AccessibleJComboBox.EditorAccessibleContext")]
    [InlineData("java.lang.invoke.ClassSpecializer<T, K, S>.SpeciesData<T, K, S>.SpeciesData", "Dova.JDK.java.lang.invoke.ClassSpecializer.SpeciesData.SpeciesData")]
    [InlineData("jdk.internal.loader.AbstractClassLoaderValue<CLV, V>$Sub<K>", "Dova.JDK.jdk.@internal.loader.AbstractClassLoaderValue.Sub")]
    public void Should_clean_Java_class_name(string str, string validStr)
    {
        var cleanedStr = DefinitionCleaner.CleanClass(str);
        
        Assert.Equal(cleanedStr, validStr);
    }
}