// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests.Telemetry;

/// <summary>
/// Collection definition for telemetry tests that must run serially to avoid cross-contamination.
/// All test classes marked with [Collection("Telemetry Tests")] will run sequentially, not in parallel.
/// </summary>
[CollectionDefinition("Telemetry Tests", DisableParallelization = true)]
public class TelemetryTestCollection
{
}
