// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Linq;

namespace Microsoft.DotNet.SourceBuild.Tasks.UsageReport
{
    public class UsageValidationData
    {
        /// <summary>
        /// A human-readable report of new and removed prebuilts.
        /// </summary>
        public XElement Report { get; set; }

        /// <summary>
        /// The actual usage data from the build. Can be used as a baseline in future runs.
        /// </summary>
        public UsageData ActualUsageData { get; set; }
    }
}
