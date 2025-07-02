// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// An opaque context used to store work when working with authentication artifacts.
    /// </summary>
    [Obsolete("The 'TokenContext' property is obsolete. Please use 'CallContext' instead.")]
    public class TokenContext : CallContext
    {
        /// <summary>
        /// Instantiates a new <see cref="TokenContext"/> with a default activity ID.
        /// </summary>
        public TokenContext() : base()
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="TokenContext"/> with an activity ID.
        /// </summary>
        public TokenContext(Guid activityId) : base (activityId)
        {
        }
    }
}
