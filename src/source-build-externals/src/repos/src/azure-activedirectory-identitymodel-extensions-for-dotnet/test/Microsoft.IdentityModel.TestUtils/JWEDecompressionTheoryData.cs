// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.TestUtils
{
    public class JWEDecompressionTheoryData : TheoryDataBase
    {
        public CompressionProviderFactory CompressionProviderFactory;
        public TokenValidationParameters ValidationParameters;
        public string JWECompressionString;
    }
}
