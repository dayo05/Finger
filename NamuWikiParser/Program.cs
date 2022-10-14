using System.Text.Json;

Console.WriteLine("Start Parsing!!");

const string jsonName = "/home/dayo/Desktop/namuwiki_20210301.json";
const string baseJsonText = "/home/dayo/Desktop/namuwiki/";
Directory.CreateDirectory(baseJsonText);

await using var fileStream = new FileStream(jsonName, FileMode.Open);

var idx = 0;
await foreach (var x in JsonSerializer.DeserializeAsyncEnumerable<NamuWikiText?>(fileStream))
{
    if (x.text.StartsWith("#redirect")) continue;
    await using var sw = new StreamWriter(baseJsonText + idx+++ ".txt");
    sw.WriteLine(x.title);
    sw.Write(x.text);
}

class NamuWikiText
{
    public string title { get; set; }
    public string text { get; set; }
}