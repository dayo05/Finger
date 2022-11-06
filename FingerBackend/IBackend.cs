namespace FingerBackend;

public interface IBackend
{
    public void BatchDirectory(string dir, IEnumerable<Analyzed> dirs);

    public void BatchDirectory(string dir, IEnumerable<Analyzed> dirs);
    public Analyzed Analyze(string path);
}