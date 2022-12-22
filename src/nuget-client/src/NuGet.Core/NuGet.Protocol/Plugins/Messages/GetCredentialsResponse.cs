// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A response to a get credentials request.
    /// </summary>
    public sealed class GetCredentialsResponse
    {
        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        [JsonRequired]
        public MessageResponseCode ResponseCode { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; }

        public IReadOnlyList<string> AuthenticationTypes { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="GetCredentialsResponse" /> class.
        /// </summary>
        /// <param name="responseCode">The plugin's response code.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is an undefined <see cref="MessageResponseCode" /> value.</exception>
        [JsonConstructor]
        public GetCredentialsResponse(
            MessageResponseCode responseCode,
            string username,
            string password,
            IReadOnlyList<string> authenticationTypes = null)
        {
            if (!Enum.IsDefined(typeof(MessageResponseCode), responseCode))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_UnrecognizedEnumValue,
                        responseCode),
                    nameof(responseCode));
            }

            ResponseCode = responseCode;
            Username = username;
            Password = password;
            AuthenticationTypes = authenticationTypes;
        }
    }
}
