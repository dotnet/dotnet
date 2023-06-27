using Moq;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Valleysoft.DockerCredsProvider.Test;

public class CredsProviderTests
{
    IEnvironment DefaultEnvironmentMock;

    public CredsProviderTests() {
        var envMock = new Mock<IEnvironment>();
        var defaultWrapper = new EnvironmentWrapper();
        envMock.Setup(e => e.GetFolderPath(Environment.SpecialFolder.UserProfile)).Returns(defaultWrapper.GetFolderPath(Environment.SpecialFolder.UserProfile));
        envMock.Setup(e => e.GetEnvironmentVariable(It.IsAny<string>())).Returns<string>(arg => defaultWrapper.GetEnvironmentVariable(arg));
        DefaultEnvironmentMock = envMock.Object;
    }

    [Fact]
    public async Task NativeStore()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string credsStore = "desktop";
        string credsStoreBinary = $"docker-credential-{credsStore}";
        string username = "testuser";
        string password = "password";
        string pathRoot = "/a";

        string dockerConfigContent =
            "{" +
            $"\"credsStore\": \"{credsStore}\"" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent)
            .WithFile(Path.Combine(pathRoot, credsStoreBinary));

        Mock<IProcessService> processServiceMock = new();
        processServiceMock.StubHelperSuccess(credsStore, "test", $"{{ \"Username\": \"{username}\", \"Secret\": \"{password}\" }}");

        Mock<IEnvironment> envMock = new();
        envMock.WithSystemProfileFolder();
        envMock.Setup(o => o.GetEnvironmentVariable("PATH")).Returns(pathRoot);
        envMock.Setup(o => o.GetEnvironmentVariable("PATHEXT")).Returns<string>(null);

        DockerCredentials creds = await CredsProvider.GetCredentialsAsync("test", fileSystemMock.Object, processServiceMock.Object, envMock.Object);

        Assert.Equal(username, creds.Username);
        Assert.Equal(password, creds.Password);
        Assert.Null(creds.IdentityToken);
    }

    [Fact]
    public async Task NativeStore_Token()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string credsStore = "desktop";
        string credsStoreBinary = $"docker-credential-{credsStore}";
        string username = "<token>";
        string token = "identitytoken";
        string pathRoot = "/a";

        string dockerConfigContent =
            "{" +
            $"\"credsStore\": \"{credsStore}\"" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent)
            .WithFile(Path.Combine(pathRoot, credsStoreBinary));

        Mock<IProcessService> processServiceMock = new();
        processServiceMock
            .StubHelperSuccess(credsStore, "test", $"{{ \"Username\": \"{username}\", \"Secret\": \"{token}\" }}");

        Mock<IEnvironment> envMock = new();
        envMock.WithSystemProfileFolder();
        envMock.Setup(o => o.GetEnvironmentVariable("PATH")).Returns(pathRoot);
        envMock.Setup(o => o.GetEnvironmentVariable("PATHEXT")).Returns<string>(null);

        DockerCredentials creds = await CredsProvider.GetCredentialsAsync("test", fileSystemMock.Object, processServiceMock.Object, envMock.Object);

        Assert.Equal(username, creds.Username);
        Assert.Null(creds.Password);
        Assert.Equal(token, creds.IdentityToken);
    }

    [Fact]
    public async Task NativeStore_ExeNotFound()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string credsStore = "desktop";

        string dockerConfigContent =
            "{" +
            $"\"credsStore\": \"{credsStore}\"" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        Mock<IProcessService> processServiceMock = new();
        processServiceMock
            .Setup(o => o.Run(
                It.Is<ProcessStartInfo>(startInfo => startInfo.FileName.EndsWith($"docker-credential-{credsStore}")),
                "test",
                It.IsAny<Action<string?>>(),
                It.IsAny<Action<string?>>()))
            .Throws(new Win32Exception(2));

        await Assert.ThrowsAsync<InvalidOperationException>(() => CredsProvider.GetCredentialsAsync("test", fileSystemMock.Object, processServiceMock.Object, DefaultEnvironmentMock));
    }

    [Fact]
    public async Task NativeStore_Error()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string credsStore = "desktop";
        string credsStoreBinary = $"docker-credential-{credsStore}";
        string pathRoot = "/a";

        string dockerConfigContent =
            "{" +
                $"\"credsStore\": \"{credsStore}\"" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent)
            .WithFile(Path.Combine(pathRoot, credsStoreBinary));

        Mock<IEnvironment> envMock = new();
        envMock.WithSystemProfileFolder();
        envMock.Setup(o => o.GetEnvironmentVariable("PATH")).Returns(pathRoot);
        envMock.Setup(o => o.GetEnvironmentVariable("PATHEXT")).Returns<string>(null);

        Mock<IProcessService> processServiceMock = new();
        processServiceMock
            .StubHelperError(credsStore, "test", "error msg");

        await Assert.ThrowsAsync<CredsNotFoundException>(
            () => CredsProvider.GetCredentialsAsync("test", fileSystemMock.Object, processServiceMock.Object, envMock.Object));
    }

    [Fact]
    public async Task EncodedStore()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string username = "testuser";
        string password = "testpass";

        string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    "\"testregistry\": {" +
                        $"\"auth\": \"{encodedCreds}\"" +
                    "}" +
                "}" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        DockerCredentials creds = await CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, Mock.Of<IProcessService>(), DefaultEnvironmentMock);

        Assert.Equal(username, creds.Username);
        Assert.Equal(password, creds.Password);
        Assert.Null(creds.IdentityToken);
    }

    [Fact]
    public async Task EncodedStore_NoMatch()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string username = "testuser";
        string password = "testpass";

        string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    "\"testregistry\": {" +
                        $"\"auth\": \"{encodedCreds}\"" +
                    "}" +
                "}" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        await Assert.ThrowsAsync<CredsNotFoundException>(
            () => CredsProvider.GetCredentialsAsync("testregistry2", fileSystemMock.Object, Mock.Of<IProcessService>(), DefaultEnvironmentMock));
    }

    [Fact]
    public async Task EncodedStore_NoUsername()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string username = String.Empty;
        string password = "testpass";

        string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    "\"testregistry\": {" +
                        $"\"auth\": \"{encodedCreds}\"" +
                    "}" +
                "}" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        await Assert.ThrowsAsync<JsonException>(() =>
            CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, Mock.Of<IProcessService>(), DefaultEnvironmentMock)
        );
    }

    [Fact]
    public async Task EncodedStore_NoPassword()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string username = "testuser";
        string password = "";

        string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    "\"testregistry\": {" +
                        $"\"auth\": \"{encodedCreds}\"" +
                    "}" +
                "}" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        await Assert.ThrowsAsync<JsonException>(() =>
            CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, Mock.Of<IProcessService>(), DefaultEnvironmentMock)
        );
    }

    [Fact]
    public async Task EncodedStore_NoSeparator()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string username = "testuser";
        string password = "testpassword";

        string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}{password}"));

        string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    "\"testregistry\": {" +
                        $"\"auth\": \"{encodedCreds}\"" +
                    "}" +
                "}" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        await Assert.ThrowsAsync<JsonException>(() =>
            CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, Mock.Of<IProcessService>(), DefaultEnvironmentMock)
        );
    }

    [Fact]
    public async Task EncodedStore_IdentityTokenWithUsernamePasswordSeparator()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string username = "00000000-0000-0000-0000-000000000000";
        string password = String.Empty;
        string token = "tokenstring";

        string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    "\"testregistry\": {" +
                        $"\"auth\": \"{encodedCreds}\"," +
                        $"\"identitytoken\": \"{token}\"" +
                    "}" +
                "}" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        DockerCredentials creds = await CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, Mock.Of<IProcessService>() ,DefaultEnvironmentMock);
        Assert.Equal(username, creds.Username);
        Assert.Null(creds.Password);
        Assert.Equal(token, creds.IdentityToken);
    }

    [Fact]
    public async Task EncodedStore_IdentityTokenWithoutUsernamePasswordSeparator()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string username = "<token>";
        string token = "tokenstring";

        string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));

        string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    "\"testregistry\": {" +
                        $"\"auth\": \"{encodedCreds}\"," +
                        $"\"identitytoken\": \"{token}\"" +
                    "}" +
                "}" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        DockerCredentials creds = await CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, Mock.Of<IProcessService>(), DefaultEnvironmentMock);
        Assert.Equal(username, creds.Username);
        Assert.Null(creds.Password);
        Assert.Equal(token, creds.IdentityToken);
    }

    [Fact]
    public Task NullRegistry()
    {
        return Assert.ThrowsAsync<ArgumentNullException>(() => CredsProvider.GetCredentialsAsync(null!));
    }

    [Fact]
    public async Task ConfigFileDoesNotExist()
    {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .Setup(o => o.FileExists(dockerConfigPath))
            .Returns(false);

        await Assert.ThrowsAsync<FileNotFoundException>(
            () => CredsProvider.GetCredentialsAsync("test", fileSystemMock.Object, Mock.Of<IProcessService>(), DefaultEnvironmentMock));
    }

    [Fact]
    public async Task NativeStore_UsesDockerConfigEnvironmentVariable() {
        var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetTempFileName());
        var dockerConfigPath = Path.Combine(tempPath, "config.json");

        Mock<IEnvironment> envMock = new();
        envMock.WithSystemProfileFolder();
        envMock.Setup(e => e.GetEnvironmentVariable("DOCKER_CONFIG")).Returns(tempPath);

        string username = "foo";
        string password = "bar";

        string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    "\"dummyRegistry.io\": {" +
                        $"\"auth\": \"{encodedCreds}\"" +
                    "}" +
                "}" +
            "}";

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent);

        var creds = await CredsProvider.GetCredentialsAsync("dummyRegistry.io", fileSystemMock.Object, Mock.Of<ProcessService>(), envMock.Object);
        Assert.Equal("foo", creds.Username);
        Assert.Equal("bar", creds.Password);
    }

    [Fact]
    public async Task NativeStore_ProbesPATHForHelper() {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string helper = "example";
        string fullHelperName = $"docker-credential-{helper}";
        string username = "<token>";
        string token = "token";

        string dockerConfigContent =
            "{" +
                "\"credHelpers\": {" +
                    $"\"testregistry\": \"{helper}\"" +
                "}" +
            "}";

        // the idea here is that we setup the helper on the second PATH entry,
        // so if we succeed that means we probed.
        var systemPaths = new List<string>{
            "/a",
            "/b"
        };

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent)
            .WithFile(Path.Combine(systemPaths[1], fullHelperName));

        Mock<IEnvironment> envMock = new();
        envMock
            .WithSystemProfileFolder()
            .WithPath(systemPaths);
        envMock.Setup(o => o.GetEnvironmentVariable("PATHEXT")).Returns<string>(null);

        Mock<IProcessService> processServiceMock = new();
        processServiceMock.StubHelperSuccess(helper, "testregistry",  $"{{ \"Username\": \"{username}\", \"Secret\": \"{token}\" }}");

        DockerCredentials creds = await CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, processServiceMock.Object, envMock.Object);

        Assert.Equal(username, creds.Username);
        Assert.Equal(token, creds.IdentityToken);
        Assert.Null(creds.Password);
    }

    [WindowsFact]
    public async Task NativeStore_ProbesPATHEXTForHelperBinaries() {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string helper = "example";
        string fullHelperName = $"docker-credential-{helper}";
        string username = "<token>";
        string token = "token";

        string dockerConfigContent =
            "{" +
                "\"credHelpers\": {" +
                    $"\"testregistry\": \"{helper}\"" +
                "}" +
            "}";

        var pathRoot = "/a";
        // the idea here is that we set up the helper with a different extension
        // and prime the system to probe that extension.  if we succeed, that means we
        // probed as expected.
        var pathExts = new List<string>{
            ".ABC",
            ".XYZ"
        };

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent)
            .WithFile(Path.Combine(pathRoot, $"{fullHelperName}{pathExts[0]}"))
            .WithFile(Path.Combine(pathRoot, $"{fullHelperName}{pathExts[1]}"));

        Mock<IEnvironment> envMock = new();
        envMock
            .WithSystemProfileFolder()
            .WithPath(new List<string> { pathRoot })
            .WithExecutableExtensions(pathExts);

        Mock<IProcessService> processServiceMock = new();
        processServiceMock.StubHelperSuccess($"{helper}{pathExts[0]}", "testregistry",  $"{{ \"Username\": \"{username}\", \"Secret\": \"{token}\" }}");

        DockerCredentials creds = await CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, processServiceMock.Object, envMock.Object);

        Assert.Equal(username, creds.Username);
        Assert.Equal(token, creds.IdentityToken);
        Assert.Null(creds.Password);
    }

    [WindowsFact]
    public async Task NativeStore_SupportsPATHEXTPrecedence() {
        string dockerConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".docker",
            "config.json");

        string helper = "example";
        string fullHelperName = $"docker-credential-{helper}";
        string username = "<token>";
        string token = "token";

        string dockerConfigContent =
            "{" +
                "\"credHelpers\": {" +
                    $"\"testregistry\": \"{helper}\"" +
                "}" +
            "}";

        var pathRoot = "/a";
        // the idea here is that we set up the helper with a different extension
        // and prime the system to probe that extension.  if we succeed, that means we
        // probed as expected.
        var pathExts = new List<string>{
            ".ABC",
            ".XYZ"
        };

        Mock<IFileSystem> fileSystemMock = new();
        fileSystemMock
            .WithFile(dockerConfigPath, dockerConfigContent)
            .WithFile(Path.Combine(pathRoot, $"{fullHelperName}{pathExts[1]}"));

        Mock<IEnvironment> envMock = new();
        envMock
            .WithSystemProfileFolder()
            .WithPath(new List<string> { pathRoot })
            .WithExecutableExtensions(pathExts);

        Mock<IProcessService> processServiceMock = new();
        processServiceMock.StubHelperSuccess($"{helper}{pathExts[1]}", "testregistry",  $"{{ \"Username\": \"{username}\", \"Secret\": \"{token}\" }}");

        DockerCredentials creds = await CredsProvider.GetCredentialsAsync("testregistry", fileSystemMock.Object, processServiceMock.Object, envMock.Object);

        Assert.Equal(username, creds.Username);
        Assert.Equal(token, creds.IdentityToken);
        Assert.Null(creds.Password);
    }

    [Fact]
    public async Task UsesAllConfigPaths()
    {
        Mock<IFileSystem> fileSystemMock = new();
        string[] configFilePaths = new[]
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".docker", "config.json"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "containers", "auth.json"),
        };
        for (int idx = 0; idx < configFilePaths.Length; idx++)
        {
            string dockerConfigPath = configFilePaths[idx];
            string registry = $"testregistry{idx}";
            string encodedCreds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"testuser{idx}:testpass{idx}"));
            string dockerConfigContent =
            "{" +
                "\"auths\": {" +
                    $"\"{registry}\": {{" +
                        $"\"auth\": \"{encodedCreds}\"" +
                    "}" +
                "}" +
            "}";

            fileSystemMock.WithFile(dockerConfigPath, dockerConfigContent);
        }

        for (int idx = 0; idx < configFilePaths.Length; idx++)
        {
            string registry = $"testregistry{idx}";
            DockerCredentials creds = await CredsProvider.GetCredentialsAsync(registry, fileSystemMock.Object, Mock.Of<IProcessService>(), DefaultEnvironmentMock);

            Assert.Equal($"testuser{idx}", creds.Username);
            Assert.Equal($"testpass{idx}", creds.Password);
            Assert.Null(creds.IdentityToken);
        }
    }

    [Theory]
    // REGISTRY_AUTH_FILE replaces all
    [InlineData("registryauthfile", "xdgruntimedir", "xdgconfigdir", "dockerconfigdir", new[] { "registryauthfile" } )]
    // Order of different paths.
    [InlineData("",                 "xdgruntimedir", "xdgconfigdir", "dockerconfigdir", new[] { "xdgruntimedir/containers/auth.json",
                                                                                                "xdgconfigdir/containers/auth.json",
                                                                                                "dockerconfigdir/config.json" } )]
    // XDG_CONFIG_DIR defaults to $HOME/.config
    [InlineData("",                 "",              "",             "dockerconfigdir", new[] { "userprofile/.config/containers/auth.json",
                                                                                                "dockerconfigdir/config.json" } )]
    // DOCKER_CONFIG defaults to $HOME/.docker
    [InlineData("",                 "",              "",             "",                new[] { "userprofile/.config/containers/auth.json",
                                                                                                "userprofile/.docker/config.json" } )]
    public void ConfigFilePaths(string? registryAuthFile, string? xdgRuntimeDir, string? xdgConfigDir, string? dockerConfig, string[] expectedConfigFilePaths)
    {
        for (int i = 0; i < expectedConfigFilePaths.Length; i++)
        {
            // Windows: use backslash path separators.
            expectedConfigFilePaths[i] = expectedConfigFilePaths[i].Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        Mock<IEnvironment> envMock = new();
        envMock.Setup(o => o.GetEnvironmentVariable("REGISTRY_AUTH_FILE")).Returns(registryAuthFile);
        envMock.Setup(o => o.GetEnvironmentVariable("XDG_RUNTIME_DIR")).Returns(xdgRuntimeDir);
        envMock.Setup(o => o.GetEnvironmentVariable("XDG_CONFIG_DIR")).Returns(xdgConfigDir);
        envMock.Setup(o => o.GetEnvironmentVariable("DOCKER_CONFIG")).Returns(dockerConfig);
        envMock.Setup(e => e.GetFolderPath(Environment.SpecialFolder.UserProfile)).Returns("userprofile");

        string[] configFilePath = CredsProvider.GetConfigFilePaths(envMock.Object);
        Assert.Equal(expectedConfigFilePaths, configFilePath);
    }
}
