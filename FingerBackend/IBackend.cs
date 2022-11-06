namespace FingerBackend;

public interface IBackend
{
    Action<string, IEnumerable<Analyzed>> BatchDirectory { get; }

    public void BatchFile(string file, IEnumerable<Analyzed> dirs);
    public Analyzed Analyze(string path);
}