// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;

public sealed record AzureDevOpsReportingParameters(
    Uri CollectionUri,
    string TeamProject,
    string TestRunId,
    string? AccessToken = null);
