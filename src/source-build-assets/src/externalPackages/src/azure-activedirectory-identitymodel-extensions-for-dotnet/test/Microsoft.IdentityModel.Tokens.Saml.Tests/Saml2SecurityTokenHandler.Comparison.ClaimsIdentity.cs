// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#nullable enable
namespace Microsoft.IdentityModel.Tokens.Saml2.Tests
{
    public partial class Saml2SecurityTokenHandlerTests
    {
        [Fact]
        public async Task ValidateTokenAsync_ClaimsIdentity_Comparison()
        {
            await SamlClaimsIdentityComparisonTestBase.ValidateTokenAsync_ClaimsIdentity_Comparison(
                this,
                nameof(ValidateTokenAsync_ClaimsIdentity_Comparison),
                "SAML2");
        }
    }
}
#nullable restore
