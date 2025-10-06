// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
#if !USES_XUNIT_3
using Xunit.Abstractions;
#endif
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    /// <summary>Wraps another test case that should be skipped.</summary>
    internal sealed class SkippedTestCase : XunitTestCase
    {
#if !USES_XUNIT_3
        private string _skipReason;
#endif
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public SkippedTestCase() : base()
        {
        }

#if USES_XUNIT_3
        public SkippedTestCase(
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
#if !USES_XUNIT_3
            _skipReason = skipReason;
#endif
        }
#else
        public SkippedTestCase(
            string skipReason,
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions,
            ITestMethod testMethod,
            object[] testMethodArguments = null)
            : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
        {
            _skipReason = skipReason;
        }
#endif

#if !USES_XUNIT_3
        protected override string GetSkipReason(IAttributeInfo factAttribute)
            => _skipReason ?? base.GetSkipReason(factAttribute);

        public override void Deserialize(IXunitSerializationInfo data)
        {
            _skipReason = data.GetValue<string>(nameof(_skipReason));

            // we need to call base after reading our value, because Deserialize will call
            // into GetSkipReason.
            base.Deserialize(data);
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            data.AddValue(nameof(_skipReason), _skipReason);
        }
#endif
    }
}
