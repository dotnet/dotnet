// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// Skip the test if not on a CI.
    /// </summary>
    public class CIOnlyTheoryAttribute
        : TheoryAttribute
    {
        private string _skip;

        public override string Skip
        {
            get
            {
                var skip = _skip;

                if (string.IsNullOrEmpty(skip))
                {
                    if (!XunitAttributeUtility.IsCI)
                    {
                        skip = "This test only runs on the CI. To run it locally set the env var CI=true";
                    }
                }

                // If this is null the test will run.
                return skip;
            }

            set
            {
                _skip = value;
            }
        }

        public CIOnlyTheoryAttribute()
        {
        }
    }
}
