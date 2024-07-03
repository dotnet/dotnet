// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Deployment.Tasks.Links
{
    /// <summary>
    ///     A single aka.ms link.
    /// </summary>
    public class AkaMSLink
    {
        /// <summary>
        /// Target of the link
        /// </summary>
        public string TargetUrl { get; set; }
        /// <summary>
        /// Short url of the link. Should only include the fragment element of the url, not the full aka.ms
        /// link.
        /// </summary>
        public string ShortUrl { get; set; }
        /// <summary>
        /// Description of the link.
        /// </summary>
        public string Description { get; set; } = "";
    }
}
