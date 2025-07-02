#:package Smith@0.*

// Sample X.AI client usage with .NET
var messages = new Chat()
{
    { "system", "You are a highly intelligent AI assistant." },
    { "user", "What is 101*3?" },
};

var grok = new GrokClient(Throw.IfNullOrEmpty(Env.Get("XAI_API_KEY")))
    .GetChatClient("grok-3-mini")
    .AsIChatClient();

var options = new GrokChatOptions
{
    ReasoningEffort = ReasoningEffort.High, // or ReasoningEffort.Low
    Search = GrokSearch.Auto,               // or GrokSearch.On/Off
};

var response = await grok.GetResponseAsync(messages, options);

AnsiConsole.MarkupLine($":robot: {response.Text}");

