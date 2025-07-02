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

var chat = new Anthropic.AnthropicClient(Throw.
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
[![Clarius Org](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/clarius.png "Clarius Org")](https://github.com/clarius)
[![MFB Technologies, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MFB-Technologies-Inc.png "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![Torutek](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/torutek-gh.png "Torutek")](https://github.com/torutek-gh)
[![DRIVE.NET, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/drivenet.png "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Keith Pickford](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Keflon.png "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tbolon.png "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/kfrancis.png "Kori Francis")](https://github.com/kfrancis)
[![Toni Wenzel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/twenzel.png "Toni Wenzel")](https://github.com/twenzel)
[![Uno Platform](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/unoplatform.png "Uno Platform")](https://github.com/unoplatform)
[![Reuben Swartz](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/rbnswartz.png "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jfoshee.png "Jacob Foshee")](https://github.com/jfoshee)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Mrxx99.png "")](https://github.com/Mrxx99)
[![Eric Johnson](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/eajhnsn1.png "Eric Johnson")](https://github.com/eajhnsn1)
[![David JENNI](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/davidjenni.png "David JENNI")](https://github.com/davidjenni)
[![Jonathan ](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Jonathan-Hickey.png "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Charley Wu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/akunzai.png "Charley Wu")](https://github.com/akunzai)
[![Ken Bonny](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KenBonny.png "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/SimonCropp.png "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/agileworks-eu.png "agileworks-eu")](https://github.com/agileworks-eu)
[![sorahex](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/sorahex.png "sorahex")](https://github.com/sorahex)
[![Zheyu Shen](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/arsdragonfly.png "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/vezel-dev.png "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/ChilliCream.png "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/4OTC.png "4OTC")](https://github.com/4OTC)
[![Vincent Limo](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/v-limo.png "Vincent Limo")](https://github.com/v-limo)
[![Jordan S. Jones](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jordansjones.png "Jordan S. Jones")](https://github.com/jordansjones)
[![domischell](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/DominicSchell.png "domischell")](https://github.com/DominicSchell)
[![Justin Wendlandt](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jwendl.png "Justin Wendlandt")](https://github.com/jwendl)
[![Adrian Alonso](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/adalon.png "Adrian Alonso")](https://github.com/adalon)
[![Michael Hagedorn](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Eule02.png "Michael Hagedorn")](https://github.com/Eule02)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
