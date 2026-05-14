// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

[Flags]
public enum DotNetSdkActions
{
    None = 0,
    Build = 1,
    Run = 2,
    RunWeb = 4,
    Publish = 8,
    PublishComplex = 16,
    PublishR2R = 32,
    PublishAot = 64,
    Test = 128,
    AddClassLibRef = 256,
    FullWorkloadTest = 512,
    WorkloadInstall = 1024,
    WorkloadUninstall = 2048
}
