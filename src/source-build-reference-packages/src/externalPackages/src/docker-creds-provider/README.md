# Docker Creds Provider

This .NET library provides a wrapper around Docker's credential configuration. Credentials for a Docker registry are [stored](https://docs.docker.com/engine/reference/commandline/login/#credentials-store) either in the operating system's native credential store or within Docker's configuration file. Docker Creds Provider determines which method is used for storing the credentials and returns the credentials for the requested registry.

## Usage

```csharp
DockerCredentials dockerHubCreds = await CredsProvider.GetCredentialsAsync("https://index.docker.io/v1/");

DockerCredentials privateRepoCreds = await CredsProvider.GetCredentialsAsync("contoso.azurecr.io");
```

The library is available as a NuGet package: [Valleysoft.DockerCredsProvider](https://www.nuget.org/packages/Valleysoft.DockerCredsProvider/).
