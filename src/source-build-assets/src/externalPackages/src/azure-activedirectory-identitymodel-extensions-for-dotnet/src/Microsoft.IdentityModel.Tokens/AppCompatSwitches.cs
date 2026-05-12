// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens;

/// <summary>
/// Identifiers used for switching between different app compat behaviors within the Microsoft.IdentityModel libraries.
/// </summary>
/// <remarks>
/// The Microsoft.IdentityModel libraries use <see cref="System.AppContext" /> to turn on or off certain API behavioral
/// changes that might have an effect on application compatibility. This class defines the set of switches that are
/// available to modify library behavior. Application compatibility is favored as the default - so if your application
/// needs to rely on the new behavior, you will need to enable the switch manually. Setting a switch's value can be
/// done programmatically through the <see cref="System.AppContext.SetSwitch" /> method, or through other means such as
/// setting it through MSBuild, app configuration, or registry settings. These alternate methods are described in the
/// <see cref="System.AppContext.SetSwitch" /> documentation.
/// </remarks>
public static class AppCompatSwitches
{
    /// <summary>
    /// Uses <see cref="EncryptingCredentials.KeyExchangePublicKey"/> for the token's `kid` header parameter. When using
    /// ECDH-based key wrap algorithms the public key portion of <see cref="EncryptingCredentials.Key" /> is also written
    /// to the token's `epk` header parameter.
    /// </summary>
    /// <remarks>
    /// Enabling this switch improves the library's conformance to RFC 7518 with regards to how the header values for
    /// `kid` and `epk` are set in ECDH key wrap scenarios. The previous behavior erroneously used key ID of
    /// <see cref="EncryptingCredentials.Key"/> as the `kid` parameter, and did not automatically set `epk` as the spec
    /// defines. This switch enables the intended behavior where <see cref="EncryptingCredentials.KeyExchangePublicKey"/>
    /// is used for `kid` and the public portion of <see cref="EncryptingCredentials.Key"/> is used for `epk`.
    /// </remarks>
    public const string UseRfcDefinitionOfEpkAndKid = "Switch.Microsoft.IdentityModel.UseRfcDefinitionOfEpkAndKid";
}
