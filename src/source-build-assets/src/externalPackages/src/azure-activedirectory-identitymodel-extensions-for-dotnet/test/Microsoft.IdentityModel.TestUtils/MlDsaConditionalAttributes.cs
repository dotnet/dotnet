// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Cryptography;
using Xunit;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// A <see cref="FactAttribute"/> that skips the test when ML-DSA is not supported
    /// on the current platform. ML-DSA requires OS-level cryptographic support (e.g.,
    /// SymCrypt) that may not be present on older OS versions.
    /// </summary>
    public sealed class MlDsaFactAttribute : FactAttribute
    {
        public MlDsaFactAttribute()
        {
            if (!MLDsa.IsSupported)
                Skip = "ML-DSA is not supported on this platform.";
        }
    }

    /// <summary>
    /// A <see cref="TheoryAttribute"/> that skips the test when ML-DSA is not supported
    /// on the current platform. ML-DSA requires OS-level cryptographic support (e.g.,
    /// SymCrypt) that may not be present on older OS versions.
    /// </summary>
    public sealed class MlDsaTheoryAttribute : TheoryAttribute
    {
        public MlDsaTheoryAttribute()
        {
            if (!MLDsa.IsSupported)
                Skip = "ML-DSA is not supported on this platform.";
        }
    }
}
