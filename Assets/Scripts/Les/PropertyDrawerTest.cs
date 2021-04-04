using System;
public enum TestUnit { Test1, Test2, Test3 };

[Serializable]
public class PropertyDrawerTest
{
    public string name = "TestName";
    public int amount = 1;
    public TestUnit unit;
}


