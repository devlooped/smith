using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;

namespace Smith;

/// <summary>
/// Usability extensions for working with MCP.
/// </summary>
public static class McpExtensions
{
    extension(IMcpServerBuilder builder)
    {
        /// <summary>
        /// Registers a specific method as a server tool.
        /// </summary>
        public IMcpServerBuilder WithTool(Delegate tool, JsonSerializerOptions? options = null)
        {
            builder.Services.AddSingleton(services
                => McpServerTool.Create(tool, new() { Services = services, SerializerOptions = options }));

            return builder;
        }
    }
}
