namespace Valleysoft.DockerCredsProvider;

internal interface ICredStore
{
    Task<DockerCredentials> GetCredentialsAsync(string registry);
}
