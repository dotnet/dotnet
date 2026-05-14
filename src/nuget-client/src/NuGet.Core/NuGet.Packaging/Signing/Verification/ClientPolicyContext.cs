// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Packaging.Signing
{
    public class ClientPolicyContext
    {
        /// <summary>
        /// Current policy the client is on.
        /// </summary>
        public SignatureValidationMode Policy { get; }

        /// <summary>
        /// Verification settings corresponding the current client policy.
        /// </summary>
        public SignedPackageVerifierSettings VerifierSettings { get; }

        /// <summary>
        /// List of signatures allowed in verification.
        /// </summary>
        public IReadOnlyCollection<TrustedSignerAllowListEntry> AllowList { get; }

        internal ClientPolicyContext(SignatureValidationMode policy, IReadOnlyCollection<TrustedSignerAllowListEntry> allowList)
        {
            Policy = policy;

            if (policy == SignatureValidationMode.Require)
            {
                VerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy();
            }
            else
            {
                VerifierSettings = SignedPackageVerifierSettings.GetAcceptModeDefaultPolicy();
            }

            AllowList = allowList;
        }

        public static ClientPolicyContext GetClientPolicy(ISettings settings, ILogger logger)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var policy = SettingsUtility.GetSignatureValidationMode(settings);
            var allowList = TrustedSignersProvider.GetAllowListEntries(settings, logger);

            return new ClientPolicyContext(policy, allowList);
        }
    }
}
