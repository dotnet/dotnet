// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SignCheck.Verification.Jar
{
    /// <summary>
    /// Represents a collection of individual entries inside a manifest file (.SF/.MF).
    /// </summary>
    public class JarIndividualSection : List<JarIndividualEntry>
    {
        /// <summary>
        /// The raw text block representing the individual-section of a manifest.
        /// </summary>
        public string RawText
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of IndividualSection from the raw text comprising the individual-section in the manifest.
        /// </summary>
        /// <param name="SectionText"></param>
        public JarIndividualSection(string rawText)
        {
            RawText = rawText;
            Parse();
        }

        /// <summary>
        /// Parse the raw text of the individual section and add each entry to the list.
        /// </summary>
        private void Parse()
        {
            // Start index of the first entry
            int entryStartIndex = RawText.IndexOf("Name:");
            string entryText = String.Empty;

            while (entryStartIndex >= 0)
            {
                // If there is another entry we can determine where the current entry ends.
                int entryEndIndex = RawText.IndexOf("Name:", entryStartIndex + 1);

                if (entryEndIndex > 0)
                {
                    int entryLength = entryEndIndex - entryStartIndex;
                    entryText = RawText.Substring(entryStartIndex, entryLength);

                    Add(new JarIndividualEntry(entryText));
                }
                else
                {
                    entryText = RawText.Substring(entryStartIndex);
                    Add(new JarIndividualEntry(entryText));
                }

                // Go to the next entry
                entryStartIndex = entryEndIndex;
            }
        }
    }
}
