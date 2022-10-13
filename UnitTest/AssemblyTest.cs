namespace UnitTest;

public class AssemblyTest
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
        backend = BackendLoader.LoadAssemblyRelative($"../../../../BackendTest/bin/{mode}/net6.0/BackendTest.dll");
    }

    [Test]
    public void VirtualBackendTest()
    {
        Assert.Multiple(() =>
        {
            Assert.That(backend.Analyze("dayodayo").Path, Is.EqualTo("dayodayo"));
            Assert.That(backend.Analyze("dayodayo").Words["안녕"] == 12, Is.True);
            Assert.That(backend.Analyze("dayodayo").Words["세계"] == 13, Is.True);
            Assert.AreEqual(backend.Analyze("dayodayo").Words.Count, 2);
        });
    }
}