using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

var builder = App.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTool(
        [Description("Converts LaTeX equations into markdown-formatted images for display inline.")] async
        (IHttpClientFactory httpFactory,
        [Description("The LaTeX equation to render.")] string latex,
        [Description("Use dark mode by inverting the colors in the output.")] bool darkMode) =>
        {
            var colors = darkMode ? @"\bg{black}\fg{white}" : @"\bg{white}\fg{black}";
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
        });

await builder.Build().RunAsync();