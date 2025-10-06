namespace Valleysoft.DockerCredsProvider;

internal interface IFileSystem
{
    Stream FileOpenRead(string path);
    bool FileExists(string path);
}

internal class FileSystem : IFileSystem
{
    public Stream FileOpenRead(string path) => File.OpenRead(path);

    public bool FileExists(string path) => File.Exists(path);
}
