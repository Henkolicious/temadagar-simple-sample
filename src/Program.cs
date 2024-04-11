using HtmlAgilityPack;

var htmlResponse = await GetHtmlDocument("https://temadagar.se/kalender/");
var document = new HtmlDocument();
document.LoadHtml(htmlResponse);

var nodes = document.DocumentNode.SelectNodes("//a");
var ignoreText = true;
var isFirstTimePrintEdgeCase = true;

foreach (var node in nodes)
{
    if (string.IsNullOrWhiteSpace(node.InnerText))
        continue;

    if (char.IsDigit(node.InnerText[0]))
        ignoreText = false;

    if (ignoreText)
        continue;

    PrettyOutput(node);

    var isEndOfFile = node
        .InnerText
        .Equals
        (
            "Internationella Expect a better tomorrow-dagen", 
            StringComparison.CurrentCultureIgnoreCase
        );

    if (isEndOfFile)
        break;
}

static async Task<string> GetHtmlDocument(string url)
{
    using var client = new HttpClient();
    var response = await client.GetAsync(url);
    return await response.Content.ReadAsStringAsync();
}

void PrettyOutput(HtmlNode node)
{
    var startsWithDigit = char.IsDigit(node.InnerText[0]);

    if (startsWithDigit)
    {
        if (isFirstTimePrintEdgeCase)
        {
            isFirstTimePrintEdgeCase = false;
            Console.WriteLine(node.InnerText);
            return;
        }
        else
        {
            Console.WriteLine();
        }

        Console.WriteLine(node.InnerText);
        return;
    }

    Console.WriteLine($"  {node?.InnerText}");
}
