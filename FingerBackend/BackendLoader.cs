using System.Reflection;

namespace FingerBackend;

public static class BackendLoader
{
    public static IBackend LoadAssembly(string? assembly = null)
        => Activator.CreateInstance(Assembly.LoadFile(assembly ?? Path.GetFullPath("ext.dll")).GetType("FingerBackend.Backend")) as IBackend;
}