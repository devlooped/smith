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
            => WithTool(builder, null!, null!, tool, options);

        /// <summary>
        /// Registers a specific method as a server tool.
        /// </summary>
        /// <param name="name">The name of the tool.</param>
        /// <remarks>
        /// The tool description will be set to the <see cref="DescriptionAttribute"/> on the method, if any.
        /// </remarks>
        public IMcpServerBuilder WithTool(string name, Delegate tool, JsonSerializerOptions? options = null)
            => WithTool(builder, name, null!, null!, tool, options);

        /// <summary>
        /// Registers a specific method as a server tool.
        /// </summary>
        /// <param name="name">The name of the tool.</param>
        /// <param name="title">A human-readable title for the tool that can be displayed to users.</param>
        /// <remarks>
        /// The tool description will be set to the <see cref="DescriptionAttribute"/> on the method, if any.
        /// </remarks>
        public IMcpServerBuilder WithTool(string name, string title, Delegate tool, JsonSerializerOptions? options = null)
            => WithTool(builder, name, title, null!, tool, options);

        /// <summary>
        /// Registers a specific method as a server tool.
        /// </summary>
        /// <param name="name">The name of the tool.</param>
        /// <param name="title">A human-readable title for the tool that can be displayed to users.</param>
        /// <param name="description">The tool description.</param>
        public IMcpServerBuilder WithTool(string name, string title, string description, Delegate tool, JsonSerializerOptions? options = null)
        {
            builder.Services.AddSingleton(services
                => McpServerTool.Create(tool, new()
                {
                    Name = name,
                    Title = title,
                    Description = description,
                    Services = services,
                    SerializerOptions = options
                }));

            return builder;
        }
    }
}
