// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Test.Utility
{
    public static class TestSources
    {
        public const string ConfigFile = "NuGet.Core.FuncTests.Config";
        public const string NuGetV2Uri = @"https://www.nuget.org/api/v2";

        public const string Artifactory = nameof(Artifactory);
        public const string Klondike = nameof(Klondike);
        public const string MyGet = nameof(MyGet);
        public const string Nexus = nameof(Nexus);
        public const string NuGetServer = nameof(NuGetServer);
        public const string ProGet = nameof(ProGet);
        public const string TeamCity = nameof(TeamCity);
        public const string VSTS = nameof(VSTS);

        public static string GetConfigFileRoot()
        {
            // The below environment variable is set on VSTS CI machines and the value is
            // equal to the root of the repository where the config files are copied as part of
            // a build step before the tests are run. If the environment variable is not set, the behavior
            // is the same as on TeamCity - this will ensure both CI's will be happy.
            var fullPath = Environment.GetEnvironmentVariable("NUGET_FUNCTESTS_CONFIG");
            return string.IsNullOrEmpty(fullPath) ? NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory) : fullPath;
        }
    }
}
