// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;

namespace Microsoft.SignCheck.Verification.Jar
{
    public class JarIndividualEntry
    {
        public JarAttributes Attributes
        {
            get;
            private set;
        }

        /// <summary>
        /// The value of the x-DIGEST attribute.
        /// </summary>
        public string DigestValue
        {
            get;
            private set;
        }

        /// <summary>
        /// The hash algorithm associated with the x-DIGEST attribute.
        /// </summary>
        public string HashAlgorithmName
        {
            get;
            private set;
        }

        /// <summary>
        /// The value of the Name attribute in the manifest, e.g. foo/bar.class.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The raw text associated with the individual entry.
        /// </summary>
        public string RawText
        {
            get;
            private set;
        }

        public bool Verified
        {
            get;
            private set;
        }

        public JarIndividualEntry(string entryText)
        {
            Attributes = JarAttributes.From(entryText);

            // Set up some properties to simplify tasks
            string name = null;
            if (Attributes.TryGetValue("Name", out name))
            {
                Name = name;
            }

            RawText = entryText;

            // Only look for xxx-DIGEST for now.
            // There are also xxx-DIGEST-yyy attributes for language specific files we need to deal with later
            string digestAttributeKey = Attributes.Keys.FirstOrDefault(key => key.EndsWith("-Digest", StringComparison.OrdinalIgnoreCase));

            if (!String.IsNullOrEmpty(digestAttributeKey))
            {
                string manifestDigest = Attributes[digestAttributeKey];
                HashAlgorithmName = JarUtils.GetHashAlgorithmFromDigest(digestAttributeKey, "-Digest");
                DigestValue = Attributes[digestAttributeKey];
            }
        }

        public void Reset()
        {
            Verified = false;
        }
    }
}
