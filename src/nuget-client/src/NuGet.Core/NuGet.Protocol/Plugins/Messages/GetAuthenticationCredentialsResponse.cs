// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A get authentication credentials response
    /// </summary>
    public sealed class GetAuthenticationCredentialsResponse
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// password token
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// message - optional, can be used as a way to communicate to NuGet why the authentication failed.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets or sets the list of authentication types this credential is applicable to. Useful values include
        /// <c>basic</c>, <c>digest</c>, <c>negotiate</c>, and <c>ntlm</c>
        /// </summary>
        public IList<string> AuthenticationTypes { get; }

        /// <summary>
        /// ResponseCode - status of the credentials
        /// </summary>
        [JsonRequired]
        public MessageResponseCode ResponseCode { get; }

        /// <summary>
        /// Create a response object
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        /// <param name="authenticationTypes"></param>
        /// <param name="responseCode"></param>
        /// <exception cref="ArgumentException">If MessageResponseCode is not defined on this runtime</exception>
        [JsonConstructor]
        public GetAuthenticationCredentialsResponse(string username, string password, string message, IList<string> authenticationTypes, MessageResponseCode responseCode)
        {

            if (!Enum.IsDefined(typeof(MessageResponseCode), responseCode))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_UnrecognizedEnumValue,
                       nameof(responseCode)));
            }


            Username = username;
            Password = password;
            Message = message;
            AuthenticationTypes = authenticationTypes;
            ResponseCode = responseCode;
        }

        /// <summary>
        /// Gets a value indicating whether the provider returnd a valid response.
        /// </summary>
        /// <remarks>
        /// Either Username or Password (or both) must be set, and AuthTypes must either be null or contain at least
        /// one element
        /// </remarks>
        public bool IsValid() => ResponseCode == MessageResponseCode.Success && (!string.IsNullOrWhiteSpace(Username) || !string.IsNullOrWhiteSpace(Password))
                               && (AuthenticationTypes == null || AuthenticationTypes.Any());
    }
}
