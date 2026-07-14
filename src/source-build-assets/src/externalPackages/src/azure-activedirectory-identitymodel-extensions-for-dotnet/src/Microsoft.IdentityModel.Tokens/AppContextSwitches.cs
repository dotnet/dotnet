// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Identifiers used for switching between different app compatibility behaviors within the Microsoft.IdentityModel packages.
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
    internal static class AppContextSwitches
    {
        /// <summary>
        /// Enables a fallback to the previous behavior of using <see cref="ClaimsIdentity"/> instead of <see cref="CaseSensitiveClaimsIdentity"/> globally.
        /// </summary>
        internal const string UseClaimsIdentityTypeSwitch = "Microsoft.IdentityModel.Tokens.UseClaimsIdentityType";

        private static bool? _useClaimsIdentityType;

        internal static bool UseClaimsIdentityType => _useClaimsIdentityType ??= (AppContext.TryGetSwitch(UseClaimsIdentityTypeSwitch, out bool useClaimsIdentityType) && useClaimsIdentityType);

        /// <summary>
        /// When validating the issuer signing key, specifies whether to fail if the 'tid' claim is missing.
        /// </summary>
        internal const string DoNotFailOnMissingTidSwitch = "Switch.Microsoft.IdentityModel.DontFailOnMissingTidValidateIssuerSigning";

        private static bool? _doNotFailOnMissingTid;

        internal static bool DontFailOnMissingTid => _doNotFailOnMissingTid ??= (AppContext.TryGetSwitch(DoNotFailOnMissingTidSwitch, out bool doNotFailOnMissingTid) && doNotFailOnMissingTid);

        // 7x was released with the property name "DoNotFailOnMissingTid" 8x was released with "DontFailOnMissingTid"
        // This is to ensure that both property names are supported.
        internal static bool DoNotFailOnMissingTid => DontFailOnMissingTid;

        /// <summary>
        /// When reading claims from the token, specifies whether to try to convert all string claims to DateTime.
        /// Some claims are known not to be DateTime, so conversion is skipped.
        /// </summary>
        internal const string TryAllStringClaimsAsDateTimeSwitch = "Switch.Microsoft.IdentityModel.TryAllStringClaimsAsDateTime";

        private static bool? _tryAllStringClaimsAsDateTime;

        internal static bool TryAllStringClaimsAsDateTime => _tryAllStringClaimsAsDateTime ??= (AppContext.TryGetSwitch(TryAllStringClaimsAsDateTimeSwitch, out bool tryAsDateTime) && tryAsDateTime);

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
        internal const string UseRfcDefinitionOfEpkAndKidSwitch = "Switch.Microsoft.IdentityModel.UseRfcDefinitionOfEpkAndKid";

        private static bool? _useRfcDefinitionOfEpkAndKid;

        internal static bool UseRfcDefinitionOfEpkAndKid => _useRfcDefinitionOfEpkAndKid ??= (AppContext.TryGetSwitch(UseRfcDefinitionOfEpkAndKidSwitch, out bool isEnabled) && isEnabled);

        /// <summary>
        /// Enabling this switch will cause the configuration manager to block other requests to GetConfigurationAsync if a request is already in progress.
        /// The default configuration refresh behavior is if a request is already in progress, the current configuration will be returned until the ongoing request is completed on
        /// a background thread.
        /// </summary>
        internal const string UpdateConfigAsBlockingSwitch = "Switch.Microsoft.IdentityModel.UpdateConfigAsBlocking";

        private static bool? _updateConfigAsBlockingCall;

        /// <summary>
        /// Unused, part of a previous release. This is a friend, so we cannot remove.
        /// </summary>
        internal static bool UpdateConfigAsBlocking => _updateConfigAsBlockingCall ??= (AppContext.TryGetSwitch(UpdateConfigAsBlockingSwitch, out bool blockingCall) && blockingCall);

        /// <summary>
        /// When enabled, some exceptions and log messages will contain additional details. Enable temporarily only for debugging purposes.
        /// </summary>
        internal const string DoNotScrubExceptionsSwitch = "Switch.Microsoft.IdentityModel.DoNotScrubExceptions";
        private static bool? _doNotScrubExceptions;
        internal static bool DoNotScrubExceptions => _doNotScrubExceptions ??= (AppContext.TryGetSwitch(DoNotScrubExceptionsSwitch, out bool doNotScrubExceptions) && doNotScrubExceptions);

        /// <summary>
        /// When enabled, the XML type attribute will be capitalized (XML) for saml configurations.
        /// </summary>
        internal const string UseCapitalizedXMLTypeAttrSwitch = "Switch.Microsoft.IdentityModel.UseCapitalizedXMLTypeAttr";
        private static bool? _useCapitalizedXMLTypeAttr;
        internal static bool UseCapitalizedXMLTypeAttr => _useCapitalizedXMLTypeAttr ??= (AppContext.TryGetSwitch(UseCapitalizedXMLTypeAttrSwitch, out bool useCapitalizedXMLTypeAttr) && useCapitalizedXMLTypeAttr);

        /// <summary>
        /// Used for testing to reset all switches to its default value.
        /// </summary>
        internal static void ResetAllSwitches()
        {
            _useClaimsIdentityType = null;
            AppContext.SetSwitch(UseClaimsIdentityTypeSwitch, false);

            _doNotFailOnMissingTid = null;
            AppContext.SetSwitch(DoNotFailOnMissingTidSwitch, false);

            _tryAllStringClaimsAsDateTime = null;
            AppContext.SetSwitch(TryAllStringClaimsAsDateTimeSwitch, false);

            _useRfcDefinitionOfEpkAndKid = null;
            AppContext.SetSwitch(UseRfcDefinitionOfEpkAndKidSwitch, false);

            _updateConfigAsBlockingCall = null;
            AppContext.SetSwitch(UpdateConfigAsBlockingSwitch, false);

            _doNotScrubExceptions = null;
            AppContext.SetSwitch(DoNotScrubExceptionsSwitch, false);

            _useCapitalizedXMLTypeAttr = null;
            AppContext.SetSwitch(UseCapitalizedXMLTypeAttrSwitch, false);
        }
    }
}
