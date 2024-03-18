// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Microsoft.DotNet.VersionTools.Tests.BuildManifest
{
    public class ManifestModelTests
    {
        private readonly ITestOutputHelper _output;

        public ManifestModelTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Given a set of file sign extension infos,
        /// ToXml should throw with an appropriate error message if they are invalid 
        /// </summary>
        [Theory]
        [InlineData(null, ".ps1", "foocert")]
        [InlineData(null, ".ps1", "foocert", ".PS1", "foocert")]
        [InlineData(null, ".bar", "foocert", ".PS1", "foocert")]
        [InlineData(null, ".ps1", "FOOCERT", ".PS1", "foocert")]
        [InlineData(typeof(ArgumentException), ".ps1", "FOOCERT", ".ps1", "barcert")] // Conflict
        [InlineData(typeof(ArgumentException), ".ps1", "FOOCERT", ".PS1", "barcert")] // Conflict
        [InlineData(typeof(ArgumentException), "foo.ps1", "FOOCERT", ".PS1", "barcert")] // Conflict
        [InlineData(typeof(ArgumentException), ".ps1", "")] // Can't be empty
        [InlineData(typeof(ArgumentException), "", "bar")] // Can't be empty
        public void ManifestModelToXmlValidatesFileExtensionSignInfos(Type exceptionType, params string[] infos)
        {
            if (infos.Length % 2 != 0)
            {
                throw new ArgumentException();
            }

            List<FileExtensionSignInfoModel> models = new List<FileExtensionSignInfoModel>();

            // Include is first arg, cert name is second
            // InlineData can't pass tuple types so using this instead.
            for (int i = 0; i < infos.Length / 2; i++)
            {
                models.Add(new FileExtensionSignInfoModel() { Include = infos[i * 2], CertificateName = infos[i * 2 + 1] });
            }

            SigningInformationModel signInfo = new SigningInformationModel()
            {
                FileExtensionSignInfo = models
            };

            VerifyToXml(exceptionType, signInfo);
        }

        private static void VerifyToXml(Type expectedExceptionType, SigningInformationModel signInfo)
        {
            if (expectedExceptionType != null)
            {
                Action act = () => signInfo.ToXml();
                act.Should().Throw<Exception>().And.Should().BeOfType(expectedExceptionType);
            }
            else
            {
                signInfo.ToXml().Should().NotBeNull();
            }
        }

        /// <summary>
        /// Given a set of file sign extension infos,
        /// Parse should throw with an appropriate error message if they are invalid.
        /// </summary>
        [Theory]
        [InlineData(null, ".ps1", "foocert")]
        [InlineData(null, ".ps1", "foocert", ".PS1", "foocert")]
        [InlineData(null, ".bar", "foocert", ".PS1", "foocert")]
        [InlineData(null, ".ps1", "FOOCERT", ".PS1", "foocert")]
        [InlineData(typeof(ArgumentException), ".ps1", "FOOCERT", ".ps1", "barcert")] // Conflict
        [InlineData(typeof(ArgumentException), ".ps1", "FOOCERT", ".PS1", "barcert")] // Conflict
        [InlineData(typeof(ArgumentException), "foo.ps1", "FOOCERT", ".PS1", "barcert")] // Conflict
        [InlineData(typeof(ArgumentException), ".ps1", "")] // Can't be empty
        [InlineData(typeof(ArgumentException), "", "bar")] // Can't be empty
        public void ManifestModelFromXmlValidatesFileExtensionSignInfos(Type exceptionType, params string[] infos)
        {
            if (infos.Length % 2 != 0)
            {
                throw new ArgumentException();
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<SigningInformation>");

            // Include is first arg, cert name is second
            // InlineData can't pass tuple types so using this instead.
            for (int i = 0; i < infos.Length / 2; i++)
            {
                builder.AppendLine($"<FileExtensionSignInfo Include=\"{infos[i * 2]}\" CertificateName=\"{infos[i * 2 + 1]}\" />");
            }

            builder.AppendLine("</SigningInformation>");

            VerifyFromXml(exceptionType, builder);
        }

        private static void VerifyFromXml(Type expectedExceptionType, StringBuilder builder)
        {
            if (expectedExceptionType != null)
            {
                Action act = () => SigningInformationModel.Parse(XElement.Parse(builder.ToString()));
                act.Should().Throw<Exception>().And.Should().BeOfType(expectedExceptionType);
            }
            else
            {
                SigningInformationModel.Parse(XElement.Parse(builder.ToString())).Should().NotBeNull();
            }
        }

        /// <summary>
        /// Given a set of explicit file sign infos that conflict,
        /// ToXml should throw with an appropriate error message.
        /// 
        /// param order is Include CertificateName TargetFramework PublicKeyToken
        /// </summary>
        [Theory]
        [InlineData(null, "bar.bat", "foocert", null, null)]
        [InlineData(typeof(ArgumentException), "foo/bar.bat", "foocert", null, null)] // Invalid file name
        [InlineData(typeof(ArgumentException), "foo\\bar.bat", "foocert", null, null)] // Invalid file name
        [InlineData(typeof(ArgumentException), "bar.bat", "", null, null)] // Empty cert
        [InlineData(typeof(ArgumentException), "bar.bat", null, null, null)] // Null cert
        [InlineData(typeof(ArgumentException), "bar.bat", "foocert", "net5", null)] // Invalid tfm
        [InlineData(typeof(ArgumentException), "bar.bat", "foocert", "net5.0", "zzz")] // Invalid pkt
        [InlineData(null, "bar.bat", "foocert", null, null, "bar.bat2", "foocert", null, null)]
        [InlineData(typeof(ArgumentException), "bar.bat", "foocert2", null, null, "bar.bat", "foocert", null, null)]
        [InlineData(null, "bar.bat", "foocert2", ".NETCoreApp,Version=v5.0", null, "bar.bat", "foocert", ".NETCoreApp,Version=v3.1", null)]
        [InlineData(null, "bar.bat", "foocert2", ".NETCoreApp,Version=v1.0", "aaaaaaaaaaaaaaaa", "bar.bat", "foocert", ".NETCoreApp,Version=v1.0", "aaaaaaaaaaaaaaab")]
        [InlineData(typeof(ArgumentException), "bar.bat", "foocert2", ".NETCoreApp,Version=v1.0", "aaaaaaaaaaaaaaaa", "bar.bat", "foocert", ".NETCoreApp,Version=v1.0", "aaaaaaaaaaaaaaaa")]
        public void ManifestModelToXmlValidatesFileSignInfos(Type exceptionType, params string[] infos)
        {
            if (infos.Length % 4 != 0)
            {
                throw new ArgumentException();
            }

            List<FileSignInfoModel> models = new List<FileSignInfoModel>();

            for (int i = 0; i < infos.Length / 4; i++)
            {
                models.Add(new FileSignInfoModel()
                    {
                        Include = infos[i * 4],
                        CertificateName = infos[i * 4 + 1],
                        TargetFramework = infos[i * 4 + 2],
                        PublicKeyToken = infos[i * 4 + 3],
                });
            }

            SigningInformationModel signInfo = new SigningInformationModel()
            {
                FileSignInfo = models
            };

            VerifyToXml(exceptionType, signInfo);
        }

        /// <summary>
        /// Per issue: dotnet/arcade#7064
        /// </summary>
        [Fact]
        public void ValidateSymreaderFileSignInfos()
        {
            List<FileSignInfoModel> models = new List<FileSignInfoModel>();            
            models.Add(new FileSignInfoModel()
            {
                Include = "Microsoft.DiaSymReader.dll",
                CertificateName = "MicrosoftWin8WinBlue",
                TargetFramework = ".NETFramework,Version=v2.0",
                PublicKeyToken = "31bf3856ad364e35",
            });
            models.Add(new FileSignInfoModel()
            {
                Include = "Microsoft.DiaSymReader.dll",
                CertificateName = "Microsoft101240624", // lgtm [cs/common-default-passwords] Safe, these are certificate names
                TargetFramework = ".NETStandard,Version=v1.1",
                PublicKeyToken = "31bf3856ad364e35",
            });

            SigningInformationModel signInfo = new SigningInformationModel()
            {
                FileSignInfo = models
            };

            signInfo.ToXml().Should().NotBeNull();
        }

        /// <summary>
        /// Given a set of explicit file sign infos that or are invalid,
        /// Parse should throw with an appropriate error message if they are invalid.
        /// 
        /// param order is Include CertificateName TargetFramework PublicKeyToken
        [Theory]
        [InlineData(null, "bar.bat", "foocert", null, null)]
        [InlineData(typeof(ArgumentException), "foo/bar.bat", "foocert", null, null)] // Invalid file name
        [InlineData(typeof(ArgumentException), "foo\\bar.bat", "foocert", null, null)] // Invalid file name
        [InlineData(typeof(ArgumentException), "bar.bat", "", null, null)] // Empty cert
        [InlineData(typeof(ArgumentException), "bar.bat", null, null, null)] // Null cert
        [InlineData(typeof(ArgumentException), "bar.bat", "foocert", "net5", null)] // Invalid tfm
        [InlineData(typeof(ArgumentException), "bar.bat", "foocert", "net5.0", "zzz")] // Invalid pkt
        [InlineData(null, "bar.bat", "foocert", null, null, "bar.bat2", "foocert", null, null)]
        [InlineData(typeof(ArgumentException), "bar.bat", "foocert2", null, null, "bar.bat", "foocert", null, null)]
        [InlineData(null, "bar.bat", "foocert2", ".NETCoreApp,Version=v5.0", null, "bar.bat", "foocert", ".NETCoreApp,Version=v3.1", null)]
        [InlineData(null, "bar.bat", "foocert2", ".NETCoreApp,Version=v1.0", "aaaaaaaaaaaaaaaa", "bar.bat", "foocert", ".NETCoreApp,Version=v1.0", "aaaaaaaaaaaaaaab")]
        [InlineData(typeof(ArgumentException), "bar.bat", "foocert2", ".NETCoreApp,Version=v1.0", "aaaaaaaaaaaaaaaa", "bar.bat", "foocert", ".NETCoreApp,Version=v1.0", "aaaaaaaaaaaaaaaa")]
        public void ManifestModelFromXmlValidatesFileSignInfos(Type exceptionType, params string[] infos)
        {
            if (infos.Length % 4 != 0)
            {
                throw new ArgumentException();
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<SigningInformation>");

            List<FileSignInfoModel> models = new List<FileSignInfoModel>();

            for (int i = 0; i < infos.Length / 4; i++)
            {
                string targetFramework = infos[i * 4 + 2] != null ? $"TargetFramework=\"{infos[i * 4 + 2]}\"" : "";
                string publicKeyToken = infos[i * 4 + 3] != null ? $"PublicKeyToken=\"{infos[i * 4 + 3]}\"" : "";
                builder.AppendLine($"<FileSignInfo Include=\"{infos[i * 4]}\" CertificateName=\"{infos[i * 4 + 1]}\" {targetFramework} {publicKeyToken} />");
            }

            builder.AppendLine("</SigningInformation>");

            VerifyFromXml(exceptionType, builder);
        }

        /// <summary>
        /// Given a set of certificate sign infos that conflict or are invalid,
        /// ToXml should throw with an appropriate error message.
        /// 
        /// param order is CertificateName DualSigningAllowed
        /// </summary>
        [Theory]
        [InlineData(null, "foocert", "true")]
        [InlineData(null, "foocert", "false")]
        [InlineData(typeof(ArgumentException), "foocert", "true", "foocert", "false")]
        [InlineData(null, "foocert", "false", "foocert2", "false")]
        public void ManifestModelToXmlValidatesCertificateSignInfo(Type exceptionType, params string[] infos)
        {
            if (infos.Length % 2 != 0)
            {
                throw new ArgumentException();
            }

            List<CertificatesSignInfoModel> models = new List<CertificatesSignInfoModel>();

            for (int i = 0; i < infos.Length / 2; i++)
            {
                models.Add(new CertificatesSignInfoModel()
                {
                    Include = infos[i * 2],
                    DualSigningAllowed = bool.Parse(infos[i * 2 + 1])
                });
            }

            SigningInformationModel signInfo = new SigningInformationModel()
            {
                CertificatesSignInfo = models
            };

            VerifyToXml(exceptionType, signInfo);
        }

        /// <summary>
        /// Given a set of certificate sign infos that conflict or are invalid,
        /// Parse should throw with an appropriate error message if they are invalid.
        /// 
        /// param order is Include DualSigningAllowed
        [Theory]
        [InlineData(null, "foocert", "true")]
        [InlineData(null, "foocert", "false")]
        [InlineData(typeof(ArgumentException), "", "true")] // No cert
        [InlineData(typeof(ArgumentException), "", "false")] // No cert
        [InlineData(typeof(FormatException), "foocert", "FORKS")] // Invalid bool
        [InlineData(typeof(FormatException), "foocert", "")] // No dual signing allowed param
        [InlineData(typeof(ArgumentException), "foocert", "true", "foocert", "false")]
        [InlineData(null, "foocert", "false", "foocert2", "false")]
        public void ManifestModelFromXmlValidatesCertificateSignInfo(Type exceptionType, params string[] infos)
        {
            if (infos.Length % 2 != 0)
            {
                throw new ArgumentException();
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<SigningInformation>");

            List<FileSignInfoModel> models = new List<FileSignInfoModel>();

            for (int i = 0; i < infos.Length / 2; i++)
            {
                builder.AppendLine($"<CertificatesSignInfo Include=\"{infos[i * 2]}\" DualSigningAllowed=\"{infos[i * 2 + 1]}\" />");
            }

            builder.AppendLine("</SigningInformation>");

            VerifyFromXml(exceptionType, builder);
        }

        /// <summary>
        /// Given a set of strong name sign that conflict or are invalid,
        /// ToXml should throw with an appropriate error message.
        /// 
        /// param order is Include (strong name) PublicKeyToken CertificateName
        /// </summary>
        [Theory]
        [InlineData(null, "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert")] // Valid
        [InlineData(typeof(ArgumentException), "MyStrongName", "", "Mycert")] // Invalid strong name key
        [InlineData(typeof(ArgumentException), "MyStrongName", "aaaaaaaaaaaaaaaa", "")] // Invalid cert
        [InlineData(typeof(ArgumentException), "MyStrongName", "aaaaaaaaaaaa", "Mycert")] // Invalid strong name key
        [InlineData(null, "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert", "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert")] // No conflicts
        [InlineData(typeof(ArgumentException), "MyStrongName2", "aaaaaaaaaaaaaaaa", "Mycert", "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert")] // Different strong names
        [InlineData(typeof(ArgumentException), "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert", "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert2")] // Different certs
        [InlineData(null, "MyStrongName", "aaaaaaaaaaaaaaab", "Mycert", "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert2")] // No conflict
        public void ManifestModelToXmlValidatesStrongNameSignInfo(Type exceptionType, params string[] infos)
        {
            if (infos.Length % 3 != 0)
            {
                throw new ArgumentException();
            }

            List<StrongNameSignInfoModel> models = new List<StrongNameSignInfoModel>();

            for (int i = 0; i < infos.Length / 3; i++)
            {
                models.Add(new StrongNameSignInfoModel()
                {
                    Include = infos[i * 3],
                    PublicKeyToken = infos[i * 3 + 1],
                    CertificateName = infos[i * 3 + 2]
                });
            }

            SigningInformationModel signInfo = new SigningInformationModel()
            {
                StrongNameSignInfo = models
            };

            VerifyToXml(exceptionType, signInfo);
        }

        /// <summary>
        /// Given a set of strong name sign that conflict or are invalid,
        /// Parse should throw with an appropriate error message if they are invalid.
        /// 
        /// param order is Include (strong name) PublicKeyToken CertificateName
        [Theory]
        [InlineData(null, "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert")] // Valid
        [InlineData(typeof(ArgumentException), "MyStrongName", "", "Mycert")] // Invalid strong name key
        [InlineData(typeof(ArgumentException), "MyStrongName", "aaaaaaaaaaaaaaaa", "")] // Invalid cert
        [InlineData(typeof(ArgumentException), "MyStrongName", "aaaaaaaaaaaa", "Mycert")] // Invalid strong name key
        [InlineData(null, "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert", "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert")] // No conflicts
        [InlineData(typeof(ArgumentException), "MyStrongName2", "aaaaaaaaaaaaaaaa", "Mycert", "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert")] // Different strong names
        [InlineData(typeof(ArgumentException), "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert", "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert2")] // Different certs
        [InlineData(null, "MyStrongName", "aaaaaaaaaaaaaaab", "Mycert", "MyStrongName", "aaaaaaaaaaaaaaaa", "Mycert2")] // No conflict
        public void ManifestModelFromXmlValidatesStrongNameSignInfo(Type exceptionType, params string[] infos)
        {
            if (infos.Length % 3 != 0)
            {
                throw new ArgumentException();
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<SigningInformation>");

            List<FileSignInfoModel> models = new List<FileSignInfoModel>();

            for (int i = 0; i < infos.Length / 3; i++)
            {
                builder.AppendLine($"<StrongNameSignInfo Include=\"{infos[i * 3]}\" PublicKeyToken=\"{infos[i * 3 + 1]}\" CertificateName=\"{infos[i * 3 + 2]}\" />");
            }

            builder.AppendLine("</SigningInformation>");

            VerifyFromXml(exceptionType, builder);
        }

        [Fact]
        public void TestExampleBuildManifestRoundtrip()
        {
            XElement xml = XElement.Parse(ExampleBuildString);
            var model = BuildModel.Parse(xml);
            XElement modelXml = model.ToXml();

            XNode.DeepEquals(xml, modelXml).Should().BeTrue("Model failed to output the parsed XML.");
        }

        [Fact]
        public void TestExampleOrchestratedBuildManifestRoundtrip()
        {
            XElement xml = XElement.Parse(ExampleOrchestratedBuildString);
            var model = OrchestratedBuildModel.Parse(xml);
            XElement modelXml = model.ToXml();

            XNode.DeepEquals(xml, modelXml).Should().BeTrue("Model failed to output the parsed XML.");
        }

        [Fact]
        public void TestExampleCustomBuildIdentityRoundtrip()
        {
            XElement xml = XElement.Parse(
                @"<Build Name=""Example"" BuildId=""123"" ProductVersion=""1.0.0-preview"" Branch=""master"" Commit=""abcdef"" BlankExtra="""" Extra=""extra-foo"" />");
            var model = BuildModel.Parse(xml);
            XElement modelXml = model.ToXml();

            XNode.DeepEquals(xml, modelXml).Should().BeTrue("Model failed to output the parsed XML.");
        }

        [Fact]
        public void TestPackageOnlyBuildManifest()
        {
            var model = CreatePackageOnlyBuildManifestModel();
            XElement modelXml = model.ToXml();
            XElement xml = XElement.Parse(@"<Build Name=""SimpleBuildManifest"" BuildId=""123""><Package Id=""Foo"" Version=""1.2.3-example"" /></Build>");

            XNode.DeepEquals(xml, modelXml).Should().BeTrue("Model failed to output the parsed XML.");
        }

        [Fact]
        public void TestMergeBuildManifests()
        {
            var orchestratedModel = new OrchestratedBuildModel(new BuildIdentity { Name = "Orchestrated", BuildId = "123" })
            {
                Endpoints = new List<EndpointModel>
                {
                    EndpointModel.CreateOrchestratedBlobFeed("http://example.org")
                }
            };

            orchestratedModel.AddParticipantBuild(CreatePackageOnlyBuildManifestModel());
            orchestratedModel.AddParticipantBuild(BuildModel.Parse(XElement.Parse(ExampleBuildString)));

            XElement modelXml = orchestratedModel.ToXml();
            XElement xml = XElement.Parse(@"
<OrchestratedBuild Name=""Orchestrated"" BuildId=""123"">
  <Endpoint Id=""Orchestrated"" Type=""BlobFeed"" Url=""http://example.org"">
    <Package Id=""Foo"" Version=""1.2.3-example"" />
    <Package Id=""runtime.rhel.6-x64.Microsoft.Private.CoreFx.NETCoreApp"" Version=""4.5.0-preview1-25929-04"" Category=""noship"" />
    <Package Id=""System.Memory"" Version=""4.5.0-preview1-25927-01"" />
    <Blob Id=""symbols/inner/blank-dir-nonshipping"" NonShipping=""false"" />
    <Blob Id=""symbols/runtime.rhel.6-x64.Microsoft.Private.CoreFx.NETCoreApp.4.5.0-preview1-25929-04.symbols.nupkg"" />
    <Blob Id=""symbols/System.ValueTuple.4.5.0-preview1-25929-04.symbols.nupkg"" NonShipping=""true"" />
  </Endpoint>
  <Build Name=""SimpleBuildManifest"" BuildId=""123"" />
  <Build Name=""corefx"" BuildId=""20171129-04"" Branch=""master"" Commit=""defb6d52047cc3d6b5f5d0853b0afdb1512dfbf4"" />
</OrchestratedBuild>");

            XNode.DeepEquals(xml, modelXml).Should().BeTrue("Model failed to output the parsed XML.");
        }

        [Fact]
        public void TestManifestWithSigningInformation()
        {
            var buildModel = CreateSigningInformationBuildManifestModel();

            XElement modelXml = buildModel.ToXml();
            XElement xml = XElement.Parse(ExampleBuildStringWithSigningInformation);

            XNode.DeepEquals(xml, modelXml).Should().BeTrue("Model failed to output the parsed XML.");
        }

        [Fact]
        public void PackageArtifactModelEquals_ReturnsTrueWhenTwoObjectsHaveMatchingAttributes()
        {
            PackageArtifactModel packageArtifact = new PackageArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName",
                Version = "AssetVersion"
            };

            PackageArtifactModel otherPackageArtifact = new PackageArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName",
                Version = "AssetVersion"
            };

            Assert.True(packageArtifact.Equals(otherPackageArtifact));
        }

        [Fact]
        public void PackageArtifactModelEquals_ReturnsFalseWhenTwoObjectsDoNotHaveMatchingAttributes()
        {
            PackageArtifactModel packageArtifact = new PackageArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "Shipping", true.ToString().ToLower() },
                    },
                Id = "AssetName",
                Version = "AssetVersion"
            };

            PackageArtifactModel otherPackageArtifact = new PackageArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName",
                Version = "AssetVersion"
            };

            Assert.False(packageArtifact.Equals(otherPackageArtifact));
        }

        [Fact]
        public void PackageArtifactModelEquals_ReturnsTrueWhenMatchingAttributesAreNull()
        {
            PackageArtifactModel packageArtifact = new PackageArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName",
                Version = null
            };

            PackageArtifactModel otherPackageArtifact = new PackageArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName",
                Version = null
            };

            Assert.True(packageArtifact.Equals(otherPackageArtifact));
        }

        [Fact]
        public void PackageArtifactModelEquals_ReturnsFalseWhenObjectsAreDifferentTypes()
        {
            PackageArtifactModel packageArtifact = new PackageArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName",
                Version = null
            };

            Assert.False(packageArtifact.Equals("thisIsNotAPackageArtifact!"));
        }

        [Fact]
        public void BlobArtifactModelEquals_ReturnsTrueWhenTwoObjectsHaveMatchingAttributes()
        {
            BlobArtifactModel blobArtifact = new BlobArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName"
            };

            BlobArtifactModel otherBlobArtifact = new BlobArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName"
            };

            Assert.True(blobArtifact.Equals(otherBlobArtifact));
        }

        [Fact]
        public void BlobArtifactModelEquals_ReturnsFalseWhenTwoObjectsDoNotHaveMatchingAttributes()
        {
            BlobArtifactModel blobArtifact = new BlobArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "Shipping", true.ToString().ToLower() },
                    },
                Id = "AssetName"
            };

            BlobArtifactModel otherBlobArtifact = new BlobArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = "AssetName"
            };

            Assert.False(blobArtifact.Equals(otherBlobArtifact));
        }

        [Fact]
        public void BlobArtifactModelEquals_ReturnsTrueWhenMatchingAttributesAreNull()
        {
            BlobArtifactModel blobArtifact = new BlobArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = null
            };

            BlobArtifactModel otherBlobArtifact = new BlobArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = null
            };

            Assert.True(blobArtifact.Equals(otherBlobArtifact));
        }

        [Fact]
        public void BlobArtifactModelEquals_ReturnsFalseWhenObjectsAreDifferentTypes()
        {
            BlobArtifactModel blobArtifact = new BlobArtifactModel
            {
                Attributes = new Dictionary<string, string>
                    {
                        { "NonShipping", true.ToString().ToLower() },
                    },
                Id = null
            };

            Assert.False(blobArtifact.Equals("thisIsNotABlobArtifact!"));
        }

        private BuildModel CreatePackageOnlyBuildManifestModel()
        {
            return new BuildModel(new BuildIdentity { Name = "SimpleBuildManifest", BuildId = "123" })
            {
                Artifacts = new ArtifactSet
                {
                    Packages = new List<PackageArtifactModel>
                    {
                        new PackageArtifactModel
                        {
                            Id = "Foo",
                            Version = "1.2.3-example"
                        }
                    }
                }
            };
        }

        private BuildModel CreateSigningInformationBuildManifestModel()
        {
            return new BuildModel(new BuildIdentity { Name = "SigningInformationBuildManifest", BuildId = "123", Branch = "refs/heads/Test", 
                Commit = "test_commit", IsStable = false, PublishingVersion = (PublishingInfraVersion)3 })
            {
                Artifacts = new ArtifactSet
                {
                    Packages = new List<PackageArtifactModel>
                    {
                        new PackageArtifactModel
                        {
                            Id = "TestPackage",
                            Version = "5.0.0",
                        },
                        new PackageArtifactModel
                        {
                            Id = "ArcadeSdkTest",
                            Version = "5.0.0",
                        },
                    },
                    Blobs = new List<BlobArtifactModel>
                    {
                        new BlobArtifactModel
                        {
                            Id = "assets/symbols/test.nupkg",
                        },
                    }
                },
                SigningInformation = new SigningInformationModel
                {
                    FileExtensionSignInfo = new List<FileExtensionSignInfoModel>
                    {
                        new FileExtensionSignInfoModel
                        {
                            Include = ".dll",
                            CertificateName = "Microsoft400", // lgtm [cs/common-default-passwords] Safe, these are certificate names
                        },
                        new FileExtensionSignInfoModel
                        {
                            Include = ".jar",
                            CertificateName = "MicrosoftJARSHA2",
                        },
                        new FileExtensionSignInfoModel
                        {
                            Include = ".nupkg",
                            CertificateName = "NuGet",
                        },
                    },
                    FileSignInfo = new List<FileSignInfoModel>
                    {
                        new FileSignInfoModel
                        {
                            Include = "Dll.dll",
                            CertificateName = "3PartySHA2",
                        },
                        new FileSignInfoModel
                        {
                            Include = "Another.dll",
                            CertificateName = "AnotherCert",
                        },
                    },
                    ItemsToSign = new List<ItemToSignModel>
                    {
                        new ItemToSignModel
                        {
                            Include = "Package1.nupkg",
                        },
                        new ItemToSignModel
                        {
                            Include = "Package2.nupkg",
                        },
                        new ItemToSignModel
                        {
                            Include = "Package3.nupkg",
                        },
                    },
                    StrongNameSignInfo = new List<StrongNameSignInfoModel>
                    {
                        new StrongNameSignInfoModel
                        {
                            Include = "StrongNameTime",
                            PublicKeyToken = "0123456789abcdef",
                            CertificateName = "Microsoft400", // lgtm [cs/common-default-passwords] Safe, these are certificate names
                        },
                        new StrongNameSignInfoModel
                        {
                            Include = "StrongButKindName",
                            PublicKeyToken = "fedcba9876543210",
                            CertificateName = "Microsoft404", // lgtm [cs/common-default-passwords] Safe, these are certificate names
                        },
                    },
                },
            };
        }

        private const string ExampleBuildString = @"
<Build
  Name=""corefx""
  BuildId=""20171129-04""
  Branch=""master""
  Commit=""defb6d52047cc3d6b5f5d0853b0afdb1512dfbf4"">

  <Package Id=""runtime.rhel.6-x64.Microsoft.Private.CoreFx.NETCoreApp"" Version=""4.5.0-preview1-25929-04"" Category=""noship"" />
  <Package Id=""System.Memory"" Version=""4.5.0-preview1-25927-01"" />

  <Blob Id=""symbols/inner/blank-dir-nonshipping"" NonShipping=""false"" />
  <Blob Id=""symbols/runtime.rhel.6-x64.Microsoft.Private.CoreFx.NETCoreApp.4.5.0-preview1-25929-04.symbols.nupkg"" />
  <Blob Id=""symbols/System.ValueTuple.4.5.0-preview1-25929-04.symbols.nupkg"" NonShipping=""true"" />

</Build>";

        private const string ExampleOrchestratedBuildString = @"
<OrchestratedBuild
  Name=""core-setup""
  BuildId=""20171129-02""
  Branch=""master"">

  <Endpoint
    Id=""Orchestrated""
    Type=""BlobFeed""
    Url=""https://dotnetfeed.blob.core.windows.net/orchestrated-aspnet/20171129-02/index.json"">

    <Package Id=""Microsoft.NETCore.App"" Version=""2.1.0-preview1-26001-02"" />
    <Package Id=""Microsoft.NETCore.UniversalWindowsPlatform"" Version=""6.1.0-preview1-25927-01"" NonShipping=""true"" />
    <Package Id=""runtime.rhel.6-x64.Microsoft.Private.CoreFx.NETCoreApp"" Version=""4.5.0-preview1-25929-04"" NonShipping=""true"" />
    <Package Id=""System.Memory"" Version=""4.5.0-preview1-25927-01"" />

    <Blob Id=""orchestration-metadata/manifests/core-setup.xml"" />
    <Blob Id=""orchestration-metadata/manifests/corefx.xml"" />
    <Blob Id=""orchestration-metadata/PackageVersions.props"" />
    <Blob Id=""Runtime/2.1.0-preview1-25929-04/dotnet-runtime-2.1.0-preview1-25929-04-win-x64.msi"" ShipInstaller=""dotnetcli"" />
    <Blob Id=""Runtime/2.1.0-preview1-25929-04/dotnet-runtime-2.1.0-preview1-25929-04-win-x64.msi.sha512"" ShipInstaller=""dotnetclichecksums"" />
    <Blob Id=""symbols/Microsoft.DotNet.PlatformAbstractions.2.1.0-preview1-25929-04.symbols.nupkg"" />
    <Blob Id=""symbols/runtime.rhel.6-x64.Microsoft.Private.CoreFx.NETCoreApp.4.5.0-preview1-25929-04.symbols.nupkg"" />
    <Blob Id=""symbols/System.ValueTuple.4.5.0-preview1-25929-04.symbols.nupkg"" />

  </Endpoint>

  <Build
    Name=""corefx""
    BuildId=""20171129-04""
    Branch=""master""
    Commit=""defb6d52047cc3d6b5f5d0853b0afdb1512dfbf4"" />

  <Build
    Name=""core-setup""
    BuildId=""20171129-04""
    Branch=""master""
    Commit=""152dbe8a4b4e30eee26208ff6a850e9aa73c07f8"" />

</OrchestratedBuild>
";

        private const string ExampleBuildStringWithSigningInformation = @"
<Build PublishingVersion=""3"" Name=""SigningInformationBuildManifest"" BuildId=""123"" Branch=""refs/heads/Test"" Commit=""test_commit"" IsStable=""false"">
  <Package Id=""ArcadeSdkTest"" Version=""5.0.0"" />
  <Package Id=""TestPackage"" Version=""5.0.0"" />
  <Blob Id=""assets/symbols/test.nupkg""/>
  <SigningInformation>
    <FileExtensionSignInfo Include="".dll"" CertificateName=""Microsoft400"" />
    <FileExtensionSignInfo Include="".jar"" CertificateName=""MicrosoftJARSHA2"" />
    <FileExtensionSignInfo Include="".nupkg"" CertificateName=""NuGet"" />
    <FileSignInfo Include=""Another.dll"" CertificateName=""AnotherCert"" />
    <FileSignInfo Include=""Dll.dll"" CertificateName=""3PartySHA2"" />
    <ItemsToSign Include=""Package1.nupkg"" />
    <ItemsToSign Include=""Package2.nupkg"" />
    <ItemsToSign Include=""Package3.nupkg"" />
    <StrongNameSignInfo Include=""StrongButKindName"" PublicKeyToken=""fedcba9876543210"" CertificateName=""Microsoft404"" />
    <StrongNameSignInfo Include=""StrongNameTime"" PublicKeyToken=""0123456789abcdef"" CertificateName=""Microsoft400"" />
  </SigningInformation>
</Build>
";
    }
}
