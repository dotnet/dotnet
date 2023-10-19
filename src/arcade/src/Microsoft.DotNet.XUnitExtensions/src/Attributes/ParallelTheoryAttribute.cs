// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions.Attributes
{
    [XunitTestCaseDiscoverer("Microsoft.DotNet.XUnitExtensions.ParallelTheoryDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ParallelTheoryAttribute : TheoryAttribute
    {
    }
}
