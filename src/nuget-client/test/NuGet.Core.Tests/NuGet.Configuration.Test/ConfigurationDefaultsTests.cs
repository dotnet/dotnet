// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration
{
    public class ConfigurationDefaultsTests
    {
        [Fact]
        public void CreateConfigurationDefaultsReturnsNonNullConfigurationDefaults()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.CreateInTemp())
            {
                ConfigurationDefaults ConfigurationDefaults = GetConfigurationDefaults(@"<configuration></configuration>", mockBaseDirectory);

                // Act & Assert
                Assert.NotNull(ConfigurationDefaults);
            }
        }

        [Fact]
        public void ConfigDefaultsAreProperlyReadFromConfigDefaultsFile()
        {
            //Arrange
            var name1 = "Contoso Package Source";
            var name2 = "My Test Package Source";
            var feed1 = "http://contoso.com/packages/";
            var feed2 = "http://wwww.somerandomURL.com/";

            using (var nugetConfigFileFolder = TestDirectory.CreateInTemp())
            {
                var nugetConfigFile = "NuGetDefaults.config";
                var nugetConfigFilePath = Path.Combine(nugetConfigFileFolder, nugetConfigFile);

                var enabledReplacement = @"<add key='" + name1 + "' value='" + feed1 + "' />";

                enabledReplacement = enabledReplacement + @"<add key='" + name2 + "' value='" + feed2 + "' />";
                var disabledReplacement = string.Empty;
                var defaultPushSource = "<add key='DefaultPushSource' value='" + feed2 + "' />";
                File.WriteAllText(nugetConfigFilePath, CreateNuGetConfigContent(enabledReplacement, disabledReplacement, defaultPushSource));

                //Act
                ConfigurationDefaults configDefaults = new ConfigurationDefaults(nugetConfigFileFolder, nugetConfigFile);
                IEnumerable<PackageSource> defaultSourcesFromConfigFile = configDefaults.DefaultPackageSources;
                string packageRestore = configDefaults.DefaultPackageRestoreConsent;
                string packagePushSource = configDefaults.DefaultPushSource;

                //Assert
                VerifyPackageSource(defaultSourcesFromConfigFile, 2, new string[] { name1, name2 }, new string[] { feed1, feed2 });
                Assert.Equal(feed2, packagePushSource);
                Assert.Equal("true", packageRestore.ToLowerInvariant());
            }
        }

        [Fact]
        public void CreateConfigurationDefaultsThrowsWhenXmlIsInvalid()
        {
            //Arrange
            var name1 = "Contoso Package Source";
            var name2 = "My Test Package Source";
            var feed1 = "http://contoso.com/packages/";
            var feed2 = "http://wwww.somerandomURL.com/";

            using (var nugetConfigFileFolder = TestDirectory.CreateInTemp())
            {
                var nugetConfigFile = "NuGetDefaults.config";
                var nugetConfigFilePath = Path.Combine(nugetConfigFileFolder, nugetConfigFile);

                var enabledReplacement = @"<add key='" + name1 + "' value='" + feed1;

                enabledReplacement = enabledReplacement + @"<add key='" + name2 + "' value='" + feed2;
                var disabledReplacement = string.Empty;
                var defaultPushSource = "<add key='DefaultPushSource' value='" + feed2 + "' />";
                File.WriteAllText(nugetConfigFilePath, CreateNuGetConfigContent(enabledReplacement, disabledReplacement, defaultPushSource));

                Assert.Throws<NuGetConfigurationException>(() =>
                {
                    ConfigurationDefaults configDefaults = new ConfigurationDefaults(nugetConfigFileFolder, nugetConfigFile);
                });
            }
        }

        [Fact]
        public void GetDefaultPushSourceReturnsNull()
        {
            //Arrange
            using (var nugetConfigFileFolder = TestDirectory.CreateInTemp())
            {
                var nugetConfigFile = "NuGetDefaults.config";
                var nugetConfigFilePath = Path.Combine(nugetConfigFileFolder, nugetConfigFile);

                var configurationDefaultsContent = @"<configuration></configuration>";
                File.WriteAllText(nugetConfigFilePath, configurationDefaultsContent);

                ConfigurationDefaults configDefaults = new ConfigurationDefaults(nugetConfigFileFolder, nugetConfigFile);
                Assert.Null(configDefaults.DefaultPushSource);
            }
        }

        [Fact]
        public void GetDefaultPushSourceReadsTheCorrectValue()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.CreateInTemp())
            {
                var configurationDefaultsContent = @"
<configuration>
     <config>
        <add key='DefaultPushSource' value='http://contoso.com/packages/' />
    </config>
</configuration>";

                // Act & Assert
                ConfigurationDefaults ConfigurationDefaults = GetConfigurationDefaults(configurationDefaultsContent, mockBaseDirectory);

                Assert.Equal("http://contoso.com/packages/", ConfigurationDefaults.DefaultPushSource);
            }
        }

        [Fact]
        public void GetDefaultPackageSourcesReturnsValidPackageSources()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.CreateInTemp())
            {
                var configurationDefaultsContent = @"
<configuration>
    <packageSources>
        <add key='Contoso Package Source' value='http://contoso.com/packages/' />
        <add key='NuGet Official Feed' value='http://www.nuget.org/api/v2/' />
    </packageSources>
    <disabledPackageSources>
        <add key='NuGet Official Feed' value='true' />
    </disabledPackageSources>
</configuration>";
                // Act & Assert
                ConfigurationDefaults ConfigurationDefaults = GetConfigurationDefaults(configurationDefaultsContent, mockBaseDirectory);

                Assert.NotNull(ConfigurationDefaults.DefaultPackageSources);

                List<PackageSource> defaultPackageSources = ConfigurationDefaults.DefaultPackageSources.ToList();

                Assert.Equal(defaultPackageSources.Count, 2);

                Assert.Equal(defaultPackageSources[0].Name, "Contoso Package Source");
                Assert.True(defaultPackageSources[0].IsEnabled);
                Assert.True(defaultPackageSources[0].IsOfficial);

                Assert.Equal(defaultPackageSources[1].Name, "NuGet Official Feed");
                Assert.False(defaultPackageSources[1].IsEnabled);
                Assert.True(defaultPackageSources[1].IsOfficial);
            }
        }

        [Fact]
        public void GetDefaultPackageSourcesReturnsEmptyList()
        {
            //Arrange
            using (var nugetConfigFileFolder = TestDirectory.CreateInTemp())
            {
                var nugetConfigFile = "NuGetDefaults.config";
                var nugetConfigFilePath = Path.Combine(nugetConfigFileFolder, nugetConfigFile);

                var configurationDefaultsContent = @"<configuration></configuration>";
                File.WriteAllText(nugetConfigFilePath, configurationDefaultsContent);

                ConfigurationDefaults configDefaults = new ConfigurationDefaults(nugetConfigFileFolder, nugetConfigFile);
                Assert.True(configDefaults.DefaultPackageSources.ToList().Count == 0);
            }
        }

        [Fact]
        public void GetDefaultPackageSourcesFromSourceProvider()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.CreateInTemp())
            {
                var configurationDefaultsContent = @"
<configuration>
    <packageSources>
        <add key='Contoso Package Source' value='http://contoso.com/packages/' />
    </packageSources>
      <config>
        <add key='DefaultPushSource' value='http://contoso.com/packages/' />
    </config>
</configuration>";
                var config = @"
<configuration>
    <packageSources>
        <add key='v2' value='http://www.nuget.org/api/v2/' />
    </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(mockBaseDirectory, "NuGet.Config"), config);
                var settings = Settings.LoadSettings(mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                ConfigurationDefaults ConfigurationDefaults = GetConfigurationDefaults(configurationDefaultsContent, mockBaseDirectory);

                var packageSourceProvider = new PackageSourceProvider(settings, ConfigurationDefaults.DefaultPackageSources);

                // Act
                List<PackageSource> packageSources = packageSourceProvider.LoadPackageSources().ToList();

                // Assert
                Assert.Equal("http://contoso.com/packages/", ConfigurationDefaults.DefaultPushSource);
                Assert.Equal(2, packageSources.Count());
                Assert.Equal("v2", packageSources[0].Name);
                Assert.Equal("Contoso Package Source", packageSources[1].Name);
            }
        }

        [Fact]
        public void GetDefaultPackageSources_LocalizatedPackagesourceKeys_ConsideredDiffererent()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.CreateInTemp())
            {
                var configurationDefaultsContent = @"
<configuration>
    <packageSources>
        <add key='encyclopaedia' value='http://contoso.com/packages1/' />
        <add key='encyclopædia' value='http://contoso.com/packages2/' />
    </packageSources>
</configuration>";

                var config = @"
<configuration>
    <packageSources>
        <add key='v2' value='http://www.nuget.org/api/v2/' />
    </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(mockBaseDirectory, "NuGet.Config"), config);
                var settings = Settings.LoadSettings(mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                ConfigurationDefaults ConfigurationDefaults = GetConfigurationDefaults(configurationDefaultsContent, mockBaseDirectory);

                List<PackageSource> defaultSources = ConfigurationDefaults.DefaultPackageSources.ToList();
                var packageSourceProvider = new PackageSourceProvider(settings, ConfigurationDefaults.DefaultPackageSources);

                // Act
                List<PackageSource> packageSources = packageSourceProvider.LoadPackageSources().ToList();

                // Assert
                Assert.Equal(3, packageSources.Count());
                Assert.Equal(2, defaultSources.Count());
                Assert.Equal("v2", packageSources[0].Name);
                Assert.Equal("encyclopaedia", packageSources[1].Name);
                Assert.Equal("encyclopaedia", defaultSources[0].Name);
                Assert.Equal("encyclopædia", packageSources[2].Name);
                Assert.Equal("encyclopædia", defaultSources[1].Name);
            }
        }

        public void GetDefaultSameNamePackageSourcesFromSourceProvider()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.CreateInTemp())
            {
                var configurationDefaultsContent = @"
<configuration>
    <packageSources>
        <add key='Contoso Package Source' value='http://contoso.com/packages/' />
    </packageSources>
</configuration>";
                var config = @"
<configuration>
    <packageSources>
        <add key='Contoso Package Source' value='http://www.nuget.org/api/v2/' />
    </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(mockBaseDirectory, "NuGet.Config"), config);
                var settings = Settings.LoadSettings(mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                ConfigurationDefaults ConfigurationDefaults = GetConfigurationDefaults(configurationDefaultsContent, mockBaseDirectory);

                var packageSourceProvider = new PackageSourceProvider(settings, ConfigurationDefaults.DefaultPackageSources);

                // Act
                List<PackageSource> packageSources = packageSourceProvider.LoadPackageSources().ToList();

                // Assert
                Assert.Equal(1, packageSources.Count());
                Assert.Equal("Contoso Package Source", packageSources[0].Name);
                Assert.Equal("http://www.nuget.org/api/v2/", packageSources[0].Source);
            }
        }

        public void GetDefaultSameSourcePackageSourcesFromSourceProvider()
        {
            // Arrange
            using (var mockBaseDirectory = TestDirectory.CreateInTemp())
            {
                var configurationDefaultsContent = @"
<configuration>
    <packageSources>
        <add key='Contoso Package Source' value='http://www.nuget.org/api/v2/' />
    </packageSources>
</configuration>";
                var config = @"
<configuration>
    <packageSources>
        <add key='v2' value='http://www.nuget.org/api/v2/' />
    </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(mockBaseDirectory, "NuGet.Config"), config);
                var settings = Settings.LoadSettings(mockBaseDirectory,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false);
                ConfigurationDefaults ConfigurationDefaults = GetConfigurationDefaults(configurationDefaultsContent, mockBaseDirectory);

                var packageSourceProvider = new PackageSourceProvider(settings, ConfigurationDefaults.DefaultPackageSources);

                // Act
                List<PackageSource> packageSources = packageSourceProvider.LoadPackageSources().ToList();

                // Assert
                Assert.Equal(1, packageSources.Count());
                Assert.Equal("v2", packageSources[0].Name);
                Assert.Equal("http://www.nuget.org/api/v2/", packageSources[0].Source);
            }
        }

        private string CreateNuGetConfigContent(string enabledReplacement = "", string disabledReplacement = "", string defaultPushSource = "")
        {
            var nugetConfigBaseString = new StringBuilder();
            nugetConfigBaseString.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
            nugetConfigBaseString.AppendLine("<configuration>");
            nugetConfigBaseString.AppendLine("<packageRestore>");
            nugetConfigBaseString.AppendLine(@"<add key='enabled' value='True' />");
            nugetConfigBaseString.AppendLine(@"<add key='automatic' value='True' />");
            nugetConfigBaseString.AppendLine("</packageRestore>");
            nugetConfigBaseString.AppendLine("<config>");
            nugetConfigBaseString.AppendLine("[DefaultPushSource]");
            nugetConfigBaseString.AppendLine("</config>");
            nugetConfigBaseString.AppendLine("<packageSources>");
            nugetConfigBaseString.AppendLine("[EnabledSources]");
            nugetConfigBaseString.AppendLine("</packageSources>");
            nugetConfigBaseString.AppendLine("<disabledPackageSources>");
            nugetConfigBaseString.AppendLine("[DisabledSources]");
            nugetConfigBaseString.AppendLine("</disabledPackageSources>");
            nugetConfigBaseString.AppendLine("<activePackageSource>");
            nugetConfigBaseString.AppendLine(@"<add key='All' value='(Aggregate source)' />");
            nugetConfigBaseString.AppendLine("</activePackageSource>");
            nugetConfigBaseString.AppendLine("</configuration>");

            var nugetConfig = nugetConfigBaseString.ToString();
            nugetConfig = nugetConfig.Replace("[EnabledSources]", enabledReplacement);
            nugetConfig = nugetConfig.Replace("[DisabledSources]", disabledReplacement);
            nugetConfig = nugetConfig.Replace("[DefaultPushSource]", defaultPushSource);
            return nugetConfig;
        }

        private void VerifyPackageSource(IEnumerable<PackageSource> toVerify, int count, string[] names, string[] feeds)
        {
            List<PackageSource> toVerifyList = new List<PackageSource>();
            toVerifyList = toVerify.ToList();

            Assert.Equal(toVerifyList.Count, count);
            var index = 0;
            foreach (PackageSource ps in toVerifyList)
            {
                Assert.Equal(ps.Name, names[index]);
                Assert.Equal(ps.Source, feeds[index]);
                index++;
            }
        }

        private ConfigurationDefaults GetConfigurationDefaults(string configurationDefaultsContent, TestDirectory mockBaseDirectory)
        {
            var configurationDefaultsPath = "NuGetDefaults.config";

            File.WriteAllText(Path.Combine(mockBaseDirectory, configurationDefaultsPath), configurationDefaultsContent);
            return new ConfigurationDefaults(mockBaseDirectory, configurationDefaultsPath);
        }
    }
}
