// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Not adding the support for xunit.v3.
// This is used by ConditionalFact and ConditionalTheory which we no longer support in xunit.v3.
// Still keeping the logic inside supporting xunit.v3 in case we decided to add it.
#if !USES_XUNIT_3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Internal;

#if !USES_XUNIT_3
using Xunit.Abstractions;
#endif
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    public class ConditionalTheoryDiscoverer : TheoryDiscoverer
    {
#if USES_XUNIT_3
        private readonly Dictionary<MethodInfo, string> _conditionCache = new();
#else
        private readonly Dictionary<IMethodInfo, string> _conditionCache = new();
#endif

#if !USES_XUNIT_3
        public ConditionalTheoryDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }
#endif


#if USES_XUNIT_3
        protected override ValueTask<IReadOnlyCollection<IXunitTestCase>> CreateTestCasesForTheory(ITestFrameworkDiscoveryOptions discoveryOptions, IXunitTestMethod testMethod, ITheoryAttribute theoryAttribute)
#else
        protected override IEnumerable<IXunitTestCase> CreateTestCasesForTheory(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
#endif
        {
#if USES_XUNIT_3
            var conditionalTheoryAttribute = (ConditionalTheoryAttribute)theoryAttribute;
            object[] constructorArgs = conditionalTheoryAttribute.CalleeType is null
                ? [conditionalTheoryAttribute.ConditionMemberNames]
                : [conditionalTheoryAttribute.CalleeType, conditionalTheoryAttribute.ConditionMemberNames];

            if (ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, testMethod, constructorArgs, out string skipReason, out ExecutionErrorTestCase errorTestCase))
#else
            if (ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, DiagnosticMessageSink, testMethod, theoryAttribute.GetConstructorArguments().ToArray(), out string skipReason, out ExecutionErrorTestCase errorTestCase))
#endif
            {
#if USES_XUNIT_3
                var details = TestIntrospectionHelper.GetTestCaseDetails(discoveryOptions, testMethod, theoryAttribute);

                var testCases = skipReason != null
                   ? new[] { new SkippedTestCase(details.ResolvedTestMethod, details.TestCaseDisplayName, details.UniqueID, details.Explicit, details.SkipExceptions, details.SkipReason, details.SkipType, details.SkipUnless, details.SkipWhen, testMethod.Traits.ToReadWrite(StringComparer.OrdinalIgnoreCase), timeout: details.Timeout) }
                   : new IXunitTestCase[] { new SkippedFactTestCase(details.ResolvedTestMethod, details.TestCaseDisplayName, details.UniqueID, details.Explicit, details.SkipExceptions, details.SkipReason, details.SkipType, details.SkipUnless, details.SkipWhen, testMethod.Traits.ToReadWrite(StringComparer.OrdinalIgnoreCase), timeout: details.Timeout) }; // Theory skippable at runtime.

                return new ValueTask<IReadOnlyCollection<IXunitTestCase>>(Task.FromResult<IReadOnlyCollection<IXunitTestCase>>(testCases));
#else
                return skipReason != null
                   ? new[] { new SkippedTestCase(skipReason, DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod) }
                   : new IXunitTestCase[] { new SkippedTheoryTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod) }; // Theory skippable at runtime.
#endif
            }

#if USES_XUNIT_3
            return new ValueTask<IReadOnlyCollection<IXunitTestCase>>(Task.FromResult<IReadOnlyCollection<IXunitTestCase>>(new IXunitTestCase[] { errorTestCase }));
#else
            return new IXunitTestCase[] { errorTestCase };
#endif
        }

#if USES_XUNIT_3
        protected override ValueTask<IReadOnlyCollection<IXunitTestCase>> CreateTestCasesForDataRow(ITestFrameworkDiscoveryOptions discoveryOptions, IXunitTestMethod testMethod, ITheoryAttribute theoryAttribute, ITheoryDataRow dataRow, object[] testMethodArguments)
#else
        protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute, object[] dataRow)
#endif
        {
            var methodInfo = testMethod.Method;
            List<IXunitTestCase> skippedTestCase = new List<IXunitTestCase>();
#if USES_XUNIT_3
            var details = TestIntrospectionHelper.GetTestCaseDetails(discoveryOptions, testMethod, theoryAttribute);
#endif

            if (!_conditionCache.TryGetValue(methodInfo, out string skipReason))
            {
#if USES_XUNIT_3
                var conditionalTheoryAttribute = (ConditionalTheoryAttribute)theoryAttribute;
                object[] constructorArgs = conditionalTheoryAttribute.CalleeType is null
                    ? [conditionalTheoryAttribute.ConditionMemberNames]
                    : [conditionalTheoryAttribute.CalleeType, conditionalTheoryAttribute.ConditionMemberNames];

                if (!ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, testMethod, constructorArgs, out skipReason, out ExecutionErrorTestCase errorTestCase))
#else
                if (!ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, DiagnosticMessageSink, testMethod, theoryAttribute.GetConstructorArguments().ToArray(), out skipReason, out ExecutionErrorTestCase errorTestCase))
#endif
                {
#if USES_XUNIT_3
                    return new ValueTask<IReadOnlyCollection<IXunitTestCase>>(Task.FromResult<IReadOnlyCollection<IXunitTestCase>>(new IXunitTestCase[] { errorTestCase }));
#else
                    return new IXunitTestCase[] { errorTestCase };
#endif
                }

                _conditionCache.Add(methodInfo, skipReason);

                if (skipReason != null)
                {
                    // If this is the first time we evalute the condition we return a SkippedTestCase to avoid printing a skip for every inline-data.
#if USES_XUNIT_3
                    skippedTestCase.Add(new SkippedTestCase(details.ResolvedTestMethod, details.TestCaseDisplayName, details.UniqueID, details.Explicit, details.SkipExceptions, details.SkipReason, details.SkipType, details.SkipUnless, details.SkipWhen, testMethod.Traits.ToReadWrite(StringComparer.OrdinalIgnoreCase), timeout: details.Timeout));
#else
                    skippedTestCase.Add(new SkippedTestCase(skipReason, DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod));
#endif
                }
            }

            var result = skipReason != null ?
                        (IReadOnlyCollection<IXunitTestCase>)skippedTestCase
                        : new[]
                        {
#if USES_XUNIT_3
                            new SkippedFactTestCase(details.ResolvedTestMethod, details.TestCaseDisplayName, details.UniqueID, details.Explicit, details.SkipExceptions, details.SkipReason, details.SkipType, details.SkipUnless, details.SkipWhen, testMethod.Traits.ToReadWrite(StringComparer.OrdinalIgnoreCase), timeout: details.Timeout)
#else
                            new SkippedFactTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, dataRow)
#endif
                        }; // Test case skippable at runtime.
#if USES_XUNIT_3
            return new ValueTask<IReadOnlyCollection<IXunitTestCase>>(Task.FromResult<IReadOnlyCollection<IXunitTestCase>>(result));
#else
            return result;
#endif
        }
    }
}
#endif
