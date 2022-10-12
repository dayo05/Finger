namespace FingerBackend;

public class Analyzed
{
    public Dictionary<string, int> Words { get; init; } = new();
    public string Path { get; init; }

    public Analyzed(string path)
    {
        Path = path;
    }
}