using System.Reflection;

namespace UnitTest;

public class ActualBackendTest
{
    private IBackend? backend;
    
    [SetUp]
    public void Setup()
    {
        if (backend is not null) return;
        const string mode = 
#if DEBUG 
            "Debug"
#else
            "Release"
#endif
                ;
        if(!File.Exists($"../../../../BackendTest/bin/{mode}/net6.0/BackendTest.dll") ||
           !File.Exists($"../../../../ActualBackend/bin/{mode}/net6.0/ActualBackend.dll"))
            Assert.Ignore("Please build whole solution before running unit test.");
        backend = BackendLoader.LoadAssemblyRelative($"../../../../ActualBackend/bin/{mode}/net6.0/ActualBackend.dll");
    }

    [Test]
    public void TestJavaExecution()
    {
        Assert.DoesNotThrow(() => backend.GetType().GetMethod("GetParsedStr", BindingFlags.Static | BindingFlags.NonPublic)
            .Invoke(null, new[] { "한국어 테스트를 진행합니다..." }));
    }
}