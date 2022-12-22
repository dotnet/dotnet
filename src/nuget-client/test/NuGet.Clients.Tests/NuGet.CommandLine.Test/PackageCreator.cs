using System;
using System.IO;
using NuGet.Packaging;
using NuGet.Versioning;

namespace NuGet.CommandLine.Test
{
    public class PackageCreator
    {
        public static string CreatePackage(string id, string version, string outputDirectory,
            Action<PackageBuilder> additionalAction = null)
        {
            PackageBuilder builder = new PackageBuilder()
            {
                Id = id,
                Version = new NuGetVersion(version),
                Description = "Descriptions",
            };
            builder.Authors.Add("test");
            builder.Files.Add(Util.CreatePackageFile(Path.Combine("content", "test1.txt")));
            if (additionalAction != null)
            {
                additionalAction(builder);
            }

            var packageFileName = Path.Combine(outputDirectory, id + "." + version + ".nupkg");
            using (var stream = new FileStream(packageFileName, FileMode.CreateNew))
            {
                builder.Save(stream);
            }

            return packageFileName;
        }

        public static string CreateSymbolPackage(string id, string version, string outputDirectory)
        {
            PackageBuilder builder = new PackageBuilder()
            {
                Id = id,
                Version = new NuGetVersion(version),
                Description = "Descriptions",
            };
            builder.Authors.Add("test");
            builder.Files.Add(Util.CreatePackageFile(Path.Combine("content", "symbol_test1.txt")));
            builder.Files.Add(Util.CreatePackageFile(@"symbol.txt"));

            var packageFileName = Path.Combine(outputDirectory, id + "." + version + ".symbol.nupkg");
            using (var stream = new FileStream(packageFileName, FileMode.CreateNew))
            {
                builder.Save(stream);
            }

            return packageFileName;
        }
    }
}
