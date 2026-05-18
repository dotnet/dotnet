// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using Xunit;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// On .NET Framework, skip the test unless running in CI (where elevation is available).
    /// On .NET 5+, always run (in-memory trust stores don't require elevation).
    /// </summary>
    public class NetFxCIOnlyFactAttribute
        : FactAttribute
    {
        private string _skip;

        public override string Skip
        {
            get
            {
                var skip = _skip;

#if !NET5_0_OR_GREATER
                if (string.IsNullOrEmpty(skip))
                {
                    if (!XunitAttributeUtility.IsCI)
                    {
                        skip = "This test requires elevation on .NET Framework. It only runs on CI for this TFM. To run it locally, use a .NET 5+ TFM or set the env var CI=true";
                    }
                }
#endif

                return skip;
            }

            set
            {
                _skip = value;
            }
        }
    }
}
