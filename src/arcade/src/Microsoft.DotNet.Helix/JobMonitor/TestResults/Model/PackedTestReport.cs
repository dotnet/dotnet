// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;

public sealed record PackedTestReport(AzureDevOpsReportingParameters AzdoParameters, IReadOnlyList<TestResult> Results);
