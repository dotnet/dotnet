//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the AuthorizationDecisionStatement specified in [Saml, 2.4.5].
    /// </summary>
    public class SamlAuthorizationDecisionStatement : SamlSubjectStatement
    {
        private string _decision;
        private string _resource;

        internal SamlAuthorizationDecisionStatement()
        {
            Actions = new List<SamlAction>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlAuthorizationDecisionStatement"/> class from
        /// a resource and decision.
        /// </summary>
        /// <param name="subject">The <see cref="SamlSubject"/> of the statement.</param>
        /// <param name="resource">The resource to be authorized.</param>
        /// <param name="decision">The AccessDecision in use.</param>
        /// <param name="actions"><see cref="IEnumerable{SamlAction}"/>.</param>
        public SamlAuthorizationDecisionStatement(
            SamlSubject subject,
            string resource,
            string decision,
            IEnumerable<SamlAction> actions)
            : this(subject, resource, decision, actions, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlAuthorizationDecisionStatement"/> class from
        /// a resource and decision.
        /// </summary>
        /// <param name="subject">The <see cref="SamlSubject"/> of the statement.</param>
        /// <param name="resource">The resource to be authorized.</param>
        /// <param name="decision">The AccessDecision in use.</param>
        /// <param name="actions"><see cref="IEnumerable{SamlAction}"/>.</param>
        /// <param name="evidence">Collection of <see cref="SamlEvidence"/> specifications.</param>
        public SamlAuthorizationDecisionStatement(
            SamlSubject subject,
            string resource,
            string decision,
            IEnumerable<SamlAction> actions,
            SamlEvidence evidence)
        {
            Actions = (actions == null) ? throw LogArgumentNullException(nameof(actions)) : new List<SamlAction>(actions);
            Evidence = evidence;
            Decision = decision;
            Resource = resource;
            Subject = subject;
            CheckObjectValidity();
        }

        /// <summary>
        /// Gets or sets the AccessDecision rendered by the SAML authority with respect to the specified resource.
        /// </summary>
        public string Decision
        {
            get { return _decision; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                if (SamlConstants.AccessDecision.Deny.Equals(value)
                    || SamlConstants.AccessDecision.Permit.Equals(value)
                    || SamlConstants.AccessDecision.Indeterminate.Equals(value))
                    _decision = value;
                else
                    throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11508));
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{SamlAction}"/> representing the action values contained in the AuthorizationDecisionStatement.
        /// </summary>
        public ICollection<SamlAction> Actions
        {
            get;
        }

        /// <summary>
        /// Gets the ClaimType.
        /// </summary>
        public static string ClaimType
        {
            get
            {
                return ClaimTypes.AuthorizationDecision;
            }
        }

        /// <summary>
        /// Gets or sets the evidence contained in the AuthorizationDecisionStatement.
        /// </summary>
        public SamlEvidence Evidence
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resource contained in the AuthorizationDecisionStatement.
        /// </summary>
        public string Resource
        {
            get { return _resource; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _resource = value;
            }
        }

        void CheckObjectValidity()
        {
            if (Actions.Count == 0)
                throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11508));
        }
    }
}
