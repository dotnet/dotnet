// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Assertion element specified in [Saml, 2.3.2].
    /// </summary>
    public class SamlAssertion
    {
        private string _assertionId;
        private string _canonicalString;
        private string _issuer;
        private DateTime _issueInstant;

        /// <summary>
        /// Creates an instance of <see cref="SamlAssertion"/>.
        /// </summary>
        /// <param name="assertionId">AssertionID of the assertion.</param>
        /// <param name="issuer">Issuer of the assertion.</param>
        /// <param name="issueInstant">IssueInstant of the assertion.</param>
        /// <param name="samlConditions">SamlConditions of the assertion.</param>
        /// <param name="samlAdvice">SamlAdvice of the assertion.</param>
        /// <param name="samlStatements"><see cref="IEnumerable{SamlStatement}"/>.</param>
        public SamlAssertion(
            string assertionId,
            string issuer,
            DateTime issueInstant,
            SamlConditions samlConditions,
            SamlAdvice samlAdvice,
            IEnumerable<SamlStatement> samlStatements
            )
        {
            Statements = (samlStatements == null) ? throw LogArgumentNullException(nameof(samlStatements)) : new List<SamlStatement>(samlStatements);

            AssertionId = assertionId;
            Issuer = issuer;
            IssueInstant = issueInstant;
            Conditions = samlConditions;
            Advice = samlAdvice;
        }

        /// <summary>
        /// Gets or sets additional information related to the assertion that assists processing in certain
        /// situations but which may be ignored by applications that do not understand the
        /// advice or do not wish to make use of it.
        /// </summary>
        public SamlAdvice Advice { get; set; }

        /// <summary>
        /// Gets or sets the identifier for this assertion.
        /// </summary>
        public string AssertionId
        {
            get { return _assertionId; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _assertionId = value;
            }
        }

        /// <summary>
        /// Gets or sets conditions that must be evaluated when assessing the validity of and/or
        /// when using the assertion.
        /// </summary>
        public SamlConditions Conditions { get; set; }

        /// <summary>
        /// Gets or sets the a PrefixList to use when there is a need to include InclusiveNamespaces writing token.
        /// </summary>
        public string InclusiveNamespacesPrefixList { get; set; }

        /// <summary>
        /// Gets or sets the issuer in the assertion.
        /// </summary>
        public string Issuer
        {
            get { return _issuer; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _issuer = value;
            }
        }

        /// <summary>
        /// Gets or sets the time instant of issue in UTC.
        /// </summary>
        public DateTime IssueInstant
        {
            get { return _issueInstant; }
            set { _issueInstant = DateTimeUtil.ToUniversalTime(value); }
        }

        /// <summary>
        /// Gets the major version of this assertion. [Saml, 2.3.2]
        /// <remarks>
        /// The identifier for the version of SAML defined in this specification is 1.
        /// </remarks>
        /// </summary>
        public string MajorVersion
        {
            get { return SamlConstants.MajorVersionValue; }
        }

        /// <summary>
        /// Gets the minor version of this assertion. [Saml, 2.3.2]
        /// <remarks>
        /// The identifier for the version of SAML defined in this specification is 1.
        /// </remarks>
        /// </summary>
        public string MinorVersion
        {
            get { return SamlConstants.MinorVersionValue; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Signature"/> on the Assertion.
        /// </summary>
        public Signature Signature { get; set; }

        /// <summary>
        /// Gets the canonicalized (ExclusiveC14n) representation without comments.
        /// </summary>
        public string CanonicalString
        {
            get
            {
                if (_canonicalString == null)
                {
                    if (XmlTokenStream != null)
                    {
                        _canonicalString = CanonicalizingTransfrom.GetString(XmlTokenStream, false, null);
                    }
                    else
                    {
                        try
                        {
                            var serializer = new SamlSerializer();
                            using (var writer = XmlDictionaryWriter.CreateTextWriter(Stream.Null))
                            using (var c14nStream = new MemoryStream())
                            {
                                writer.StartCanonicalization(c14nStream, false, null);
                                serializer.WriteAssertion(writer, this);
                                writer.Flush();
                                _canonicalString = Encoding.UTF8.GetString(c14nStream.GetBuffer(), 0, (int)c14nStream.Length);
                            }
                        }
                        catch
                        { }
                    }
                }

                return _canonicalString;
            }
            internal set
            {
                _canonicalString = string.IsNullOrEmpty(value) ? throw LogArgumentNullException(nameof(value)) : value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="SigningCredentials"/> used by the issuer to protect the integrity of the assertion.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Gets the <see cref="IList{SamlStatement}"/>(s) regarding the subject.
        /// </summary>
        public IList<SamlStatement> Statements { get; }

        internal XmlTokenStream XmlTokenStream { get; set; }
    }
}
