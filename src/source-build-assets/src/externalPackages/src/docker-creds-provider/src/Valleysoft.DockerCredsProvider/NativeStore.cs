using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using static Valleysoft.DockerCredsProvider.ListHelper;

namespace Valleysoft.DockerCredsProvider;
 
internal class NativeStore : ICredStore
{
    private readonly string credHelperName;
    private readonly IProcessService processService;
    private readonly IFileSystem fileSystem;
    private readonly IEnvironment environment;

    // A username of <token> indicates the secret is an identity token
    // See https://docs.docker.com/engine/reference/commandline/login/#credential-helper-protocol
    private const string TokenSpecifier = "<token>";

    public NativeStore(string credHelperName, IProcessService processService, IFileSystem fileSystem, IEnvironment environment)
    {
        this.credHelperName = credHelperName;
        this.processService = processService;
        this.fileSystem = fileSystem;
        this.environment = environment;
    }

    public async Task<DockerCredentials> GetCredentialsAsync(string registry)
    {
        const string Username = "Username";
        const string Secret = "Secret";
        string output = ExecuteCredHelper("get", registry);

        using JsonDocument configDoc = await JsonDocument.ParseAsync(new MemoryStream(Encoding.UTF8.GetBytes(output)));

        string? username = null;
        if (configDoc.RootElement.TryGetProperty(Username, out JsonElement usernameElement))
        {
            username = usernameElement.GetString();
        }

        string? password = null;
        if (configDoc.RootElement.TryGetProperty(Secret, out JsonElement secretElement))
        {
            password = secretElement.GetString();
        }

        if (username is null)
        {
            throw new InvalidOperationException($"Output of cred helper doesn't contain '{Username}': {output}");
        }

        if (password is null)
        {
            throw new InvalidOperationException($"Output of cred helper doesn't contain '{Secret}': {output}");
        }

        string? identityToken = null;
        if (username == TokenSpecifier)
        {
            identityToken = password;
            password = null;
        }

        return new DockerCredentials(username, password, identityToken);
    }

    private string? CheckForCandidateOnPath(List<string> candidates, string path) =>
        candidates
            .Select(candidate => Path.Combine(path, candidate))
            .FirstOrDefault(absoluteCandidatePath => fileSystem.FileExists(absoluteCandidatePath));

    private string? ProbePathForNames(List<string> commandNameCandidates) {
        if (this.environment.GetEnvironmentVariable("PATH") is string path  && path is not null) {
            return path
                .Split(Path.PathSeparator)
                .Select(pathDir => this.CheckForCandidateOnPath(commandNameCandidates, pathDir))
                .FirstOrDefault(candidate => candidate is not null);
        } else {
            return null;
        }
    }

    private List<string> ExtendViaPathExt(string commandName) {
        if (environment.GetEnvironmentVariable("PATHEXT") is string pathext && pathext is not null) {
            var executableExtensions = pathext.Split(';');
            // order is important here - the raw name should come first
            var variations = new List<string>(1 + executableExtensions.Length){
                commandName
            };
            // but PATHEXT determines the probing behavior if the raw form isn't found
            variations.AddRange(executableExtensions.Select(ext => Path.ChangeExtension(commandName, ext)));
            return variations;
        } else {
            return Singleton(commandName);
        }
    }

    private List<string> CommandNameCandidates(string toolName) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return ExtendViaPathExt(toolName);
        } else {
            return Singleton(toolName);
        }
    }

    public string? LocateExecutable(string executableName) => this.ProbePathForNames(this.CommandNameCandidates(executableName));

    private string ExecuteCredHelper(string command, string? input)
    {   
        var helperName = $"docker-credential-{this.credHelperName}";
        var commandPath = LocateExecutable(helperName);
        if (commandPath is null) {
            throw new InvalidOperationException($"Unable to locate {helperName} on the system PATH. Be sure that the directory containing {helperName} is on your PATH.");
        }
        ProcessStartInfo startInfo = new(commandPath, command)
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = false,
            UseShellExecute = false,
            RedirectStandardInput = input is not null,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        StringBuilder stdOutput = new();
        StringBuilder stdError = new();

        int exitCode;
        try
        {
            exitCode = processService.Run(startInfo, input, GetDataReceivedHandler(stdOutput), GetDataReceivedHandler(stdError));
        }
        catch (Win32Exception e) when (e.NativeErrorCode == 2)
        {
            throw new InvalidOperationException($"Unable to execute the '{startInfo.FileName}' executable. Be sure that Docker is installed and that its bin location is specified in your environment's path.", e);
        }

        if (exitCode != 0)
        {
            string err = stdError.Length > 0 ? stdError.ToString() : stdOutput.ToString();

            throw new CredsNotFoundException(
                $"Failed to execute '{startInfo.FileName} {startInfo.Arguments}':" +
                Environment.NewLine + err);
        }

        return stdOutput.ToString();
    }

    private static Action<string?> GetDataReceivedHandler(StringBuilder stringBuilder)
    {
        return new Action<string?>(value =>
        {
            if (value is not null)
            {
                stringBuilder.AppendLine(value);
            }
        });
    }
}
