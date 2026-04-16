// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Subject element specified in [Saml2Core, 2.4.2.1].
    /// </summary>
    /// <remarks>
    /// If the NameId is null and the SubjectConfirmations collection is empty,
    /// an InvalidOperationException will be thrown during serialization.
    /// </remarks>
    public class SamlSubject
    {
        // Saml SubjectConfirmation parts.
        private SecurityKey _securityKey;
        private KeyInfo _keyInfo;

        // TODO should this be internal?
        /// <summary>
        /// Initialize an instance of <see cref="SamlSubject"/>.
        /// </summary>
        public SamlSubject()
        {
            ConfirmationMethods = new List<string>();
        }

        /// <summary>
        /// Initialize an instance of <see cref="SamlSubject"/>.
        /// </summary>
        /// <param name="nameFormat">The format of the subject.</param>
        /// <param name="nameQualifier">The NameIdentifier of the subject.</param>
        /// <param name="name">The name of the subject.</param>
        public SamlSubject(string nameFormat, string nameQualifier, string name)
            : this(nameFormat, nameQualifier, name, null, null)
        {
        }

        /// <summary>
        /// Initialize an instance of <see cref="SamlSubject"/>.
        /// </summary>
        /// <param name="nameFormat">The format of the subject.</param>
        /// <param name="nameQualifier">The NameIdentifier of the subject.</param>
        /// <param name="name">The name of the subject.</param>
        /// <param name="confirmations"><see cref="IEnumerable{String}"/>.</param>
        /// <param name="confirmationData">The confirmation data contained in the subject.</param>
        public SamlSubject(
            string nameFormat,
            string nameQualifier,
            string name,
            IEnumerable<string> confirmations,
            string confirmationData)
        {

            ConfirmationMethods = (confirmations == null) ? new List<string>() : new List<string>(confirmations);
            Name = name;
            NameFormat = nameFormat;
            NameQualifier = nameQualifier;
            ConfirmationData = confirmationData;
        }

        /// <summary>
        /// Gets or sets confirmation data.
        /// </summary>
        public string ConfirmationData
        {
            get; set;
        }

        /// <summary>
        /// Gets confirmation methods.
        /// </summary>
        public ICollection<string> ConfirmationMethods { get; }

        /// <summary>
        /// Gets or sets the <see cref="SecurityKey"/>.
        /// </summary>
        public SecurityKey Key
        {
            get { return _securityKey; }
            set
            {
                _securityKey = value ?? throw LogArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets or sets the<see cref="KeyInfo"/>.
        /// </summary>
        public KeyInfo KeyInfo
        {
            get { return _keyInfo; }
            set
            {
                _keyInfo = value ?? throw LogArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets or sets the name of the Subject.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the ClaimType.
        /// </summary>
        public static string NameClaimType
        {
            get
            {
                return ClaimTypes.NameIdentifier;
            }
        }

        /// <summary>
        /// Gets or sets the format of the Subject.
        /// </summary>
        public string NameFormat
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the name qualifier of the Subject.
        /// </summary>
        public string NameQualifier
        {
            get; set;
        }

        void CheckObjectValidity()
        {
            if ((ConfirmationMethods.Count == 0) && (string.IsNullOrEmpty(Name)))
                throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11107));

            if ((ConfirmationMethods.Count == 0) && (ConfirmationData != null))
                throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11510));
        }
    }

    internal class SamlSubjectEqualityComparer : EqualityComparer<SamlSubject>
    {
        public override bool Equals(SamlSubject subject1, SamlSubject subject2)
        {
            if (subject1 == null && subject2 == null)
                return true;

            if (subject1 == null || subject2 == null)
                return false;

            if (ReferenceEquals(subject1, subject2))
                return true;

            if (string.Compare(subject1.Name, subject2.Name, StringComparison.OrdinalIgnoreCase) != 0 ||
                string.Compare(subject1.NameFormat, subject2.NameFormat, StringComparison.OrdinalIgnoreCase) != 0 ||
                string.Compare(subject1.NameQualifier, subject2.NameQualifier, StringComparison.OrdinalIgnoreCase) != 0 ||
                string.Compare(subject1.ConfirmationData, subject2.ConfirmationData, StringComparison.OrdinalIgnoreCase) != 0)
                return false;

            if (subject1.KeyInfo != null && subject2.KeyInfo != null)
                if (!subject1.KeyInfo.Equals(subject2.KeyInfo))
                    return false;
                else if (subject1.KeyInfo == null || subject2.KeyInfo == null)
                    return false;

            if (subject1.ConfirmationMethods.Count != subject2.ConfirmationMethods.Count)
                return false;

            var query1 = subject1.ConfirmationMethods.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var query2 = subject2.ConfirmationMethods.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            if (query1.Count != query2.Count)
                return false;

            foreach (var query in query1)
            {
                if (!query2.Contains(query))
                    return false;
            }

            return true;
        }

        public override int GetHashCode(SamlSubject subject)
        {
            int defaultHash = string.Empty.GetHashCode();
            int hashCode = defaultHash;
            hashCode ^= (subject.Name == null) ? defaultHash : subject.Name.GetHashCode();
            hashCode ^= (subject.NameFormat == null) ? defaultHash : subject.NameFormat.GetHashCode();
            hashCode ^= (subject.NameQualifier == null) ? defaultHash : subject.NameQualifier.GetHashCode();
            hashCode ^= (subject.ConfirmationData == null) ? defaultHash : subject.ConfirmationData.GetHashCode();

            if (subject.KeyInfo != null)
                hashCode ^= subject.KeyInfo.GetHashCode();

            foreach (var method in subject.ConfirmationMethods)
                hashCode ^= method.GetHashCode();

            return hashCode.GetHashCode();
        }
    }
}
