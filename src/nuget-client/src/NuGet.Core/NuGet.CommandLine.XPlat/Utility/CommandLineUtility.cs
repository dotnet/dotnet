// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace NuGet.CommandLine.XPlat
{
    /// <summary>
    /// This class holds common commandline helper methods.
    /// </summary>
    internal static class CommandLineUtility
    {
        /// <summary>
        /// Helper method to join across multiple values of a commandline option.
        /// E.g. -
        /// Input - { "net45; netcoreapp1.1", "netstandard1.6; net46", "net451"}
        /// Output - [ "net45", "netcoreapp1.1", "netstandard1.6", "net46", "net451" ]
        /// </summary>
        /// <param name="inputs">List of values.</param>
        /// <returns>A string array of values joined from across multiple input values.</returns>
        public static string[] SplitAndJoinAcrossMultipleValues(IList<string> inputs)
        {
            var result = new List<string>();

            if (inputs?.Count > 0)
            {
                foreach (var input in inputs)
                {
                    result.AddRange(MSBuildStringUtility.Split(input));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parses a command line argument's value to a supported hash algorithm and validates it is supported in the given specification
        /// </summary>
        /// <param name="optionValue">Value entered by the user in the given command line argument</param>
        /// <param name="optionName">Name of the command line argument</param>
        /// <param name="spec">Signing specification to validate parsed hash algorithm</param>
        /// <returns>Supported hash algorithm</returns>
        internal static HashAlgorithmName ParseAndValidateHashAlgorithm(string optionValue, string optionName, SigningSpecifications spec)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

            if (!string.IsNullOrEmpty(optionValue))
            {
                hashAlgorithm = CryptoHashUtility.GetHashAlgorithmName(optionValue);
            }

            if (hashAlgorithm == HashAlgorithmName.Unknown || !spec.AllowedHashAlgorithms.Contains(hashAlgorithm))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Strings.Err_InvalidValue,
                    optionName, string.Join(",", spec.AllowedHashAlgorithms)));
            }

            return hashAlgorithm;
        }
    }
}
