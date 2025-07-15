using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Smith;

/// <summary>
/// Extensions for configuring the host application builder and accessing services.
/// </summary>
public static class HostExtensions
{
    extension(IHost app)
    {
        /// <summary>
        /// Gest the host app configuration;
        /// </summary>
        public IConfiguration Configuration => app.Services.GetRequiredService<IConfiguration>();
    }
}
