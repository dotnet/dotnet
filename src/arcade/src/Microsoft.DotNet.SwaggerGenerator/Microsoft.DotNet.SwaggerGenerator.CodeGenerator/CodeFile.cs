namespace Microsoft.DotNet.SwaggerGenerator
{
    public class CodeFile
    {
        public CodeFile(string path, string contents)
        {
            Path = path;
            Contents = contents;
        }

        public string Path { get; }
        public string Contents { get; }

        public void Deconstruct(out string path, out string contents)
        {
            path = Path;
            contents = Contents;
        }
    }
}
