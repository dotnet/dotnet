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
        // Cohosting only has runtime code-gen, so the old formatting engine doesn't work with them
        if (!testMethod.TestClass.TestCollection.TestAssembly.Assembly.Name.StartsWith("Microsoft.VisualStudio.LanguageServices.Razor"))
        {
            yield return CreateTestCase(shouldFlipLineEndings: false, forceRuntimeCodeGeneration: false, useNewFormattingEngine: false);
            yield return CreateTestCase(shouldFlipLineEndings: false, forceRuntimeCodeGeneration: true, useNewFormattingEngine: false);
            yield return CreateTestCase(shouldFlipLineEndings: true, forceRuntimeCodeGeneration: false, useNewFormattingEngine: false);
            yield return CreateTestCase(shouldFlipLineEndings: true, forceRuntimeCodeGeneration: true, useNewFormattingEngine: false);

            yield return CreateTestCase(shouldFlipLineEndings: false, forceRuntimeCodeGeneration: false, useNewFormattingEngine: true);
            yield return CreateTestCase(shouldFlipLineEndings: true, forceRuntimeCodeGeneration: false, useNewFormattingEngine: true);
        }

        yield return CreateTestCase(shouldFlipLineEndings: false, forceRuntimeCodeGeneration: true, useNewFormattingEngine: true);
        yield return CreateTestCase(shouldFlipLineEndings: true, forceRuntimeCodeGeneration: true, useNewFormattingEngine: true);

        FormattingTestCase CreateTestCase(bool shouldFlipLineEndings, bool forceRuntimeCodeGeneration, bool useNewFormattingEngine)
        {
            return new FormattingTestCase(shouldFlipLineEndings, forceRuntimeCodeGeneration, useNewFormattingEngine, messageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, dataRow);
        }
    }
}
