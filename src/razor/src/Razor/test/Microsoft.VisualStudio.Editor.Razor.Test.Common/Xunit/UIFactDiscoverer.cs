// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit;

internal class UIFactDiscoverer : FactDiscoverer
{
    public UIFactDiscoverer(IMessageSink diagnosticMessageSink)
        : base(diagnosticMessageSink)
    {
    }

    protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    {
        return new UITestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod);
    }
}