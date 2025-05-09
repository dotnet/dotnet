// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the SubjectStatement element.
    /// </summary>
    public abstract class SamlSubjectStatement : SamlStatement
    {
        private SamlSubject _subject;

        /// <summary>
        /// Gets or sets the subject of the statement.
        /// </summary>
        public virtual SamlSubject Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value ?? throw LogArgumentNullException(nameof(value));
            }
        }
    }
}
