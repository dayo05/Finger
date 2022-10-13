using System.Reflection;

namespace UnitTest;

public class ActualBackendTest
{
    private IBackend? backend;
    
    [SetUp]
    public void Setup()
    {
        backend = BackendLoader.LoadAssemblyRelative("../../../../ActualBackend/bin/Debug/net6.0/ActualBackend.dll");
    }

    [Test]
    public void TestJavaExecution()
    {
        Assert.DoesNotThrow(() => backend.GetType().GetMethod("GetParsedStr", BindingFlags.Static | BindingFlags.NonPublic)
            .Invoke(null, new[] { "한국어 테스트를 진행합니다..." }));
    }
}