// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Microsoft.SignCheck.Verification.Jar
{
    public class JarAttributes : Dictionary<string, string>
    {

        /// <summary>
        /// The raw text comprising a set of attributes.
        /// </summary>
        public string RawText
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a JarAttributes instance from a raw text block.
        /// </summary>
        /// <param name="rawText">The raw text containing the attributes.</param>
        public JarAttributes(string rawText)
        {
            RawText = rawText;
        }

        /// <summary>
        /// Creates an instance of JarAttributes using the raw section from a manifest file. 
        /// </summary>
        /// <param name="rawText">A string containing the raw section (main, individual) text.</param>
        /// <returns></returns>
        public static JarAttributes From(string rawText)
        {
            var jarAttributes = new JarAttributes(rawText);

            if (String.IsNullOrEmpty(rawText))
            {
                return jarAttributes;
            }

            // We need to deal with continuations (multi-line attributes), so convert the raw text to a stream
            // and then parse it.
            using (Stream s = JarUtils.ToStream(rawText))
            using (StreamReader reader = new StreamReader(s))
            {
                string line = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    if (line.Contains(':'))
                    {
                        string[] attributeParts = line.Split(':');

                        string attributeName = attributeParts[0];
                        string attributeValue = attributeParts[1].TrimStart(' ');

                        line = reader.ReadLine();

                        // Continuation values start with SPACE
                        while (line.StartsWith(" "))
                        {
                            line = line.TrimStart(' ').TrimEnd(JarUtils.NewLine);
                            attributeValue += line;
                        }

                        jarAttributes[attributeName] = attributeValue;
                    }
                    else
                    {
                        line = reader.ReadLine();
                    }
                }
            }

            return jarAttributes;
        }
    }
}
