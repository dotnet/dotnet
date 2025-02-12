namespace Valleysoft.DockerCredsProvider;

public class DockerCredentials
{
    public DockerCredentials(string username, string? password = null, string? identityToken = null)
    {
        Username = username;
        Password = password;
        IdentityToken = identityToken;
    }

    public string Username { get; }

    public string? Password { get; }

    public string? IdentityToken { get; }
}
