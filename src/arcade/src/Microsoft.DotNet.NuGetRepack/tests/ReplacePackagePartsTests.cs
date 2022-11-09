// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.DotNet.Tools.Tests.Utilities;
using Xunit;

namespace Microsoft.DotNet.Tools.Tests
{
    public class ReplacePackagePartsTests
    {
        [Fact(Skip = "https://github.com/dotnet/arcade/issues/3794")]
        public static void ReplaceFile()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            string originalNupkgPath;
            File.WriteAllBytes(originalNupkgPath = Path.Combine(dir, TestResources.MiscPackages.NameSigned), TestResources.MiscPackages.Signed);

            string replacementFilePath;
            File.WriteAllText(replacementFilePath = Path.Combine(dir, "Replacement.txt"), "<replacement>");

            var engine = new FakeBuildEngine();
            var task = new ReplacePackageParts()
            {
                BuildEngine = engine,
                SourcePackage = originalNupkgPath,
                Parts = new[] { "tools/EmptyBinary.dll" },
                ReplacementFiles = new[] { replacementFilePath },
                NewVersionSuffix = "replaced",
                DestinationFolder = dir
            };

            bool result = task.Execute();
            AssertEx.Equal(Array.Empty<string>(), engine.LogErrorEvents.Select(w => w.Message));
            AssertEx.Equal(Array.Empty<string>(), engine.LogWarningEvents.Select(w => $"{w.Code}: {w.Message}"));
            Assert.True(result);

            using (var archive = new ZipArchive(File.Open(task.NewPackage, FileMode.Open, FileAccess.Read), ZipArchiveMode.Read))
            {
                AssertEx.Equal(new[]
                {
                    "_rels/.rels",
                    "Signed.nuspec",
                    "tools/EmptyBinary.dll",
                    "[Content_Types].xml",
                    "package/services/metadata/core-properties/501236e2491945269f45c5507859c951.psmdcp"
                }, archive.Entries.Select(e => e.FullName));

                using (var reader = new StreamReader(archive.GetEntry("tools/EmptyBinary.dll").Open()))
                {
                    Assert.Equal("<replacement>", reader.ReadToEnd());
                }

                using (var reader = new StreamReader(archive.GetEntry("Signed.nuspec").Open()))
                {
                    AssertEx.AssertEqualToleratingWhitespaceDifferences(@"
<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>Signed</id>
    <version>1.2.3-replaced</version>
    <authors>Microsoft</authors>
    <owners>Microsoft</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Signed</description>
  </metadata>
</package>", reader.ReadToEnd());
                }

                using (var reader = new StreamReader(archive.GetEntry("package/services/metadata/core-properties/501236e2491945269f45c5507859c951.psmdcp").Open()))
                {
                    AssertEx.AssertEqualToleratingWhitespaceDifferences(
@"<?xml version=""1.0"" encoding=""utf-8""?>" +
@"<coreProperties xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:dcterms=""http://purl.org/dc/terms/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.openxmlformats.org/package/2006/metadata/core-properties"">" +
@"<dc:creator>Microsoft</dc:creator>" +
@"<dc:description>Signed</dc:description>" +
@"<dc:identifier>Signed</dc:identifier>" +
@"<version>1.2.3-replaced</version>" +
@"<keywords></keywords>" +
@"<lastModifiedBy>NuGet, Version=4.7.0.5, Culture=neutral, PublicKeyToken=31bf3856ad364e35;Microsoft Windows NT 6.2.9200.0;.NET Framework 4.6</lastModifiedBy>" +
@"</coreProperties>", reader.ReadToEnd());
                }
            }

            Directory.Delete(dir, recursive: true);
        }
    }
}
