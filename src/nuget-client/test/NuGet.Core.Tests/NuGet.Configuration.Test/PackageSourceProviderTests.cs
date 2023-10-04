// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class PackageSourceProviderTests
    {
        [Fact]
        public void ActivePackageSourceCanBeReadAndWrittenInNuGetConfig()
        {
            // Act
            //Create nuget.config that has active package source defined
            using (var directory = TestDirectory.Create())
            {
                var nugetConfigFilePath = Path.Combine(directory, "nuget.Config");

                var enabledReplacement = @"<add key='" + NuGetConstants.FeedName + "' value='" + NuGetConstants.V2FeedUrl + "' />";
                var disabledReplacement = string.Empty;
                var activeReplacement = @"<add key='" + NuGetConstants.FeedName + "' value='" + NuGetConstants.V2FeedUrl + "' />";
                File.WriteAllText(nugetConfigFilePath, CreateNuGetConfigContent(enabledReplacement, disabledReplacement, activeReplacement));

                var settings = new Settings(directory, "nuget.Config");

                var before = new PackageSourceProvider(settings);
                Assert.Equal(NuGetConstants.FeedName, before.ActivePackageSourceName);

                before.SaveActivePackageSource(new PackageSource(NuGetConstants.V3FeedUrl, NuGetConstants.FeedName));
                Assert.Equal(NuGetConstants.FeedName, before.ActivePackageSourceName);
            }
        }

        [Fact]
        public void ActivePackageSourceReturnsNullIfNotSetInNuGetConfig()
        {
            // Act
            //Create nuget.config that has active package source defined
            using (var directory = TestDirectory.Create())
            {
                var nugetConfigFilePath = Path.Combine(directory, "nuget.Config");

                var enabledReplacement = @"<add key='" + NuGetConstants.FeedName + "' value='" + NuGetConstants.V2FeedUrl + "' />";
                var text = CreateNuGetConfigContent(enabledReplacement);
                File.WriteAllText(nugetConfigFilePath, text);

                var settings = new Settings(directory, "nuget.config");
                var before = new PackageSourceProvider(settings);
                Assert.Null(before.ActivePackageSourceName);

                before.SaveActivePackageSource(new PackageSource(NuGetConstants.V3FeedUrl, NuGetConstants.FeedName));
                Assert.Equal(NuGetConstants.FeedName, before.ActivePackageSourceName);
            }
        }

        [Fact]
        public void ActivePackageSourceReturnsNullIfNotPresentInNuGetConfig()
        {
            // Act
            //Create nuget.config that has active package source defined
            using (var directory = TestDirectory.Create())
            {
                var nugetConfigFilePath = Path.Combine(directory, "nuget.Config");

                var enabledReplacement = @"<add key='" + NuGetConstants.FeedName + "' value='" + NuGetConstants.V2FeedUrl + "' />";
                var fileContents = CreateNuGetConfigContent(enabledReplacement);
                fileContents = fileContents.Replace("<activePackageSource>", string.Empty);
                fileContents = fileContents.Replace("</activePackageSource>", string.Empty);
                File.WriteAllText(nugetConfigFilePath, fileContents);

                var settings = new Settings(directory, "nuget.Config");
                var before = new PackageSourceProvider(settings);
                Assert.Null(before.ActivePackageSourceName);

                before.SaveActivePackageSource(new PackageSource(NuGetConstants.V3FeedUrl, NuGetConstants.FeedName));
                Assert.Equal(NuGetConstants.FeedName, before.ActivePackageSourceName);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSources_LoadsCredentials(bool useStaticMethod)
        {
            // Arrange
            var nugetConfigFilePath = "NuGet.Config";
            var configContent = @"<?xml version='1.0' encoding='utf-8'?>
<configuration>
  <apikeys>
    <add key='https://a.test' value='removed' />
    <add key='https://b.test/somecompanyfeed-unstable/' value='removed' />
    <add key='https://b.test/somecompanyfeed/' value='removed' />
    <add key='https://b.test/somecompanyfeed-unstable/api/v2/package' value='removed' />
    <add key='https://b.test/somecompanyfeed/api/v2/package' value='removed' />
    <add key='https://b.test/somecompanyfeed-unstable/api/v2/' value='removed' />
    <add key='https://c.test/' value='removed' />
  </apikeys>
  <packageRestore>
    <add key='enabled' value='True' />
    <add key='automatic' value='True' />
  </packageRestore>
  <activePackageSource>
    <add key='d' value='https://d.test/' />
  </activePackageSource>
  <packageSources>
    <add key='CodeCrackerUnstable' value='https://b.test/codecrackerbuild/api/v2' />
    <add key='CompanyFeedUnstable' value='https://b.test/somecompanyfeed-unstable/api/v2/' />
    <add key='d' value='https://d.test/' />
    <add key='AspNetVNextStable' value='https://b.test/aspnetmaster/api/v2' />
    <add key='AspNetVNextUnstable' value='https://b.test/aspnetvnext/api/v2' />
    <add key='CompanyFeed' value='https://b.test/somecompanyfeed/api/v2/' />
  </packageSources>
  <packageSourceCredentials>
    <CodeCrackerUnstable>
      <add key='Username' value='myusername' />
      <add key='ClearTextPassword' value='pass' />
    </CodeCrackerUnstable>
    <AspNetVNextUnstable>
      <add key='Username' value='myusername' />
      <add key='ClearTextPassword' value='pass' />
    </AspNetVNextUnstable>
    <AspNetVNextStable>
      <add key='Username' value='myusername' />
      <add key='ClearTextPassword' value='pass' />
    </AspNetVNextStable>
    <NuGet.org>
      <add key='Username' value='myusername' />
      <add key='ClearTextPassword' value='pass' />
    </NuGet.org>
    <CompanyFeedUnstable>
      <add key='Username' value='myusername' />
      <add key='ClearTextPassword' value='removed' />
    </CompanyFeedUnstable>
    <CompanyFeed>
      <add key='Username' value='myusername' />
      <add key='ClearTextPassword' value='removed' />
    </CompanyFeed>
  </packageSourceCredentials>
</configuration>";

            using (var directory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigFilePath, directory, configContent);
                var settings = new Settings(directory);

                // Act
                List<PackageSource> sources = LoadPackageSources(useStaticMethod, settings);

                // Assert
                sources.Count.Should().Be(6);
                AssertCredentials(sources[1].Credentials, "CompanyFeedUnstable", "myusername", "removed");
                AssertCredentials(sources[5].Credentials, "CompanyFeed", "myusername", "removed");
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestNoPackageSourcesAreReturnedIfUserSettingsIsEmpty(bool useStaticMethod)
        {
            // Arrange
            var settings = new Mock<ISettings>();

            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Equal(0, values.Count);
        }

        [Fact]
        public void SavePackageSourcesTest()
        {
            // Arrange
            var nugetConfigFilePath = "NuGet.Config";
            var configContent = @"<?xml version='1.0' encoding='utf-8'?>
<configuration>
</configuration>";

            using (var directory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigFilePath, directory, configContent);
                var settings = new Settings(directory);
                var provider = new PackageSourceProvider(settings);

                // Act
                provider.SavePackageSources(
                    new PackageSource[]
                        {
                        new PackageSource("http://a.test", "a")
                            {
                                IsEnabled = true
                            },
                        new PackageSource("http://b.test", "b")
                            {
                                IsEnabled = false
                            },
                        new PackageSource("http://c.test", "c", isEnabled: true, isOfficial: false, isPersistable: false),
                        new PackageSource("http://d.test", "d", isEnabled: false, isOfficial: false, isPersistable: false),
                        });

                // Assert:
                // - source a is persisted in <packageSources>
                // - source b is persisted in <packageSources> and <disabledPackageSources>
                // - source c is not spersisted at all since its IsPersistable is false and it's enabled.
                // - source d is persisted in <disabledPackageSources> only since its IsPersistable is false and it's disabled.

                var configFileContent = File.ReadAllText(Path.Combine(directory, nugetConfigFilePath));
                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""a"" value=""http://a.test"" />
    <add key=""b"" value=""http://b.test"" />
  </packageSources>
  <disabledPackageSources>
    <add key=""b"" value=""true"" />
    <add key=""d"" value=""true"" />
  </disabledPackageSources>
</configuration>");

                Assert.Equal(result, SettingsTestUtils.RemoveWhitespace(configFileContent));
            }
        }

        [Fact]
        public void SavePackageSourcesWithRelativePath()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""Packages"" />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), configContents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());

                var settings = Settings.LoadSettings(rootPath,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();

                // Act
                packageSourceProvider.SavePackageSources(packageSourceList);

                // Assert
                Assert.Equal(
                       @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""Packages"" />
    </packageSources>
</configuration>
".Replace("\r\n", "\n"),
                   File.ReadAllText(Path.Combine(directory.Path, "NuGet.config")).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SavePackageSourcesWithRelativePathAndAddNewSource()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""Packages"" />
    </packageSources>
</configuration>
";
                var settingsFile = new FileInfo(Path.Combine(directory.Path, "NuGet.config"));

                File.WriteAllText(settingsFile.FullName, configContents);

                var settings = Settings.LoadSettings(
                    settingsFile.Directory,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();

                // Act
                packageSourceList.Add(new PackageSource("https://c.test", "c"));
                packageSourceProvider.SavePackageSources(packageSourceList);

                // Assert
                Assert.Equal(
                       $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""Packages"" />
        <add key=""c"" value=""https://c.test"" />
    </packageSources>
</configuration>
".Replace("\r\n", "\n"),
                   File.ReadAllText(settingsFile.FullName).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SavePackageSourcesWithOneClear()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://b.test"" />
         <clear />
        <add key=""b"" value=""https://new.b.test"" protocolVersion=""3"" />
        <add key=""c"" value=""https://c.test"" />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), configContents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());

                var settings = Settings.LoadSettings(rootPath,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();

                // act
                packageSourceList.Add(new PackageSource("https://d.test", "d"));
                packageSourceProvider.SavePackageSources(packageSourceList);

                // Assert
                Assert.Equal(
                       @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://b.test"" />
         <clear />
        <add key=""b"" value=""https://new.b.test"" protocolVersion=""3"" />
        <add key=""c"" value=""https://c.test"" />
        <add key=""d"" value=""https://d.test"" />
    </packageSources>
</configuration>
".Replace("\r\n", "\n"),
                   File.ReadAllText(Path.Combine(directory.Path, "NuGet.config")).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SavePackageSourcesWithMoreClear()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://b.test"" />
        <clear />
        <add key=""b"" value=""https://new.b.test"" protocolVersion=""3"" />
        <clear />
        <add key=""c"" value=""https://c.test"" />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), configContents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());

                var settings = Settings.LoadSettings(rootPath,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();

                // act
                packageSourceList.Add(new PackageSource("https://d.test", "d"));
                packageSourceProvider.SavePackageSources(packageSourceList);

                // Assert
                Assert.Equal(
                       @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://b.test"" />
        <clear />
        <add key=""b"" value=""https://new.b.test"" protocolVersion=""3"" />
        <clear />
        <add key=""c"" value=""https://c.test"" />
        <add key=""d"" value=""https://d.test"" />
    </packageSources>
</configuration>
".Replace("\r\n", "\n"),
                   File.ReadAllText(Path.Combine(directory.Path, "NuGet.config")).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SavePackageSourcesWithOnlyClear()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <clear />
    </packageSources>
    <disabledPackageSources>
        <clear />
    </disabledPackageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), configContents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());

                var settings = Settings.LoadSettings(rootPath,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();

                // act
                packageSourceList.Add(new PackageSource("https://a.test", "a"));
                packageSourceProvider.SavePackageSources(packageSourceList);

                // Assert
                Assert.Equal(
                       @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <clear />
        <add key=""a"" value=""https://a.test"" />
    </packageSources>
    <disabledPackageSources>
        <clear />
    </disabledPackageSources>
</configuration>
".Replace("\r\n", "\n"),
                   File.ReadAllText(Path.Combine(directory.Path, "NuGet.config")).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SavePackageSourcesWithHierarchyClear()
        {
            using (var directory = TestDirectory.Create())
            {
                // assert
                var nugetConfigPath = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <!-- i.e. ignore values from prior conf files -->
    <clear />
    <add key=""a"" value=""https://a.test"" />
  </packageSources>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(directory, "dir1", "dir2"), config);
                config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""b"" value=""https://b.test"" />
    <clear />
  </packageSources>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, Path.Combine(directory, "dir1"), config);

                var rootPath = Path.Combine(Path.Combine(directory, "dir1", "dir2"), Path.GetRandomFileName());
                var settings = Settings.LoadSettings(rootPath,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);

                var packageSourceProvider = new PackageSourceProvider(settings);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();

                // act
                packageSourceList.Add(new PackageSource("https://c.test", "c"));
                packageSourceProvider.SavePackageSources(packageSourceList);

                // Assert
                Assert.Equal(
                       @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <!-- i.e. ignore values from prior conf files -->
    <clear />
    <add key=""a"" value=""https://a.test"" />
    <add key=""c"" value=""https://c.test"" />
  </packageSources>
</configuration>".Replace("\r\n", "\n"),
                   File.ReadAllText(Path.Combine(directory.Path, "dir1", "dir2", "NuGet.Config")).Replace("\r\n", "\n"));

                Assert.Equal(
                     @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""b"" value=""https://b.test"" />
    <clear />
  </packageSources>
</configuration>".Replace("\r\n", "\n"),
                  File.ReadAllText(Path.Combine(directory.Path, "dir1", "NuGet.Config")).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SavePackageSources_RetainUnavailableDisabledSources()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://b.test"" />
    </packageSources>
    <disabledPackageSources>
        <add key=""Microsoft and .NET"" value=""true"" />
        <add key=""b"" value=""true"" />
    </disabledPackageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), configContents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());

                var settings = Settings.LoadSettings(rootPath,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);

                var disabledPackagesSection = settings.GetSection("disabledPackageSources");
                disabledPackagesSection.Should().NotBeNull();

                var expectedDisabledSources = disabledPackagesSection?.Items.ToList();

                var packageSourceProvider = new PackageSourceProvider(settings);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();

                // Act
                packageSourceProvider.SavePackageSources(packageSourceList);

                var newSettings = Settings.LoadSettings(rootPath,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);

                var actualDisabledSourcesSection = newSettings.GetSection("disabledPackageSources");
                actualDisabledSourcesSection.Should().NotBeNull();

                var actualDisabledSources = actualDisabledSourcesSection?.Items.ToList();

                Assert.Equal(expectedDisabledSources.Count, actualDisabledSources.Count);
                foreach (var item in expectedDisabledSources)
                {
                    Assert.Contains(item, actualDisabledSources);
                }
            }
        }

        [Fact]
        public void SavePackageSources_EnablesDisabledSources()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://b.test"" />
    </packageSources>
    <disabledPackageSources>
        <add key=""Microsoft and .NET"" value=""true"" />
        <add key=""b"" value=""true"" />
    </disabledPackageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), configContents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);

                var disabledPackagesSection = settings.GetSection("disabledPackageSources");
                disabledPackagesSection.Should().NotBeNull();

                var disabledSources = disabledPackagesSection?.Items.Select(c => c as AddItem).ToList();

                // Pre-Assert
                Assert.Equal(2, disabledSources.Count);
                Assert.Equal("Microsoft and .NET", disabledSources[0].Key);
                Assert.Equal("b", disabledSources[1].Key);

                var packageSourceProvider = new PackageSourceProvider(settings);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();

                Assert.Equal(2, packageSourceList.Count);
                Assert.Equal("a", packageSourceList[0].Name);
                Assert.True(packageSourceList[0].IsEnabled);
                Assert.Equal("b", packageSourceList[1].Name);
                Assert.False(packageSourceList[1].IsEnabled);

                // Main Act
                packageSourceList[1].IsEnabled = true;
                packageSourceProvider.SavePackageSources(packageSourceList);

                var newSettings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);

                // Main Assert
                disabledPackagesSection = newSettings.GetSection("disabledPackageSources");
                disabledPackagesSection.Should().NotBeNull();

                disabledSources = disabledPackagesSection?.Items.Select(c => c as AddItem).ToList();

                Assert.Equal(1, disabledSources.Count);
                Assert.Equal("Microsoft and .NET", disabledSources[0].Key);

                packageSourceList = PackageSourceProvider.LoadPackageSources(newSettings).ToList();

                Assert.Equal(2, packageSourceList.Count);
                Assert.Equal("a", packageSourceList[0].Name);
                Assert.True(packageSourceList[0].IsEnabled);
                Assert.Equal("b", packageSourceList[1].Name);
                Assert.True(packageSourceList[1].IsEnabled);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSourcesReturnCorrectDataFromSettings(bool useStaticMethod)
        {
            // Arrange
            var settings = new Mock<ISettings>(MockBehavior.Strict);

            settings.Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    new SourceItem("one", "onesource"),
                    new SourceItem("two", "twosource"),
                    new SourceItem("three", "threesource")
                ))
                .Verifiable();

            settings.Setup(s => s.GetSection("disabledPackageSources"))
                .Returns(new VirtualSettingSection("disabledPackageSources"));
            settings.Setup(s => s.GetSection("packageSourceCredentials"))
                .Returns(new VirtualSettingSection("packageSourceCredentials"));
            settings.Setup(s => s.GetSection("config"))
                .Returns(new VirtualSettingSection("config"));
            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());
            settings.Setup(s => s.GetSection("clientCertificates"))
                    .Returns(new VirtualSettingSection("clientCertificates"));

            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Equal(3, values.Count);
            AssertPackageSource(values[0], "one", "onesource", isEnabled: true);
            AssertPackageSource(values[1], "two", "twosource", isEnabled: true);
            AssertPackageSource(values[2], "three", "threesource", isEnabled: true);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSourcesReturnCorrectDataFromSettingsWhenSomePackageSourceIsDisabled(bool useStaticMethod)
        {
            // Arrange
            var settings = new Mock<ISettings>(MockBehavior.Strict);
            settings.Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                        new SourceItem("one", "onesource"),
                        new SourceItem("two", "twosource"),
                        new SourceItem("three", "threesource")
                    ));

            settings.Setup(s => s.GetSection("disabledPackageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    new AddItem("two", "true")
                    ));

            settings.Setup(s => s.GetSection("packageSourceCredentials"))
                .Returns(new VirtualSettingSection("packageSourceCredentials"));

            settings.Setup(s => s.GetSection("config"))
                .Returns(new VirtualSettingSection("config"));
            settings.Setup(s => s.GetConfigFilePaths())
                    .Returns(new List<string>());

            settings.Setup(s => s.GetSection("clientCertificates"))
                .Returns(new VirtualSettingSection("clientCertificates"));

            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Equal(3, values.Count);
            AssertPackageSource(values[0], "one", "onesource", isEnabled: true);
            AssertPackageSource(values[1], "two", "twosource", isEnabled: false);
            AssertPackageSource(values[2], "three", "threesource", isEnabled: true);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSources_ReadsSourcesWithProtocolVersionFromPackageSourceSections(bool useStaticMethod)
        {
            // Arrange
            var settings = new Mock<ISettings>();
            var settingWithV3Protocol1 = new SourceItem("Source2", "https://source-with-newer-protocol.test", "3");
            var settingWithV3Protocol2 = new SourceItem("Source3", "Source3", "3");

            settings.Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    new SourceItem("Source1", "https://some-source.test"),
                    settingWithV3Protocol1,
                    settingWithV3Protocol2,
                    new SourceItem("Source3", "Source3"),
                    new SourceItem("Source4", "//Source4")));

            settings
                .Setup(s => s.GetSection("packageSourceCredentials"))
                .Returns(new VirtualSettingSection("packageSourceCredentials",
                    new CredentialsItem("Source3", "source3-user", "source3-password", isPasswordClearText: true, validAuthenticationTypes: null)));

            settings
                .Setup(s => s.GetSection("disabledPackageSources"))
                .Returns(new VirtualSettingSection("disabledPackageSources",
                        new AddItem("Source4", "true")
                    ));
            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());
            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Collection(values,
                source =>
                    {
                        Assert.Equal("Source1", source.Name);
                        Assert.Equal("https://some-source.test", source.Source);
                        Assert.Null(source.Credentials);
                        Assert.Equal(2, source.ProtocolVersion);
                        Assert.True(source.IsEnabled);
                    },
                source =>
                    {
                        Assert.Equal("Source2", source.Name);
                        Assert.Equal("https://source-with-newer-protocol.test", source.Source);
                        Assert.Null(source.Credentials);
                        Assert.Equal(3, source.ProtocolVersion);
                        Assert.True(source.IsEnabled);
                    },
                source =>
                    {
                        Assert.Equal("Source3", source.Name);
                        Assert.Equal("Source3", source.Source);
                        AssertCredentials(source.Credentials, "Source3", "source3-user", "source3-password");
                        Assert.Equal(3, source.ProtocolVersion);
                        Assert.True(source.IsEnabled);
                    },
                source =>
                    {
                        Assert.Equal("Source4", source.Name);
                        Assert.Equal("//Source4", source.Source);
                        Assert.Null(source.Credentials);
                        Assert.Equal(2, source.ProtocolVersion);
                        Assert.False(source.IsEnabled);
                    });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSources_ReadsSourcesWithNullAllowInsecureConnectionsFromPackageSourceSections_LoadsDefault(bool useStaticMethod)
        {
            // Arrange
            var settings = new Mock<ISettings>();
            var sourceItem = new SourceItem("Source", "https://some-source.test", protocolVersion: null, allowInsecureConnections: null);

            settings.Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    sourceItem));

            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());

            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            var loadedSource = values.Single();
            Assert.Equal("Source", loadedSource.Name);
            Assert.Equal("https://some-source.test", loadedSource.Source);
            Assert.Equal(PackageSource.DefaultAllowInsecureConnections, loadedSource.AllowInsecureConnections);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSources_ReadsSourcesWithInvalidAllowInsecureConnectionsFromPackageSourceSections_LoadsDefault(bool useStaticMethod)
        {
            // Arrange
            var settings = new Mock<ISettings>();
            var sourceItem = new SourceItem("Source", "https://some-source.test", protocolVersion: null, allowInsecureConnections: "invalidString");

            settings.Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    sourceItem));

            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());

            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            var loadedSource = values.Single();
            Assert.Equal("Source", loadedSource.Name);
            Assert.Equal("https://some-source.test", loadedSource.Source);
            Assert.Equal(PackageSource.DefaultAllowInsecureConnections, loadedSource.AllowInsecureConnections);
        }

        [Theory]
        [InlineData(true, "true")]
        [InlineData(true, "TRUE")]
        [InlineData(true, "false")]
        [InlineData(false, "false")]
        [InlineData(false, "fALSE")]
        [InlineData(false, "true")]
        public void LoadPackageSources_ReadsSourcesWithNotNullAllowInsecureConnectionsFromPackageSourceSections_LoadsValue(bool useStaticMethod, string allowInsecureConnections)
        {
            // Arrange
            var settings = new Mock<ISettings>();
            var sourceItem = new SourceItem("Source", "https://some-source.test", protocolVersion: null, allowInsecureConnections: allowInsecureConnections);

            settings.Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    sourceItem));

            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());

            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            var loadedSource = values.Single();
            Assert.Equal("Source", loadedSource.Name);
            Assert.Equal("https://some-source.test", loadedSource.Source);
            Assert.Equal(bool.Parse(allowInsecureConnections), loadedSource.AllowInsecureConnections);
        }

        [Fact]
        public void DisablePackageSourceAddEntryToSettings()
        {
            // Arrange
            var settings = new Mock<ISettings>(MockBehavior.Strict);
            settings.Setup(s => s.GetSection("packageSources"))
                .Returns(() => null).Verifiable();
            settings.Setup(s => s.AddOrUpdate("disabledPackageSources", It.IsAny<AddItem>())).Verifiable();
            settings.Setup(s => s.SaveToDisk()).Verifiable();

            var provider = new PackageSourceProvider(settings.Object);

            // Act
            provider.DisablePackageSource("A");

            // Assert
            settings.Verify();
        }

        [Fact]
        public void IsPackageSourceEnabledReturnsFalseIfTheSourceIsDisabled()
        {
            // Arrange
            var settings = new Mock<ISettings>(MockBehavior.Strict);
            settings.Setup(s => s.GetSection("disabledPackageSources"))
                .Returns(new VirtualSettingSection("disabledPackageSources",
                    new AddItem("A", "sdfds")));
            var provider = new PackageSourceProvider(settings.Object);

            // Act
            var isEnabled = provider.IsPackageSourceEnabled("A");

            // Assert
            Assert.False(isEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSources_ReadsCredentialPairsFromSettings(bool useStaticMethod)
        {
            // Arrange
            var encryptedPassword = Guid.NewGuid().ToString();

            var settings = new Mock<ISettings>();
            settings
                .Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    new SourceItem("one", "onesource"),
                    new SourceItem("two", "twosource"),
                    new SourceItem("three", "threesource")
                ));
            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());
            settings
                .Setup(s => s.GetSection("packageSourceCredentials"))
                .Returns(new VirtualSettingSection("two",
                    new CredentialsItem("two", "user1", encryptedPassword, isPasswordClearText: false, validAuthenticationTypes: null)
                    ));

            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Equal(3, values.Count);
            AssertPackageSource(values[1], "two", "twosource", true);
            AssertCredentials(values[1].Credentials, "two", "user1", encryptedPassword, isPasswordClearText: false);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSources_WithSpaceInName_ReadsCredentialPairsFromSettings(bool useStaticMethod)
        {
            // Arrange
            var encryptedPassword = Guid.NewGuid().ToString();

            var settings = new Mock<ISettings>();
            settings
                .Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    new SourceItem("one source", "onesource"),
                    new SourceItem("two source", "twosource"),
                    new SourceItem("three source", "threesource")
                ));

            settings
                .Setup(s => s.GetSection("packageSourceCredentials"))
                .Returns(new VirtualSettingSection("packageSourceCredentials",
                    new CredentialsItem("two source", "user1", encryptedPassword, isPasswordClearText: false, validAuthenticationTypes: null)
                    ));
            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());
            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Equal(3, values.Count);
            AssertPackageSource(values[1], "two source", "twosource", true);
            AssertCredentials(values[1].Credentials, "two source", "user1", encryptedPassword, isPasswordClearText: false);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSources_ReadsClearTextCredentialPairsFromSettings(bool useStaticMethod)
        {
            // Arrange
            const string clearTextPassword = "topsecret";

            var settings = new Mock<ISettings>();
            settings
                .Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    new SourceItem("one", "onesource"),
                    new SourceItem("two", "twosource"),
                    new SourceItem("three", "threesource")
                ));

            settings
             .Setup(s => s.GetSection("packageSourceCredentials"))
             .Returns(new VirtualSettingSection("two",
                 new CredentialsItem("two", "user1", clearTextPassword, isPasswordClearText: true, validAuthenticationTypes: null)
                 ));
            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());
            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Equal(3, values.Count);
            AssertPackageSource(values[1], "two", "twosource", true);
            AssertCredentials(values[1].Credentials, "two", "user1", clearTextPassword);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSources_WhenEnvironmentCredentialsAreMalformed_FallsbackToSettingsCredentials(bool useStaticMethod)
        {
            // Arrange
            var settings = new Mock<ISettings>();
            settings
                .Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                    new SourceItem("one", "onesource"),
                    new SourceItem("two", "twosource"),
                    new SourceItem("three", "threesource")
                ));

            settings
                .Setup(s => s.GetSection("packageSourceCredentials"))
                .Returns(new VirtualSettingSection("two",
                    new CredentialsItem("two", "settinguser", "settingpassword", isPasswordClearText: true, validAuthenticationTypes: null)
                    ));
            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());
            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Equal(3, values.Count);
            AssertPackageSource(values[1], "two", "twosource", true);
            AssertCredentials(values[1].Credentials, "two", "settinguser", "settingpassword");
        }

        [Fact]
        public void SavePackageSourcesSaveCorrectDataToSettings()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);
                var settings = new Settings(directory);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var sources = new[] { new PackageSource("one"), new PackageSource("two"), new PackageSource("three") };

                // Act
                packageSourceProvider.SavePackageSources(sources);

                // Assert
                var packageSourcesSection = settings.GetSection("packageSources");
                packageSourcesSection.Should().NotBeNull();
                packageSourcesSection.Items.Count.Should().Be(3);
                packageSourcesSection.Items.Should().AllBeOfType<SourceItem>();

                var children = packageSourcesSection.Items.Select(c => c as SourceItem).ToList();
                children[0].Key.Should().Be("one");
                children[0].ProtocolVersion.Should().BeNullOrEmpty();
                children[1].Key.Should().Be("two");
                children[1].ProtocolVersion.Should().BeNullOrEmpty();
                children[2].Key.Should().Be("three");
                children[2].ProtocolVersion.Should().BeNullOrEmpty();

                var disabledPackageSourcesSection = settings.GetSection("disabledPackageSources");
                disabledPackageSourcesSection.Should().BeNull();
            }
        }

        [Fact]
        public void SavePackage_KeepsBothNewAndOldSources()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""Source2-Name"" value=""Legacy-Source"" />
    </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);
                var settings = new Settings(directory);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var sources = new[]
                    {
                    new PackageSource("Source1", "Source1-Name"),
                    new PackageSource("Source2", "Source2-Name")
               };
                // Act
                packageSourceProvider.SavePackageSources(sources);

                // Assert
                var packageSourcesSection = settings.GetSection("packageSources");
                packageSourcesSection.Should().NotBeNull();
                packageSourcesSection.Items.Count.Should().Be(2);
                packageSourcesSection.Items.Should().AllBeOfType<SourceItem>();

                var children = packageSourcesSection.Items.Select(c => c as SourceItem).ToList();
                children[0].Key.Should().Be("Source2-Name");
                children[0].ProtocolVersion.Should().BeNullOrEmpty();
                children[1].Key.Should().Be("Source1-Name");
                children[1].ProtocolVersion.Should().BeNullOrEmpty();

                var disabledPackageSourcesSection = settings.GetSection("disabledPackageSources");
                disabledPackageSourcesSection.Should().BeNull();
            }
        }

        [Fact]
        public void SavePackageSourcesSaveCorrectDataToSettingsWhenSomePackageSourceIsDisabled()
        {

            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);
                var settings = new Settings(directory);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var sources = new[] { new PackageSource("one"), new PackageSource("two", "two", isEnabled: false), new PackageSource("three") };

                // Act
                packageSourceProvider.SavePackageSources(sources);

                // Assert
                var packageSourcesSection = settings.GetSection("packageSources");
                packageSourcesSection.Should().NotBeNull();
                packageSourcesSection.Items.Count.Should().Be(3);
                packageSourcesSection.Items.Should().AllBeOfType<SourceItem>();

                var children = packageSourcesSection.Items.Select(c => c as SourceItem).ToList();
                children[0].Key.Should().Be("one");
                children[0].ProtocolVersion.Should().BeNullOrEmpty();
                children[1].Key.Should().Be("two");
                children[1].ProtocolVersion.Should().BeNullOrEmpty();
                children[2].Key.Should().Be("three");
                children[2].ProtocolVersion.Should().BeNullOrEmpty();

                var disabledPackageSourcesSection = settings.GetSection("disabledPackageSources");
                disabledPackageSourcesSection.Should().NotBeNull();
                disabledPackageSourcesSection.Items.Count.Should().Be(1);

                var two = disabledPackageSourcesSection.Items.FirstOrDefault() as AddItem;
                two.Should().NotBeNull();
                two.Key.Should().Be("two");
                two.Value.Should().Be("true");
            }
        }

        [Fact]
        public void SavePackageSources_SavesEncryptedCredentials()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);
                var settings = new Settings(directory);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var encryptedPassword = Guid.NewGuid().ToString();
                var credentials = new PackageSourceCredential("twoname", "User", encryptedPassword, isPasswordClearText: false, validAuthenticationTypesText: null);

                var sources = new[]
                {
                    new PackageSource("one"),
                    new PackageSource("http://twosource.test", "twoname") { Credentials = credentials },
                    new PackageSource("three")
                };

                // Act
                packageSourceProvider.SavePackageSources(sources);

                // Assert
                var packageSourcesSection = settings.GetSection("packageSources");
                packageSourcesSection.Should().NotBeNull();
                packageSourcesSection.Items.Count.Should().Be(3);
                packageSourcesSection.Items.Should().AllBeOfType<SourceItem>();

                var children = packageSourcesSection.Items.Select(c => c as SourceItem).ToList();
                children[0].Key.Should().Be("one");
                children[0].ProtocolVersion.Should().BeNullOrEmpty();
                children[1].Key.Should().Be("twoname");
                children[1].GetValueAsPath().Should().Be("http://twosource.test");
                children[1].ProtocolVersion.Should().BeNullOrEmpty();
                children[2].Key.Should().Be("three");
                children[2].ProtocolVersion.Should().BeNullOrEmpty();

                var disabledPackageSourcesSection = settings.GetSection("disabledPackageSources");
                disabledPackageSourcesSection.Should().BeNull();

                var sourcesCredentialsSection = settings.GetSection("packageSourceCredentials");
                sourcesCredentialsSection.Should().NotBeNull();
                sourcesCredentialsSection.Items.Count.Should().Be(1);
                var two = sourcesCredentialsSection.Items.FirstOrDefault() as CredentialsItem;
                two.Should().NotBeNull();
                two.ElementName.Should().Be("twoname");
                two.Username.Should().Be("User");
                two.IsPasswordClearText.Should().BeFalse();
                two.Password.Should().Be(encryptedPassword);
            }
        }

        [Fact]
        public void SavePackageSources_SavesClearTextCredentials()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);
                var settings = new Settings(directory);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var credentials = new PackageSourceCredential("twoname", "User", "password", isPasswordClearText: true, validAuthenticationTypesText: null);

                var sources = new[]
                {
                    new PackageSource("one"),
                    new PackageSource("http://twosource.test", "twoname") { Credentials = credentials },
                    new PackageSource("three")
                };

                // Act
                packageSourceProvider.SavePackageSources(sources);

                // Assert
                var packageSourcesSection = settings.GetSection("packageSources");
                packageSourcesSection.Should().NotBeNull();
                packageSourcesSection.Items.Count.Should().Be(3);
                packageSourcesSection.Items.Should().AllBeOfType<SourceItem>();

                var children = packageSourcesSection.Items.Select(c => c as SourceItem).ToList();
                children[0].Key.Should().Be("one");
                children[0].ProtocolVersion.Should().BeNullOrEmpty();
                children[1].Key.Should().Be("twoname");
                children[1].GetValueAsPath().Should().Be("http://twosource.test");
                children[1].ProtocolVersion.Should().BeNullOrEmpty();
                children[2].Key.Should().Be("three");
                children[2].ProtocolVersion.Should().BeNullOrEmpty();

                var disabledPackageSourcesSection = settings.GetSection("disabledPackageSources");
                disabledPackageSourcesSection.Should().BeNull();

                var sourcesCredentialsSection = settings.GetSection("packageSourceCredentials");
                sourcesCredentialsSection.Should().NotBeNull();
                sourcesCredentialsSection.Items.Count.Should().Be(1);
                var two = sourcesCredentialsSection.Items.FirstOrDefault() as CredentialsItem;
                two.Should().NotBeNull();
                two.ElementName.Should().Be("twoname");
                two.Username.Should().Be("User");
                two.IsPasswordClearText.Should().BeTrue();
                two.Password.Should().Be("password");
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSourcesWithDisabledPackageSourceIsUpperCase(bool useStaticMethod)
        {
            // Arrange
            var settings = new Mock<ISettings>(MockBehavior.Strict);
            settings.Setup(s => s.GetSection("packageSources"))
                .Returns(new VirtualSettingSection("packageSources",
                        new SourceItem("one", "onesource"),
                        new SourceItem("TWO", "twosource"),
                        new SourceItem("three", "threesource")
                    ));
            settings.Setup(s => s.GetSection("disabledPackageSources"))
                .Returns(new VirtualSettingSection("disabledPackageSources",
                    new AddItem("TWO", "true")
                    ));
            settings.Setup(s => s.GetSection("packageSourceCredentials"))
                .Returns(new VirtualSettingSection("packageSourceCredentials"));
            settings.Setup(s => s.GetSection("config"))
                .Returns(new VirtualSettingSection("config"));
            settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());
            settings.Setup(s => s.GetSection("clientCertificates"))
                .Returns(new VirtualSettingSection("clientCertificates"));
            // Act
            List<PackageSource> values = LoadPackageSources(useStaticMethod, settings.Object);

            // Assert
            Assert.Equal(3, values.Count);
            AssertPackageSource(values[0], "one", "onesource", true);
            AssertPackageSource(values[1], "TWO", "twosource", false);
            AssertPackageSource(values[2], "three", "threesource", true);
        }

        // Test that a source added in a high priority config file is not
        // disabled by <disabledPackageSources> in a low priority file.
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HighPrioritySourceDisabled(bool useStaticMethod)
        {
            // Arrange
            using (var directory = TestDirectory.Create())
            {
                var configContent1 = @"<configuration>
    <disabledPackageSources>
        <add key='a' value='true' />
    </disabledPackageSources>
</configuration>";
                var configContent2 = @"<configuration>
    <packageSources>
        <add key='a' value='http://a.test' />
    </packageSources>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile("nuget.config", Path.Combine(directory, "a", "b"), configContent1);
                SettingsTestUtils.CreateConfigurationFile("nuget.config", Path.Combine(directory, "a", "b", "c"), configContent2);

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);

                // Act
                List<PackageSource> values = LoadPackageSources(useStaticMethod, settings);

                // Assert
                Assert.Equal(1, values.Count);
                Assert.False(values[0].IsEnabled);
                Assert.Equal("a", values[0].Name);
                Assert.Equal("http://a.test", values[0].Source);
            }
        }

        // Test that a source added in a low priority config file is disabled
        // if it's listed in <disabledPackageSources> in a high priority file.
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LowPrioritySourceDisabled(bool useStaticMethod)
        {
            // Arrange
            using (var directory = TestDirectory.Create())
            {
                var configContent1 = @"<configuration>
    <disabledPackageSources>
        <add key='a' value='true' />
    </disabledPackageSources>
</configuration>";
                var configContent2 = @"<configuration>
    <packageSources>
        <add key='a' value='http://a.test' />
    </packageSources>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile("nuget.config", Path.Combine(directory, "a", "b"), configContent2);
                SettingsTestUtils.CreateConfigurationFile("nuget.config", Path.Combine(directory, "a", "b", "c"), configContent1);

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);

                // Act
                List<PackageSource> values = LoadPackageSources(useStaticMethod, settings);

                // Assert
                Assert.Equal(1, values.Count);
                Assert.False(values[0].IsEnabled);
                Assert.Equal("a", values[0].Name);
                Assert.Equal("http://a.test", values[0].Source);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void V2NotDisabled(bool useStaticMethod)
        {
            // Arrange
            using (var directory = TestDirectory.Create())
            {
                var configContent = @"<configuration>
    <packageSources>
        <add key='nuget.org' value='https://www.nuget.org/api/v2/' />
    </packageSources>
</configuration>";
                SettingsTestUtils.CreateConfigurationFile("nuget.config", directory, configContent);

                var settings = Settings.LoadSettings(
                    directory,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);

                // Act
                List<PackageSource> values = LoadPackageSources(useStaticMethod, settings);

                // Assert
                Assert.True(values.Single(p => p.Name.Equals("nuget.org", StringComparison.OrdinalIgnoreCase)).IsEnabled);
            }
        }

        [Fact]
        public void AddPackageSourcesWithConfigFile()
        {

            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                     @"<?xml version=""1.0""?>
<configuration>
    <packageSources>
        <add key='a' value='https://a.test' />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), configContents);

                var settings = Settings.LoadSettings(directory.Path,
                   configFileName: "NuGet.config",
                   machineWideSettings: null,
                   loadUserWideSettings: true,
                   useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act
                var sources = packageSourceProvider.LoadPackageSources().ToList();
                sources.Add(new PackageSource("https://b.test", "b"));
                packageSourceProvider.SavePackageSources(sources);

                // Assert
                Assert.Equal(
                      SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://b.test"" />
    </packageSources>
</configuration>
"),
                  SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(directory.Path, "NuGet.config"))));
            }
        }

        [Fact]
        public void SavePackageSources_AddDisabledSourceToTheConfigContainingSource()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var config1Contents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), config1Contents);

                var config2Contents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
</configuration>";

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());
                Directory.CreateDirectory(rootPath);
                File.WriteAllText(Path.Combine(rootPath, "NuGet.config"), config2Contents);

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act - 1
                var sources = packageSourceProvider.LoadPackageSources();

                // Assert - 2
                var source = Assert.Single(sources);
                Assert.Equal("a", source.Name);
                Assert.Equal("https://a.test", source.Source);
                Assert.True(source.IsEnabled);

                // Act - 2
                source.IsEnabled = false;
                packageSourceProvider.SavePackageSources(sources);

                // Assert - 3
                Assert.Equal(SettingsTestUtils.RemoveWhitespace(config2Contents),
                    SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(rootPath, "NuGet.config"))));
                Assert.Equal(
                        SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
    </packageSources>
    <disabledPackageSources>
        <add key=""a"" value=""true"" />
    </disabledPackageSources>
</configuration>
"), SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(directory.Path, "NuGet.config"))));
            }
        }

        [Fact]
        public void SavePackageSources_WritesToTheSettingsFileWithTheNearestPriority()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var config1Contents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
    </packageSources>
</configuration>
";
                var config2Contents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key='b' value='https://b.test' />
        <add key='a' value='https://a.test' />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), config1Contents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());
                Directory.CreateDirectory(rootPath);
                File.WriteAllText(Path.Combine(rootPath, "NuGet.config"), config2Contents);

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act - 1
                var sources = packageSourceProvider.LoadPackageSources();

                // Assert - 1
                Assert.Collection(sources,
                    source =>
                    {
                        Assert.Equal("b", source.Name);
                        Assert.Equal("https://b.test", source.Source);
                        Assert.True(source.IsEnabled);
                    },
                    source =>
                    {
                        Assert.Equal("a", source.Name);
                        Assert.Equal("https://a.test", source.Source);
                        Assert.True(source.IsEnabled);
                    });

                // Act - 2
                sources.Last().IsEnabled = false;
                packageSourceProvider.SavePackageSources(sources);

                // Assert - 2
                Assert.Equal(SettingsTestUtils.RemoveWhitespace(config1Contents),
                    SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(directory.Path, "NuGet.config"))));

                Assert.Equal(SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
            <add key=""b"" value=""https://b.test"" />
            <add key=""a"" value=""https://a.test"" />
    </packageSources>
    <disabledPackageSources>
        <add key=""a"" value=""true"" />
    </disabledPackageSources>
</configuration>"), SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(rootPath, "NuGet.config"))));
            }
        }

        [Fact]
        public void SavePackageSources_AddsNewSourcesToTheSettingWithLowestPriority()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var config1Contents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
    </packageSources>
</configuration>";

                var config2Contents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key='b' value='https://b.test' />
    </packageSources>
</configuration>";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), config1Contents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());
                Directory.CreateDirectory(rootPath);
                File.WriteAllText(Path.Combine(rootPath, "NuGet.config"), config2Contents);

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act - 1
                var sources = packageSourceProvider.LoadPackageSources().ToList();

                // Assert - 1
                Assert.Collection(sources,
                    source =>
                    {
                        Assert.Equal("b", source.Name);
                        Assert.Equal("https://b.test", source.Source);
                        Assert.True(source.IsEnabled);
                    },
                    source =>
                    {
                        Assert.Equal("a", source.Name);
                        Assert.Equal("https://a.test", source.Source);
                        Assert.True(source.IsEnabled);
                    }
                    );

                // Act - 2
                sources[1].IsEnabled = false;
                sources.Add(new PackageSource("http://c.test", "c"));

                packageSourceProvider.SavePackageSources(sources);

                // Assert - 2
                Assert.Equal(SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <configuration>
                    <packageSources>
                        <add key=""a"" value=""https://a.test"" />
                        <add key=""c"" value=""http://c.test"" />
                    </packageSources>
                    <disabledPackageSources>
                        <add key=""a"" value=""true"" />
                    </disabledPackageSources>
                </configuration>
                "), SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(directory.Path, "NuGet.config"))));

                Assert.Equal(SettingsTestUtils.RemoveWhitespace(config2Contents), SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(rootPath, "NuGet.config"))));
            }
        }

        [Fact]
        public void SavePackageSources_AddsOrderingForCollapsedFeeds()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://new.b.test"" protocolVersion=""3"" />
        <add key=""c"" value=""https://c.test"" />
    </packageSources>
</configuration>";
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.config"), configContents);

                var rootPath = Path.Combine(directory.Path, Path.GetRandomFileName());

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act - 1
                var sources = packageSourceProvider.LoadPackageSources().ToList();

                // Assert - 2
                Assert.Collection(sources,
                    source =>
                        {
                            Assert.Equal("a", source.Name);
                            Assert.Equal("https://a.test", source.Source);
                            Assert.True(source.IsEnabled);
                        },
                    source =>
                        {
                            Assert.Equal("b", source.Name);
                            Assert.Equal("https://new.b.test", source.Source);
                            Assert.True(source.IsEnabled);
                            Assert.Equal(3, source.ProtocolVersion);
                        },
                    source =>
                        {
                            Assert.Equal("c", source.Name);
                            Assert.Equal("https://c.test", source.Source);
                            Assert.True(source.IsEnabled);
                        });

                // Act - 2
                sources[1].Source = "https://newer.b.test";
                var sourcesToSave = new[]
                    {
                        sources[1], sources[2], sources[0]
                    };
                packageSourceProvider.SavePackageSources(sourcesToSave);

                // Assert - 2
                Assert.Equal(SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""a"" value=""https://a.test"" />
        <add key=""b"" value=""https://newer.b.test"" protocolVersion=""3"" />
        <add key=""c"" value=""https://c.test"" />
    </packageSources>
</configuration>"), SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(directory.Path, "NuGet.config"))));
            }
        }

        [Fact]
        public void SavePackageSources_DisabledOnMachineWideSource()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""Microsoft and .NET""
         value = ""https://a.test"" />
        <add key=""test1""
         value = ""//test/source"" />
    </packageSources>
</configuration>
";

                File.WriteAllText(Path.Combine(directory.Path, "machinewide.config"), configContents);

                var machineWideSetting = new Settings(directory.Path, "machinewide.config", isMachineWide: true);
                var m = new Mock<IMachineWideSettings>();
                m.SetupGet(obj => obj.Settings).Returns(machineWideSetting);

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: m.Object,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();

                // Act
                sources[2].IsEnabled = false;
                packageSourceProvider.SavePackageSources(sources);

                // Assert
                var newSources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.True(newSources[1].IsEnabled);
                Assert.Equal("Microsoft and .NET", newSources[1].Name);

                Assert.False(newSources[2].IsEnabled);
                Assert.Equal("test1", newSources[2].Name);
            }
        }

        [Fact]
        public void SavePackageSources_WhenDisablingASourceFromReadOnlyConfig_DisablesInDefaultUserWideConfigInstead()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var additionalConfigContents =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""Contoso""
         value = ""https://contoso.test"" />
    </packageSources>
</configuration>
";

                var machineWideContents =
    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    </packageSources>
</configuration>
";

                File.WriteAllText(Path.Combine(directory.Path, "machinewide.config"), machineWideContents);
                var additionalConfigPath = Path.Combine(directory.Path, "TestingGlobalPath", "config", "contoso.nuget.config");
                Directory.CreateDirectory(Path.GetDirectoryName(additionalConfigPath));
                File.WriteAllText(additionalConfigPath, additionalConfigContents);

                var machineWideSetting = new Settings(directory.Path, "machinewide.config", isMachineWide: true);
                var m = new Mock<IMachineWideSettings>();
                m.SetupGet(obj => obj.Settings).Returns(machineWideSetting);

                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: m.Object,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);
                var packageSourceProvider = new PackageSourceProvider(settings);
                var sources = packageSourceProvider.LoadPackageSources().ToList();


                // Act
                sources.Count.Should().Be(2);
                sources[1].IsEnabled = false;
                packageSourceProvider.SavePackageSources(sources);

                // Assert
                var newSources = packageSourceProvider.LoadPackageSources().ToList();
                Assert.False(newSources[1].IsEnabled);
                Assert.Equal("Contoso", newSources[1].Name);

                SettingsTestUtils.RemoveWhitespace(File.ReadAllText(additionalConfigPath)).Should().Be(SettingsTestUtils.RemoveWhitespace(additionalConfigContents));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DisabledMachineWideSourceByDefaultWithNull(bool useStaticMethod)
        {
            using (var directory = TestDirectory.Create())
            {
                CreateSettingsFileInTestingGlobalDirectory(directory);

                // Arrange
                var settings = Settings.LoadSettings(
                    new DirectoryInfo(directory),
                    machineWideSettings: null,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);

                // Act
                List<PackageSource> sources = LoadPackageSources(useStaticMethod, settings);

                // Assert
                Assert.Equal(0, sources.Count);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSourceEmptyConfigFileOnUserMachine(bool useStaticMethod)
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                     @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
      <clear />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "nuget.config"), configContents);
                var settings = Settings.LoadSettings(directory.Path,
                                  configFileName: null,
                                  machineWideSettings: null,
                                  loadUserWideSettings: true,
                                  useTestingGlobalPath: true);

                // Act
                List<PackageSource> sources = LoadPackageSources(useStaticMethod, settings);

                // Assert
                Assert.Equal(0, sources.Count);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadPackageSourceLocalConfigFileOnUserMachine(bool useStaticMethod)
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                     @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
      <clear />
      <add key=""a"" value=""https://a.test"" />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "nuget.config"), configContents);
                var settings = Settings.LoadSettings(directory.Path,
                                  configFileName: null,
                                  machineWideSettings: null,
                                  loadUserWideSettings: true,
                                  useTestingGlobalPath: true);

                // Act
                List<PackageSource> sources = LoadPackageSources(useStaticMethod, settings);

                // Assert
                Assert.Equal(1, sources.Count);
                Assert.Equal("https://a.test", sources[0].Source);
                Assert.Equal("a", sources[0].Name);
            }
        }

        [Fact]

        public void SavePackageSources_IgnoreSettingBeforeClear()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                     @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
      <clear />
      <add key=""a"" value=""https://a.test"" />
    </packageSources>
</configuration>
";
                File.WriteAllText(Path.Combine(directory.Path, "nuget.config"), configContents);
                var settings = Settings.LoadSettings(directory.Path,
                                  configFileName: null,
                                  machineWideSettings: null,
                                  loadUserWideSettings: true,
                                  useTestingGlobalPath: true);
                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act
                var sources = packageSourceProvider.LoadPackageSources().ToList();

                packageSourceProvider.SavePackageSources(sources);

                // Assert
                var text = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(Path.Combine(directory, "TestingGlobalPath", "NuGet.Config")));
                var result = SettingsTestUtils.RemoveWhitespace(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""nuget.org"" value=""https://api.nuget.org/v3/index.json"" protocolVersion=""3"" />
  </packageSources>
</configuration>");
                Assert.Equal(result, text);
            }
        }

        [Fact]
        public void SavePackageSources_ThrowWhenConfigReadOnly()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
                     @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
      <clear />
      <add key=""a"" value=""https://a.test"" />
    </packageSources>
</configuration>
";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);
                File.SetAttributes(Path.Combine(directory.Path, "NuGet.Config"), FileAttributes.ReadOnly);

                try
                {
                    var settings = Settings.LoadSettings(directory.Path,
                        configFileName: null,
                        machineWideSettings: null,
                        loadUserWideSettings: true,
                        useTestingGlobalPath: true);
                    var packageSourceProvider = new PackageSourceProvider(settings);

                    // Act
                    var sources = packageSourceProvider.LoadPackageSources().ToList();

                    sources.Add(new PackageSource("https://b.test", "b"));

                    var ex = Assert.Throws<NuGetConfigurationException>(() => packageSourceProvider.SavePackageSources(sources));

                    // Assert
                    var path = Path.Combine(directory, "NuGet.Config");
                    Assert.Equal($"Failed to read NuGet.Config due to unauthorized access. Path: '{path}'.", ex.Message);
                }
                finally
                {
                    File.SetAttributes(Path.Combine(directory.Path, "NuGet.Config"), FileAttributes.Normal);
                }
            }
        }

        [Fact]
        public void DefaultPushSourceInNuGetConfig()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContentsWithDefault =
@"<?xml version='1.0'?>
<configuration>
    <config>
        <add key='DefaultPushSource' value='\\myshare.test\packages' />
    </config>
    <packageSources>
        <add key='a' value='https://a.test' />
    </packageSources>
</configuration>";
                var configContentWithoutDefault = configContentsWithDefault.Replace("DefaultPushSource", "WithoutDefaultPushSource");

                File.WriteAllText(Path.Combine(directory.Path, "WithDefaultPushSource.config"), configContentsWithDefault);
                File.WriteAllText(Path.Combine(directory.Path, "WithoutDefaultPushSource.config"), configContentWithoutDefault);

                var settingsWithDefault = Settings.LoadSettings(directory.Path,
                   configFileName: "WithDefaultPushSource.config",
                   machineWideSettings: null,
                   loadUserWideSettings: true,
                   useTestingGlobalPath: false);

                var settingsWithoutDefault = Settings.LoadSettings(directory.Path,
                   configFileName: "WithoutDefaultPushSource.config",
                   machineWideSettings: null,
                   loadUserWideSettings: true,
                   useTestingGlobalPath: false);

                var packageSourceProviderWithDefault = new PackageSourceProvider(settingsWithDefault);
                var packageSourceProviderWithoutDefault = new PackageSourceProvider(settingsWithoutDefault);

                // Act
                var defaultPushSourceWithDefault = packageSourceProviderWithDefault.DefaultPushSource;
                var defaultPushSourceWithoutDefault = packageSourceProviderWithoutDefault.DefaultPushSource;

                // Assert
                Assert.Equal(@"\\myshare.test\packages", defaultPushSourceWithDefault);
                Assert.True(string.IsNullOrEmpty(defaultPushSourceWithoutDefault));
            }
        }

        [Fact]
        public void LoadPackageSources_DoesNotDecryptPassword()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
      <clear />
      <add key=""a"" value=""https://a.test"" />
    </packageSources>
<packageSourceCredentials>
    <a>
      <add key='Username' value='myusername' />
      <add key='Password' value='random-encrypted-password' />
    </a>
  </packageSourceCredentials>
</configuration>";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);
                var settings = Settings.LoadSettings(directory.Path,
                                  configFileName: null,
                                  machineWideSettings: null,
                                  loadUserWideSettings: false,
                                  useTestingGlobalPath: false);
                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act
                var sources = packageSourceProvider.LoadPackageSources().ToList();

                // Assert
                Assert.Equal(1, sources.Count);
                Assert.Equal("a", sources[0].Name);
                Assert.Equal("https://a.test", sources[0].Source);
                AssertCredentials(sources[0].Credentials, "a", "myusername", "random-encrypted-password", isPasswordClearText: false);
            }
        }

        [Fact]
        public void LoadPackageSources_DoesNotLoadClearedSource()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
      <add key=""a"" value=""https://a.test"" />
    </packageSources>
<packageSourceCredentials>
    <test>
      <add key='Username' value='myusername' />
      <add key='Password' value='removed' />
    </test>
  </packageSourceCredentials>
</configuration>
";
                var configContents1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
      <clear />
      <add key=""b"" value=""https://b.test"" />
    </packageSources>
</configuration>
";
                SettingsTestUtils.CreateConfigurationFile(
                    "nuget.config",
                    Path.Combine(directory, "TestingGlobalPath"),
                    configContents);
                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents1);
                var settings = Settings.LoadSettings(
                    directory.Path,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: true,
                    useTestingGlobalPath: true);
                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act
                var sources = packageSourceProvider.LoadPackageSources().ToList();

                // Assert
                Assert.Equal(1, sources.Count);
                Assert.Equal("b", sources[0].Name);
                Assert.Equal("https://b.test", sources[0].Source);
                Assert.Null(sources[0].Credentials);
            }
        }

        [Fact]
        public void LoadPackageSources_SetMaxHttpRequest()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
@"<?xml version='1.0'?>
<configuration>
    <config>
        <add key='maxHttpRequestsPerSource' value='2' />
    </config>
    <packageSources>
        <add key='a' value='https://a.test' />
    </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);

                var settings = Settings.LoadSettings(directory.Path,
                   configFileName: null,
                   machineWideSettings: null,
                   loadUserWideSettings: true,
                   useTestingGlobalPath: false);

                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act
                var packageSources = packageSourceProvider.LoadPackageSources();

                // Assert
                Assert.True(packageSources.All(p => p.MaxHttpRequestsPerSource == 2));
            }
        }

        [Fact]
        public void LoadPackageSources_NoMaxHttpRequest()
        {
            using (var directory = TestDirectory.Create())
            {
                // Arrange
                var configContents =
@"<?xml version='1.0'?>
<configuration>
    <packageSources>
        <add key='a' value='https://a.test' />
    </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(directory.Path, "NuGet.Config"), configContents);

                var settings = Settings.LoadSettings(directory.Path,
                   configFileName: "NuGet.Config",
                   machineWideSettings: null,
                   loadUserWideSettings: true,
                   useTestingGlobalPath: false);

                var packageSourceProvider = new PackageSourceProvider(settings);

                // Act
                var packageSources = packageSourceProvider.LoadPackageSources();

                // Assert
                Assert.True(packageSources.All(p => p.MaxHttpRequestsPerSource == 0));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PackageSourcesChanged_EventRunsSubscriptions(bool subscribeToEvent)
        {
            // Arrange
            var setting = new Mock<ISettings>();
#pragma warning disable CS0618 // Type or member is obsolete
            var target = new PackageSourceProvider(setting.Object, subscribeToEvent);
#pragma warning restore CS0618 // Type or member is obsolete
            bool eventRun = false;
            target.PackageSourcesChanged += (s, e) => { eventRun = true; };

            // Act
            setting.Raise(s => s.SettingsChanged += null, (EventArgs)null);

            // Assert
            Assert.Equal(subscribeToEvent, eventRun);
        }

        private string CreateNuGetConfigContent(string enabledReplacement = "", string disabledReplacement = "", string activeSourceReplacement = "")
        {
            var nugetConfigBaseString = new StringBuilder();
            nugetConfigBaseString.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
            nugetConfigBaseString.AppendLine("<configuration>");
            nugetConfigBaseString.AppendLine("<packageRestore>");
            nugetConfigBaseString.AppendLine(@"<add key='enabled' value='True' />");
            nugetConfigBaseString.AppendLine(@"<add key='automatic' value='True' />");
            nugetConfigBaseString.AppendLine("</packageRestore>");
            nugetConfigBaseString.AppendLine("<packageSources>");
            nugetConfigBaseString.AppendLine("[EnabledSources]");
            nugetConfigBaseString.AppendLine("</packageSources>");
            nugetConfigBaseString.AppendLine("<disabledPackageSources>");
            nugetConfigBaseString.AppendLine("[DisabledSources]");
            nugetConfigBaseString.AppendLine("</disabledPackageSources>");
            nugetConfigBaseString.AppendLine("<activePackageSource>");
            nugetConfigBaseString.AppendLine("[ActiveSource]");
            nugetConfigBaseString.AppendLine("</activePackageSource>");
            nugetConfigBaseString.AppendLine("</configuration>");

            var nugetConfig = nugetConfigBaseString.ToString();
            nugetConfig = nugetConfig.Replace("[EnabledSources]", enabledReplacement);
            nugetConfig = nugetConfig.Replace("[DisabledSources]", disabledReplacement);
            nugetConfig = nugetConfig.Replace("[ActiveSource]", activeSourceReplacement);
            return nugetConfig;
        }

        private List<PackageSource> LoadPackageSources(bool useStaticMethod, ISettings settings)
        {
            if (useStaticMethod)
            {
                return PackageSourceProvider.LoadPackageSources(settings).ToList();
            }
            else
            {
                var provider = new PackageSourceProvider(settings);
                return provider.LoadPackageSources().ToList();
            }
        }

        private void AssertPackageSource(PackageSource ps, string name, string source, bool isEnabled, bool isMachineWide = false, bool isOfficial = false)
        {
            Assert.Equal(name, ps.Name);
            Assert.Equal(source, ps.Source);
            Assert.True(ps.IsEnabled == isEnabled);
            Assert.True(ps.IsMachineWide == isMachineWide);
            Assert.True(ps.IsOfficial == isOfficial);
        }

        private void AssertCredentials(PackageSourceCredential actual, string source, string userName, string passwordText, bool isPasswordClearText = true)
        {
            Assert.NotNull(actual);
            Assert.Equal(source, actual.Source);
            Assert.Equal(userName, actual.Username);
            Assert.Equal(passwordText, actual.PasswordText);
            Assert.Equal(isPasswordClearText, actual.IsPasswordClearText);
        }

        private static void CreateSettingsFileInTestingGlobalDirectory(TestDirectory directory)
        {
            var settingsFile = new FileInfo(Path.Combine(directory.Path, "TestingGlobalPath", "NuGet.Config"));

            settingsFile.Directory.Create();

            File.WriteAllText(settingsFile.FullName, @"<?xml version=""1.0"" encoding=""utf-8""?><configuration />");
        }
    }
}
