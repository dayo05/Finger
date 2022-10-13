namespace UnitTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        if(!File.Exists("../../../../BackendTest/bin/Debug/net6.0/BackendTest.dll") ||
           !File.Exists("../../../../ActualBackend/bin/Debug/net6.0/ActualBackend.dll"))
            Assert.Ignore("Please build whole solution before running unit test.");
    }

    [Test]
    public void LoadAssemblyTest()
    { 
        BackendLoader.LoadAssemblyRelative("../../../../BackendTest/bin/Debug/net6.0/BackendTest.dll");
        Assert.Throws<InvalidOperationException>(() =>
            BackendLoader.LoadAssemblyRelative("../../../../ActualBackend/bin/Debug/net6.0/ActualBackend.dll"));
    }
}