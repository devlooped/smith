#:package Smith@0.2.3
#:package ModelContextProtocol@0.3.0-preview.*
#:package Microsoft.Extensions.Http@9.*
#:package SixLabors.ImageSharp@3.1.*

using Smith;
using System.ComponentModel;
using ModelContextProtocol.Server;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Logging.AddConsole(consoleLogOptions =>
{
// Configure all logs to go to stderr
consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();

[McpServerToolType]
public class LaTeX(IHttpClientFactory httpFactory)
{
    [McpServerTool, Description("Converts LaTeX equations into markdown-formatted images for display inline.")]
    public async Task<string> LatexMarkdown(
        [Description("The LaTeX equation to render.")] string latex, 
        [Description("Use dark mode by inverting the colors in the output.")] bool darkMode)
    {
        var colors = darkMode ? @"\fg{white}" : @"\fg{black}";
        var query = WebUtility.UrlEncode(@"\small\dpi{300}" + colors + latex);
        var url = $"https://latex.codecogs.com/png.image?{query}";
        using var client = httpFactory.CreateClient();
        using var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            using var image = Image.Load<Rgba32>(await response.Content.ReadAsStreamAsync());
            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());
            return
                $"""
                ![{latex}](
                data:image/png;base64,{base64}
                )
                """;
        }
        else
        {
            return
                $"""
                ```latex
                {latex}
                ```
                > {response.ReasonPhrase}
                """;
        }
    }
}