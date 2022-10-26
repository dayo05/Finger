//#define PH1
//#define PH2
#define PH3
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

Console.WriteLine("Start Parsing!!");

const string jsonName = "/Users/dayo/namuwiki_20210301.json";
const string baseJsonText = "/Users/dayo/namuwiki/";
const string parsedJsonText = "/Users/dayo/nw_parsed";
const string analyzedJsonText = "/Users/dayo/nw_anzd";

#if PH1
Pharse1();
Console.WriteLine("P1 Finished");
#else
Console.WriteLine("P1 Skipped");
#endif
#if PH2
Pharse2();
Console.WriteLine("P2 Finished");
#else
Console.WriteLine("P2 Skipped");
#endif
#if PH3
Pharse3();
Console.WriteLine("P3 finished");
#else
Console.WriteLine("P3 Skipped");
#endif

// json to mdtxt
async void Pharse1()
{
    Directory.CreateDirectory(baseJsonText);

    await using var fileStream = new FileStream(jsonName, FileMode.Open);

    var idx = 0;
    await foreach (var x in JsonSerializer.DeserializeAsyncEnumerable<NamuWikiText?>(fileStream))
    {
        if (x.text.StartsWith("#redirect")) continue;
        await using var sw = new StreamWriter(baseJsonText + idx++ + ".txt");
        sw.WriteLine(x.title);
        sw.Write(x.text);
    }
}

// mdtxt to txt
void Pharse2()
{
    //Removement of special characters.
    if (!Directory.Exists(parsedJsonText))
        Directory.CreateDirectory(parsedJsonText);

    var fix = 0;
    while (fix < 1000000)
    {
        var fd = Path.Combine(baseJsonText, $"{fix}.txt");
        if (!File.Exists(fd)) continue;
        var file = File.ReadAllText(fd);
        var sb = new StringBuilder();
        foreach (var s in file
                     .Split(new[] { "\n", ".", ",", "?", "!" },
                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x =>
                         string.Concat(x.Select(x => char.IsLetter(x) ? x : ' '))))
            sb.AppendLine(Regex.Replace(s, @"\s+", " ").Trim());
        File.WriteAllText(Path.Combine(parsedJsonText, $"{fix++}.txt"), sb.ToString());
        if(fix % 10000 == 0) Console.WriteLine($"Processing: {fix}");
    }
}

void Pharse3()
{
    if (!Directory.Exists(analyzedJsonText))
        Directory.CreateDirectory(analyzedJsonText);

    var dic = new ConcurrentDictionary<string, Dictionary<string, double>>();

    var totalRunningTask = 0;

    const int startPos = 0;
    for (var i = startPos; i < startPos + 20; i++)
    {
        var fix = i;
        while (totalRunningTask >= 4)
            Thread.Sleep(1);

        Interlocked.Add(ref totalRunningTask, 1);
        new Task(() =>
        {
            Console.WriteLine("Execution of task: " + fix);
            var fd = Path.Combine(parsedJsonText, $"{fix}.txt");
            if (!File.Exists(fd))
            {
                Interlocked.Add(ref totalRunningTask, -1);
                return;
            }

            var fdic = new Dictionary<string, Dictionary<string, int>>();
            using var sr = new StreamReader(fd);
            while (!sr.EndOfStream)
            {
                var l = GetParsedStr(sr.ReadLine()).Where(x => !string.IsNullOrEmpty(x)).ToList();
                for (var e1 = 0; e1 < l.Count; e1++)
                for (var e2 = e1 + 1; e2 < l.Count; e2++)
                {
                    if (!fdic.ContainsKey(l[e1]))
                        fdic[l[e1]] = new Dictionary<string, int>();

                    if (fdic[l[e1]].ContainsKey(l[e2]))
                        fdic[l[e1]][l[e2]]++;
                    else fdic[l[e1]][l[e2]] = 1;

                    if (!fdic.ContainsKey(l[e2]))
                        fdic[l[e2]] = new Dictionary<string, int>();

                    if (fdic[l[e2]].ContainsKey(l[e1]))
                        fdic[l[e2]][l[e1]]++;
                    else fdic[l[e2]][l[e1]] = 1;
                }
            }

            Console.WriteLine("Enter final pharse: " + fix);
            foreach (var x in fdic)
            foreach (var y in x.Value)
            {
                if (!dic.ContainsKey(x.Key))
                    dic[x.Key] = new Dictionary<string, double>();
                if (!dic[x.Key].ContainsKey(y.Key))
                    dic[x.Key][y.Key] = 2 / Math.PI * Math.Atan(y.Value);
                else dic[x.Key][y.Key] += 2 / Math.PI * Math.Atan(y.Value);
            }

            Console.WriteLine("Exiting: " + fix);
            Interlocked.Add(ref totalRunningTask, -1);
        }).Start();
    }
    
    while (totalRunningTask != 0)
        Thread.Sleep(1); //Wait for all tasks finished
    File.WriteAllText($"{startPos}to{startPos+19}.txt", string.Join("\n", dic.SelectMany(x => x.Value.Select(y => $"{x.Key}|{y.Key}|{y.Value}"))));

    IEnumerable<string> GetParsedStr(string str)
    {
        var prc = Process.Start(new ProcessStartInfo
        {
            FileName = "java",
            Arguments = $"-jar parsekor.jar {str}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        });
        prc.WaitForExit();
        return prc.StandardOutput.ReadToEnd().Split("\n");
    }
}

class NamuWikiText
{
    public string title { get; set; }
    public string text { get; set; }
}