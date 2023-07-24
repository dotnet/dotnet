// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// Set defaults for TheoryData
    /// </summary>
    public class TheoryDataBase
    {
        public TheoryDataBase() : this(Guid.NewGuid().ToString())
        {
        }

        public TheoryDataBase(string testId)
        {
            IdentityModelEventSource.ShowPII = true;
            CallContext = new CallContext
            {
                CaptureLogs = true,
                DebugId = testId
            };

            TestId = testId;
        }

        public TheoryDataBase(bool showPII)
        {
            IdentityModelEventSource.ShowPII = showPII;
        }

        public CallContext CallContext { get; set; } = new CallContext();

        public ExpectedException ExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public bool First { get; set; } = false;

        public Dictionary<Type, List<string>> PropertiesToIgnoreWhenComparing { get; set; } = new Dictionary<Type, List<string>>();

        public string TestId { get; set; }

        public override string ToString()
        {
            return $"{TestId}, {ExpectedException}";
        }
    }
}
