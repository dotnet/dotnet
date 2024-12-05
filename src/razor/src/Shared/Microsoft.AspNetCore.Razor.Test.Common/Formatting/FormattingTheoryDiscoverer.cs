// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal sealed class FormattingTheoryDiscoverer(IMessageSink diagnosticMessageSink)
    : TheoryDiscoverer(diagnosticMessageSink)
{
    protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute, object[] dataRow)
    {
        return FormattingFactDiscoverer.CreateTestCases(discoveryOptions, testMethod, DiagnosticMessageSink, dataRow);
    }
}
