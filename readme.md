![Icon](assets/icon.png) Smith
============

[![Version](https://img.shields.io/nuget/vpre/Smith.svg?color=royalblue)](https://www.nuget.org/packages/Smith)
[![Downloads](https://img.shields.io/nuget/dt/Smith.svg?color=green)](https://www.nuget.org/packages/Smith)
[![License](https://img.shields.io/github/license/devlooped/smith.svg?color=blue)](https://github.com//devlooped/smith/blob/main/license.txt)
[![Build](https://github.com/devlooped/smith/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/devlooped/smith/actions)

<!-- #content -->
Run AI-powered C# files using Microsoft.Extensions.AI and Devlooped.Extensions.AI

Example leveraging [Grok](https://console.x.ai/):

```csharp
#:package Smith@0.*

// Sample X.AI client usage with .NET
var messages = new Chat()
{
    { "system", "You are a highly intelligent AI assistant." },
    { "user", "What is 101*3?" },
};

IChatClient grok = new GrokClient(Throw.IfNullOrEmpty(Env.Get("XAI_API_KEY")))
    .GetChatClient("grok-3-mini")
    .AsIChatClient();

var options = new GrokChatOptions
{
    ReasoningEffort = ReasoningEffort.High, // or ReasoningEffort.Low
    Search = GrokSearch.Auto,               // or GrokSearch.On/GrokSearch.Off
};

var response = await grok.GetResponseAsync(messages, options);

AnsiConsole.MarkupLine($":robot: {response.Text}");
```

> [!NOTE]
> The most useful namespaces and dependencies for developing Microsoft.Extensions.AI-powered 
> applications are automatically referenced and imported when using this package.

Example using Claude:

![](https://raw.githubusercontent.com/devlooped/smith/main/assets/run.png)

```csharp
#:package Smith@0.*

var client = new Anthropic.AnthropicClient(Throw.
    Env.Get("Claude:Key") ?? throw new InvalidOperationException("Missing Claude:Key configuration."),
    services.GetRequiredService<IHttpClientFactory>().CreateClient("ai")))
    .UseLogging()
    .UseFunctionInvocation();

var builder = App.CreateBuilder(args);
builder.Services.AddChatClient(new );

var app = builder.Build();

var history = new List<ChatMessage> { new ChatMessage(ChatRole.System, Prompts.System) };
var chat = app.Services.GetRequiredService<IChatClient>();

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


## Configuration / Environment Variables

The `Env` class provides access to the following variables/configuration automatically: 

* `.env` files: in local and parent directories
* `~/.env` file: in the user's home directory (`%userprofile%\.env` on Windows)
* All default configuration sources from [App Builder](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder#host-builder-settings): 
    * Environment variables prefixed with DOTNET_.
    * Command-line arguments.
    * appsettings.json.
    * appsettings.{Environment}.json.
    * Secret Manager when the app runs in the Development environment.
    * Environment variables.
    * Command-line arguments.


<!-- #content -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
# Sponsors 

<!-- sponsors.md -->
[![Clarius Org](https://avatars.githubusercontent.com/u/71888636?v=4&s=39 "Clarius Org")](https://github.com/clarius)
[![MFB Technologies, Inc.](https://avatars.githubusercontent.com/u/87181630?v=4&s=39 "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![SandRock](https://avatars.githubusercontent.com/u/321868?u=99e50a714276c43ae820632f1da88cb71632ec97&v=4&s=39 "SandRock")](https://github.com/sandrock)
[![DRIVE.NET, Inc.](https://avatars.githubusercontent.com/u/15047123?v=4&s=39 "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Keith Pickford](https://avatars.githubusercontent.com/u/16598898?u=64416b80caf7092a885f60bb31612270bffc9598&v=4&s=39 "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://avatars.githubusercontent.com/u/127185?u=7f50babfc888675e37feb80851a4e9708f573386&v=4&s=39 "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://avatars.githubusercontent.com/u/67574?u=3991fb983e1c399edf39aebc00a9f9cd425703bd&v=4&s=39 "Kori Francis")](https://github.com/kfrancis)
[![Reuben Swartz](https://avatars.githubusercontent.com/u/724704?u=2076fe336f9f6ad678009f1595cbea434b0c5a41&v=4&s=39 "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://avatars.githubusercontent.com/u/480334?v=4&s=39 "Jacob Foshee")](https://github.com/jfoshee)
[![](https://avatars.githubusercontent.com/u/33566379?u=bf62e2b46435a267fa246a64537870fd2449410f&v=4&s=39 "")](https://github.com/Mrxx99)
[![Eric Johnson](https://avatars.githubusercontent.com/u/26369281?u=41b560c2bc493149b32d384b960e0948c78767ab&v=4&s=39 "Eric Johnson")](https://github.com/eajhnsn1)
[![Jonathan ](https://avatars.githubusercontent.com/u/5510103?u=98dcfbef3f32de629d30f1f418a095bf09e14891&v=4&s=39 "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Ken Bonny](https://avatars.githubusercontent.com/u/6417376?u=569af445b6f387917029ffb5129e9cf9f6f68421&v=4&s=39 "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://avatars.githubusercontent.com/u/122666?v=4&s=39 "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://avatars.githubusercontent.com/u/5989304?v=4&s=39 "agileworks-eu")](https://github.com/agileworks-eu)
[![Zheyu Shen](https://avatars.githubusercontent.com/u/4067473?v=4&s=39 "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://avatars.githubusercontent.com/u/87844133?v=4&s=39 "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://avatars.githubusercontent.com/u/16239022?v=4&s=39 "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://avatars.githubusercontent.com/u/68428092?v=4&s=39 "4OTC")](https://github.com/4OTC)
[![domischell](https://avatars.githubusercontent.com/u/66068846?u=0a5c5e2e7d90f15ea657bc660f175605935c5bea&v=4&s=39 "domischell")](https://github.com/DominicSchell)
[![Adrian Alonso](https://avatars.githubusercontent.com/u/2027083?u=129cf516d99f5cb2fd0f4a0787a069f3446b7522&v=4&s=39 "Adrian Alonso")](https://github.com/adalon)
[![torutek](https://avatars.githubusercontent.com/u/33917059?v=4&s=39 "torutek")](https://github.com/torutek)
[![Ryan McCaffery](https://avatars.githubusercontent.com/u/16667079?u=c0daa64bb5c1b572130e05ae2b6f609ecc912d4d&v=4&s=39 "Ryan McCaffery")](https://github.com/mccaffers)
[![Seika Logiciel](https://avatars.githubusercontent.com/u/2564602?v=4&s=39 "Seika Logiciel")](https://github.com/SeikaLogiciel)
[![Andrew Grant](https://avatars.githubusercontent.com/devlooped-user?s=39 "Andrew Grant")](https://github.com/wizardness)
[![eska-gmbh](https://avatars.githubusercontent.com/devlooped-team?s=39 "eska-gmbh")](https://github.com/eska-gmbh)
[![Geodata AS](https://avatars.githubusercontent.com/u/5946299?v=4&s=39 "Geodata AS")](https://github.com/geodata-no)


<!-- sponsors.md -->
[![Sponsor this project](https://avatars.githubusercontent.com/devlooped-sponsor?s=118 "Sponsor this project")](https://github.com/sponsors/devlooped)

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
