namespace Microsoft.Extensions.Configuration;

/// <summary>
/// Provides configuration extensions for adding user secrets configuration source.
/// </summary>
public static class UserSecretsConfigurationExtensions
{
    /// <summary>
    /// <para>
    /// Adds the user secrets configuration source with the project-specified user secrets ID.
    /// </para>
    /// </summary>
    /// <param name="configuration">The configuration builder.</param>
    /// <returns>The configuration builder.</returns>
    public static IConfigurationBuilder AddUserSecrets(this IConfigurationBuilder configuration)
    {
        if (!string.IsNullOrEmpty(ThisAssembly.Project.UserSecretsId))
            return configuration.AddUserSecrets(ThisAssembly.Project.UserSecretsId, reloadOnChange: false);

        return configuration;
    }
}