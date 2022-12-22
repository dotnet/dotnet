// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// Skip Theory if it is running on a CI machine
    /// </summary>
    /// <remarks>Unset CI environment variable or set it to CI=false to run this Theory</remarks>
    public class LocalOnlyTheoryAttribute
        : TheoryAttribute
    {
        public override string Skip
        {
            get
            {
                if (string.IsNullOrEmpty(base.Skip)
                    && XunitAttributeUtility.IsCI)
                {
                    base.Skip = "This Theory only run on non-CI machines. To run it, set the env var CI=false";
                }

                return base.Skip;
            }

            set => base.Skip = value;
        }
    }
}
