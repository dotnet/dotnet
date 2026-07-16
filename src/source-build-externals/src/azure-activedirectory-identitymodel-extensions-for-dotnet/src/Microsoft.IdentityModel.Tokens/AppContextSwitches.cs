// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// AppContext switches for Microsoft.IdentityModel.Tokens and referencing packages.
    /// </summary>
    internal static class AppContextSwitches
    {
        /// <summary>
        /// Enables a new behavior of using <see cref="CaseSensitiveClaimsIdentity"/> instead of <see cref="ClaimsIdentity"/> globally.
        /// </summary>
        internal const string UseCaseSensitiveClaimsIdentityTypeSwitch = "Microsoft.IdentityModel.Tokens.UseCaseSensitiveClaimsIdentity";

        private static bool? _useCaseSensitiveClaimsIdentityType;

        internal static bool UseCaseSensitiveClaimsIdentityType => _useCaseSensitiveClaimsIdentityType ??= (AppContext.TryGetSwitch(UseCaseSensitiveClaimsIdentityTypeSwitch, out bool useCaseSensitiveClaimsIdentityType) && useCaseSensitiveClaimsIdentityType);

        /// <summary>
        /// When validating the issuer signing key, specifies whether to fail if the 'tid' claim is missing.
        /// </summary>
        internal const string DoNotFailOnMissingTidSwitch = "Switch.Microsoft.IdentityModel.DontFailOnMissingTidValidateIssuerSigning";

        private static bool? _doNotFailOnMissingTid;

        internal static bool DoNotFailOnMissingTid => _doNotFailOnMissingTid ??= (AppContext.TryGetSwitch(DoNotFailOnMissingTidSwitch, out bool doNotFailOnMissingTid) && doNotFailOnMissingTid);

        /// <summary>
        /// When reading claims from the token, specifies whether to try to convert all string claims to DateTime.
        /// Some claims are known not to be DateTime, so conversion is skipped.
        /// </summary>
        internal const string TryAllStringClaimsAsDateTimeSwitch = "Switch.Microsoft.IdentityModel.TryAllStringClaimsAsDateTime";

        private static bool? _tryAllStringClaimsAsDateTime;

        internal static bool TryAllStringClaimsAsDateTime => _tryAllStringClaimsAsDateTime ??= (AppContext.TryGetSwitch(TryAllStringClaimsAsDateTimeSwitch, out bool tryAsDateTime) && tryAsDateTime);

        /// <summary>
        /// Controls whether to validate the length of the authentication tag when decrypting a token.
        /// </summary>
        internal const string SkipValidationOfAuthenticationTagLengthSwitch = "Switch.Microsoft.IdentityModel.SkipAuthenticationTagLengthValidation";

        private static bool? _skipValidationOfAuthenticationTagLength;

        internal static bool ShouldValidateAuthenticationTagLength => _skipValidationOfAuthenticationTagLength ??= !(AppContext.TryGetSwitch(SkipValidationOfAuthenticationTagLengthSwitch, out bool skipValidation) && skipValidation);

        /// <summary>
        /// Controls whether to use the short name for the RSA OAEP key wrap algorithm.
        /// </summary>
        internal const string UseShortNameForRsaOaepKeySwitch = "Switch.Microsoft.IdentityModel.UseShortNameForRsaOaepKey";

        private static bool? _useShortNameForRsaOaepKey;

        internal static bool ShouldUseShortNameForRsaOaepKey => _useShortNameForRsaOaepKey ??= AppContext.TryGetSwitch(UseShortNameForRsaOaepKeySwitch, out var useKeyWrap) && useKeyWrap;

        /// <summary>
        /// Used for testing to reset all switches to its default value.
        /// </summary>
        internal static void ResetAllSwitches()
        {
            _useCaseSensitiveClaimsIdentityType = null;
            AppContext.SetSwitch(UseCaseSensitiveClaimsIdentityTypeSwitch, false);

            _doNotFailOnMissingTid = null;
            AppContext.SetSwitch(DoNotFailOnMissingTidSwitch, false);

            _tryAllStringClaimsAsDateTime = null;
            AppContext.SetSwitch(TryAllStringClaimsAsDateTimeSwitch, false);

            _skipValidationOfAuthenticationTagLength = null;
            AppContext.SetSwitch(SkipValidationOfAuthenticationTagLengthSwitch, false);

            _useShortNameForRsaOaepKey = null;
            AppContext.SetSwitch(UseShortNameForRsaOaepKeySwitch, false);
        }
    }
}
