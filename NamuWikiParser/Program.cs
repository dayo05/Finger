using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

Console.WriteLine("Start Parsing!!");

const string jsonName = "/home/dayo/Desktop/namuwiki_20210301.json";
const string baseJsonText = "/home/dayo/Desktop/namuwiki/";

Pharse1();
Console.WriteLine("P1 Finished");
Pharse2();
Console.WriteLine("P2 Finished");

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
    foreach (var file in Directory.GetFiles(baseJsonText).Select(File.ReadAllText))
    {
        foreach (var s in file
                     .Split(new[] {"\n", ".", ","},
                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => Regex.Replace(x, @"/[^\w\s]+/u','", "")
                     /*
                     {
                         var sb = new StringBuilder();
                         foreach (var k in x.Where(k => k is not ('\\' or '/' or '[' or ']' or '!' or '@' or '#' or '$' or '%' or '^' or '&' or '*' or '(' or ')' or '{' or '}' or ':' or ';' or '"' or '\'' or '<' or '>' or '?' or '`' or '~' or '+' or '=' or '_' or '-' or '|')))
                             sb.Append(k);
                         return sb.ToString();
                     }*/))
            Console.WriteLine(s);
    }
}

class NamuWikiText
{
    public string title { get; set; }
    public string text { get; set; }
}