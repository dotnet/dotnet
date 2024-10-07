// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Frameworks;

namespace Dotnet.Integration.Test
{
    internal static class Constants
    {
#if NET8_0 || NET9_0
        // Specifies a target framework for projects used during testing.  This should match the framework that the SDK being tested has.
        internal const string ProjectTargetFramework = "net9.0";
        internal static readonly NuGetFramework DefaultTargetFramework = NuGetFramework.Parse(ProjectTargetFramework);
#else
#error Update the logic for which target framework to use for tests projects!!!
#endif
        internal static readonly Uri DotNetPackageSource = new("https://dnceng.pkgs.visualstudio.com/public/_packaging/dotnet7/nuget/v3/index.json");
    }
}
