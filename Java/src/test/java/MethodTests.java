import org.junit.jupiter.api.Test;

import java.lang.invoke.MethodHandle;
import java.lang.invoke.MethodHandles;
import java.lang.invoke.MethodType;
import java.lang.reflect.Method;

public class MethodTests {
    @Test
    public void Should_Return_Method_Parent() throws Throwable {
        Class<?> clazz = Class.forName("Test2");


        Class<?> test2Class = Class.forName("Test2");
        Method testMethod = test2Class.getDeclaredMethods()[0];
    }
}

class Test1 {
    public int X = 2;
    public void Test() {
        X = 3;
    }
}

class Test2 extends Test1 {
    public void Test() {
        X = 4;
    }
}