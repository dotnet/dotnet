// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Credentials
{
    /// <summary>
    /// Response data returned from plugin credential provider applications
    /// </summary>
    public class PluginCredentialResponse
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the list of authentication types this credential is applicable to. Useful values include
        /// <c>basic</c>, <c>digest</c>, <c>negotiate</c>, and <c>ntlm</c>
        /// </summary>
        public IList<string>? AuthTypes { get; set; }

        /// <summary>
        /// Gets a value indicating whether the provider returned a valid response.
        /// </summary>
        /// <remarks>
        /// Either Username or Password (or both) must be set, and AuthTypes must either be null or contain at least
        /// one element
        /// </remarks>
        public bool IsValid => (!String.IsNullOrWhiteSpace(Username) || !String.IsNullOrWhiteSpace(Password))
                               && (AuthTypes == null || AuthTypes.Any());
    }

    /// <summary>
    /// Specifies the exit codes returned by a plugin credential provider.
    /// </summary>
    public enum PluginCredentialResponseExitCode
    {
        /// <summary>
        /// The provider successfully returned credentials.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The provider does not apply to the credential request.
        /// </summary>
        ProviderNotApplicable = 1,

        /// <summary>
        /// The provider failed to process the credential request.
        /// </summary>
        Failure = 2
    }
}
