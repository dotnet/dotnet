// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;

namespace NuGet.Credentials
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CredentialResponse
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Creates a credential response object without giving credentials. This constructor is used only if the
        /// credential provider was not able to get credentials. The <paramref name="status"/> indicates why the
        /// provider was not able to get credentials.
        /// </summary>
        /// <param name="status">The status of why the credential provider was not able to get credentials.</param>
        public CredentialResponse(CredentialStatus status) : this(null, status)
        {
        }

        /// <summary>
        /// Creates a credential response object with a given set of credentials. This constuctor is used only if the
        /// credential provider was able to get credentials.
        /// </summary>
        /// <param name="credentials">The credentials fetched by the credential provider.</param>

        public CredentialResponse(ICredentials credentials) : this(credentials, CredentialStatus.Success)
        {
        }

        private CredentialResponse(ICredentials credentials, CredentialStatus status)
        {
            if ((credentials != null && status != CredentialStatus.Success) ||
                (credentials == null && status == CredentialStatus.Success))
            {
                throw new ProviderException(Resources.ProviderException_InvalidCredentialResponse);
            }

            Credentials = credentials;
            Status = status;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ICredentials Credentials { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CredentialStatus Status { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
