// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Not adding the support for xunit.v3.
// This is used by ConditionalFact and ConditionalTheory which we no longer support in xunit.v3.
// Still keeping the logic inside supporting xunit.v3 in case we decided to add it.
#if !USES_XUNIT_3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
#if !USES_XUNIT_3
using Xunit.Abstractions;
#endif
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    /// <summary>Wraps RunAsync for ConditionalFact.</summary>
    public class SkippedFactTestCase : XunitTestCase
#if USES_XUNIT_3
        , ISelfExecutingXunitTestCase
#endif
    {
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes", error: true)]
        public SkippedFactTestCase() { }

#if USES_XUNIT_3
        public SkippedFactTestCase(
            IXunitTestMethod testMethod,
            string testCaseDisplayName,
            string uniqueID,
            bool @explicit,
            Type[] skipExceptions = null,
            string skipReason = null,
            Type skipType = null,
            string skipUnless = null,
            string skipWhen = null,
            Dictionary<string, HashSet<string>> traits = null,
            object[] testMethodArguments = null,
            string sourceFilePath = null,
            int? sourceLineNumber = null,
            int? timeout = null)
            : base(testMethod, testCaseDisplayName, uniqueID, @explicit, skipExceptions, skipReason, skipType, skipUnless, skipWhen, traits, testMethodArguments, sourceFilePath, sourceLineNumber, timeout)
        {
            
        }
#else
        public SkippedFactTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod, object[] testMethodArguments = null)
            : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments) { }
#endif

#if USES_XUNIT_3
        public async ValueTask<RunSummary> Run(ExplicitOption explicitOption, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
#else
        public override async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                        IMessageBus messageBus,
                                                        object[] constructorArguments,
                                                        ExceptionAggregator aggregator,
                                                        CancellationTokenSource cancellationTokenSource)
#endif
        {
            SkippedTestMessageBus skipMessageBus = new SkippedTestMessageBus(messageBus);
#if USES_XUNIT_3
            var result = await XunitRunnerHelper.RunXunitTestCase(this, skipMessageBus, cancellationTokenSource, aggregator, explicitOption, constructorArguments);
#else
            var result = await base.RunAsync(diagnosticMessageSink, skipMessageBus, constructorArguments, aggregator, cancellationTokenSource);
#endif
            if (skipMessageBus.SkippedTestCount > 0)
            {
                result.Failed -= skipMessageBus.SkippedTestCount;
                result.Skipped += skipMessageBus.SkippedTestCount;
            }

            return result;
        }
    }
}
#endif
