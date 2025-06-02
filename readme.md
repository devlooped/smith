An opinionated meta-package for doing AI agents using Microsoft.Extensions.AI and MCP and dotnet run file.

Example Claude-based agent:

![](https://github.com/devlooped/smith/blob/main/assets/run.png?raw=true)

```csharp
#:package Smith@0.*

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddUserSecrets()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddIniFile("appsettings.ini", optional: true, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();

services.AddHttpClient("ai").AddStandardResilienceHandler();

services.AddChatClient(services => new Anthropic.AnthropicClient(
    configuration["Claude:Key"] ?? throw new InvalidOperationException("Missing Claude:Key configuration."),
    services.GetRequiredService<IHttpClientFactory>().CreateClient("ai")))
    .UseLogging()
    .UseFunctionInvocation();

var provider = services.BuildServiceProvider();
var history = new List<ChatMessage> { new ChatMessage(ChatRole.System, Prompts.System) };
var chat = provider.GetRequiredService<IChatClient>();
var options = new ChatOptions
{
    ModelId = "claude-sonnet-4-20250514",
    MaxOutputTokens = 1000,
    Temperature = 0.7f,
    Tools = [AIFunctionFactory.Create(() => DateTime.Now, "get_datetime", "Gets the current date and time on the user's local machine.")]
};

AnsiConsole.MarkupLine($":robot: Ready v{ThisAssembly.Info.Version}");
AnsiConsole.Markup($":person_beard: ");
while (true)
{
    var input = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(input))
        continue;

    history.Add(new ChatMessage(ChatRole.User, input));

    try
    {
        var response = await AnsiConsole.Status().StartAsync(":robot: Thinking...",
            ctx => chat.GetResponseAsync(input, options));

        history.AddRange(response.Messages);
        // Try rendering as formatted markup
        try
        {
            if (response.Text is { Length: > 0 })
                AnsiConsole.MarkupLine($":robot: {response.Text}");
        }
        catch (Exception)
        {
            // Fallback to escaped markup text if rendering fails
            AnsiConsole.MarkupInterpolated($":robot: {response.Text}");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Markup($":person_beard: ");
    }
    catch (Exception e)
    {
        AnsiConsole.WriteException(e);
    }
}

static class Prompts
{
    public const string System =
        """
        Your responses will be rendered using Spectre.Console.AnsiConsole.Write(new Markup(string text))). 
        This means that you can use rich text formatting, colors, and styles in your responses, but you must 
        ensure that the text is valid markup syntax. 
        """;
}
```

<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
