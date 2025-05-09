using System.Text;
using System.Text.Json;

namespace Valleysoft.DockerCredsProvider;

internal class EncodedStore : ICredStore
{
    private readonly DockerCredentials creds;

    public EncodedStore(JsonProperty registryProperty, string configPath)
    {
        string? identityToken = null;
        string credentialEncoding;

        if (registryProperty.Value.TryGetProperty("identitytoken", out JsonElement tokenElement))
        {
            identityToken = tokenElement.GetString();
        }

        if (registryProperty.Value.TryGetProperty("auth", out JsonElement authElement))
        {
            string? encodedValue = authElement.GetString();
            if (encodedValue is null)
            {
                throw new JsonException(
                    $"No auth value specified for registry '{registryProperty.Name}' in Docker config '{configPath}'.");
            }

            credentialEncoding = encodedValue;
        }
        else
        {
            throw new JsonException(
                $"Auth property doesn't exist for registry '{registryProperty.Name}' in Docker config '{configPath}'.");
        }

        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(credentialEncoding));
        int separatorIndex = decoded.IndexOf(':');

        string username;
        if (separatorIndex < 0)
        {
            if (identityToken is null)
            {
                throw new JsonException(
                $"The username/password separator character ':' is missing in the decoded string from the auth value of registry '{registryProperty.Name}' in Docker config '{configPath}'.");
            }

            username = decoded;
        }
        else
        {
            username = decoded.Substring(0, separatorIndex);
        }

        if (username.Length == 0)
        {
            throw new JsonException(
                $"Encoded auth value does not contain a username for registry '{registryProperty.Name}' in Docker config '{configPath}'.");
        }

        string? password = null;
        if (separatorIndex >= 0)
        {
            password = decoded.Substring(separatorIndex + 1);
        }

        if (password?.Length == 0)
        {
            password = null;
            if (identityToken is null)
            {
                throw new JsonException(
                    $"Configuration for registry '{registryProperty.Name}' in Docker config '{configPath}' must contain either a password in the encoded auth value or have an identity token set.");
            }
        }

        creds = new DockerCredentials(username, password, identityToken);
    }

    public Task<DockerCredentials> GetCredentialsAsync(string registry) => Task.FromResult(creds);
}
