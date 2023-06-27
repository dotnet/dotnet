namespace Valleysoft.DockerCredsProvider.Test;

using System.Diagnostics;
using System.Runtime.InteropServices;
using Moq;

public static class MockExtensions {

    internal static Mock<IFileSystem> WithFile(this Mock<IFileSystem> mock, string fileThatExists) {
        mock
            .Setup(m => m.FileExists(fileThatExists))
            .Returns(true);
        return mock;
    }

    internal static Mock<IFileSystem> WithFile(this Mock<IFileSystem> mock, string fileThatExists, string fileContent) {
        mock.WithFile(fileThatExists);
        mock
            .Setup(m => m.FileOpenRead(fileThatExists))
            .Returns(() => new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent)));
        return mock;
    }

    internal static Mock<IFileSystem> WithFiles(this Mock<IFileSystem> mock, List<string> filesWithContent) {
        foreach(var file in filesWithContent) {
            mock.WithFile(file);
        }
        return mock;
    }

    internal static Mock<IFileSystem> WithFiles(this Mock<IFileSystem> mock, List<(string, string)> filesWithContent) {
        foreach((string path, string content) in filesWithContent) {
            mock.WithFile(path, content);
        }
        return mock;
    }

    internal static Mock<IEnvironment> WithPath(this Mock<IEnvironment> mock, List<string> pathDirs) {
        var pathStr = String.Join(Path.PathSeparator, pathDirs);
        mock.Setup(o => o.GetEnvironmentVariable("PATH")).Returns(pathStr);
        return mock;
    }

    internal static Mock<IEnvironment> WithExecutableExtensions(this Mock<IEnvironment> mock, List<string> extensionKinds) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            var extensions = String.Join(Path.PathSeparator, extensionKinds);
            mock.Setup(o => o.GetEnvironmentVariable("PATHEXT")).Returns(extensions);
        }
        return mock;
    }

    internal static Mock<IProcessService> StubHelperSuccess(this Mock<IProcessService> mock, string helperShortName, string helperCommand, string output) {
        var fullName = $"docker-credential-{helperShortName}";
        mock.Setup(o => o.Run(
                It.Is<ProcessStartInfo>(startInfo => startInfo.FileName.EndsWith(fullName)),
                It.IsIn(helperCommand),
                It.IsAny<Action<string?>>(),
                It.IsAny<Action<string?>>())
            ).Callback((ProcessStartInfo startInfo, string? input, Action<string?> outputDataReceived, Action<string?> errorDataReceived) =>
            {
                outputDataReceived(output);
            })
            .Returns(0);
        return mock;
    }

    internal static Mock<IProcessService> StubHelperError(this Mock<IProcessService> mock, string helperShortName, string helperCommand, string errorOutput) {
        var fullName = $"docker-credential-{helperShortName}";
        mock.Setup(o => o.Run(
                It.Is<ProcessStartInfo>(startInfo => startInfo.FileName.EndsWith(fullName)),
                helperCommand,
                It.IsAny<Action<string?>>(),
                It.IsAny<Action<string?>>())
            ).Callback((ProcessStartInfo startInfo, string? input, Action<string?> outputDataReceived, Action<string?> errorDataReceived) =>
            {
                errorDataReceived(errorOutput);
            })
            .Returns(1);
        return mock;
    }

    internal static Mock<IEnvironment> WithSystemProfileFolder(this Mock<IEnvironment> mock) {
        mock.Setup(o => o.GetFolderPath(It.IsAny<Environment.SpecialFolder>())).Returns<Environment.SpecialFolder>(arg => new EnvironmentWrapper().GetFolderPath(arg));
        return mock;
    }
}
