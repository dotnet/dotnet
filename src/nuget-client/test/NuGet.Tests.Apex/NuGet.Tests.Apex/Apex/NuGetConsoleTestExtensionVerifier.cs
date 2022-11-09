// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Test.Apex.Hosts;

namespace NuGet.Tests.Apex
{
    public class NuGetConsoleTestExtensionVerifier : RemoteReferenceTypeTestExtensionVerifier
    {
        /// <summary>
        /// Gets the test extension that is being verified.
        /// </summary>
        protected new NuGetConsoleTestExtension TestExtension => base.TestExtension as NuGetConsoleTestExtension;
    }
}
