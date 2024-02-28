// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class EnvironmentSupportTests
    {
        private readonly string _defaultNuGetConfigurationWithEnvironmentVariable = @"
<configuration>
    <config>
        <add key='repositoryPath' value='%RP_ENV_VAR%' />
    </config>
</configuration>";

        [Fact]
        public void GetValueEvaluatesEnvironmentVariable()
        {
            //Arrange
            var expectedRepositoryPath = @"ONE";

            using (var nugetConfigFileFolder = TestDirectory.Create())
            {
                var nugetConfigFile = "NuGet.config";
                var nugetConfigFilePath = Path.Combine(nugetConfigFileFolder, nugetConfigFile);

                File.WriteAllText(nugetConfigFilePath, _defaultNuGetConfigurationWithEnvironmentVariable);

                Environment.SetEnvironmentVariable("RP_ENV_VAR", "ONE");

                //Act
                var settings = new Settings(nugetConfigFileFolder, nugetConfigFile);

                //Assert
                Assert.Equal(SettingsUtility.GetValueForAddItem(settings, "config", "repositoryPath", isPath: true), Path.Combine(nugetConfigFileFolder, expectedRepositoryPath));
            }
        }

        [Fact]
        public void GetSettingValuesEvaluatesEnvironmentVariable()
        {
            //Arrange
            var expectedRepositoryPath = @"ONE";

            using (var nugetConfigFileFolder = TestDirectory.Create())
            {
                var nugetConfigFile = "NuGet.config";
                var nugetConfigFilePath = Path.Combine(nugetConfigFileFolder, nugetConfigFile);

                File.WriteAllText(nugetConfigFilePath, _defaultNuGetConfigurationWithEnvironmentVariable);

                Environment.SetEnvironmentVariable("RP_ENV_VAR", "ONE");

                //Act
                var settings = new Settings(nugetConfigFileFolder, nugetConfigFile);

                //Assert
                var settingsForConfig = settings.GetSection("config")?.Items.OfType<AddItem>()!;
                Assert.Single(settingsForConfig);
                Assert.Equal(settingsForConfig.Single().GetValueAsPath(), Path.Combine(nugetConfigFileFolder, expectedRepositoryPath));
            }
        }

        [Fact]
        public void GetValueEvaluatesEnvironmentVariableWithAbsolutePath()
        {
            //Arrange
            var expectedRepositoryPath = @"/home/log";
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                expectedRepositoryPath = @"C:\log";
            }

            using (var nugetConfigFileFolder = TestDirectory.Create())
            {
                var nugetConfigFile = "NuGet.config";
                var nugetConfigFilePath = Path.Combine(nugetConfigFileFolder, nugetConfigFile);

                File.WriteAllText(nugetConfigFilePath, _defaultNuGetConfigurationWithEnvironmentVariable);

                Environment.SetEnvironmentVariable("RP_ENV_VAR", expectedRepositoryPath);

                //Act
                var settings = new Settings(nugetConfigFileFolder, nugetConfigFile);

                //Assert
                Assert.Equal(expectedRepositoryPath, SettingsUtility.GetValueForAddItem(settings, "config", "repositoryPath", isPath: true));
            }
        }

        [Fact]
        public void GetSettingValuesEvaluatesEnvironmentVariableWithAbsolutePath()
        {
            //Arrange
            var expectedRepositoryPath = @"/home/log";
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                expectedRepositoryPath = @"C:\log";
            }

            using (var nugetConfigFileFolder = TestDirectory.Create())
            {
                var nugetConfigFile = "NuGet.config";
                var nugetConfigFilePath = Path.Combine(nugetConfigFileFolder, nugetConfigFile);

                File.WriteAllText(nugetConfigFilePath, _defaultNuGetConfigurationWithEnvironmentVariable);

                Environment.SetEnvironmentVariable("RP_ENV_VAR", expectedRepositoryPath);

                //Act
                var settings = new Settings(nugetConfigFileFolder, nugetConfigFile);

                //Assert
                var settingsForConfig = settings.GetSection("config")?.Items.OfType<AddItem>()!;
                Assert.Single(settingsForConfig);
                Assert.Equal(expectedRepositoryPath, settingsForConfig.Single().GetValueAsPath());
            }
        }
    }
}
