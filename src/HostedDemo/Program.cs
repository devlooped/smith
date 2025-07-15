using Anthropic;

const string Instructions =
    """
    Your responses will be rendered using Spectre.Console.AnsiConsole.Write(new Markup(string text))). 
    This means that you can use rich text formatting, colors, and styles in your responses, but you must 
    ensure that the text is valid markup syntax. 
    """;

var builder = App.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.Services
    .AddChatClient(services => new AnthropicClient(
        Throw.IfNullOrEmpty(Env.Get("ANTHROPIC_KEY")),
        services.GetRequiredService<IHttpClientFactory>().CreateClient()))
    .UseLogging()
    .UseFunctionInvocation();

var app = builder.Build(async (IChatClient chat, CancellationToken token) =>
{
    var history = new List<ChatMessage> { new(ChatRole.System, Instructions) };
    var options = new ChatOptions
    {
        ModelId = "claude-sonnet-4-20250514",
        MaxOutputTokens = 1000,
        Temperature = 0.7f,
        Tools = [AIFunctionFactory.Create(() => DateTime.Now, "get_datetime", "Gets the current date and time on the user's local machine.")]
    };

    AnsiConsole.MarkupLine($":robot: Ready");
    AnsiConsole.Markup($":person_beard: ");
    while (!token.IsCancellationRequested)
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
            try
            {
                // Try rendering as formatted markup
                if (response.Text is { Length: > 0 })
                    AnsiConsole.MarkupLine($":robot: {response.Text}");
            }
            catch (Exception)
            {
                // Fallback to escaped markup text if rendering fails
                AnsiConsole.MarkupLineInterpolated($":robot: {response.Text}");
            }

            AnsiConsole.Markup($":person_beard: ");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }

    AnsiConsole.MarkupLine($":robot: Shutting down...");
});

Console.WriteLine("Powered by Smith");


await app.RunAsync();