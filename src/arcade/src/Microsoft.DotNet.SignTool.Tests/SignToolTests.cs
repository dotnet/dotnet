// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;
using Xunit.Abstractions;
using Microsoft.DotNet.Build.Tasks.Installers;
using Microsoft.DotNet.StrongName;

namespace Microsoft.DotNet.SignTool.Tests
{
    public class SignToolTests : IDisposable
    {
        private readonly string _tmpDir;
        private readonly ITestOutputHelper _output;

        // Default extension based signing information
        private static readonly Dictionary<string, List<SignInfo>> s_fileExtensionSignInfo = new Dictionary<string, List<SignInfo>>()
        {
            {".js", new List<SignInfo>{ new SignInfo("JSCertificate") } },
            {".jar",  new List<SignInfo>{ new SignInfo("JARCertificate") } },
            {".ps1",  new List<SignInfo>{ new SignInfo("PSCertificate") } },
            {".psd1",  new List<SignInfo>{ new SignInfo("PSDCertificate") } },
            {".psm1",  new List<SignInfo>{ new SignInfo("PSMCertificate") } },
            {".psc1",   new List<SignInfo>{ new SignInfo("PSCCertificate") } },
            {".dylib", new List<SignInfo>{ new SignInfo("DylibCertificate") } },
            {".deb", new List<SignInfo>{ new SignInfo("LinuxSign") } },
            {".rpm", new List<SignInfo>{ new SignInfo("LinuxSign") } },
            {".dll",  new List<SignInfo>{ new SignInfo("Microsoft400") } }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
            {".exe",  new List<SignInfo>{ new SignInfo("Microsoft400") } }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
            {".msi",  new List<SignInfo>{ new SignInfo("Microsoft400") } }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
            {".vsix",  new List<SignInfo>{ new SignInfo("VsixSHA2") } },
            {".zip",  new List<SignInfo>{ SignInfo.Ignore } },
            {".tgz",  new List<SignInfo>{ SignInfo.Ignore } },
            {".pkg",  new List<SignInfo>{ new SignInfo("MacDeveloperHarden") } }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
            {".app",  new List<SignInfo>{ new SignInfo("MacDeveloperHarden") } }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
            {".py",  new List<SignInfo>{ new SignInfo("Microsoft400") } }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
            {".nupkg",  new List<SignInfo>{ new SignInfo("NuGet") } },
            {".symbols.nupkg",  new List<SignInfo>{ SignInfo.Ignore } },
        };

        private static readonly Dictionary<string, List<SignInfo>> s_fileExtensionSignInfoWithCollisionId = 
            new Dictionary<string, List<SignInfo>>()
        {
            {".js", new List<SignInfo>{ new SignInfo("JSCertificate", collisionPriorityId: "123") } },
            {".jar", new List<SignInfo>{ new SignInfo("JARCertificate", collisionPriorityId: "123") } },
            { ".ps1", new List<SignInfo>{ new SignInfo("PSCertificate", collisionPriorityId: "123") } },
            { ".psd1", new List<SignInfo>{ new SignInfo("PSDCertificate", collisionPriorityId: "123") } },
            { ".psm1", new List<SignInfo>{ new SignInfo("PSMCertificate", collisionPriorityId: "123") } },
            { ".psc1", new List<SignInfo>{ new SignInfo("PSCCertificate", collisionPriorityId: "123") } },
            { ".dylib", new List<SignInfo>{ new SignInfo("DylibCertificate", collisionPriorityId: "123") } },
            { ".deb", new List<SignInfo>{ new SignInfo("LinuxSign", collisionPriorityId: "123") } },
            { ".dll", new List<SignInfo>
                { 
                    new SignInfo("Microsoft400", collisionPriorityId: "123"), // lgtm [cs/common-default-passwords] Safe, these are certificate names
                    new SignInfo("FakeOne", collisionPriorityId: "456")
                } 
             },
            { ".exe", new List<SignInfo>{ new SignInfo("Microsoft400", collisionPriorityId:  "123") } }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
            { ".msi", new List<SignInfo>{ new SignInfo("Microsoft400", collisionPriorityId:  "123") } }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
            { ".vsix", new List<SignInfo>{ new SignInfo("VsixSHA2", collisionPriorityId: "123") } },
            { ".zip", new List<SignInfo>{ SignInfo.Ignore } },
            { ".tgz", new List<SignInfo>{ SignInfo.Ignore } },
            { ".pkg", new List<SignInfo>{ new SignInfo("Microsoft400", collisionPriorityId:  "123") } },
            { ".app",  new List<SignInfo>{ new SignInfo("Microsoft400", collisionPriorityId:  "123") } },
            { ".nupkg", new List<SignInfo>{ new SignInfo("NuGet", collisionPriorityId: "123") } },
            { ".symbols.nupkg",  new List<SignInfo>{ SignInfo.Ignore } },
        };

        // Default extension based signing information post build
        private static readonly ITaskItem[] s_fileExtensionSignInfoPostBuild = new ITaskItem[]
        {
            new TaskItem(".js", new Dictionary<string, string> {
                { "CertificateName", "JSCertificate" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".jar", new Dictionary<string, string> {
                { "CertificateName", "JARCertificate" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".ps1", new Dictionary<string, string> {
                { "CertificateName", "PSCertificate" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".psd1", new Dictionary<string, string> {
                { "CertificateName", "PSDCertificate" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".psm1", new Dictionary<string, string> {
                { "CertificateName", "PSMCertificate" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".psc1", new Dictionary<string, string> {
                { "CertificateName", "PSCCertificate" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".dylib", new Dictionary<string, string> {
                { "CertificateName", "DylibCertificate" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".deb", new Dictionary<string, string> {
                { "CertificateName", "LinuxSign" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".dll", new Dictionary<string, string> {
                { "CertificateName", "Microsoft400" }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".exe", new Dictionary<string, string> {
                { "CertificateName", "Microsoft400" }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".zip", new Dictionary<string, string> {
                { "CertificateName", "None" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".tgz", new Dictionary<string, string> {
                { "CertificateName", "None" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".pkg", new Dictionary<string, string> {
                { "CertificateName", "Microsoft400" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".app", new Dictionary<string, string> {
                { "CertificateName", "Microsoft400" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".nupkg", new Dictionary<string, string> {
                { "CertificateName", "NuGet" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".vsix", new Dictionary<string, string> {
                { "CertificateName", "VsixSHA2" },
                { SignToolConstants.CollisionPriorityId, "123" }
            }),
            new TaskItem(".js", new Dictionary<string, string> {
                { "CertificateName", "JSCertificate" },
                { SignToolConstants.CollisionPriorityId, "234" }
            }),
            new TaskItem(".jar", new Dictionary<string, string> {
                { "CertificateName", "JARCertificate" },
                { SignToolConstants.CollisionPriorityId, "234" }
            }),
            new TaskItem(".ps1", new Dictionary<string, string> {
                { "CertificateName", "PSCertificate" },
                { SignToolConstants.CollisionPriorityId, "234" }
            }),
            new TaskItem(".psd1", new Dictionary<string, string> {
                { "CertificateName", "PSDCertificate" },
                { SignToolConstants.CollisionPriorityId, "234" }
            }),
            new TaskItem(".dll", new Dictionary<string, string> {
                { "CertificateName", "Microsoft400" }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
                { SignToolConstants.CollisionPriorityId, "234" }
            }),
            new TaskItem(".nupkg", new Dictionary<string, string> {
                { "CertificateName", "NuGet" },
                { SignToolConstants.CollisionPriorityId, "234" }
            }),
            new TaskItem(".vsix", new Dictionary<string, string> {
                { "CertificateName", "VsixSHA2" },
                { SignToolConstants.CollisionPriorityId, "234" }
            })
        };

        /// <summary>
        /// List of known signable extensions. Copied, removing duplicates, from here:
        /// https://microsoft.sharepoint.com/teams/codesigninfo/Wiki/Signable%20Files.aspx
        /// </summary>
        public static readonly string[] SignableExtensions =
        {
            ".exe",
            ".dll",
            ".rll",
            ".olb",
            ".ocx",

            ".cab",

            ".cat",

            ".vbs",
            ".js",
            ".wfs",

            ".msi",
            ".mui",
            ".msp",
            ".msu",
            ".psf",
            ".mpb",
            ".mp",
            ".msm",

            ".doc",
            ".xls",
            ".ppt",
            ".xla",
            ".vdx",
            ".xsn",
            ".mpp",

            ".xlam",
            ".xlsb",
            ".xlsm",
            ".xltm",
            ".potm",
            ".ppsm",
            ".pptm",
            ".docm",
            ".dotm",

            ".ttf",
            ".otf",

            ".ps1",
            ".ps1xml",
            ".psm1",
            ".psd1",
            ".psc1",
            ".cdxml",
            ".wsf",
            ".mof",

            ".sft",
            ".dsft",

            ".vsi",

            ".xap",

            ".efi",

            ".vsix",

            ".jar",

            ".winmd",

            ".appx",
            ".appxbundle",

            ".esd",

            ".py",
            ".pyd",
#if !NETFRAMEWORK
            ".deb",
#endif
        };

        public static IEnumerable<object[]> GetSignableExtensions()
        {
            foreach (var extension in SignableExtensions)
            {
                yield return new object[] { extension };
            }
        }

        public SignToolTests(ITestOutputHelper output)
        {
            _tmpDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tmpDir);
            _output = output;
        }

        private string GetWixToolPath()
        {
            return Path.Combine(Path.GetDirectoryName(typeof(SignToolTests).Assembly.Location), "tools", "wix");
        }

        private static string s_snPath = Path.Combine(Path.GetDirectoryName(typeof(SignToolTests).Assembly.Location), "tools", "sn", "sn.exe");
        private static string s_tarToolPath = Path.Combine(Path.GetDirectoryName(typeof(SignToolTests).Assembly.Location), "tools", "tar", "Microsoft.Dotnet.Tar.dll");
        private static string s_pkgToolPath = Path.Combine(Path.GetDirectoryName(typeof(SignToolTests).Assembly.Location), "tools", "pkg", "Microsoft.Dotnet.MacOsPkg.dll");

        private string GetResourcePath(string name, string relativePath = null)
        {
            var srcPath = Path.Combine(Path.GetDirectoryName(typeof(SignToolTests).Assembly.Location), "Resources", name);

            var dstDir = _tmpDir;

            if (relativePath != null)
            {
                dstDir = Path.Combine(dstDir, relativePath);
                Directory.CreateDirectory(dstDir);
            }

            var dstPath = Path.Combine(dstDir, name);

            if (!File.Exists(dstPath))
            {
                File.Copy(srcPath, dstPath);
            }

            return dstPath;
        }

        private string CreateTestResource(string name)
        {
            var dstPath = Path.Combine(_tmpDir, name);

            File.WriteAllText(dstPath, "This is a test file content");

            return dstPath;
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(_tmpDir, recursive: true);
            }
            catch
            {
            }
        }

        private void ValidateGeneratedProject(
            List<ItemToSign> itemsToSign,
            Dictionary<string, List<SignInfo>> strongNameSignInfo,
            Dictionary<ExplicitCertificateKey, string> fileSignInfo,
            Dictionary<string, List<SignInfo>> extensionsSignInfo,
            string[] expectedXmlElementsPerSigningRound,
            Dictionary<string, List<AdditionalCertificateInformation>> additionalCertificateInfo = null,
            string wixToolsPath = null)
        {
            var buildEngine = new FakeBuildEngine();

            var task = new SignToolTask { BuildEngine = buildEngine };

            // The path to DotNet will always be null in these tests, this will force
            // the signing logic to call our FakeBuildEngine.BuildProjectFile with a path
            // to the XML that store the content of the would be Microbuild sign request.
            var signToolArgs = new SignToolArgs(_tmpDir, microBuildCorePath: "MicroBuildCorePath", testSign: true, dotnetPath: null, msbuildVerbosity: "quiet", _tmpDir, enclosingDir: "", "", wixToolsPath: wixToolsPath, tarToolPath: s_tarToolPath, pkgToolPath: s_pkgToolPath, dotnetTimeout: -1);

            var signTool = new FakeSignTool(signToolArgs, task.Log);
            var configuration = new Configuration(signToolArgs.TempDir, itemsToSign, strongNameSignInfo, fileSignInfo, extensionsSignInfo, additionalCertificateInfo, tarToolPath: s_tarToolPath, pkgToolPath: s_pkgToolPath, snPath: s_snPath, task.Log);
            var signingInput = configuration.GenerateListOfFiles();
            var util = new BatchSignUtil(
                task.BuildEngine,
                task.Log,
                signTool,
                signingInput,
                new string[] { },
                configuration._hashToCollisionIdMap);

            var beforeSigningEngineFilesList = Directory.GetFiles(signToolArgs.TempDir, "*-engine.exe", SearchOption.AllDirectories);
            util.Go(doStrongNameCheck: true);
            var afterSigningEngineFilesList = Directory.GetFiles(signToolArgs.TempDir, "*-engine.exe", SearchOption.AllDirectories);

            // validate no intermediate msi engine files have populated the drop (they fail signing validation).
            beforeSigningEngineFilesList.SequenceEqual(afterSigningEngineFilesList).Should().BeTrue();

            // The list of files that would be signed was captured inside the FakeBuildEngine,
            // here we check if that matches what we expected
            var actualXmlElementsPerSigningRound = buildEngine.FilesToSign.Select(round => string.Join(Environment.NewLine, round));
            actualXmlElementsPerSigningRound.Count().Should().Be(expectedXmlElementsPerSigningRound.Length);
            int i = 0;
            foreach (var actual in actualXmlElementsPerSigningRound)
            {
                var actualXml = AssertEx.NormalizeWhitespace(actual);
                var expectedXml = AssertEx.NormalizeWhitespace(expectedXmlElementsPerSigningRound[i]);
                actualXml.Should().Be(expectedXml);
                i++;
            }

            task.Log.HasLoggedErrors.Should().BeFalse();
        }

        private void ValidateFileSignInfos(
            List<ItemToSign> itemsToSign,
            Dictionary<string, List<SignInfo>> strongNameSignInfo,
            Dictionary<ExplicitCertificateKey, string> fileSignInfo,
            Dictionary<string, List<SignInfo>> extensionsSignInfo,
            string[] expected,
            string[] expectedCopyFiles = null,
            Dictionary<string, List<AdditionalCertificateInformation>> additionalCertificateInfo = null,
            string[] expectedErrors = null,
            string[] expectedWarnings = null)
        {
            var engine = new FakeBuildEngine();
            var task = new SignToolTask { BuildEngine = engine };
            var signingInput = new Configuration(_tmpDir, itemsToSign, strongNameSignInfo, fileSignInfo, extensionsSignInfo, additionalCertificateInfo, tarToolPath: s_tarToolPath, pkgToolPath: s_pkgToolPath, snPath: s_snPath, task.Log).GenerateListOfFiles();

            signingInput.FilesToSign.Select(f => f.ToString()).Should().BeEquivalentTo(expected);
            signingInput.FilesToCopy.Select(f => $"{f.Key} -> {f.Value}").Should().BeEquivalentTo(expectedCopyFiles ?? Array.Empty<string>());
            engine.LogErrorEvents.Select(w => w.Message).Should().BeEquivalentTo(expectedErrors ?? Array.Empty<string>());
            engine.LogWarningEvents.Select(w => $"{w.Code}: {w.Message}").Should().BeEquivalentTo(expectedWarnings ?? Array.Empty<string>());
        }

#if !NETFRAMEWORK
        private void ValidateProducedDebContent(
            string debianPackage,
            (string, string)[] expectedFilesOriginalHashes,
            string[] signableFiles,
            string expectedControlFileContent)
        {
            string tempDir = Path.Combine(_tmpDir, "verification");
            Directory.CreateDirectory(tempDir);

            string controlArchive = ExtractArchiveFromDebPackage(debianPackage, "control.tar", tempDir);
            string dataArchive = ExtractArchiveFromDebPackage(debianPackage, "data.tar", tempDir);

            string controlLayout = Path.Combine(tempDir, "control");
            string dataLayout = Path.Combine(tempDir, "data");

            Directory.CreateDirectory(controlLayout);
            Directory.CreateDirectory(dataLayout);

            ZipData.ExtractTarballContents(dataArchive, dataLayout, skipSymlinks: false);
            ZipData.ExtractTarballContents(controlArchive, controlLayout);

            string md5sumsContents = File.ReadAllText(Path.Combine(controlLayout, "md5sums"));

            // Checks:
            // Expected files are present
            // Signed files have hashes different than original
            // md5sums file contains the correct hashes of all files
            // md5sums file does not contain the original hashes of signable files
            foreach ((string targetSystemFilePath, string originalHash) in expectedFilesOriginalHashes)
            {
                string layoutFilePath = Path.Combine(dataLayout, targetSystemFilePath);
                File.Exists(layoutFilePath).Should().BeTrue();

                using MD5 md5 = MD5.Create();
                using FileStream fileStream = File.OpenRead(layoutFilePath);
                string newHash = Convert.ToHexString(md5.ComputeHash(fileStream));

                if (signableFiles.Contains(targetSystemFilePath))
                {
                    newHash.Should().NotBe(originalHash);
                    md5sumsContents.Should().Contain($"{newHash} {targetSystemFilePath}");
                    md5sumsContents.Should().NotContain($"{originalHash} {targetSystemFilePath}");
                }
                else
                {
                    newHash.Should().Be(originalHash);
                    md5sumsContents.Should().Contain($"{originalHash} {targetSystemFilePath}");
                }
            }

            // Check: control file contents matches the expected contents
            string controlFileContents = File.ReadAllText(Path.Combine(controlLayout, "control"));
            controlFileContents.Should().Be(expectedControlFileContent);
        }

        private string ExtractArchiveFromDebPackage(string debianPackage, string archiveName, string destinationFolder)
        {
            var (relativePath, content, contentSize) = ZipData.ReadDebContainerEntries(debianPackage, archiveName).Single();
            string archive = Path.Combine(destinationFolder, relativePath);
            File.WriteAllBytes(archive, ((MemoryStream)content).ToArray());
            return archive;
        }

        private void ValidateProducedRpmContent(
            string rpmPackage,
            (string, string)[] expectedFilesOriginalHashes,
            string[] signableFiles,
            string originalUncompressedPayloadChecksum)
        {
            string tempDir = Path.Combine(_tmpDir, "verification");
            Directory.CreateDirectory(tempDir);

            string layout = Path.Combine(tempDir, "layout");
            Directory.CreateDirectory(layout);

            ZipData.ExtractRpmPayloadContents(rpmPackage, layout);

            // Checks:
            // Expected files are present
            // Signed files have hashes different than original
            foreach ((string targetSystemFilePath, string originalHash) in expectedFilesOriginalHashes)
            {
                string layoutFilePath = Path.Combine(layout, targetSystemFilePath);
                File.Exists(layoutFilePath).Should().BeTrue();

                using MD5 md5 = MD5.Create(); // lgtm [cs/weak-crypto] Azure Storage specifies use of MD5
                using FileStream fileStream = File.OpenRead(layoutFilePath);
                string newHash = Convert.ToHexString(md5.ComputeHash(fileStream));

                if (signableFiles.Contains(targetSystemFilePath))
                {
                    newHash.Should().NotBe(originalHash);
                }
                else
                {
                    newHash.Should().Be(originalHash);
                }
            }

            // Checks:
            // Header payload digest matches the hash of the payload
            // Header payload digest is different than the hash of the original payload
            IReadOnlyList<RpmHeader<RpmHeaderTag>.Entry> headerEntries = ZipData.GetRpmHeaderEntries(rpmPackage);
            string uncompressedPayloadDigest = ((string[])headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.UncompressedPayloadDigest).Value)[0].ToString();
            uncompressedPayloadDigest.Should().NotBe(originalUncompressedPayloadChecksum);

            using var stream = File.Open(rpmPackage, FileMode.Open);
            using RpmPackage package = RpmPackage.Read(stream);
            package.ArchiveStream.Seek(0, SeekOrigin.Begin);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(package.ArchiveStream);
                string checksum = Convert.ToHexString(hash).ToLower();
                checksum.Should().Be(uncompressedPayloadDigest);
            }
        }
#endif
        [Fact]
        public void EmptySigningList()
        {
            var itemsToSign = new List<ItemToSign>();

            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            var task = new SignToolTask { BuildEngine = new FakeBuildEngine() };
            var signingInput = new Configuration(_tmpDir, itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, null, tarToolPath: s_tarToolPath, pkgToolPath: s_pkgToolPath, snPath: s_snPath, task.Log).GenerateListOfFiles();

            signingInput.FilesToSign.Should().BeEmpty();
            signingInput.ZipDataMap.Should().BeEmpty();
            task.Log.HasLoggedErrors.Should().BeFalse();
        }

        [Fact]
        public void EmptySigningListForTask()
        {
            var task = new SignToolTask
            {
                BuildEngine = new FakeBuildEngine(),
                ItemsToSign = Array.Empty<ITaskItem>(),
                StrongNameSignInfo = Array.Empty<ITaskItem>(),
                LogDir = "LogDir",
                TempDir = "TempDir",
                DryRun = false,
                TestSign = true,
                DotNetPath = CreateTestResource("dotnet.fake"),
                SNBinaryPath = CreateTestResource("fake.sn.exe"),
                PkgToolPath = s_pkgToolPath,
            };

            task.Execute().Should().BeTrue();
        }

        [Fact]
        public void SignWhenSnExeIsNotRequired()
        {
            var task = new SignToolTask
            {
                BuildEngine = new FakeBuildEngine(_output),
                ItemsToSign = Array.Empty<ITaskItem>(),
                StrongNameSignInfo = Array.Empty<ITaskItem>(),
                LogDir = "LogDir",
                TempDir = "TempDir",
                DryRun = false,
                TestSign = true,
                DotNetPath = CreateTestResource("dotnet.fake"),
                DoStrongNameCheck = false,
                SNBinaryPath = null,
                PkgToolPath = s_pkgToolPath,
            };

            task.Execute().Should().BeTrue();
        }

        [Fact]
        public void OnlyContainer()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("ContainerOne.1.0.0.nupkg"), "")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo> {new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.1.0.0.nupkg' Certificate='NuGet'",
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "lib/native/NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
            }*/);
        }

        [Fact]
        public void SkipSigning()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("ContainerOne.1.0.0.nupkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo> {new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>
            {
                { new ExplicitCertificateKey("NativeLibrary.dll"), "None" },
                { new ExplicitCertificateKey("ProjectOne.dll", publicKeyToken: "581d91ccdfc4ea9c", targetFramework: ".NETCoreApp,Version=v2.1"), "None" }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.1.0.0.nupkg' Certificate='NuGet'"
            });
        }

        [Fact]
        public void SkipStrongNamingForAlreadyStrongNamedBinary()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("SignedLibrary.dll")),
                new ItemToSign(GetResourcePath("StrongNamedWithEcmaKey.dll"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "31bf3856ad364e35", new List<SignInfo> {new SignInfo(certificate: "FooCert", strongName: "Blah.snk") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, Array.Empty<string>());
        }

        [Fact]
        public void DoNotSkipStrongNamingForDelaySignedBinary()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("DelaySigned.dll"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "b03f5f7f11d50a3a", new List<SignInfo> {new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[] 
            {
                "File 'DelaySigned.dll' TargetFramework='.NETCoreApp,Version=v9.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'"
            });
        }

        [Fact]
        public void SkipStrongNamingForCrossGennedBinary()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("Crossgenned.exe"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "b03f5f7f11d50a3a", new List<SignInfo> {new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'Crossgenned.exe' Certificate='Microsoft400'"
            });
        }

        [Fact]
        public void SkipStrongNamingBinaryButDontSkipAuthenticode()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("OpenSigned.dll"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "cc7b13ffcd2ddd51", new List<SignInfo> {new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'OpenSigned.dll' TargetFramework='.NETCoreApp,Version=v9.0' Certificate='3PartySHA2'"
            });
        }

        [Fact]
        public void OnlyAuthenticodeSignByPKT()
        {
            var fileToTest = "ProjectOne.dll";
            var pktToTest = "581d91ccdfc4ea9c";
            var certificateToTest = "3PartySHA2";

            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath(fileToTest), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { pktToTest, new List<SignInfo> { new SignInfo(certificateToTest, collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, new Dictionary<string, List<SignInfo>>(), new[]
            {
                $"File '{fileToTest}' TargetFramework='.NETStandard,Version=v2.0' Certificate='{certificateToTest}'",
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, new Dictionary<string, List<SignInfo>>(), new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, fileToTest))}"">
  <Authenticode>{certificateToTest}</Authenticode>
</FilesToSign>
"
            });
        }

        [Fact]
        public void OnlyContainerAndOverridingByPKT()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath(GetResourcePath("ContainerOne.1.0.0.nupkg")))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo> { new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("ProjectOne.dll", "581d91ccdfc4ea9c"), "OverriddenCertificate" }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='OverriddenCertificate' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='OverriddenCertificate' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='OverriddenCertificate' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='OverriddenCertificate' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.1.0.0.nupkg' Certificate='NuGet'"
            },
            expectedWarnings: new[]
            {
                // Reenable after https://github.com/dotnet/arcade/issues/10293
                // $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "lib/native/NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "3", "lib/net461/ProjectOne.dll")}' with Microsoft certificate 'OverriddenCertificate'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "5", "lib/netcoreapp2.0/ProjectOne.dll")}' with Microsoft certificate 'OverriddenCertificate'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "6", "lib/netcoreapp2.1/ProjectOne.dll")}' with Microsoft certificate 'OverriddenCertificate'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "7", "lib/netstandard2.0/ProjectOne.dll")}' with Microsoft certificate 'OverriddenCertificate'. The library is considered 3rd party library due to its copyright: ''."
            });
        }

        [Fact]
        public void OnlyContainerAndOverridingByFileName()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("ContainerOne.1.0.0.nupkg"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo> { new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("NativeLibrary.dll", collisionPriorityId: "123"), "OverriddenCertificate1" },
                { new ExplicitCertificateKey("ProjectOne.dll", collisionPriorityId: "123"), "3PartySHA2" }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'NativeLibrary.dll' Certificate='OverriddenCertificate1'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='ArcadeCertTest' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.1.0.0.nupkg' Certificate='NuGet'"
            },
            expectedWarnings: new[]
            {
                // Reenable after https://github.com/dotnet/arcade/issues/10293
                // $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "lib/native/NativeLibrary.dll")}' with Microsoft certificate 'OverriddenCertificate1'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "4", "lib/netcoreapp2.0/ContainerOne.dll")}' with Microsoft certificate 'ArcadeCertTest'. The library is considered 3rd party library due to its copyright: ''."
            });
        }

        [Fact]
        public void EmptyPKT()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("EmptyPKT.dll"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo("ArcadeCertTest", "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("EmptyPKT.dll"), "3PartySHA2" }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'EmptyPKT.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='3PartySHA2'",
            });
        }

        [Fact]
        public void CrossGenerated()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("CoreLibCrossARM.dll"), "123"),
                new ItemToSign(GetResourcePath("AspNetCoreCrossLib.dll"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "7cec85d7bea7798e", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } },
                { "adb9793829ddae60", new List<SignInfo>{ new SignInfo(certificate: "Microsoft400", strongName: "AspNetCore", collisionPriorityId: "123") } } // lgtm [cs/common-default-passwords] Safe, these are certificate names
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("EmptyPKT.dll", collisionPriorityId: "123"), "3PartySHA2" }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, new Dictionary<string, List<SignInfo>>(), new[]
            {
                "File 'CoreLibCrossARM.dll' Certificate='ArcadeCertTest'",
                "File 'AspNetCoreCrossLib.dll' TargetFramework='.NETCoreApp,Version=v3.0' Certificate='Microsoft400'",
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, new Dictionary<string, List<SignInfo>>(), new[]
            {
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "CoreLibCrossARM.dll"))}"">
  <Authenticode>ArcadeCertTest</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "AspNetCoreCrossLib.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
            });
        }

        [Fact]
        public void DefaultCertificateForAssemblyWithoutStrongName()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("EmptyPKT.dll"), "123")
            };

            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "", new List<SignInfo>{ new SignInfo("3PartySHA2", collisionPriorityId: "123") } }
            };

            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>() { };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'EmptyPKT.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='3PartySHA2'",
            });
        }

        [Fact]
        public void CustomTargetFrameworkAttribute()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("CustomTargetFrameworkAttribute.dll"), "123")
            };

            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                {  "", new List<SignInfo>{ new SignInfo("DefaultCertificate", collisionPriorityId: "123") } }
            };

            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("CustomTargetFrameworkAttribute.dll", targetFramework: ".NETFramework,Version=v2.0", collisionPriorityId: "123"), "3PartySHA2" }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'CustomTargetFrameworkAttribute.dll' TargetFramework='.NETFramework,Version=v2.0' Certificate='3PartySHA2'",
            });
        }

        [Fact]
        public void ThirdPartyLibraryMicrosoftCertificate()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("EmptyPKT.dll"))
            };

            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>() { };
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>() { };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'EmptyPKT.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='Microsoft400'",
            },
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "EmptyPKT.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''."
            });
        }

        [WindowsOnlyFact]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void DoubleNestedContainer()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("PackageWithWix.nupkg"), "123"),
                new ItemToSign(GetResourcePath("MsiBootstrapper.exe.wixpack.zip"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo("3PartySHA2", "ArcadeStrongTest", "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'MsiSetup.msi' Certificate='Microsoft400'",
                "File 'MsiBootstrapper.exe' Certificate='Microsoft400'",
                "File 'PackageWithWix.nupkg' Certificate='NuGet'"
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293
            expectedWarnings: new[]
            {
                
                // $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "4", "MsiBootstrapper.exe")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: 'Copyright (c). All rights reserved.'."
            }*/);

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "ABCDEFG/MsiSetup.msi"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "engines\\0\\MsiBootstrapper.exe-engine.exe"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "MsiBootstrapper.exe"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "PackageWithWix.nupkg"))}"">
  <Authenticode>NuGet</Authenticode>
</FilesToSign>"
            },
            wixToolsPath: GetWixToolPath());
        }


        [Fact]
        public void NestedContainer()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("NestedContainer.1.0.0.nupkg"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerTwo.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.1.0.0.nupkg' Certificate='NuGet'",
                "File 'NestedContainer.1.0.0.nupkg' Certificate='NuGet'"
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "lib/native/NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''."
            }*/);

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "lib/native/NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "lib/net461/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "lib/netcoreapp2.0/ContainerTwo.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "5", "lib/netcoreapp2.0/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "6", "lib/netcoreapp2.1/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "7", "lib/netstandard2.0/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "9", "lib/netcoreapp2.0/ContainerOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "8", "ContainerOne.1.0.0.nupkg"))}"">
  <Authenticode>NuGet</Authenticode>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(GetResourcePath("NestedContainer.1.0.0.nupkg"))}"">
  <Authenticode>NuGet</Authenticode>
</FilesToSign>"
            });
        }

        [Fact]
        public void NestedContainerWithCollisions()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("NestedContainer.1.0.0.nupkg"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information. Since ContainerOne.dll collides with ContainerTwo.dll already in the hash mapping
            // table with collition id 123, we end up using ArcadeStrongTest instead of OverriddenCertificate1
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("ContainerOne.dll", collisionPriorityId: "456"), "OverriddenCertificate1" }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerTwo.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.1.0.0.nupkg' Certificate='NuGet'",
                "File 'NestedContainer.1.0.0.nupkg' Certificate='NuGet'"
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "lib/native/NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''."
            }*/);

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "lib/native/NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "lib/net461/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "lib/netcoreapp2.0/ContainerTwo.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "5", "lib/netcoreapp2.0/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "6", "lib/netcoreapp2.1/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "7", "lib/netstandard2.0/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "9", "lib/netcoreapp2.0/ContainerOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "8", "ContainerOne.1.0.0.nupkg"))}"">
  <Authenticode>NuGet</Authenticode>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(GetResourcePath("NestedContainer.1.0.0.nupkg"))}"">
  <Authenticode>NuGet</Authenticode>
</FilesToSign>"
            });
        }

        [Fact]
        public void SignZipFile()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.zip"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'Nested.NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'Nested.SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'test.zip'",
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "0", "NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
            }*/);

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "0", "NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "SOS.NETCore.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.SOS.NETCore.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
"
            });
        }

        /// <summary>
        /// Verifies that signing of pkgs can be done on Windows, even though
        /// we will not unpack or repack them.
        /// </summary>
        [WindowsOnlyFact]
        public void SignJustPkgWithoutUnpack()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.pkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'test.pkg' Certificate='MacDeveloperHarden'",
            });

            // OSX files need to be zipped first before being signed
            // This is why the .pkgs are listed as .zip files below
            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.pkg.zip"))}"">
                <Authenticode>MacDeveloperHarden</Authenticode>
                </FilesToSign>",
            });
        }

        [MacOSOnlyFact]
        public void UnpackAndSignPkg()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.pkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'Nested.NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'Nested.SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'NestedPkg.pkg' Certificate='MacDeveloperHarden'",
                "File 'test.pkg' Certificate='MacDeveloperHarden'",
            });

            // OSX files need to be zipped first before being signed
            // This is why the .pkgs are listed as .zip files below
            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "Payload/SOS.NETCore.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "Payload/NativeLibrary.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "5", "Payload/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.SOS.NETCore.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "6", "Payload/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                ",
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "NestedPkg.pkg.zip"))}"">
                <Authenticode>MacDeveloperHarden</Authenticode>
                </FilesToSign>",
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.pkg.zip"))}"">
                <Authenticode>MacDeveloperHarden</Authenticode>
                </FilesToSign>",
            });
        }

        [MacOSOnlyFact]
        public void SignAndNotarizePkgFile()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.pkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Set up the cert to allow for signing and notarization.
            var additionalCertificateInfo = new Dictionary<string, List<AdditionalCertificateInformation>>()
            {
                {  "MacDeveloperHardenWithNotarization",
                    new List<AdditionalCertificateInformation>() {
                        new AdditionalCertificateInformation() { MacNotarizationAppName = "dotnet", MacSigningOperation = "MacDeveloperHarden" }
                    } 
                }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("test.pkg"), "MacDeveloperHardenWithNotarization" }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'Nested.NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'Nested.SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'NestedPkg.pkg' Certificate='MacDeveloperHarden'",
                "File 'test.pkg' Certificate='MacDeveloperHarden' NotarizationAppName='com.microsoft.dotnet'",
            }, additionalCertificateInfo: additionalCertificateInfo);

            // OSX files need to be zipped first before being signed
            // This is why the .pkgs are listed as .zip files below
            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "Payload/SOS.NETCore.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "Payload/NativeLibrary.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "5", "Payload/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.SOS.NETCore.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "6", "Payload/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                ",
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "NestedPkg.pkg.zip"))}"">
                <Authenticode>MacDeveloperHarden</Authenticode>
                </FilesToSign>",
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.pkg.zip"))}"">
                <Authenticode>MacDeveloperHarden</Authenticode>
                </FilesToSign>
                ",
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.pkg.zip"))}"">
                <Authenticode>8020</Authenticode>
                <MacAppName>com.microsoft.dotnet</MacAppName>
                </FilesToSign>",
            }, additionalCertificateInfo: additionalCertificateInfo);
        }

        [MacOSOnlyFact]
        public void SignNestedPkgFile()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign( GetResourcePath("NestedPkg.pkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'Nested.SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'Nested.NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'NestedPkg.pkg' Certificate='MacDeveloperHarden'",
            });

            // OSX files need to be zipped first before being signed
            // This is why the .pkgs and .apps are listed as .zip files below
            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "Payload/SOS.NETCore.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "Payload/NativeLibrary.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "Payload/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.SOS.NETCore.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "5", "Payload/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll"))}"">
                <Authenticode>Microsoft400</Authenticode>
                </FilesToSign>
                ",
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "NestedPkg.pkg.zip"))}"">
                <Authenticode>MacDeveloperHarden</Authenticode>
                </FilesToSign>"
            });
        }

        [MacOSOnlyFact]
        public void SignPkgFileWithApp()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign( GetResourcePath("WithApp.pkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            // When .apps are unpacked from .pkgs, they get zipped so they can be signed
            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'libexample.dylib' Certificate='DylibCertificate'",
                "File 'test.app' Certificate='MacDeveloperHarden'",
                "File 'WithApp.pkg' Certificate='MacDeveloperHarden'",
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                // This dylib does not go to a zip file because the cert chosen is DylibCertificate.
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "Contents/Resources/libexample.dylib"))}"">
                <Authenticode>DylibCertificate</Authenticode>
                </FilesToSign>
                ",
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "Payload", "test.app.zip"))}"">
                <Authenticode>MacDeveloperHarden</Authenticode>
                </FilesToSign>
                ",
                $@"
                <FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "WithApp.pkg.zip"))}"">
                <Authenticode>MacDeveloperHarden</Authenticode>
                </FilesToSign>"
            });
        }

        [Fact]
        public void SignTarGZipFile()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.tgz"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'Nested.NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'Nested.SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'test.tgz'",
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "0", "test/NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "test/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
            }*/);

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "0", "test/NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "test/SOS.NETCore.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "test/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "test/this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.SOS.NETCore.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
"
            });
        }

        [Fact]
        public void SymbolsNupkg()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.symbols.nupkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'Nested.NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'Nested.SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'test.symbols.nupkg'",
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "0", "NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
            }*/);

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "0", "NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "SOS.NETCore.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.SOS.NETCore.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
"
            });
        }

        [Fact]
        public void SignedSymbolsNupkg()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.symbols.nupkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();
            var tempFileExtensionSignInfo = s_fileExtensionSignInfo.Where(s => s.Key != ".symbols.nupkg").ToDictionary(e => e.Key, e => e.Value);
            
            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, tempFileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'Nested.NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'Nested.SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'test.symbols.nupkg' Certificate='NuGet'",
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "0", "NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
            }*/);

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "0", "NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "SOS.NETCore.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.NativeLibrary.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "3", "this_is_a_big_folder_name_look/this_is_an_even_more_longer_folder_name/but_this_one_is_ever_longer_than_the_previous_other_two/Nested.SOS.NETCore.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>
"
            });
        }

#if !NETFRAMEWORK
        [Fact]
        public void CheckDebSigning()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>
            {
                new ItemToSign(GetResourcePath("test.deb"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'mscorlib.dll' TargetFramework='.NETCoreApp,Version=v10.0' Certificate='Microsoft400'",
                "File 'data.tar.gz'",
                "File 'test.deb' Certificate='LinuxSign'"
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "./usr/local/bin/mscorlib.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.deb"))}"">
  <Authenticode>LinuxSign</Authenticode>
</FilesToSign>"
            });

            var expectedFilesOriginalHashes = new (string, string)[]
            {
                ("usr/local/bin/hello", "644981BBD6F4ED1B3CF68CD0F47981AA"),
                ("usr/local/bin/mscorlib.dll", "B80EEBA2B8616B7C37E49B004D69BBB7")
            };
            string[] signableFiles = ["usr/local/bin/mscorlib.dll"];
            string expectedControlFileContent = "Package: test\nVersion: 1.0\nSection: base\nPriority: optional\nArchitecture: all\n";
            expectedControlFileContent +="Maintainer: Arcade <test@example.com>\nInstalled-Size: 49697\nDescription: A simple test package\n This is a simple generated .deb package for testing purposes.\n";

            ValidateProducedDebContent(Path.Combine(_tmpDir, "test.deb"), expectedFilesOriginalHashes, signableFiles, expectedControlFileContent);
        }

        [WindowsOnlyFact]
        public void CheckRpmSigningOnWindows()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>
            {
                new ItemToSign(GetResourcePath("test.rpm"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'test.rpm' Certificate='LinuxSign'"
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.rpm"))}"">
  <Authenticode>LinuxSign</Authenticode>
</FilesToSign>"
            });
        }

        [LinuxOnlyFact]
        public void CheckRpmSigning()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>
            {
                new ItemToSign(GetResourcePath("test.rpm"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'mscorlib.dll' TargetFramework='.NETCoreApp,Version=v10.0' Certificate='Microsoft400'",
                "File 'test.rpm' Certificate='LinuxSign'"
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "./usr/local/bin/mscorlib.dll"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.rpm"))}"">
  <Authenticode>LinuxSign</Authenticode>
</FilesToSign>"
            });

            var expectedFilesOriginalHashes = new (string, string)[]
            {
                ("usr/local/bin/hello", "644981BBD6F4ED1B3CF68CD0F47981AA"),
                ("usr/local/bin/mscorlib.dll", "B80EEBA2B8616B7C37E49B004D69BBB7")
            };
            string[] signableFiles = ["usr/local/bin/mscorlib.dll"];
            string originalUncompressedPayloadChecksum = "216c2a99006d2e14d28a40c0f14a63f6462f533e89789a6f294186e0a0aad3fd";

            ValidateProducedRpmContent(Path.Combine(_tmpDir, "test.rpm"), expectedFilesOriginalHashes, signableFiles, originalUncompressedPayloadChecksum);
        }

        [Fact]
        public void VerifyDebIntegrity()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>
            {
                new ItemToSign(GetResourcePath("SignedDeb.deb")),
                new ItemToSign(GetResourcePath("IncorrectlySignedDeb.deb"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            var expectedFilesToBeSigned = new List<string>
            {
                "File 'IncorrectlySignedDeb.deb' Certificate='LinuxSign'"
            };

            // If on windows, both packages will be submitted for signing
            // because the CL verification tool (gpg) is not available on Windows.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                expectedFilesToBeSigned.Add("File 'SignedDeb.deb' Certificate='LinuxSign'");
            }

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, expectedFilesToBeSigned.ToArray());
        }

        [Fact]
        public void VerifyRpmIntegrity()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>
            {
                new ItemToSign(GetResourcePath("SignedRpm.rpm")),
                new ItemToSign(GetResourcePath("IncorrectlySignedRpm.rpm"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            var expectedFilesToBeSigned = new List<string>
            {
                "File 'IncorrectlySignedRpm.rpm' Certificate='LinuxSign'"
            };

            // If on windows, both packages will be submitted for signing
            // because the CL verification tool (gpg) is not available on Windows.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                expectedFilesToBeSigned.Add("File 'SignedRpm.rpm' Certificate='LinuxSign'");
            }

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, expectedFilesToBeSigned.ToArray());
        }
#endif

        [Fact]
        public void CheckPowershellSigning()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("SignedScript.ps1")),
                new ItemToSign(GetResourcePath("UnsignedScript.ps1"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'UnsignedScript.ps1' Certificate='PSCertificate'"
            });
        }

        /* These tests return different results on netcoreapp. ie, we can only truly validate nuget integrity when running on framework.
         * NuGet behaves differently on core vs framework 
         * - https://github.com/NuGet/NuGet.Client/blob/e88a5a03a1b26099f8be225d3ee3a897b2edb1d0/build/common.targets#L18-L25
         */
#if NETFRAMEWORK
        [Fact]
        public void VerifyNupkgIntegrity()
        {
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("SignedPackage.1.0.0.nupkg")),
                new ItemToSign(GetResourcePath("IncorrectlySignedPackage.1.0.0.nupkg"))
            };

            ValidateFileSignInfos(itemsToSign,
                                  new Dictionary<string, List<SignInfo>>(),
                                  new Dictionary<ExplicitCertificateKey, string>(),
                                  s_fileExtensionSignInfo,
                                  new[] { "File 'IncorrectlySignedPackage.1.0.0.nupkg' Certificate='NuGet'" });
        }

        [Fact]
        public void SignNupkgWithUnsignedContents()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("UnsignedContents.nupkg")),
                new ItemToSign(GetResourcePath("FakeSignedContents.nupkg"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'UnsignedScript.ps1' Certificate='PSCertificate'",
                "File 'UnsignedContents.nupkg' Certificate='NuGet'",
                "File 'FakeSignedContents.nupkg' Certificate='NuGet'"
            });
        }
#endif
        [WindowsOnlyFact]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void SignMsiEngine()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("MsiBootstrapper.exe")),
                new ItemToSign(GetResourcePath("MsiBootstrapper.exe.wixpack.zip"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'MsiSetup.msi' Certificate='Microsoft400'",
                "File 'MsiBootstrapper.exe' Certificate='Microsoft400'"
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "MsiBootstrapper.exe")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: 'Copyright (c). All rights reserved.'."
            }*/);

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
{
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "0", "ABCDEFG/MsiSetup.msi"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
 $@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "engines", "0", "MsiBootstrapper.exe-engine.exe"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
 $@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "MsiBootstrapper.exe"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>"
            },
            wixToolsPath: GetWixToolPath());

        }

        [WindowsOnlyFact]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void MsiWithWixpack()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("MsiSetup.msi"), "123"),
                new ItemToSign(GetResourcePath("MsiSetup.msi.wixpack.zip"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'MsiApplication.exe' TargetFramework='.NETFramework,Version=v4.7.2' Certificate='Microsoft400'",
                "File 'MsiSetup.msi' Certificate='Microsoft400'"
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
$@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "0", "ABCDEFG/MsiApplication.exe"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>",
 $@"<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "MsiSetup.msi"))}"">
  <Authenticode>Microsoft400</Authenticode>
</FilesToSign>"
            },
            wixToolsPath: GetWixToolPath());
        }

        /// <summary>
        /// Validate that an invalid wix toolset path causes an error
        /// </summary>
        [WindowsOnlyFact]
        public void BadWixToolsetPath()
        {
            var badPath = Path.Combine(GetWixToolPath(), "badpath");

            var fakeBuildEngine = new FakeBuildEngine(_output);
            var task = new SignToolTask
            {
                BuildEngine = fakeBuildEngine,
                ItemsToSign =  Array.Empty<ITaskItem>(),
                StrongNameSignInfo = Array.Empty<ITaskItem>(),
                FileExtensionSignInfo = Array.Empty<ITaskItem>(),
                LogDir = "LogDir",
                TempDir = "TempDir",
                DryRun = true,
                DotNetPath = CreateTestResource("dotnet.fake"),
                DoStrongNameCheck = false,
                SNBinaryPath = null,
                WixToolsPath = badPath
            };

            task.Execute().Should().BeFalse();
            task.Log.HasLoggedErrors.Should().BeTrue();
            fakeBuildEngine.LogErrorEvents.ForEach(a => a.Message.Should().EndWithEquivalent(" does not exist." ));
        }

        [Fact]
        public void MPackFile()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.mpack"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "3PartySHA2") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'VisualStudio.Mac.Banana.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2'",
                "File 'test.mpack'",
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "VisualStudio.Mac.Banana.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
</FilesToSign>
"
            });
        }

        [Fact]
        public void VsixPackage_DuplicateVsixAfter()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("test.vsix"), "123"),
                new ItemToSign(GetResourcePath("PackageWithRelationships.vsix"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'PackageWithRelationships.vsix' Certificate='VsixSHA2'",
                "File 'test.vsix' Certificate='VsixSHA2'",
            },
            new[]
            {
                $"{Path.Combine(_tmpDir, "ContainerSigning", "6", "PackageWithRelationships.vsix")} -> {Path.Combine(_tmpDir, "PackageWithRelationships.vsix")}"
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "1", "lib/net461/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "lib/netstandard2.0/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "8", "Contents/Common7/IDE/PrivateAssemblies/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "6", "PackageWithRelationships.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>
"
            });
        }

        [Fact]
        public void VsixPackage_WithSpaces()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("TestSpaces.vsix"), "123"),
                new ItemToSign(GetResourcePath("PackageWithRelationships.vsix"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'PackageWithRelationships.vsix' Certificate='VsixSHA2'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",                            
                "File 'TestSpaces.vsix' Certificate='VsixSHA2'"
            },
            new[]
            {
                $"{Path.Combine(_tmpDir, "ContainerSigning", "4", "PackageWithRelationships.vsix")} -> {Path.Combine(_tmpDir, "PackageWithRelationships.vsix")}"
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "6", "Contents/Common7/IDE/PrivateAssemblies/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "10", "Team%20Tools/Dynamic Code Coverage/net461/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "11", "Team%20Tools/Dynamic Code Coverage/netstandard2.0/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "4", "PackageWithRelationships.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "TestSpaces.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>
"
            });
        }

        [Fact]
        public void VsixPackage_DuplicateVsixBefore()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("PackageWithRelationships.vsix")),
                new ItemToSign(GetResourcePath("test.vsix"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'PackageWithRelationships.vsix' Certificate='VsixSHA2'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'test.vsix' Certificate='VsixSHA2'",
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "Contents/Common7/IDE/PrivateAssemblies/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "7", "lib/net461/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "8", "lib/netstandard2.0/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "PackageWithRelationships.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>
"
            });
        }

        [Fact]
        public void VsixPackage_DuplicateVsixBeforeAndAfter()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("PackageWithRelationships.vsix", relativePath: "A"), "123"),
                new ItemToSign(GetResourcePath("test.vsix"), "123"),
                new ItemToSign(GetResourcePath("PackageWithRelationships.vsix", relativePath: "B"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'PackageWithRelationships.vsix' Certificate='VsixSHA2'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'test.vsix' Certificate='VsixSHA2'",
            },
            new[]
            {
                $"{Path.Combine(_tmpDir, "A", "PackageWithRelationships.vsix")} -> {Path.Combine(_tmpDir, "B", "PackageWithRelationships.vsix")}"
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "Contents/Common7/IDE/PrivateAssemblies/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "7", "lib/net461/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "8", "lib/netstandard2.0/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "A", "PackageWithRelationships.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "test.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>
"
            });
        }

        [Fact]
        public void VsixPackageWithRelationships()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("PackageWithRelationships.vsix"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "3PartySHA2", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'PackageWithRelationships.vsix' Certificate='VsixSHA2'"
            });

            ValidateGeneratedProject(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "ContainerSigning", "2", "Contents/Common7/IDE/PrivateAssemblies/ProjectOne.dll"))}"">
  <Authenticode>3PartySHA2</Authenticode>
  <StrongName>ArcadeStrongTest</StrongName>
</FilesToSign>",

$@"
<FilesToSign Include=""{Uri.EscapeDataString(Path.Combine(_tmpDir, "PackageWithRelationships.vsix"))}"">
  <Authenticode>VsixSHA2</Authenticode>
</FilesToSign>"
            });
        }

        [Fact]
        public void ZeroLengthFilesShouldNotBeSigned()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("ZeroLengthPythonFile.py"))
            };
            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();
            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("ZeroLengthPythonFile.py"), "3PartySHA2" }
            };
            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, Array.Empty<string>());
        }

        [Fact]
        public void CheckFileExtensionSignInfo()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(CreateTestResource("dynalib.dylib"), "123"),
                new ItemToSign(CreateTestResource("javascript.js"), "123"),
                new ItemToSign(CreateTestResource("javatest.jar"), "123"),
                new ItemToSign(CreateTestResource("power.ps1"), "123"),
                new ItemToSign(CreateTestResource("powerc.psc1"), "123"),
                new ItemToSign(CreateTestResource("powerd.psd1"), "123"),
                new ItemToSign(CreateTestResource("powerm.psm1"), "123"),
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>();

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'dynalib.dylib' Certificate='DylibCertificate'",
                "File 'javascript.js' Certificate='JSCertificate'",
                "File 'javatest.jar' Certificate='JARCertificate'",
                "File 'power.ps1' Certificate='PSCertificate'",
                "File 'powerc.psc1' Certificate='PSCCertificate'",
                "File 'powerd.psd1' Certificate='PSDCertificate'",
                "File 'powerm.psm1' Certificate='PSMCertificate'",
            });
        }

        [Fact]
        public void ValidateParseFileExtensionEntriesForSameCollisionPriorityIdFails()
        {
            var fileExtensionSignInfo = new List<ITaskItem>();

            // Validate that multiple entries will collide and fail
            fileExtensionSignInfo.Add(new TaskItem(".js", new Dictionary<string, string>
            {
                { "CertificateName", "JSCertificate" },
                { "CollisionPriorityId", "123" }
            }));
            fileExtensionSignInfo.Add(new TaskItem(".js", new Dictionary<string, string>{
                { "CertificateName", "None" },
                { "CollisionPriorityId", "123" }
            }));

            runTask(fileExtensionSignInfo: fileExtensionSignInfo.ToArray()).Should().BeFalse();
        }

        [Fact]
        public void ValidateParseFileExtensionEntriesForDifferentCollisionPriorityIdSucceeds()
        {
            var fileExtensionSignInfo = new List<ITaskItem>();

            // Validate that multiple entries will collide and fail
            fileExtensionSignInfo.Add(new TaskItem(".js", new Dictionary<string, string>
            {
                { "CertificateName", "JSCertificate" },
                { "CollisionPriorityId", "123" }
            }));
            fileExtensionSignInfo.Add(new TaskItem(".js", new Dictionary<string, string>{
                { "CertificateName", "None" }
            }));
            fileExtensionSignInfo.Add(new TaskItem(".js", new Dictionary<string, string>
            {
                { "CertificateName", "JSCertificate" },
                { "CollisionPriorityId", "456" }
            }));

            runTask(fileExtensionSignInfo: fileExtensionSignInfo.ToArray()).Should().BeTrue();
        }

        [Fact]
        public void ValidateParseFileExtensionEntriesForTarGzExtensionPasses()
        {
            var fileExtensionSignInfo = new List<ITaskItem>();

            fileExtensionSignInfo.Add(new TaskItem(".tar.gz", new Dictionary<string, string>
            {
                { "CertificateName", "None" }
            }));

            runTask(fileExtensionSignInfo: fileExtensionSignInfo.ToArray()).Should().BeTrue();
        }

        // Given:
        // - "SameFiles1.zip" contains "Simple1.exe" and "Simple2.exe"
        // - "SameFiles2.zip" contains "Simple1.exe"
        // - "Simple1.exe" and "Simple2.exe" have identical contents
        // This test shows that:
        // - even though Simple1 and Simple2 have identical contents, they are treated as unique files
        // - Simple1 from SameFiles1.zip and Simple1 from SameFiles2.zip are treated as the same files because they have the
        //   same content and the same name
        [Fact]
        public void FilesAreUniqueByName()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("SameFiles1.zip"), "123"),
                new ItemToSign(GetResourcePath("SameFiles2.zip"), "123"),
            };

            ValidateFileSignInfos(itemsToSign, new Dictionary<string, List<SignInfo>>(), new Dictionary<ExplicitCertificateKey, string>(), s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'Simple1.exe' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='Microsoft400'",
                "File 'Simple2.exe' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='Microsoft400'",
                "File 'SameFiles1.zip'",
                "File 'SameFiles2.zip'",
            },
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "0", "Simple1.exe")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "1", "Simple2.exe")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''."
            });
        }

        /// <summary>
        /// This test is intended to validate that the argument parsing which occurs
        /// in the SignToolTask class are properly parsed before they are passed
        /// to sign tool.
        /// </summary>
        [Fact]
        public void ValidateSignToolTaskParsing()
        {
            // List of files to be considered for signing
            var itemsToSign = new ITaskItem[]
            {
                // Unsigned package
                new TaskItem(GetResourcePath("ContainerOne.1.0.0.nupkg"), new Dictionary<string, string>
                {
                    { SignToolConstants.CollisionPriorityId, "123" }
                }),
                // Signed pe file
                new TaskItem(GetResourcePath("SignedLibrary.dll"), new Dictionary<string, string>
                {
                    { SignToolConstants.CollisionPriorityId, "123" }
                })
            };

            var strongNameSignInfo = new ITaskItem[]
            {
                new TaskItem("ArcadeStrongTest", new Dictionary<string, string>
                {
                    { "CertificateName", "3PartySHA2" },
                    { "PublicKeyToken", "581d91ccdfc4ea9c" },
                    { "CollisionPriorityId", "123" }
                })
            };

            // Overriding file signing information
            var fileSignInfo = new ITaskItem[]
            {
                new TaskItem("ProjectOne.dll", new Dictionary<string, string>
                {
                    { "TargetFramework", ".NETStandard,Version=v2.0" },
                    { "CertificateName", "OverrideCertificateName" },
                    { "PublicKeyToken", "581d91ccdfc4ea9c" },
                    { "CollisionPriorityId", "123" }
                }),
                new TaskItem("SignedLibrary.dll", new Dictionary<string, string>
                {
                    { "TargetFramework", ".NETCoreApp,Version=v2.0" },
                    { "CertificateName", "DualSignCertificate" },
                    { "PublicKeyToken", "31bf3856ad364e35" },
                    { "CollisionPriorityId", "123" }
                })
            };

            // Enable dual signing for signed library
            var certificatesSignInfo = new ITaskItem[]
            {
                new TaskItem("DualSignCertificate", new Dictionary<string, string>
                {
                    { "DualSigningAllowed", "true" },
                    { "CollisionPriorityId", "123" }
                }),
                new TaskItem("MacDeveloperHardenWithNotarization", new Dictionary<string, string>
                {
                    { "MacCertificate", "MacDeveloperHarden" },
                    { "MacNotarizationAppName", "com.microsoft.dotnet" },
                    { "CollisionPriorityId", "123" }
                })
            };

            var task = new SignToolTask
            {
                BuildEngine = new FakeBuildEngine(_output),
                ItemsToSign = itemsToSign,
                StrongNameSignInfo = strongNameSignInfo,
                FileExtensionSignInfo = s_fileExtensionSignInfoPostBuild,
                FileSignInfo = fileSignInfo,
                CertificatesSignInfo = certificatesSignInfo,
                LogDir = "LogDir",
                TempDir = "TempDir",
                DryRun = true,
                DotNetPath = CreateTestResource("dotnet.fake"),
                MicroBuildCorePath = "MicroBuildCorePath",
                DoStrongNameCheck = false,
                SNBinaryPath = null,
                TarToolPath = s_tarToolPath,
                PkgToolPath = s_pkgToolPath,
            };

            task.Execute().Should().BeTrue();

            var expected = new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='3PartySHA2' StrongName='ArcadeStrongTest'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='OverrideCertificateName' StrongName='ArcadeStrongTest'",
                "File 'ContainerOne.1.0.0.nupkg' Certificate='NuGet'",
                "File 'SignedLibrary.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='DualSignCertificate'"
            };
            task.ParsedSigningInput.FilesToSign.Select(f => f.ToString()).Should().BeEquivalentTo(expected);
        }

        private bool runTask(ITaskItem[] itemsToSign = null, ITaskItem[] strongNameSignInfo = null, ITaskItem[] fileExtensionSignInfo = null)
        {
            var task = new SignToolTask
            {
                BuildEngine = new FakeBuildEngine(_output),
                ItemsToSign = itemsToSign ?? Array.Empty<ITaskItem>(),
                StrongNameSignInfo = strongNameSignInfo ?? Array.Empty<ITaskItem>(),
                FileExtensionSignInfo = fileExtensionSignInfo ?? Array.Empty<ITaskItem>(),
                LogDir = "LogDir",
                TempDir = "TempDir",
                DryRun = true,
                DotNetPath = CreateTestResource("dotnet.fake"),
                DoStrongNameCheck = false,
                SNBinaryPath = null,
            };

            return task.Execute();
        }

        [Fact]
        public void ValidateAppendingCertificate()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("SignedLibrary.dll")),
            };

            const string dualCertName = "DualCertificateName";
            var additionalCertInfo = new Dictionary<string, List<AdditionalCertificateInformation>>()
            {
                {dualCertName, new List<AdditionalCertificateInformation>(){new AdditionalCertificateInformation() { DualSigningAllowed = true } } },
            };

            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "31bf3856ad364e35", new List<SignInfo>{ new SignInfo(certificate: dualCertName, strongName: null) } }
            };

            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                $"File 'SignedLibrary.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='{dualCertName}'",
            },
            additionalCertificateInfo: additionalCertInfo);
        }

        [Fact]
        public void ValidateCertNotAppendedWithNonMatchingCollisionId()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("SignedLibrary.dll")),
            };

            const string dualCertName = "DualCertificateName";
            var additionalCertInfo = new Dictionary<string, List<AdditionalCertificateInformation>>()
            {
                { dualCertName, new List<AdditionalCertificateInformation>(){new AdditionalCertificateInformation()
                {
                    DualSigningAllowed = true,
                    CollisionPriorityId = "123"
                } } },
            };

            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "31bf3856ad364e35", new List<SignInfo>{ new SignInfo(certificate: dualCertName, strongName: null) } }
            };

            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new string[] { }, additionalCertificateInfo: additionalCertInfo);
        }

        [Fact]
        public void PackageWithZipFile()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign( GetResourcePath("PackageWithZip.nupkg"), "123")
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest", collisionPriorityId: "123") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfoWithCollisionId, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'test.zip'",
                "File 'PackageWithZip.nupkg' Certificate='NuGet'",
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "2", "NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''."
            }*/);
        }

        [Fact]
        public void NestedZipFile()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign( GetResourcePath("NestedZip.zip"))
            };

            // Default signing information
            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo>{ new SignInfo(certificate: "ArcadeCertTest", strongName: "ArcadeStrongTest") } }
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>();

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'NativeLibrary.dll' Certificate='Microsoft400'",
                "File 'SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'InnerZipFile.zip'",
                "File 'Mid.SOS.NETCore.dll' TargetFramework='.NETCoreApp,Version=v1.0' Certificate='Microsoft400'",
                "File 'MidNativeLibrary.dll' Certificate='Microsoft400'",
                "File 'NestedZip.zip'",
            }/*,
            Reenable after https://github.com/dotnet/arcade/issues/10293,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "0", "NativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "4", "MidNativeLibrary.dll")}' with Microsoft certificate 'Microsoft400'. The library is considered 3rd party library due to its copyright: ''."
            }*/);
        }

        [Fact]
        public void SpecificFileSignInfos()
        {
            // List of files to be considered for signing
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(CreateTestResource("test.js"), "123"),
                new ItemToSign(CreateTestResource("test.jar"), "123"),
                new ItemToSign(CreateTestResource("test.ps1"), "123"),
                new ItemToSign(CreateTestResource("test.psd1"), "123"),
                new ItemToSign(CreateTestResource("test.psm1"), "123"),
                new ItemToSign(CreateTestResource("test.psc1"), "123"),
                new ItemToSign(CreateTestResource("test.dylib"), "123"),
                new ItemToSign(GetResourcePath("EmptyPKT.dll"), "123"),
                new ItemToSign(GetResourcePath("test.vsix"), "123"),
                new ItemToSign(GetResourcePath("Simple.nupkg"), "123"),
                // This symbols nupkg has the same hash as Simple.nupkg.
                // It should still get signed with a different signature.
                new ItemToSign(GetResourcePath("Simple.symbols.nupkg"), "123"),
                // A few extra interesting cases. This has no file extension
                new ItemToSign(GetResourcePath("filewithoutextension"), "123"),
                // This will be marked as not having any cert.
                new ItemToSign(GetResourcePath("SPCNoPKT.dll"), "123"),
                // This will be marked to have hardening and notarization
                new ItemToSign(GetResourcePath("Simple.exe"), "1234")
            };

            var strongNameSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { "581d91ccdfc4ea9c", new List<SignInfo> {new SignInfo(certificate: "ArcadeCertTest", strongName: "StrongNameValue", collisionPriorityId: "123") } },
            };

            // Overriding information
            var fileSignInfo = new Dictionary<ExplicitCertificateKey, string>()
            {
                { new ExplicitCertificateKey("test.js", collisionPriorityId: "123"), "JSCertificate" },
                { new ExplicitCertificateKey("test.jar", collisionPriorityId: "123"), "JARCertificate" },
                { new ExplicitCertificateKey("test.ps1", collisionPriorityId: "123"), "PS1Certificate" },
                { new ExplicitCertificateKey("test.psd1", collisionPriorityId: "123"), "PSD1Certificate" },
                { new ExplicitCertificateKey("test.psm1", collisionPriorityId: "123"), "PSM1Certificate" },
                { new ExplicitCertificateKey("test.psc1", collisionPriorityId: "123"), "PSC1Certificate" },
                { new ExplicitCertificateKey("test.dylib", collisionPriorityId: "123"), "DYLIBCertificate" },
                { new ExplicitCertificateKey("EmptyPKT.dll", collisionPriorityId: "123"), "DLLCertificate" },
                { new ExplicitCertificateKey("test.vsix", collisionPriorityId: "123"), "VSIXCertificate" },
                { new ExplicitCertificateKey("PackageWithRelationships.vsix", collisionPriorityId: "123"), "VSIXCertificate2" },
                { new ExplicitCertificateKey("Simple.dll", collisionPriorityId: "123"), "DLLCertificate2" },
                { new ExplicitCertificateKey("Simple.nupkg", collisionPriorityId: "123"), "NUPKGCertificate" },
                { new ExplicitCertificateKey("Simple.symbols.nupkg", collisionPriorityId: "123"), "NUPKGCertificate2" },
                { new ExplicitCertificateKey("ProjectOne.dll", "581d91ccdfc4ea9c", ".NETFramework,Version=v4.6.1", "123"), "DLLCertificate3" },
                { new ExplicitCertificateKey("ProjectOne.dll", "581d91ccdfc4ea9c", ".NETStandard,Version=v2.0", "123"), "DLLCertificate4" },
                { new ExplicitCertificateKey("ProjectOne.dll", "581d91ccdfc4ea9c", ".NETCoreApp,Version=v2.0", "123"), "DLLCertificate5" },
                { new ExplicitCertificateKey("filewithoutextension", collisionPriorityId: "123"), "MacDeveloperHarden" },
                { new ExplicitCertificateKey("SPCNoPKT.dll", collisionPriorityId: "123"), "None" },
                { new ExplicitCertificateKey("Simple.exe", collisionPriorityId: "1234"), "MacDeveloperHardenWithNotarization" },
            };

            // Set up the cert to allow for signing and notarization.
            var certificatesSignInfo = new Dictionary<string, List<AdditionalCertificateInformation>>()
            {
                {  "MacDeveloperHardenWithNotarization",
                    new List<AdditionalCertificateInformation>() {
                        new AdditionalCertificateInformation() { MacNotarizationAppName = "dotnet", MacSigningOperation = "MacDeveloperHarden" }
                    }
                }
            };

            ValidateFileSignInfos(itemsToSign, strongNameSignInfo, fileSignInfo, s_fileExtensionSignInfo, new[]
            {
                "File 'test.js' Certificate='JSCertificate'",
                "File 'test.jar' Certificate='JARCertificate'",
                "File 'test.ps1' Certificate='PS1Certificate'",
                "File 'test.psd1' Certificate='PSD1Certificate'",
                "File 'test.psm1' Certificate='PSM1Certificate'",
                "File 'test.psc1' Certificate='PSC1Certificate'",
                "File 'test.dylib' Certificate='DYLIBCertificate'",
                "File 'EmptyPKT.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='DLLCertificate'",
                "File 'ProjectOne.dll' TargetFramework='.NETFramework,Version=v4.6.1' Certificate='DLLCertificate3' StrongName='StrongNameValue'",
                "File 'ProjectOne.dll' TargetFramework='.NETStandard,Version=v2.0' Certificate='DLLCertificate4' StrongName='StrongNameValue'",
                "File 'ProjectOne.dll' TargetFramework='.NETCoreApp,Version=v2.0' Certificate='DLLCertificate5' StrongName='StrongNameValue'",
                "File 'PackageWithRelationships.vsix' Certificate='VSIXCertificate2'",
                "File 'test.vsix' Certificate='VSIXCertificate'",
                "File 'Simple.dll' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='DLLCertificate2'",
                "File 'Simple.nupkg' Certificate='NUPKGCertificate'",
                "File 'Simple.symbols.nupkg' Certificate='NUPKGCertificate2'",
                "File 'filewithoutextension' Certificate='MacDeveloperHarden'",
                "File 'Simple.exe' TargetFramework='.NETCoreApp,Version=v2.1' Certificate='MacDeveloperHarden' NotarizationAppName='dotnet'",
            },
            additionalCertificateInfo: certificatesSignInfo,
            expectedWarnings: new[]
            {
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "EmptyPKT.dll")}' with Microsoft certificate 'DLLCertificate'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "9", "lib/net461/ProjectOne.dll")}' with Microsoft certificate 'DLLCertificate3'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "10", "lib/netstandard2.0/ProjectOne.dll")}' with Microsoft certificate 'DLLCertificate4'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "16", "Contents/Common7/IDE/PrivateAssemblies/ProjectOne.dll")}' with Microsoft certificate 'DLLCertificate5'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "ContainerSigning", "23", "Simple.dll")}' with Microsoft certificate 'DLLCertificate2'. The library is considered 3rd party library due to its copyright: ''.",
                $@"SIGN004: Signing 3rd party library '{Path.Combine(_tmpDir, "Simple.exe")}' with Microsoft certificate 'MacDeveloperHarden'. The library is considered 3rd party library due to its copyright: ''."
            });
        }

        [Theory]
        [MemberData(nameof(GetSignableExtensions))]
        public void MissingCertificateName(string extension)
        {
            var needContent = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { ".dll", "EmptyPKT.dll" },
                { ".vsix", "Simple.vsix" },
                { ".nupkg", "Simple.nupkg" },
                { ".exe", "Simple.exe" },
                { ".deb", "test.deb" }
            };

            var task = new SignToolTask { BuildEngine = new FakeBuildEngine() };

            var inputFilePath = needContent.TryGetValue(extension, out var resourcePath) ?
                GetResourcePath(resourcePath) :
                CreateTestResource("test" + extension);
            
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(inputFilePath)
            };

            new Configuration(_tmpDir,
                itemsToSign,
                new Dictionary<string, List<SignInfo>>(),
                new Dictionary<ExplicitCertificateKey, string>(),
                new Dictionary<string, List<SignInfo>>(),
                new(),
                tarToolPath: s_tarToolPath,
                pkgToolPath: s_pkgToolPath,
                snPath: s_snPath,
                task.Log)
                .GenerateListOfFiles();

            task.Log.HasLoggedErrors.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetSignableExtensions))]
        public void MissingCertificateNameButExtensionIsIgnored(string extension)
        {
            var needContent = new Dictionary<string, (string, string[])>(StringComparer.OrdinalIgnoreCase)
            {
                { ".dll", ("EmptyPKT.dll", []) },
                { ".vsix", ("Simple.vsix", []) },
                { ".nupkg", ("Simple.nupkg", []) },
                { ".exe", ("Simple.exe", []) },
                { ".deb", ("test.deb", [".dll"]) }
            };

            var task = new SignToolTask { BuildEngine = new FakeBuildEngine() };

            needContent.TryGetValue(extension, out (string ResourcePath, string[] AdditionalExtensions) value);
            var inputFilePath = value.ResourcePath != null ?
                GetResourcePath(value.ResourcePath) :
                CreateTestResource("test" + extension);

            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(inputFilePath)
            };

            var extensionSignInfo = new Dictionary<string, List<SignInfo>>()
            {
                { extension, new List<SignInfo> { SignInfo.Ignore } }
            };

            foreach (var additionalExtension in value.AdditionalExtensions ?? [])
            {
                extensionSignInfo.Add(additionalExtension, new List<SignInfo> { SignInfo.Ignore });
            }

            new Configuration(_tmpDir,
                itemsToSign,
                new Dictionary<string, List<SignInfo>>(),
                new Dictionary<ExplicitCertificateKey, string>(),
                extensionSignInfo,
                new(),
                tarToolPath: s_tarToolPath,
                pkgToolPath: s_pkgToolPath,
                snPath: s_snPath,
                task.Log)
                .GenerateListOfFiles();

            task.Log.HasLoggedErrors.Should().BeFalse();
        }

        [Fact]
        public void CrossGeneratedLibraryWithoutPKT()
        {
            var itemsToSign = new List<ItemToSign>()
            {
                new ItemToSign(GetResourcePath("SPCNoPKT.dll"), "123")
            };

            ValidateFileSignInfos(
                itemsToSign, 
                new Dictionary<string, List<SignInfo>>(), 
                new Dictionary<ExplicitCertificateKey, 
                string>(), 
                s_fileExtensionSignInfoWithCollisionId, 
                new string[0]);

            ValidateGeneratedProject(
                itemsToSign, 
                new Dictionary<string, List<SignInfo>>(), 
                new Dictionary<ExplicitCertificateKey, string>(), 
                s_fileExtensionSignInfoWithCollisionId, 
                new string[0]);
        }

        /// <summary>
        /// Verify that running the wixpack returns passing result and that the expected output file
        /// is created, or a negative result if the wix tool fails.
        /// </summary>
        [WindowsOnlyTheory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void RunWixToolRunsOrFailsProperly(bool deleteWixobjBeforeRunningTool)
        {
            var task = new SignToolTask { BuildEngine = new FakeBuildEngine() };

            const string expectedExe = "MsiBootstrapper.exe";
            const string wixPack = "MsiBootstrapper.exe.wixpack.zip";
            var wixToolsPath = GetWixToolPath();
            var wixpackPath = GetResourcePath(wixPack);
            var tempDir = Path.GetTempPath();
            string workingDir = Path.Combine(tempDir, "extract", Guid.NewGuid().ToString());
            string outputDir = Path.Combine(tempDir, "output", Guid.NewGuid().ToString());
            string createFileName = Path.Combine(workingDir, "create.cmd");
            string outputFileName = Path.Combine(outputDir, expectedExe);
            Directory.CreateDirectory(outputDir);

            try
            {
                // Unzip the wixpack zip, run the tool, and check the exit code
                ZipFile.ExtractToDirectory(wixpackPath, workingDir);

                if (deleteWixobjBeforeRunningTool)
                {
                    File.Delete(Path.Combine(workingDir, "Bundle.wixobj"));
                }

                BatchSignUtil.RunWixTool(createFileName, outputDir, workingDir, wixToolsPath, task.Log).Should().Be(!deleteWixobjBeforeRunningTool);
                File.Exists(outputFileName).Should().Be(!deleteWixobjBeforeRunningTool);
            }
            finally
            {
                Directory.Delete(workingDir, true);
                Directory.Delete(outputDir, true);
            }
        }

        /// <summary>
        /// Run a wix tool, but with an empty wix path.
        /// </summary>
        [Fact]
        public void RunWixToolThrowsErrorIfNoWixToolsProvided()
        {
            var fakeBuildEngine = new FakeBuildEngine();
            var task = new SignToolTask { BuildEngine = fakeBuildEngine };

            BatchSignUtil.RunWixTool("create.cmd", "foodir", "bardir", null, task.Log).Should().BeFalse();
            task.Log.HasLoggedErrors.Should().BeTrue();
            fakeBuildEngine.LogErrorEvents.Should().Contain(e => e.Message.Contains("WixToolsPath must be defined to run WiX tooling"));
        }

        /// <summary>
        /// If attempting to repack a wix container, but a wix path was not
        /// provided
        /// </summary>
        [Fact]
        public void RunWixToolThrowsErrorIfWixToolsProvidedButDirDoesNotExist()
        {
            const string totalWixToolDir = "totally/wix/tools";
            var fakeBuildEngine = new FakeBuildEngine();
            var task = new SignToolTask { BuildEngine = fakeBuildEngine };

            BatchSignUtil.RunWixTool("create.cmd", "foodir", "bardir", "totally/wix/tools", task.Log).Should().BeFalse();
            task.Log.HasLoggedErrors.Should().BeTrue();
            fakeBuildEngine.LogErrorEvents.Should().Contain(e => e.Message.Contains($"WixToolsPath '{totalWixToolDir}' not found."));
        }

        [Fact]
        public void MissingStrongNameSignaturesDoNotValidate()
        {
            StrongNameHelper.IsSigned(GetResourcePath("AspNetCoreCrossLib.dll")).Should().BeFalse();
            StrongNameHelper.IsSigned(GetResourcePath("CoreLibCrossARM.dll")).Should().BeFalse();
            StrongNameHelper.IsSigned(GetResourcePath("EmptyPKT.dll")).Should().BeFalse();
            StrongNameHelper.IsSigned(GetResourcePath("DelaySigned.dll")).Should().BeFalse();
            StrongNameHelper.IsSigned(GetResourcePath("ProjectOne.dll")).Should().BeFalse();
        }

        /// <summary>
        /// Add one to a byte in the input stream and write to the output stream. Both streams are left open.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="offset"></param>
        private void FlipBit(Stream inputStream, Stream outputStream, int offset, byte flipz)
        {
            using BinaryReader reader = new BinaryReader(inputStream, System.Text.Encoding.Default, true);
            using BinaryWriter writer = new BinaryWriter(outputStream, System.Text.Encoding.Default, true);

            // Read up to the checksum and write to the binary
            var bytesBeforeOffset = reader.ReadBytes(offset);
            writer.Write(bytesBeforeOffset);

            // Toggle a bit and write it out
            // Cast to byte explicitly to avoid writing an int.
            byte b = reader.ReadByte();
            byte f = (byte)(b ^ flipz);
            writer.Write(f);

            // Then write the read
            var bytesAfterChecksum = reader.ReadBytes((int)inputStream.Length - offset - 1);
            writer.Write(bytesAfterChecksum);

            outputStream.Position = 0;
            inputStream.Position = 0;
        }

        private void WriteBytesToLocation(Stream inputStream, Stream outputStream, int offset, uint bytez)
        {
            using BinaryReader reader = new BinaryReader(inputStream, System.Text.Encoding.Default, true);
            using BinaryWriter writer = new BinaryWriter(outputStream, System.Text.Encoding.Default, true);

            // Read up to the checksum and write to the binary
            var bytesBeforeOffset = reader.ReadBytes(offset);
            writer.Write(bytesBeforeOffset);

            // Toggle a bit and write it out
            // Cast to byte explicitly to avoid writing an int.
            writer.Write(bytez);
            // Advance the reader.
            reader.ReadUInt32();

            // Then write the read
            var bytesAfterChecksum = reader.ReadBytes((int)inputStream.Length - offset - sizeof(uint));
            writer.Write(bytesAfterChecksum);

            outputStream.Position = 0;
            inputStream.Position = 0;
        }

        /// <summary>
        /// Verify that flipbit works properly by flipping twice.
        /// </summary>
        [Fact]
        public void NoFlipButWriteShouldVerify()
        {
            // We're going to open the file and flip a bit in the checksum
            using var inputStream = File.OpenRead(GetResourcePath("SignedLibrary.dll"));
            using MemoryStream outputStream = new();

            PEHeaders peHeaders = new PEHeaders(inputStream);
            inputStream.Position = 0;
            int checksumStart = peHeaders.PEHeaderStartOffset + Microsoft.DotNet.StrongName.Constants.ChecksumOffsetInPEHeader;
            WriteBytesToLocation(inputStream, outputStream, checksumStart, peHeaders.PEHeader.CheckSum);
            StrongNameHelper.IsSigned(outputStream).Should().BeTrue();
        }

        [Fact]
        public void IncorrectChecksumsDoNotValidate()
        {
            // We're going to open the file and flip a bit in the checksum
            using var inputStream = File.OpenRead(GetResourcePath("SignedLibrary.dll"));
            using MemoryStream outputStream = new();

            PEHeaders peHeaders = new PEHeaders(inputStream);
            inputStream.Position = 0;
            int checksumStart = peHeaders.PEHeaderStartOffset + Microsoft.DotNet.StrongName.Constants.ChecksumOffsetInPEHeader;
            WriteBytesToLocation(inputStream, outputStream, checksumStart, peHeaders.PEHeader.CheckSum ^ 0x1);
            StrongNameHelper.IsSigned(outputStream).Should().BeFalse();
        }

        // This binary has had a resource added after it was strong name. This invalidated the checksum too,
        // so we write the expected checksum.
        [Fact]
        public void InvalidatedSNSignatureDoesNotValidate()
        {
            using var inputStream = File.OpenRead(GetResourcePath("InvalidatedStrongName.dll"));
            using MemoryStream outputStream = new();

            PEHeaders peHeaders = new PEHeaders(inputStream);
            inputStream.Position = 0;

            int checksumStart = peHeaders.PEHeaderStartOffset + Microsoft.DotNet.StrongName.Constants.ChecksumOffsetInPEHeader;
            // Write the checksum that would be expected after editing the binary.
            WriteBytesToLocation(inputStream, outputStream, checksumStart, 110286);

            StrongNameHelper.IsSigned(outputStream).Should().BeFalse();
        }

        [Fact]
        public void ValidStrongNameSignaturesValidate()
        {
            StrongNameHelper.IsSigned(GetResourcePath("SignedLibrary.dll")).Should().BeTrue();
            StrongNameHelper.IsSigned(GetResourcePath("StrongNamedWithEcmaKey.dll")).Should().BeTrue();
        }

        [WindowsOnlyFact]
        public void ValidStrongNameSignaturesValidateWithFallback()
        {
            StrongNameHelper.IsSigned_Legacy(GetResourcePath("SignedLibrary.dll"), s_snPath).Should().BeTrue();
            StrongNameHelper.IsSigned_Legacy(GetResourcePath("StrongNamedWithEcmaKey.dll"), s_snPath).Should().BeTrue();
        }

        [Theory]
        [InlineData("OpenSigned.dll", "OpenSignedCorrespondingKey.snk", true)]
        [InlineData("DelaySignedWithOpen.dll", "OpenSignedCorrespondingKey.snk", false)]
        public void SigningSignsAsExpected(string file, string key, bool initiallySigned)
        {
            // Make sure this is unique
            string resourcePath = GetResourcePath(file, Guid.NewGuid().ToString());
            StrongNameHelper.IsSigned(resourcePath).Should().Be(initiallySigned);
            StrongNameHelper.Sign(resourcePath, GetResourcePath(key));
            StrongNameHelper.IsSigned(resourcePath).Should().BeTrue();

            // Legacy sn verification works on on Windows only
            StrongNameHelper.IsSigned_Legacy(resourcePath, s_snPath).Should().Be(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        }

        [WindowsOnlyTheory]
        [InlineData("OpenSigned.dll", "OpenSignedCorrespondingKey.snk", true)]
        [InlineData("DelaySignedWithOpen.dll", "OpenSignedCorrespondingKey.snk", false)]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void SigningSignsAsExpectedWithLegacyAndVerifiesWithNonLegacy(string file, string key, bool initiallySigned)
        {
            // Make sure this is unique
            string resourcePath = GetResourcePath(file, Guid.NewGuid().ToString());
            StrongNameHelper.IsSigned_Legacy(resourcePath, s_snPath).Should().Be(initiallySigned);
            // Unset the strong name bit first
            StrongNameHelper.ClearStrongNameSignedBit(resourcePath);
            StrongNameHelper.Sign_Legacy(resourcePath, GetResourcePath(key), s_snPath).Should().BeTrue();
            StrongNameHelper.IsSigned(resourcePath).Should().BeTrue();
        }

        [Fact]
        public void CannotSignWithTheEcmaKey()
        {
            // Using stream variant so that legacy fallback doesn't swallow the exception.
            using (var inputStream = File.OpenRead(GetResourcePath("StrongNamedWithEcmaKey.dll")))
            {
                Action shouldFail = () => StrongNameHelper.Sign(inputStream, GetResourcePath("OpenSignedCorrespondingKey.snk"));
                shouldFail.Should().Throw<NotImplementedException>();
            }
        }

        [Fact]
        public void DelaySignedBinaryFailsToSignWithDifferentKey()
        {
            // Using stream variant so that legacy fallback doesn't swallow the exception.
            using (var inputStream = File.OpenRead(GetResourcePath("DelaySigned.dll")))
            {
                Action shouldFail = () => StrongNameHelper.Sign(inputStream, GetResourcePath("OpenSignedCorrespondingKey.snk"));
                shouldFail.Should().Throw<InvalidOperationException>();
            }
        }
    }
}
