// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Test.Apex;

namespace NuGet.Tests.Apex
{
    internal class NuGetTestOperationConfiguration : OperationsConfiguration
    {
        protected override Type Verifier => typeof(IAssertionVerifier);
    }
}
