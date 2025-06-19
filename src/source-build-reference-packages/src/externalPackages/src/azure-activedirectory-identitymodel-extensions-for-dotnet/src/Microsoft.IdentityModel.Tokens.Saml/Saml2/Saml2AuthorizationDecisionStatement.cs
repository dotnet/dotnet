// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the AuthzDecisionStatement specified in [Saml2Core, 2.7.4].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2AuthorizationDecisionStatement : Saml2Statement
    {
        /// <summary>
        /// The empty URI reference, which may be used with the meaning 
        /// "the start of the current document" for the Resource property.
        /// </summary>
        internal static readonly Uri EmptyResource = new Uri(string.Empty, UriKind.Relative);

        private string _decision;
        private Uri _resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AuthorizationDecisionStatement"/> class from
        /// a resource and decision.
        /// </summary>
        /// <param name="resource">The <see cref="Uri"/> of the resource to be authorized.</param>
        /// <param name="decision">The AccessDecision in use.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="resource"/> is null.</exception>
        /// <exception cref="ArgumentException">if <paramref name="resource"/> is not an absolute Uri.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="decision"/> is null or empty.</exception>
        public Saml2AuthorizationDecisionStatement(Uri resource, string decision)
            : this(resource, decision, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2AuthorizationDecisionStatement"/> class from
        /// a resource and decision.
        /// </summary>
        /// <param name="resource">The <see cref="Uri"/> of the resource to be authorized.</param>
        /// <param name="decision">The AccessDecision in use.</param>
        /// <param name="actions">Collection of <see cref="Saml2Action"/> specifications.</param>
        public Saml2AuthorizationDecisionStatement(Uri resource, string decision, IEnumerable<Saml2Action> actions)
        {
            Resource = resource;
            Decision = decision;
            Actions = (actions == null) ? new List<Saml2Action>() : new List<Saml2Action>(actions);
        }

        /// <summary>
        /// Gets of set the set of <see cref="Saml2Action"/> authorized to be performed on the specified
        /// resource. [Saml2Core, 2.7.4]
        /// </summary>
        public ICollection<Saml2Action> Actions
        {
            get;
        }

        /// <summary>
        /// Gets or sets the AccessDecision rendered by the SAML authority with respect to the specified resource. [Saml2Core, 2.7.4]
        /// </summary>
        public string Decision
        {
            get { return _decision; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                if (Saml2Constants.AccessDecision.Deny.Equals(value)
                    || Saml2Constants.AccessDecision.Permit.Equals(value)
                    || Saml2Constants.AccessDecision.Indeterminate.Equals(value))
                    _decision = value;
                else
                    throw LogExceptionMessage(new Saml2SecurityTokenException(LogMessages.IDX13310));
            }
        }

        /// <summary>
        /// Gets or sets a set of <see cref="Saml2Evidence"/> that the SAML authority relied on in making 
        /// the decision. [Saml2Core, 2.7.4]
        /// </summary>
        public Saml2Evidence Evidence
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a URI reference identifying the resource to which access 
        /// authorization is sought. [Saml2Core, 2.7.4]
        /// </summary>
        /// <remarks>
        /// In addition to any absolute URI, the Resource may also be the 
        /// empty URI reference, and the meaning is defined to be "the start
        /// of the current document". [Saml2Core, 2.7.4]
        /// </remarks>
        public Uri Resource
        {
            get { return _resource; }
            set
            {
                if (value == null)
                    throw LogArgumentNullException(nameof(value));

                if (!value.IsAbsoluteUri)
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13300, MarkAsNonPII(nameof(Resource)), value)));

                _resource = value;
            }
        }
    }
}
