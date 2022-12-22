// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using Xunit.Sdk;

namespace NuGet.StaFact
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("NuGet.StaFact.NuGetWpfTheoryDiscoverer", "NuGet.StaFact")]
    public class NuGetWpfTheoryAttribute : TheoryAttribute
    {
    }
}
