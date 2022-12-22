using System.IO;

namespace NuGet.CommandLine
{
    public static class StreamExtensions
    {
        public static string ReadToEnd(this Stream stream)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
