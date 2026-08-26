// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Test.Apex.VisualStudio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public sealed class AssemblyHooks
    {
        private static VisualStudioHost? _cacheWarmer;

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            // The first Visual Studio host creation can take over five minutes, so warm it outside a timed test.
            // CreateHost composes the Apex host, warming its static assembly probing cache, but
            // does not call Start and therefore does not launch devenv.exe.
            var fixture = new VisualStudioOperationsFixture();
            _cacheWarmer = fixture.Operations.CreateHost<VisualStudioHost>(fixture.VisualStudioHostConfiguration);
        }
    }
}
