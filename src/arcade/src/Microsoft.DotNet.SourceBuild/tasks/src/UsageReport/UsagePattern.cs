// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Microsoft.DotNet.SourceBuild.Tasks.UsageReport
{
    public class UsagePattern
    {
        public string IdentityRegex { get; set; }

        public string IdentityGlob { get; set; }

        public XElement ToXml() => new XElement(
            nameof(UsagePattern),
            IdentityRegex.ToXAttributeIfNotNull(nameof(IdentityRegex)),
            IdentityGlob.ToXAttributeIfNotNull(nameof(IdentityGlob)));

        public static UsagePattern Parse(XElement xml) => new UsagePattern
        {
            IdentityRegex = xml.Attribute(nameof(IdentityRegex))?.Value,
            IdentityGlob = xml.Attribute(nameof(IdentityGlob))?.Value
        };

        public Regex CreateRegex()
        {
            if (!string.IsNullOrEmpty(IdentityRegex))
            {
                return new Regex(IdentityRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }

            if (!string.IsNullOrEmpty(IdentityGlob))
            {
                // Escape regex characters like '.', but handle '*' as regex '.*'.
                string regex = Regex.Escape(IdentityGlob).Replace("\\*", ".*");

                return new Regex(
                    $"^{regex}$",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }

            return new Regex("");
        }
    }
}
