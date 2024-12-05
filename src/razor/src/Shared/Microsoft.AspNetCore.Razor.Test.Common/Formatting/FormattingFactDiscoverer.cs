// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal sealed class FormattingFactDiscoverer(IMessageSink diagnosticMessageSink)
    : FactDiscoverer(diagnosticMessageSink)
{
    public override IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    {
        return CreateTestCases(discoveryOptions, testMethod, DiagnosticMessageSink);
    }

    public static IEnumerable<IXunitTestCase> CreateTestCases(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IMessageSink messageSink, object[]? dataRow = null)
    {
        yield return CreateTestCase(shouldFlipLineEndings: false, forceRuntimeCodeGeneration: false);
        yield return CreateTestCase(shouldFlipLineEndings: false, forceRuntimeCodeGeneration: true);

        yield return CreateTestCase(shouldFlipLineEndings: true, forceRuntimeCodeGeneration: false);
        yield return CreateTestCase(shouldFlipLineEndings: true, forceRuntimeCodeGeneration: true);

        FormattingTestCase CreateTestCase(bool shouldFlipLineEndings, bool forceRuntimeCodeGeneration)
        {
            return new FormattingTestCase(shouldFlipLineEndings, forceRuntimeCodeGeneration, messageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, dataRow);
        }
    }
}
