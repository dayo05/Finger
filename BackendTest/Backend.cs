namespace FingerBackend;

public class Backend: IBackend
{
    public void BatchFile(string file, IEnumerable<Analyzed> dirs)
    {
        
    }

    public Analyzed Analyze(string path)
    {
        return new Analyzed
        {
            Path = path,
            Words = new Dictionary<string, double>
            {
                { "안녕", 12 },
                { "세계", 13 }
            }
        };
    }

    public void BatchDirectory(string dir, IEnumerable<Analyzed> dirs)
    {

    }
}