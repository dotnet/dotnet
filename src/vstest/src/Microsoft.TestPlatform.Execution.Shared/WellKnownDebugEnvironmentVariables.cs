// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Execution;

internal static class WellKnownDebugEnvironmentVariables
{
    public const string VSTEST_BLAMEDATACOLLECTOR_DEBUG = nameof(VSTEST_BLAMEDATACOLLECTOR_DEBUG);
    public const string VSTEST_DATACOLLECTOR_DEBUG = nameof(VSTEST_DATACOLLECTOR_DEBUG);
    public const string VSTEST_HOST_DEBUG = nameof(VSTEST_HOST_DEBUG);
    public const string VSTEST_RUNNER_DEBUG = nameof(VSTEST_RUNNER_DEBUG);
    public const string VSTEST_DATACOLLECTOR_DEBUG_ATTACHVS = nameof(VSTEST_DATACOLLECTOR_DEBUG_ATTACHVS);
    public const string VSTEST_HOST_DEBUG_ATTACHVS = nameof(VSTEST_HOST_DEBUG_ATTACHVS);
    public const string VSTEST_RUNNER_DEBUG_ATTACHVS = nameof(VSTEST_RUNNER_DEBUG_ATTACHVS);
    public const string VSTEST_HOST_NATIVE_DEBUG = nameof(VSTEST_HOST_NATIVE_DEBUG);
    public const string VSTEST_RUNNER_NATIVE_DEBUG = nameof(VSTEST_RUNNER_NATIVE_DEBUG);
}
