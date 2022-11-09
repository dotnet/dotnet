// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Build.Framework;

using SdkResultFactoryBase = Microsoft.Build.Framework.SdkResultFactory;

namespace Microsoft.Build.NuGetSdkResolver.Test
{
    internal class MockSdkResultFactory : SdkResultFactoryBase
    {
        /// <inheritdoc cref="Microsoft.Build.Framework.SdkResultFactory.IndicateFailure(IEnumerable{string}, IEnumerable{string})" />
        public override SdkResult IndicateFailure(IEnumerable<string> errors, IEnumerable<string> warnings = null)
        {
            return new MockSdkResult(errors, warnings);
        }

        /// <inheritdoc cref="Microsoft.Build.Framework.SdkResultFactory.IndicateSuccess(string, string, IEnumerable{string})" />
        public override SdkResult IndicateSuccess(string path, string version, IEnumerable<string> warnings = null)
        {
            return new MockSdkResult(path, version, warnings);
        }
    }
}
