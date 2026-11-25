// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using NuGet.Common;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Prompts for source credentials, optionally saving them in Windows Credential Manager.
    /// Saved credentials use NuGet-prefixed scheme-and-server targets, so they are isolated
    /// from other applications and from sources with a different scheme or port.
    /// Credential Manager errors are logged without preventing the prompt or use of entered credentials.
    /// </summary>
    internal sealed class PackageSourceCredentialPrompt : IPackageSourceCredentialPrompt
    {
        private readonly Func<string, NetworkCredential?> _readCredentials;
        private readonly NativeMethods.CredentialPrompt _promptForCredentials;
        private readonly Action<string, NetworkCredential> _writeCredentials;
        private readonly ILogger _logger;

        public PackageSourceCredentialPrompt()
            : this(
                  NativeMethods.ReadCredentials,
                  NativeMethods.PromptForCredentials,
                  NativeMethods.WriteCredentials,
                  new VisualStudioActivityLogger())
        {
        }

        internal PackageSourceCredentialPrompt(
            Func<string, NetworkCredential?> readCredentials,
            NativeMethods.CredentialPrompt promptForCredentials,
            Action<string, NetworkCredential> writeCredentials,
            ILogger logger)
        {
            _readCredentials = readCredentials;
            _promptForCredentials = promptForCredentials;
            _writeCredentials = writeCredentials;
            _logger = logger;
        }

        public NetworkCredential? PromptForCredentials(Uri packageSourceUri, bool isRetry, IntPtr parentWindow)
        {
            string targetName = "NuGet:" + packageSourceUri.GetComponents(
                UriComponents.SchemeAndServer,
                UriFormat.UriEscaped);
            if (!isRetry)
            {
                try
                {
                    if (_readCredentials(targetName) is NetworkCredential savedCredentials)
                    {
                        return savedCredentials;
                    }
                }
                catch (Win32Exception exception)
                {
                    _logger.LogWarning(
                        $"Failed to read package source credentials from Windows Credential Manager " +
                        $"(error {exception.NativeErrorCode}): {exception}");
                }
            }

            string source = GetDisplayName(packageSourceUri);
            string message = string.Format(
                CultureInfo.CurrentCulture,
                Strings.CredentialPrompt_PackageSourceMessage,
                source);

            NetworkCredential? credentials = _promptForCredentials(
                Strings.CredentialPrompt_PackageSourceCaption,
                message,
                targetName,
                isRetry,
                parentWindow,
                out bool saveCredentials);

            if (credentials != null && saveCredentials)
            {
                try
                {
                    _writeCredentials(targetName, credentials);
                }
                catch (Win32Exception exception)
                {
                    _logger.LogWarning(
                        $"Failed to save package source credentials to Windows Credential Manager " +
                        $"(error {exception.NativeErrorCode}): {exception}");
                }
            }

            return credentials;
        }

        internal static string GetDisplayName(Uri packageSourceUri)
        {
            return packageSourceUri.GetComponents(
                UriComponents.SchemeAndServer | UriComponents.Path,
                UriFormat.SafeUnescaped);
        }
    }
}
