// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// TODO: Not yet supported for xunit.v3
#if !USES_XUNIT_3
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions.Attributes
{
#if USES_XUNIT_3
    [XunitTestCaseDiscoverer(typeof(ParallelTheoryDiscoverer))]
#else
    [XunitTestCaseDiscoverer("Microsoft.DotNet.XUnitExtensions.ParallelTheoryDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ParallelTheoryAttribute : TheoryAttribute
    {
    }
}
#endif
