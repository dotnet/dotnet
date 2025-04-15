// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Not adding the support for xunit.v3.
// This is used by ConditionalFact and ConditionalTheory which we no longer support in xunit.v3.
// Still keeping the logic inside supporting xunit.v3 in case we decided to add it.
#if !USES_XUNIT_3

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Internal;


#if !USES_XUNIT_3
using Xunit.Abstractions;
#endif
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    public class ConditionalFactDiscoverer : FactDiscoverer
    {
#if !USES_XUNIT_3
        public ConditionalFactDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink) { }
#endif

#if USES_XUNIT_3
        protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, IXunitTestMethod testMethod, IFactAttribute factAttribute)
#else
        protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
#endif
        {
#if USES_XUNIT_3
            var conditionalFactAttribute = (ConditionalFactAttribute)factAttribute;
            object[] constructorArgs = conditionalFactAttribute.CalleeType is null
                ? [conditionalFactAttribute.ConditionMemberNames]
                : [conditionalFactAttribute.CalleeType, conditionalFactAttribute.ConditionMemberNames];

            if (ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, testMethod, constructorArgs, out string skipReason, out ExecutionErrorTestCase errorTestCase))
#else
            if (ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, DiagnosticMessageSink, testMethod, factAttribute.GetConstructorArguments().ToArray(), out string skipReason, out ExecutionErrorTestCase errorTestCase))
#endif
            {
#if USES_XUNIT_3
                var details = TestIntrospectionHelper.GetTestCaseDetails(discoveryOptions, testMethod, factAttribute);

                return skipReason != null
                    ? (IXunitTestCase)new SkippedTestCase(details.ResolvedTestMethod, details.TestCaseDisplayName, details.UniqueID, details.Explicit, details.SkipExceptions, details.SkipReason, details.SkipType, details.SkipUnless, details.SkipWhen, testMethod.Traits.ToReadWrite(StringComparer.OrdinalIgnoreCase), timeout: details.Timeout)
                    : new SkippedFactTestCase(details.ResolvedTestMethod, details.TestCaseDisplayName, details.UniqueID, details.Explicit, details.SkipExceptions, details.SkipReason, details.SkipType, details.SkipUnless, details.SkipWhen, testMethod.Traits.ToReadWrite(StringComparer.OrdinalIgnoreCase), timeout: details.Timeout); // Test case skippable at runtime.
#else
                return skipReason != null
                    ? (IXunitTestCase) new SkippedTestCase(skipReason, DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod)
                    : new SkippedFactTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod); // Test case skippable at runtime.
#endif
            }

            return errorTestCase;
        }
    }
}
#endif
