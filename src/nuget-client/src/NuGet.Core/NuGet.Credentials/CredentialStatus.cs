// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Credentials
{
    /// <summary>
    /// Result of an attempt to acquire credentials.
    /// Keep in sync with NuGet.VisualStudio.VsCredentialStatus
    /// </summary>
    public enum CredentialStatus
    {
        /// <summary>
        /// Credentials were successfully acquired.
        /// </summary>
        Success,

        /// <summary>
        /// The provider was not applicable for acquiring credentials.
        /// </summary>
        ProviderNotApplicable,

        /// <summary>
        /// The user canceled the credential acquisition process.
        /// </summary>
        UserCanceled
    }
}
