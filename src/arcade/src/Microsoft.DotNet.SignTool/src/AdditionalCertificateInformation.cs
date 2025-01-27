// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.DotNet.SignTool
{
    public class AdditionalCertificateInformation
    {
        /// <summary>
        /// If true, the certificate name can be used to sign already signed binaries.
        /// </summary>
        public bool DualSigningAllowed { get; set; }
        /// <summary>
        /// If the certificate name represents a sign+notarize operation, this is the name of the sign operation.
        /// </summary>
        public string MacSigningOperation { get; set; }
        /// <summary>
        /// If the certificate name represents a sign+notarize operation, this is the name of the notarize operation.
        /// </summary>
        public string MacNotarizationAppName { get; set; }
        public string CollisionPriorityId { get; set; }
    }
}
