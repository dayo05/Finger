using System.Diagnostics;

namespace FingerBackend;

public class Backend: IBackend
{
    public void BatchFile(string file, IEnumerable<Analyzed> dirs)
    {
        var parsedFile = GetParsedStr(file);
        File.Move(file, Path.Combine(dirs.MaxBy(x => CalculateBias(parsedFile, x.Words)).Path, Path.GetFileName(file)));
    }

    public Analyzed Analyze(string path)
    {
        var dictionary = new Dictionary<string, double>();
        foreach (var s in Directory.GetFiles(path).SelectMany(GetParsedStr))
        {
            if (dictionary.ContainsKey(s)) dictionary[s]++;
            else dictionary[s] = 1;
        }

        return new Analyzed
        {
            Path = path,
            Words = dictionary
        };
    }

    private static double CalculateBias(IEnumerable<string> parsed, IDictionary<string, double> bias)
    {
        return parsed.Sum(x => bias.Sum(b =>
        {
            try
            {
                return wordBias[x][b.Key] * b.Value;
            }
            catch (Exception)
            {
                return 0;
            }
        }));
    }

    static IEnumerable<string> GetParsedStr(string str)
    {
        var prc = Process.Start(new ProcessStartInfo
        {
            FileName = "java",
            Arguments = $"-jar parsekor.jar {str}",
            RedirectStandardOutput = true
        });
        prc.WaitForExit();
        return prc.StandardOutput.ReadToEnd().Split("\n");
    }

    private static Dictionary<string, Dictionary<string, double>> wordBias = new();
    static Backend()
    {
        if (!File.Exists("parsekor.jar"))
            throw new FileNotFoundException("Token parser not found");
        //TODO: Initialize word bias here
    }
}