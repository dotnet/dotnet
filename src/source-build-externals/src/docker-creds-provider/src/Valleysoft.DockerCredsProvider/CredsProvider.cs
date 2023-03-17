using System.Text.Json;

namespace Valleysoft.DockerCredsProvider;

public static class CredsProvider
{
    private static IEnvironment DefaultEnvironment = new EnvironmentWrapper();
    private static IProcessService DefaultProcessService = new ProcessService();
    private static IFileSystem DefaultFileSystem = new FileSystem();

    public static Task<DockerCredentials> GetCredentialsAsync(string registry) =>
        GetCredentialsAsync(registry, DefaultFileSystem, DefaultProcessService, DefaultEnvironment);

    internal static async Task<DockerCredentials> GetCredentialsAsync(string registry, IFileSystem fileSystem, IProcessService processService, IEnvironment environment)
    {
        if (registry is null)
        {
            throw new ArgumentNullException(nameof(registry));
        }

        ICredStore credStore = await GetCredStoreAsync(registry, fileSystem, processService, environment);
        return await credStore.GetCredentialsAsync(registry);
    }

    private static string GetConfigFilePath(IEnvironment env) {
        if (env.GetEnvironmentVariable("DOCKER_CONFIG") is string configDirectory
            && !System.String.IsNullOrEmpty(configDirectory)) {
                return Path.Combine(configDirectory, "config.json");
            } else {
                return Path.Combine(
                    env.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    ".docker",
                    "config.json");
            }
    }

    private static async Task<ICredStore> GetCredStoreAsync(string registry, IFileSystem fileSystem, IProcessService processService, IEnvironment environment)
    {
        string dockerConfigPath = GetConfigFilePath(environment);

        if (!fileSystem.FileExists(dockerConfigPath))
        {
            throw new FileNotFoundException($"Docker config '{dockerConfigPath}' doesn't exist.");
        }

        using Stream openStream = fileSystem.FileOpenRead(dockerConfigPath);
        using JsonDocument configDoc = await JsonDocument.ParseAsync(openStream);

        if (configDoc.RootElement.TryGetProperty("credHelpers", out JsonElement credHelpersElement) &&
            credHelpersElement.TryGetProperty(registry, out JsonElement credHelperElement))
        {
            string? credHelperName = credHelperElement.GetString();
            if (credHelperName is null)
            {
                throw new JsonException($"Name of the credHelper for host '{registry}' was not set in Docker config {dockerConfigPath}.");
            }

            return new NativeStore(credHelperName, processService, fileSystem, environment);
        }

        if (configDoc.RootElement.TryGetProperty("credsStore", out JsonElement credsStoreElement))
        {
            string? credHelperName = credsStoreElement.GetString();
            if (credHelperName is null)
            {
                throw new JsonException($"Name of the credsStore was not set in Docker config {dockerConfigPath}.");
            }

            return new NativeStore(credHelperName, processService, fileSystem, environment);
        }

        if (configDoc.RootElement.TryGetProperty("auths", out JsonElement authsElement))
        {
            JsonProperty property = authsElement.EnumerateObject().FirstOrDefault(prop => prop.Name == registry);

            if (property.Equals(default(JsonProperty)))
            {
                throw new CredsNotFoundException($"No matching auth specified for registry '{registry}' in Docker config '{dockerConfigPath}'.");
            }

            return new EncodedStore(property, dockerConfigPath);
        }

        throw new JsonException($"Unable to find credential information in Docker config '{dockerConfigPath}'.");
    }
}
