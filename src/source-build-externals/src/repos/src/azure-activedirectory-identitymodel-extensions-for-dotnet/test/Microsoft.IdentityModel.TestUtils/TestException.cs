// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// Throw this exception instead of Assert(false, ...) so we know we threw it.
    /// </summary>
    public class TestException : Exception
    {
        public TestException(string message)
            : base(message)
        {
        }
    }
}
