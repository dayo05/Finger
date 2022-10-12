using System.Reflection;

namespace FingerBackend;

public static class BackendLoader
{
    public static IBackend LoadAssembly(string assembly = "ext.dll")
        => Activator.CreateInstance(Assembly.LoadFile(assembly).GetType("FingerBackend.Backend")) as IBackend;
}