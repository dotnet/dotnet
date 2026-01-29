// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <summary>
/// Available architectures for test platform.
/// </summary>
public enum PlatformArchitecture
{
    X86,
    X64,
    ARM,
    ARM64,
    S390x,
    Ppc64le,
    RiscV64,
    LoongArch64,
}
