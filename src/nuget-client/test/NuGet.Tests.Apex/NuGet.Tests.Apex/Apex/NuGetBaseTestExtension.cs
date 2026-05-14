// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Test.Apex.Hosts;

namespace NuGet.Tests.Apex
{
    public abstract class NuGetBaseTestExtension<TObjectUnderTest, TVerify> :
        RemoteReferenceTypeTestExtension<TObjectUnderTest, TVerify>
        where TVerify : RemoteTestExtensionVerifier
        where TObjectUnderTest : class
    {
    }
}
