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

    /// <summary>
    /// Returns a set of candidate file paths for the config file that should be checked in the order listed (i.e. they are listed in priority order).
    /// </summary>
    internal static string[] GetConfigFilePaths(IEnvironment env)
    {
        if (env.GetEnvironmentVariable("REGISTRY_AUTH_FILE") is { Length: > 0 } configFile)
        {
            return new[] { configFile };
        }

        List<string> paths = new();

        if (env.GetEnvironmentVariable("XDG_RUNTIME_DIR") is { Length: > 0 } xdgRuntimeDir)
        {
            paths.Add(Path.Combine(xdgRuntimeDir, "containers", "auth.json"));
        }

        string? xdgConfigDir = env.GetEnvironmentVariable("XDG_CONFIG_DIR");
        if (string.IsNullOrEmpty(xdgConfigDir))
        {
            xdgConfigDir = Path.Combine(env.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");
        }
        paths.Add(Path.Combine(xdgConfigDir, "containers", "auth.json"));

        string? dockerConfigDir = env.GetEnvironmentVariable("DOCKER_CONFIG");
        if (string.IsNullOrEmpty(dockerConfigDir))
        {
            dockerConfigDir = Path.Combine(env.GetFolderPath(Environment.SpecialFolder.UserProfile), ".docker");
        }
        paths.Add(Path.Combine(dockerConfigDir, "config.json"));

        return paths.ToArray();
    }

    private static async Task<ICredStore> GetCredStoreAsync(string registry, IFileSystem fileSystem, IProcessService processService, IEnvironment environment)
    {
        string[] configFilePaths = GetConfigFilePaths(environment);

        bool configFileFound = false;
        foreach (var configFilePath in configFilePaths)
        {
            if (!fileSystem.FileExists(configFilePath))
            {
                continue;
            }

            configFileFound = true;

            using Stream openStream = fileSystem.FileOpenRead(configFilePath);
            using JsonDocument configDoc = await JsonDocument.ParseAsync(openStream);

            if (configDoc.RootElement.TryGetProperty("credHelpers", out JsonElement credHelpersElement) &&
                credHelpersElement.TryGetProperty(registry, out JsonElement credHelperElement))
            {
                string? credHelperName = credHelperElement.GetString();
                if (credHelperName is null)
                {
                    throw new JsonException($"Name of the credHelper for host '{registry}' was not set in Docker config {configFilePath}.");
                }

                return new NativeStore(credHelperName, processService, fileSystem, environment);
            }

            if (configDoc.RootElement.TryGetProperty("credsStore", out JsonElement credsStoreElement))
            {
                string? credHelperName = credsStoreElement.GetString();
                if (credHelperName is null)
                {
                    throw new JsonException($"Name of the credsStore was not set in Docker config {configFilePath}.");
                }

                return new NativeStore(credHelperName, processService, fileSystem, environment);
            }

            if (configDoc.RootElement.TryGetProperty("auths", out JsonElement authsElement))
            {
                JsonProperty property = authsElement.EnumerateObject().FirstOrDefault(prop => prop.Name == registry);

                if (property.Equals(default(JsonProperty)))
                {
                    continue;
                }

                return new EncodedStore(property, configFilePath);
            }
        }

        if (!configFileFound)
        {
            throw new FileNotFoundException($"Docker config file doesn't exist.");
        }

        throw new CredsNotFoundException($"No matching auth specified for registry '{registry}' in Docker config.");
    }
}
