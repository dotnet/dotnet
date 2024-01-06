using System;

namespace Valleysoft.DockerCredsProvider;

internal interface IEnvironment
{
    string? GetEnvironmentVariable(string variable);
    string GetFolderPath(Environment.SpecialFolder folder);
}

internal class EnvironmentWrapper : IEnvironment
{
    public string? GetEnvironmentVariable(string variable) => Environment.GetEnvironmentVariable(variable);

    public string GetFolderPath(Environment.SpecialFolder folder) => Environment.GetFolderPath(folder, Environment.SpecialFolderOption.DoNotVerify);
}
