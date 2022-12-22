// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk
{
    /// <summary>
    /// Checks that the content of two license files is the same modulo line breaks, leading and trailing whitespace.
    /// </summary>
    public class ValidateLicense : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to the file that contains the license text to be validated.
        /// </summary>
        [Required]
        public string LicensePath { get; set; }

        /// <summary>
        /// Full path to the file that contains expected license text.
        /// </summary>
        [Required]
        public string ExpectedLicensePath { get; set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            var actualLines = File.ReadAllLines(LicensePath, Encoding.UTF8);
            var expectedLines = File.ReadAllLines(ExpectedLicensePath, Encoding.UTF8);

            if (!LinesEqual(actualLines, expectedLines))
            {
                Log.LogError($"License file content '{LicensePath}' doesn't match the expected license '{ExpectedLicensePath}'.");
            }
        }

        internal static bool LinesEqual(IEnumerable<string> actual, IEnumerable<string> expected)
        {
            IEnumerable<string> normalize(IEnumerable<string> lines)
                => from line in lines
                   where !string.IsNullOrWhiteSpace(line)
                   select line.Trim();

            var normalizedActual = normalize(actual).ToArray();
            var normalizedExpected = normalize(expected).ToArray();

            if (normalizedActual.Length != normalizedExpected.Length)
            {
                return false;
            }

            for (int i = 0; i < normalizedActual.Length; i++)
            {
                if (normalizedExpected[i] == "*ignore-line*")
                {
                    continue;
                }

                if (normalizedActual[i] != normalizedExpected[i])
                {
                    return false;
                }
            }

            return true;
        }

    }
}
