// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
#if NET
using System.Formats.Tar;
#endif

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
            timestamp = Regex.Replace(timestamp, @"\s{2,}", " ").Trim();

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

            if (TryParseCodeSignTimestamp(timestamp, out dateTime))
            {
                return dateTime;
            }

            if (TryParseOpensslTimestamp(timestamp, out dateTime))
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
        public static (int exitCode, string output, string error) RunBashCommand(string command, string workingDirectory = null)
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Environment.CurrentDirectory;
            }

            var psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory,
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
        /// Download the Microsoft and Azure Linux public keys and import them into the keyring.
        /// </summary>
        public static void DownloadAndConfigurePublicKeys(string tempDir)
        {
            string[] keyUrls = new string[]
            {
                "https://packages.microsoft.com/keys/microsoft.asc", // Microsoft public key
                "https://raw.githubusercontent.com/microsoft/azurelinux/3.0/SPECS/azurelinux-repos/MICROSOFT-RPM-GPG-KEY" // Azure linux public key
            };
            foreach (string keyUrl in keyUrls)
            {
                string keyPath = Path.Combine(tempDir, Path.GetFileName(keyUrl));
                using (Stream stream = s_client.GetStreamAsync(keyUrl).Result)
                {
                    using (FileStream fileStream = File.Create(keyPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                (int exitCode, _, string error) = RunBashCommand($"gpg --import {keyPath}");

                if (exitCode != 0)
                {
                    throw new Exception($"Failed to import Microsoft public key: {(string.IsNullOrEmpty(error) ? "unknown error" : error)}");
                }
            }
        }

        /// <summary>
        /// Gets the next entry in a tar archive.
        /// </summary>
        public static TarEntry TryGetNextTarEntry(this TarReader reader)
        {
            try
            {
                return reader.GetNextEntry();
            }
            catch (EndOfStreamException)
            {
                // The stream is empty
                return null;
            }
        }
#endif

        /// <summary>
        /// Parses a code signing timestamp string into a DateTime object.
        /// </summary>
        private static bool TryParseCodeSignTimestamp(string timestamp, out DateTime dateTime)
        {
            // Normalize single-digit day and hour by adding a leading zero where necessary (e.g., "Feb 1," or "at 7:" => "Feb 01," or "at 07:")
            string normalizedTimestamp = Regex.Replace(timestamp, @"(?<=\b[A-Za-z]{3}\s)(\d)(?=,\s)|(?<=at\s)(\d)(?=:)", match =>
            {
                return "0" + match.Value;
            });
            

            string codesignFormat = "MMM dd, yyyy 'at' hh:mm:ss tt";
            if (DateTime.TryParseExact(normalizedTimestamp, codesignFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parses an OpenSSL timestamp string into a DateTime object.
        /// </summary>
        private static bool TryParseOpensslTimestamp(string timestamp, out DateTime dateTime)
        {
            // As per https://www.ietf.org/rfc/rfc5280.txt, X.509 certificate time fields must be in GMT.
            string timezone = timestamp.ExtractTimezone();
            if (!string.IsNullOrEmpty(timezone) && timezone.Equals("GMT"))
            {
                // Normalize single-digit day and hour by adding a leading zero where necessary (e.g., "Feb 1" or "7:" => "Feb 01" or "07:").
                string normalizedTimestamp = Regex.Replace(timestamp, @"(?<=\b[A-Za-z]{3}\s)(\d)(?=\s)|(?<=\s)(\d)(?=:)", match =>
                {
                    return "0" + match.Value;
                });

                // GMT is equivalent to UTC+0
                normalizedTimestamp = normalizedTimestamp.Replace(timezone, "+00:00").Trim();

                string opensslFormat = "MMM dd HH:mm:ss yyyy zzz";
                if (DateTime.TryParseExact(normalizedTimestamp, opensslFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dateTime))
                {
                    return true;
                }
            }

            dateTime = default;
            return false;
        }

        /// <summary>
        /// Extracts the timezone from a timestamp string.
        /// </summary>
        private static string ExtractTimezone(this string timestamp)
        {
            var timezoneRegex = new Regex(@"\s(?<timezone>[a-zA-Z]{3,4})");
            return timezoneRegex.Match(timestamp).GroupValueOrDefault("timezone");
        }
    }
}
