namespace FingerBackend;

public class Backend: IBackend
{
    public void BatchFile(string file, IEnumerable<Analyzed> dirs)
    {
        
    }

    public Analyzed Analyze(string path)
    {
        return new Analyzed(path)
        {
            Words = new Dictionary<string, int>
            {
                { "안녕", 12 },
                { "세계", 13 }
            }
        };
    }
}