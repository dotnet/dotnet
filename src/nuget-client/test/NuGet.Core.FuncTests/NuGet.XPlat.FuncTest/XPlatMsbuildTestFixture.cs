// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Build.Locator;
using NuGet.Test.Utility;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatMsbuildTestFixture : IDisposable
    {
        private readonly string _dotnetCli = TestFileSystemUtility.GetDotnetCli();

        private readonly string _previousDotNetRoot;

        public XPlatMsbuildTestFixture()
        {
            _previousDotNetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");

            Environment.SetEnvironmentVariable("DOTNET_ROOT", _dotnetCli);

            MSBuildLocator.RegisterDefaults();
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable("DOTNET_ROOT", _previousDotNetRoot);
        }
    }
}
