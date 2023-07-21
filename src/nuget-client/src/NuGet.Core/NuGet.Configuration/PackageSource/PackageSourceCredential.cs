// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.Configuration
{
    /// <summary>
    /// Represents credentials required to authenticate user within package source web requests.
    /// </summary>
    public class PackageSourceCredential : IEquatable<PackageSourceCredential>
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Password text as stored in config file. May be encrypted.
        /// </summary>
        public string PasswordText { get; }

        /// <summary>
        /// Indicates if password is stored in clear text.
        /// </summary>
        public bool IsPasswordClearText { get; }

        /// <summary>
        /// List of authentication types the credential is valid for, e.g. 'basic'. If empty, all authentication types
        /// are allowed.
        /// </summary>
        public IEnumerable<string> ValidAuthenticationTypes =>
            ParseAuthTypeFilterString(ValidAuthenticationTypesText);

        private readonly Lazy<int> _hashCode;

        /// <summary>
        /// Comma-delimited list of authentication types the credential is valid for as stored in the config file.
        /// If null or empty, all authentication types are valid. Example: 'basic,negotiate'
        /// </summary>
        public string ValidAuthenticationTypesText { get; }

        /// <summary>
        /// Retrieves password in clear text. Decrypts on-demand.
        /// </summary>
        public string Password
        {
            get
            {
                if (PasswordText != null && !IsPasswordClearText)
                {
                    try
                    {
                        return EncryptionUtility.DecryptString(PasswordText);
                    }
                    catch (NotSupportedException e)
                    {
                        throw new NuGetConfigurationException(
                            string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedDecryptPassword, Source), e);
                    }
                }
                else
                {
                    return PasswordText;
                }
            }
        }

        /// <summary>
        /// Associated source ID
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Verifies if object contains valid data, e.g. not empty user name and password.
        /// </summary>
        /// <returns>True if credentials object is valid</returns>
        public bool IsValid() => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(PasswordText);

        /// <summary>
        /// Instantiates the credential instance out of raw values read from a config file.
        /// </summary>
        /// <param name="source">Associated source ID (needed for reporting errors)</param>
        /// <param name="username">User name</param>
        /// <param name="passwordText">Password as stored in config file</param>
        /// <param name="isPasswordClearText">Hints if password provided in clear text</param>
        /// <param name="validAuthenticationTypesText">
        /// Comma-delimited list of authentication types the credential is valid for as stored in the config file.
        /// If null or empty, all authentication types are valid. Example: 'basic,negotiate'
        /// </param>
        public PackageSourceCredential(string source, string username, string passwordText, bool isPasswordClearText, string validAuthenticationTypesText)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordText = passwordText;
            IsPasswordClearText = isPasswordClearText;

            // validAuthenticationTypesText is permitted to be null
            ValidAuthenticationTypesText = validAuthenticationTypesText;

            _hashCode = new Lazy<int>(() =>
            {
                var combiner = new HashCodeCombiner();

                combiner.AddObject(Source);
                combiner.AddObject(Username);
                combiner.AddObject(PasswordText);
                combiner.AddObject(IsPasswordClearText);

                return combiner.CombinedHash;
            });
        }

        /// <summary>
        /// Creates new instance of credential object out values provided by user.
        /// </summary>
        /// <param name="source">Source ID needed for reporting errors if any</param>
        /// <param name="username">User name</param>
        /// <param name="password">Password text in clear</param>
        /// <param name="storePasswordInClearText">Hints if the password should be stored in clear text on disk.</param>
        /// <param name="validAuthenticationTypesText">
        /// Comma-delimited list of authentication types the credential is valid for as stored in the config file.
        /// If null or empty, all authentication types are valid. Example: 'basic,negotiate'
        /// </param>
        /// <returns>New instance of <see cref="PackageSourceCredential"/></returns>
        public static PackageSourceCredential FromUserInput(
            string source,
            string username,
            string password,
            bool storePasswordInClearText,
            string validAuthenticationTypesText)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            // validAuthenticationTypes is permitted to be null.

            try
            {
                var passwordText = storePasswordInClearText ? password : EncryptionUtility.EncryptString(password);
                return new PackageSourceCredential(
                    source,
                    username,
                    passwordText,
                    isPasswordClearText: storePasswordInClearText,
                    validAuthenticationTypesText: validAuthenticationTypesText);
            }
            catch (NotSupportedException e)
            {
                throw new NuGetConfigurationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedEncryptPassword, source), e);
            }
        }

        /// <summary>
        /// Converts this object to an ICredentials, capturing the username, password and valid authentication types
        /// </summary>
        public ICredentials ToICredentials()
        {
            return new AuthTypeFilteredCredentials(new NetworkCredential(Username, Password), ValidAuthenticationTypes);
        }

        internal PackageSourceCredential Clone()
        {
            return new PackageSourceCredential(Source, Username, PasswordText, IsPasswordClearText, ValidAuthenticationTypesText);
        }

        /// <summary>
        /// Converts an authentication type filter string from the config file syntax to a list of valid authentication
        /// types
        /// </summary>
        /// <param name="str">
        /// Comma-delimited list of authentication types the credential is valid for as stored in the config file.
        /// If null or empty, all authentication types are valid. Example: 'basic,negotiate'
        /// </param>
        /// <returns>
        /// Enumeration of valid authentication types. If empty, all authentication types are valid.
        /// </returns>
        private static IEnumerable<string> ParseAuthTypeFilterString(string str)
        {
            return string.IsNullOrWhiteSpace(str)
                ? Enumerable.Empty<string>()
                : str.Split(',').Select(x => x.Trim()).Where(x => x != string.Empty);
        }


        public CredentialsItem AsCredentialsItem()
        {
            return new CredentialsItem(Source, Username, PasswordText, IsPasswordClearText, ValidAuthenticationTypesText);
        }

        public bool Equals(PackageSourceCredential other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Source, other.Source, StringComparison.Ordinal) &&
                string.Equals(Username, other.Username, StringComparison.Ordinal) &&
                string.Equals(PasswordText, other.PasswordText, StringComparison.Ordinal) &&
                IsPasswordClearText == other.IsPasswordClearText;
        }

        public override bool Equals(object other)
        {
            return Equals(other as PackageSourceCredential);
        }

        public override int GetHashCode()
        {
            return _hashCode.Value;
        }
    }
}
