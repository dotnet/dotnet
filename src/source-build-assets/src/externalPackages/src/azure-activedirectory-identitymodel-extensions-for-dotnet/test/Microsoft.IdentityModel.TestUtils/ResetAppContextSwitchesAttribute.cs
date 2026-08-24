// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using Xunit.Sdk;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <inheritdoc/>
    public class ResetAppContextSwitchesAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            AppContextSwitches.ResetAllSwitches();
        }

        public override void After(MethodInfo methodUnderTest)
        {
            AppContextSwitches.ResetAllSwitches();
        }
    }
}
