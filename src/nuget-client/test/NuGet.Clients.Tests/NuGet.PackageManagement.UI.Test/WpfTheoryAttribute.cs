// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using Xunit.Sdk;

namespace NuGet.PackageManagement.UI.Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("NuGet.PackageManagement.UI.Test.WpfTheoryDiscoverer", "NuGet.PackageManagement.UI.Test")]
    public sealed class WpfTheoryAttribute : TheoryAttribute
    {
    }
}
