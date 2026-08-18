// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AwesomeAssertions;
using Microsoft.Build.Utilities.ProjectCreation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Validation.Tests
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    public class RepoTests : IClassFixture<CommonRepoResourcesFixture>
    {
        private const string DotNetCertificate = "MicrosoftDotNet500";
        private const string MicrosoftCertificate = "Microsoft400";
        private CommonRepoResourcesFixture _commonRepoResourcesFixture;

        public RepoTests(CommonRepoResourcesFixture commonResourcesFixture)
        {
            _commonRepoResourcesFixture = commonResourcesFixture;
        }

        /// <summary>
        /// Restore + sign with AllowEmptySignList=true should succeed. This intentionally does not pass
        /// --build: in Arcade's Build.proj, Sign is not part of the solution build targets, so this
        /// exercises the sign path with an empty sign list rather than compiling the repo.
        /// </summary>
        [Fact]
        public async Task RestoreAndSignSucceedsWithAllowEmptySignList()
        {
            using (var builder = new TestRepoBuilder(nameof(RestoreAndSignSucceedsWithAllowEmptySignList), _commonRepoResourcesFixture.CommonResources))
            {
                await builder.AddDefaultRepoSetupAsync();

                builder.AddProject(ProjectCreator
                    .Create()
                    .PropertyGroup()
                    .Property("AllowEmptySignList", "true"), "eng/Signing.props");

                // Create a simple project
                builder.AddProject(ProjectCreator
                        .Templates
                        .SdkCsproj(
                            targetFramework: "net8.0",
                            outputType: "Exe")
                        .PropertyGroup()
                        .Property("IsPackable", "true"),
                    "./src/FooPackage/FooPackage.csproj");
                await builder.AddSimpleCSFile("src/FooPackage/Program.cs");

                await builder.Build(
                    TestRepoUtils.BuildArg("configuration"),
                    "Release",
                    TestRepoUtils.BuildArg("restore"),
                    TestRepoUtils.BuildArg("sign"),
                    TestRepoUtils.BuildArg("projects"),
                    Path.Combine(builder.TestRepoRoot, "src/FooPackage/FooPackage.csproj"))
                    .Should().NotThrowAsync();
            }
        }

        /// <summary>
        /// We should get an error if AllowEmptySignList is set to false, or
        /// if it is not set at all (default behavior), and there are no items to sign.
        /// </summary>
        /// <param name="propertyIsSet">Is the property set or are we using the expected default?</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task BuildShouldErrorIfNoItemsToSignAndNonEmptySignList(bool propertyIsSet)
        {
            using (var builder = new TestRepoBuilder(nameof(BuildShouldErrorIfNoItemsToSignAndNonEmptySignList), _commonRepoResourcesFixture.CommonResources))
            {
                await builder.AddDefaultRepoSetupAsync();

                if (propertyIsSet)
                {
                    builder.AddProject(ProjectCreator
                        .Create()
                        .PropertyGroup()
                        .Property("AllowEmptySignList", "false"), "eng/Signing.props");
                }

                // Create a simple project
                builder.AddProject(ProjectCreator
                        .Templates
                        .SdkCsproj(
                            targetFramework: "net8.0",
                            outputType: "Exe")
                        .PropertyGroup()
                        .Property("IsPackable", "true"),
                    "./src/FooPackage/FooPackage.csproj");
                await builder.AddSimpleCSFile("src/FooPackage/Program.cs");

                await builder.Build(
                    TestRepoUtils.BuildArg("configuration"),
                    "Release",
                    TestRepoUtils.BuildArg("restore"),
                    TestRepoUtils.BuildArg("sign"),
                    TestRepoUtils.BuildArg("projects"),
                    Path.Combine(builder.TestRepoRoot, "src/FooPackage/FooPackage.csproj"))
                    .Should().ThrowAsync<Exception>().WithMessage("*error : List of files to sign is empty. Make sure that ItemsToSign is configured correctly*");
            }
        }

        /// <summary>
        /// UseDotNetCertificate should replace Microsoft400 with MicrosoftDotNet500 in the call to signing
        /// </summary>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task BuildShouldUseDotNetCertificateIfSet(bool? useDotNetCert)
        {
            using (var builder = new TestRepoBuilder(nameof(BuildShouldUseDotNetCertificateIfSet), _commonRepoResourcesFixture.CommonResources))
            {
                await builder.AddDefaultRepoSetupAsync();

                // Always put in the AllowEmptySignList
                var signingProps = ProjectCreator.Create().PropertyGroup();
                signingProps.Property("AllowEmptySignList", "true");

                if (useDotNetCert.HasValue)
                {
                    signingProps.Property("UseDotNetCertificate", useDotNetCert.Value.ToString());
                }

                builder.AddProject(signingProps, "eng/Signing.props");

                // Create a simple project
                builder.AddProject(ProjectCreator
                        .Templates
                        .SdkCsproj(
                            targetFramework: "net8.0",
                            outputType: "Exe")
                        .PropertyGroup()
                        .Property("IsPackable", "true")
                        .Property("EnableSourceLink", "false"),
                    "./src/FooPackage/FooPackage.csproj");
                await builder.AddSimpleCSFile("src/FooPackage/Program.cs");

                await builder.Build(
                    TestRepoUtils.BuildArg("configuration"),
                    "Release",
                    TestRepoUtils.BuildArg("restore"),
                    TestRepoUtils.BuildArg("pack"),
                    TestRepoUtils.BuildArg("publish"),
                    TestRepoUtils.BuildArg("sign"),
                    TestRepoUtils.BuildArg("projects"),
                    Path.Combine(builder.TestRepoRoot, "src/FooPackage/FooPackage.csproj"),
                    "/p:AutoGenerateSymbolPackages=false")
                    .Should().NotThrowAsync();

                // Now, go find the Round0 signing project and ensure that the certificate names were set properly.
                // The arcade default for an exe is Microsoft400
                string round0FilePath = Path.Combine(builder.TestRepoRoot, "artifacts", "tmp", "Release", "Signing", "Round0-Sign.proj");
                string round0ProjectText = File.ReadAllText(round0FilePath);
                string expectedCert = useDotNetCert.GetValueOrDefault() ? DotNetCertificate : MicrosoftCertificate;

                Regex authenticodeRegex = new Regex("<Authenticode>([^<]*)</Authenticode>");
                var matches = authenticodeRegex.Matches(round0ProjectText);
                matches.Count.Should().Be(1);
                matches[0].Groups[1].Value.Should().Be(expectedCert);
            }
        }

        /// <summary>
        /// UseDotNetCertificate should replace not replace non-Microsoft400 with MicrosoftDotNet500 when using Sign.proj.
        /// </summary>
        [Fact]
        public async Task BuildShouldNotChangeNonMicrosoft400CertsWhenSigning()
        {
            using (var builder = new TestRepoBuilder(nameof(BuildShouldNotChangeNonMicrosoft400CertsWhenSigning), _commonRepoResourcesFixture.CommonResources))
            {
                await builder.AddDefaultRepoSetupAsync();

                // Always put in the AllowEmptySignList
                var signingProps = ProjectCreator.Create().PropertyGroup();
                signingProps.Property("AllowEmptySignList", "true");

                // Update the .exe extension with a new cert.
                // <StrongNameSignInfo Include="MsSharedLib72" PublicKeyToken="31bf3856ad364e35" CertificateName="Microsoft400" />
                const string certOverride = "Microsoft401";

                signingProps.ItemGroup()
                    .ItemUpdate("StrongNameSignInfo", update: "MsSharedLib72",
                    metadata: new Dictionary<string, string> { { "PublicKeyToken", "31bf3856ad364e35" }, { "CertificateName", certOverride } } );

                builder.AddProject(signingProps, "eng/Signing.props");

                // Create a simple project
                builder.AddProject(ProjectCreator
                        .Templates
                        .SdkCsproj(
                            targetFramework: "net8.0",
                            outputType: "Exe")
                        .PropertyGroup()
                        .Property("IsPackable", "true")
                        .Property("EnableSourceLink", "false"),
                    "./src/FooPackage/FooPackage.csproj");
                await builder.AddSimpleCSFile("src/FooPackage/Program.cs");

                await builder.Build(
                    TestRepoUtils.BuildArg("configuration"),
                    "Release",
                    TestRepoUtils.BuildArg("restore"),
                    TestRepoUtils.BuildArg("pack"),
                    TestRepoUtils.BuildArg("publish"),
                    TestRepoUtils.BuildArg("sign"),
                    TestRepoUtils.BuildArg("projects"),
                    Path.Combine(builder.TestRepoRoot, "src/FooPackage/FooPackage.csproj"),
                    "/p:AutoGenerateSymbolPackages=false")
                    .Should().NotThrowAsync();

                // Now, go find the Round0 signing project and ensure that the certificate names were set properly.
                // The arcade default for an exe is Microsoft400
                string round0FilePath = Path.Combine(builder.TestRepoRoot, "artifacts", "tmp", "Release", "Signing", "Round0-Sign.proj");
                string round0ProjectText = File.ReadAllText(round0FilePath);

                Regex authenticodeRegex = new Regex("<Authenticode>([^<]*)</Authenticode>");
                var matches = authenticodeRegex.Matches(round0ProjectText);
                matches.Count.Should().Be(1);
                matches[0].Groups[1].Value.Should().Be(certOverride);
            }
        }

        /// <summary>
        /// Retrieve the text from the asset manifest file. Checks that there is only a single asset manifest.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static string GetAssetManifest(TestRepoBuilder builder)
        {
            // Now, go find the asset manifest. Since we don't know exactly where it will be and what it will
            // be named (configuration and OS names end up influencing the path), just find an asset manifest under
            // artifacts/log/**/AssetManifest/*. There should only be one.
            string logsDirectory = Path.Combine(builder.TestRepoRoot, "artifacts", "log");
            string[] logFiles = Directory.GetFiles(logsDirectory, "*.xml", SearchOption.AllDirectories);
            string escapedDirSeparator = Regex.Escape($"{Path.DirectorySeparatorChar}");
            Regex assetManifestRegex = new Regex(@$".*{escapedDirSeparator}AssetManifest{escapedDirSeparator}.*\.xml");
            var assetManifests = logFiles.Where(am => assetManifestRegex.IsMatch(am)).ToArray();
            assetManifests.Length.Should().Be(1);
            string assetManifestText = File.ReadAllText(assetManifests[0]);
            return assetManifestText;
        }
    }
}
