// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Not adding the support for xunit.v3.
// This is used by ConditionalFact and ConditionalTheory which we no longer support in xunit.v3.
// Still keeping the logic inside supporting xunit.v3 in case we decided to add it.
#if !USES_XUNIT_3

using System;
using System.Linq;
#if !USES_XUNIT_3
using Xunit.Abstractions;
#endif
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    public class SkipTestException : Exception
    {
        public SkipTestException(string reason)
            : base(reason) { }
    }

    /// <summary>Implements message bus to communicate tests skipped via SkipTestException.</summary>
    public class SkippedTestMessageBus : IMessageBus
    {
        readonly IMessageBus innerBus;

        public SkippedTestMessageBus(IMessageBus innerBus)
        {
            this.innerBus = innerBus;
        }

        public int SkippedTestCount { get; private set; }

        public void Dispose() { }

        public bool QueueMessage(IMessageSinkMessage message)
        {
            var testFailed = message as ITestFailed;

            if (testFailed != null)
            {
                var exceptionType = testFailed.ExceptionTypes.FirstOrDefault();
                if (exceptionType == typeof(SkipTestException).FullName)
                {
                    SkippedTestCount++;
#if USES_XUNIT_3
                    var testSkippedMessage = new TestSkipped()
                    {
                        AssemblyUniqueID = testFailed.AssemblyUniqueID,
                        ExecutionTime = testFailed.ExecutionTime,
                        FinishTime = testFailed.FinishTime,
                        Output = testFailed.Output,
                        Reason = testFailed.Messages.FirstOrDefault(),
                        TestCaseUniqueID = testFailed.TestCaseUniqueID,
                        TestClassUniqueID = testFailed.TestClassUniqueID,
                        TestCollectionUniqueID = testFailed.TestCollectionUniqueID,
                        TestMethodUniqueID = testFailed.TestMethodUniqueID,
                        TestUniqueID = testFailed.TestUniqueID,
                        Warnings = testFailed.Warnings,
                    };
#else
                    var testSkippedMessage = new TestSkipped(testFailed.Test, testFailed.Messages.FirstOrDefault());
#endif
                    return innerBus.QueueMessage(testSkippedMessage);
                }
            }

            // Nothing we care about, send it on its way
            return innerBus.QueueMessage(message);
        }
    }
}
#endif
