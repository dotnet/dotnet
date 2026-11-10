// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.VisualStudio.TestPlatform.Common.Utilities;

/// <summary>
/// Decides whether a name looks like it holds a secret, so its value can be kept out of
/// diagnostic logs and other diagnostic output.
///
/// This matches on the name rather than on a list of exact variables on purpose. An exact list
/// has to be updated whenever a variable is added or renamed, and the one time it is not, the
/// value is written to a log. Matching on the name is wrong only in the safe direction: a
/// variable that is not a secret is redacted, which costs a little diagnostic detail, while a
/// secret that nobody remembered to add to a list is still redacted.
///
/// This is not in CoreUtilities, where it started, and the reason is worth keeping.
/// Message.ToString calls it on the logging path, and that runs in testhost. When app domains are
/// disabled the testhost assembly base is the test assembly's own folder, and a .NET Framework test
/// project's output carries exactly three of this product's assemblies - ObjectModel, CoreUtilities
/// and PlatformAbstractions - at whichever version it happened to restore. Every version of this
/// product stamps AssemblyVersion 15.0.0.0, so the loader accepts an old one rather than failing to
/// bind, and a type added since then is simply missing, which surfaces as a TypeLoadException on the
/// first traced message. Common is not one of those three, so it is always resolved from testhost's
/// own folder.
/// </summary>
internal static class SecretRedaction
{
    /// <summary>
    /// What is written in place of a value that looks like a secret.
    /// </summary>
    internal const string RedactedValue = "<redacted>";

    /// <summary>
    /// Substrings that mark a name as holding a secret. Compared case-insensitively, anywhere in
    /// the name, so <c>VSTEST_CONNECTION_SECRETS</c>, <c>MySecret</c> and <c>my.secret.value</c>
    /// all match. Kept deliberately broad, in line with the deny lists build systems use for the
    /// same job, because over-redacting is cheap and under-redacting is not.
    /// </summary>
    private static readonly string[] SecretNameMarkers =
    {
        "SECRET",
        "PASSWORD",
        "PASSWD",
        "PWD",
        "TOKEN",
        "CREDENTIAL",
        "KEY",
        "SIGNATURE",
        "BEARER",
        "AUTH",
        "NONCE",
        "SALT",
        "CIPHER",
        "COOKIE",
    };

    /// <summary>
    /// Matches a JSON string property and captures its name, tolerating escaped characters in
    /// both the name and the value so a value containing a quote does not end the match early.
    /// </summary>
    private static readonly Regex SecretJsonProperty = new Regex(
        "\"(?<name>(?:[^\"\\\\]|\\\\.)*)\"\\s*:\\s*\"(?:[^\"\\\\]|\\\\.)*\"",
        RegexOptions.CultureInvariant);

    /// <summary>
    /// Returns true when the name looks like it holds a secret.
    /// </summary>
    internal static bool IsSecretName(string name)
    {
        if (name == null)
        {
            return false;
        }

        foreach (var marker in SecretNameMarkers)
        {
            if (name.IndexOf(marker, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the value, or <see cref="RedactedValue"/> when the name looks like it holds a
    /// secret. Use this wherever a name and value pair is about to be written somewhere a person
    /// can read.
    /// </summary>
    internal static string RedactIfSecret(string name, string value)
    {
        return IsSecretName(name) ? RedactedValue : value;
    }

    /// <summary>
    /// Replaces the value of every JSON string property whose name looks like it holds a secret.
    /// Used on a raw protocol message before it is written to a diagnostic log, so a message
    /// that carries a secret directly, or carries a child process's environment block, does not
    /// put the value in the log.
    ///
    /// This works on the raw text rather than by parsing, because it runs on a logging path for
    /// every message, and because it must not fail on a message it does not understand.
    /// </summary>
    internal static string RedactSecretsInJson(string json)
    {
        // Deliberately not string.IsNullOrEmpty or an IsNullOrEmpty extension. The first is
        // banned by this repo's analyzers, and the second is a per-assembly helper that the
        // oldest servicing branches do not have, so neither travels with a linked file.
        if (json == null || json.Length == 0)
        {
            return string.Empty;
        }

        // Almost no message mentions a secret, so pay for the scan below only when one might.
        if (!ContainsSecretMarker(json))
        {
            return json;
        }

        return SecretJsonProperty.Replace(
            json,
            match => match.Groups["name"].Value.Length > 0 && IsSecretName(match.Groups["name"].Value)
                ? string.Format(
                    CultureInfo.InvariantCulture,
                    "\"{0}\":\"{1}\"",
                    match.Groups["name"].Value,
                    RedactedValue)
                : match.Value);
    }

    private static bool ContainsSecretMarker(string text)
    {
        foreach (var marker in SecretNameMarkers)
        {
            if (text.IndexOf(marker, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }

        return false;
    }
}
