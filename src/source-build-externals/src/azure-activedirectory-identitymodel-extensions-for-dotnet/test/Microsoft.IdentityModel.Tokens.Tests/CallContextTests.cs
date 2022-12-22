// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.CodeDom;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class CallContextTests
    {
        [Theory, MemberData(nameof(CallContextTestTheoryData))]
        public void LoggerInstanceTests(CallContextTheoryData theoryData)
        {
            var context = new CallContext(theoryData.ActivityId) { DebugId = theoryData.TestId };

            Assert.IsAssignableFrom<LoggerContext>(context);
            Assert.Equal(theoryData.TestId, context.DebugId);
            Assert.Equal(theoryData.ActivityId, context.ActivityId);
            Assert.False(context.CaptureLogs);
            Assert.Empty(context.Logs);
            Assert.Null(context.PropertyBag);
        }

        public static TheoryData<TheoryDataBase> CallContextTestTheoryData
        {
            get
            {
                var theoryData = new TheoryData<TheoryDataBase>();

                theoryData.Add(new CallContextTheoryData
                {
                    TestId = "abdc",
                    ActivityId = new Guid()
                });

                return theoryData;
            }
        }
    }

    public class CallContextTheoryData : TheoryDataBase
    {
        public Guid ActivityId;
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
