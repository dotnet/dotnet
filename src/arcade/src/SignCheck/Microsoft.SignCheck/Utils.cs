// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.SignCheck
{
    public static class Utils
    {
#if NET
        private static readonly HttpClient s_client = new(new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(10) });
#endif
        /// <summary>
        /// Generate a hash for a string value using a given hash algorithm.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <param name="hashName">The name of the <see cref="HashAlgorithm"/> to use.</param>
        /// <returns>A string containing the hash result.</returns>
        public static string GetHash(string value, string hashName)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            HashAlgorithm ha = CreateHashAlgorithm(hashName);
            byte[] hash = ha.ComputeHash(bytes);

            var sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static HashAlgorithm CreateHashAlgorithm(string hashName)
        {
            switch (hashName.ToUpperInvariant())
            {
                case "SHA256":
                    return SHA256.Create();
                case "SHA1":
                    return SHA1.Create();
                case "MD5":
                    return MD5.Create();
                case "SHA384":
                    return SHA384.Create();
                case "SHA512":
                    return SHA512.Create();
                default:
                    throw new ArgumentException("Unsupported hash algorithm name", nameof(hashName));
            }
        }

        /// <summary>
        /// Converts a string containing wildcards (*, ?) into a regular expression pattern string.
        /// </summary>
        /// <param name="wildcardPattern">The string pattern.</param>
        /// <returns>A string containing regular expression pattern.</returns>
        public static string ConvertToRegexPattern(string wildcardPattern)
        {
            string escapedPattern = Regex.Escape(wildcardPattern).Replace(@"\*", ".*").Replace(@"\?", ".");

            if ((wildcardPattern.EndsWith("*")) || (wildcardPattern.EndsWith("?")))
            {
                return escapedPattern;
            }
            else
            {
                return String.Concat(escapedPattern, "$");
            }            
        }

        /// <summary>
        /// Gets the DateTime value from a string
        /// Returns the specified default value if the match is unsuccessful or the timestamp value is 0.
        /// </summary>
        /// <param name="timestamp">The timestamp string to parse.</param>
        /// <param name="defaultValue">The default DateTime value to return if parsing fails.</param>
        /// <returns>The parsed DateTime value or the default value.</returns>
        public static DateTime DateTimeOrDefault(this string timestamp, DateTime defaultValue)
        {
            // Try to parse the timestamp as a Unix timestamp (seconds since epoch)
            if (long.TryParse(timestamp, out long unixTime) && unixTime > 0)
            {
                return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
            }

            // Try to parse the timestamp as a DateTime string
            if (DateTime.TryParse(timestamp, out DateTime dateTime))
            {
                return dateTime;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the value of a named group from a regex match.
        /// Returns null if the match is unsuccessful.
        /// </summary>
        /// <param name="match">The regex match.</param>
        /// <param name="groupName">The name of the group.</param>
        /// <returns>The value of the named group or null if the match is unsuccessful.</returns>
        public static string GroupValueOrDefault(this Match match, string groupName) =>
            match.Success ? match.Groups[groupName].Value : null;

        /// <summary>
        /// Captures the console output of an action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>A tuple containing the result of the action, the standard output, and the error output.</returns>
        public static (bool, string, string) CaptureConsoleOutput(Func<bool> action)
        {
            var consoleOutput = Console.Out;
            StringWriter outputWriter = new StringWriter();
            Console.SetOut(outputWriter);

            var errorOutput = Console.Error;
            StringWriter errorOutputWriter = new StringWriter();
            Console.SetError(errorOutputWriter);

            try
            {
                bool result = action();
                return (result, outputWriter.ToString(), errorOutputWriter.ToString());
            }
            finally
            {
                Console.SetOut(consoleOutput);
                Console.SetError(errorOutput);
            }
        }

        /// <summary>
        /// Runs a bash command and returns the output, error, and exit code.
        /// </summary>
        /// <param name="command">The command to run.</param>
        /// <returns>A tuple containing the exit code, output, and error.</returns>
        public static (int exitCode, string output, string error) RunBashCommand(string command)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit(10000); // 10 seconds
                
                return (process.ExitCode, output, error);
            }
        }

#if NET
        /// <summary>
        /// Download the Microsoft public key and import it into the keyring.
        /// </summary>
        public static void DownloadAndConfigureMicrosoftPublicKey(string tempDir)
        {
            using (Stream stream = s_client.GetStreamAsync("https://packages.microsoft.com/keys/microsoft.asc").Result)
            {
                using (FileStream fileStream = File.Create($"{tempDir}/microsoft.asc"))
                {
                    stream.CopyTo(fileStream);
                }
            }

            (int exitCode, _, string error) = RunBashCommand($"gpg --import {tempDir}/microsoft.asc");

            if (exitCode != 0)
            {
                throw new Exception($"Failed to import Microsoft public key: {(string.IsNullOrEmpty(error) ? "unknown error" : error)}");
            }
        }
#endif
    }
}
