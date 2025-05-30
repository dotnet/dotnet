// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestUtilities;

public class ConditionalFactDiscoverer : FactDiscoverer
{
    public ConditionalFactDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink) { }

    protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    {
        if (ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, DiagnosticMessageSink, testMethod, factAttribute.GetConstructorArguments().ToArray(), out string skipReason, out ExecutionErrorTestCase errorTestCase))
        {
            return skipReason != null
                ? (IXunitTestCase) new SkippedTestCase(skipReason, DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod)
                : base.CreateTestCase(discoveryOptions, testMethod, factAttribute);
        }

        return errorTestCase;
    }
}