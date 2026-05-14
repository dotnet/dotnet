// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;

namespace Test.Utility;

public static class TestConstants
{
#if NET10_0 || NETFRAMEWORK
    // Specifies a target framework for projects used during testing.  This should match the framework that the SDK being tested has.
    public const string ProjectTargetFramework = "net10.0";
    public static readonly NuGetFramework DefaultTargetFramework = NuGetFramework.Parse(ProjectTargetFramework);
#else
#error Update the logic for which target framework to use for tests projects!!!
#endif
}
