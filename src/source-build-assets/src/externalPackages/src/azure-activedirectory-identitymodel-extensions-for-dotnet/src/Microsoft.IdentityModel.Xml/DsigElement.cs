// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Base class for a XmlDsig element as per: https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/
    /// </summary>
    public class DSigElement
    {
        /// <summary>
        /// Initializes a <see cref="DSigElement"/> instance.
        /// </summary>
        protected DSigElement()
        {
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the prefix associated with the element.
        /// </summary>
        public string Prefix
        {
            get;
            set;
        } = "";
    }
}
