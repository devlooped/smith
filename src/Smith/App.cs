using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Smith;

/// <summary>
/// Main entry point for the Smith application.
/// </summary>
public static class App
{
    /// <summary>
    /// Invokes the <see cref="Host.CreateApplicationBuilder(string[]?)"/> with additional pre-configured defaults.
    /// </summary>
    /// <remarks>
    ///   The following defaults are applied to the returned <see cref="HostApplicationBuilder"/>:
    ///   <list type="bullet">
    ///     <item><description>Load environment variables from .env files in current dir and above, and user profile dir.</description></item>
    ///   </list>
    /// </remarks>
    /// <param name="args">The command line args.</param>
    /// <returns>The initialized <see cref="HostApplicationBuilder"/>.</returns>
    public static HostApplicationBuilder CreateBuilder(string[]? args) => Host.CreateApplicationBuilder(args);

    /// <summary>
    /// Builds the host app and registers the provided <paramref name="main"/> function as a hosted service to be 
    /// executed automatically when the host is run.
    /// </summary>
    public static IHost Build(this HostApplicationBuilder builder, Func<CancellationToken, Task> main)
    {
        builder.Services.AddHostedService(sp => new AnonymousHostedService(main, sp.GetRequiredService<IHostApplicationLifetime>()));
        return builder.Build();
    }

    /// <summary>
    /// Builds the host app and registers the provided <paramref name="main"/> function as a hosted service to be 
    /// executed automatically when the host is run.
    /// </summary>
    public static IHost Build<TDependency>(this HostApplicationBuilder builder, Func<TDependency, CancellationToken, Task> main) where TDependency : notnull
    {
        builder.Services.AddHostedService(sp => new AnonymousHostedService<TDependency>(main, sp.GetRequiredService<TDependency>(), sp.GetRequiredService<IHostApplicationLifetime>()));
        return builder.Build();
    }

    class AnonymousHostedService(Func<CancellationToken, Task> run, IHostApplicationLifetime host) : IHostedService
    {
        Task? main;

        public Task StartAsync(CancellationToken token)
        {
            main = Task.Run(() => run(host.ApplicationStopping), host.ApplicationStopped);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken token)
        {
            if (main != null)
                await main.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }
    }

    class AnonymousHostedService<TDependency>(Func<TDependency, CancellationToken, Task> run, TDependency dependency, IHostApplicationLifetime host) : IHostedService
    {
        Task? main;

        public Task StartAsync(CancellationToken token)
        {
            main = Task.Run(() => run(dependency, host.ApplicationStopping), host.ApplicationStopped);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken token)
        {
            if (main != null)
                await main.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }
    }

}
