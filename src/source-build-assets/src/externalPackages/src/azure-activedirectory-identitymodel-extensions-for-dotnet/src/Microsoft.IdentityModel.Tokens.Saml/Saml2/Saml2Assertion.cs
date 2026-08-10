// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the Assertion element specified in [Saml2Core, 2.3.3].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2Assertion
    {
        private string _canonicalString;
        private Saml2Id _id;
        private DateTime _issueInstant;
        private Saml2NameIdentifier _issuer;

        /// <summary>
        /// Creates an instance of a Saml2Assertion.
        /// </summary>
        /// <param name="issuer">Issuer of the assertion.</param>
        public Saml2Assertion(Saml2NameIdentifier issuer)
        {
            Id = new Saml2Id();
            IssueInstant = DateTime.UtcNow;
            Issuer = issuer;
            Statements = new List<Saml2Statement>();
        }

        /// <summary>
        /// Gets or sets the <see cref="Signature"/> on the Assertion.
        /// </summary>
        public Signature Signature
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets additional information related to the assertion that assists processing in certain
        /// situations but which may be ignored by applications that do not understand the 
        /// advice or do not wish to make use of it. [Saml2Core, 2.3.3]
        /// </summary>
        public Saml2Advice Advice
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets conditions that must be evaluated when assessing the validity of and/or
        /// when using the assertion. [Saml2Core 2.3.3]
        /// </summary>
        public Saml2Conditions Conditions
        {
            get;
            set;
        }

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
                            var serializer = new Saml2Serializer();
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
        /// Gets or sets the <see cref="Saml2Id"/> identifier for this assertion. [Saml2Core, 2.3.3]
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' if null.</exception>
        public Saml2Id Id
        {
            get => _id;
            set => _id = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the time instant of issue in UTC. [Saml2Core, 2.3.3]
        /// </summary>
        public DateTime IssueInstant
        {
            get => _issueInstant;
            set => _issueInstant = DateTimeUtil.ToUniversalTime(value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Saml2NameIdentifier"/> as the authority that is making the claim(s) in the assertion. [Saml2Core, 2.3.3]
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        public Saml2NameIdentifier Issuer
        {
            get => _issuer;
            set => _issuer = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the a PrefixList to use when there is a need to include InclusiveNamespaces writing token.
        /// </summary>
        public string InclusiveNamespacesPrefixList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="SigningCredentials"/> used by the issuer to protect the integrity of the assertion.
        /// </summary>
        public SigningCredentials SigningCredentials
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Saml2Subject"/> of the statement(s) in the assertion. [Saml2Core, 2.3.3]
        /// </summary>
        public Saml2Subject Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="Saml2Statement"/>(s) regarding the subject.
        /// </summary>
        public ICollection<Saml2Statement> Statements
        {
            get;
        }

        /// <summary>
        /// Gets the version of this assertion. [Saml2Core, 2.3.3]
        /// </summary>
        public string Version
        {
            get => Saml2Constants.Version;
        }

        internal XmlTokenStream XmlTokenStream { get; set; }
    }
}
