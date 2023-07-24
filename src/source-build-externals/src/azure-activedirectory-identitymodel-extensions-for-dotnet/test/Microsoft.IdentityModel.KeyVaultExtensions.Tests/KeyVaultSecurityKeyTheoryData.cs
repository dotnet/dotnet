// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;

namespace Microsoft.IdentityModel.KeyVaultExtensions.Tests
{
    public abstract class KeyVaultSecurityKeyTheoryData : TheoryDataBase
    {
        public string KeyIdentifier { get; set; } = KeyVaultUtilities.CreateKeyIdentifier();
        public Type Type { get; set; }
    }
}
