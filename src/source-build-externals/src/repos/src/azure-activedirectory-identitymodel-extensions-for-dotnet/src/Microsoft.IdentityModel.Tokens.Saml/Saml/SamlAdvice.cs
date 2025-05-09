// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Advice element specified in [Saml, 2.3.2.2].
    /// </summary>
    /// <remarks>
    /// This information MAY be ignored by applications without affecting either
    /// the semantics or the validity of the assertion.
    /// </remarks>
    public class SamlAdvice
    {
        /// <summary>
        /// Creates an instance of <see cref="SamlAdvice"/>.
        /// </summary>
        public SamlAdvice()
            : this(null, null)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAdvice"/>.
        /// </summary>
        /// <param name="references"><see cref="IEnumerable{String}"/>.</param>
        public SamlAdvice(IEnumerable<string> references)
            : this(references, null)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAdvice"/>.
        /// </summary>
        /// <param name="assertions"><see cref="IEnumerable{SamlAssertion}"/></param>
        public SamlAdvice(IEnumerable<SamlAssertion> assertions)
            : this(null, assertions)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAdvice"/>.
        /// </summary>
        /// <param name="references"><see cref="IEnumerable{String}"/>.</param>
        /// <param name="assertions"><see cref="IEnumerable{SamlAssertion}"/>.</param>
        public SamlAdvice(IEnumerable<string> references, IEnumerable<SamlAssertion> assertions)
        {
            AssertionIdReferences = (references != null) ? new List<string>(references) : new List<string>();
            Assertions = (assertions != null) ? new List<SamlAssertion>(assertions) : new List<SamlAssertion>();
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{String}"/> representing the assertions in the <see cref="SamlAdvice"/>.
        /// </summary>
        public ICollection<string> AssertionIdReferences
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{SamlAssertion}"/> representating the assertions in the <see cref="SamlAdvice"/>.
        /// </summary>
        public ICollection<SamlAssertion> Assertions
        {
            get;
        }
    }
}

