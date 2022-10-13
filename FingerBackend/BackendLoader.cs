using System.Reflection;

namespace FingerBackend;

public static class BackendLoader
{
    private static bool isLoaded = false;

    public static IBackend LoadAssembly(string? assembly = null)
    {
        if (isLoaded) throw new InvalidOperationException("Do not load backend twice.");
        isLoaded = true;
        return Activator.CreateInstance(Assembly.LoadFile(assembly ?? Path.GetFullPath("ext.dll"))
            .GetType("FingerBackend.Backend")) as IBackend;
    }

    public static IBackend LoadAssemblyRelative(string? assembly = "ext.dll")
        => LoadAssembly(Path.GetFullPath(assembly));
}