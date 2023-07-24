// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// An opaque context used to store work when working with authentication artifacts.
    /// </summary>
    public class CallContext : LoggerContext
    {
        /// <summary>
        /// Instantiates a new <see cref="CallContext"/> with a default activityId.
        /// </summary>
        public CallContext() : base()
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="CallContext"/> with an activityId.
        /// </summary>
        public CallContext(Guid activityId) : base(activityId)
        {
        }
    }
}
