// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Net;

namespace NuGet.PackageManagement.VisualStudio
{
    internal sealed class PackageSourceCredentialPrompt : IPackageSourceCredentialPrompt
    {
        public NetworkCredential? PromptForCredentials(Uri packageSourceUri, bool isRetry, IntPtr parentWindow)
        {
            string source = GetDisplayName(packageSourceUri);
            string message = string.Format(
                CultureInfo.CurrentCulture,
                Strings.CredentialPrompt_PackageSourceMessage,
                source);

            return NativeMethods.PromptForCredentials(
                Strings.CredentialPrompt_PackageSourceCaption,
                message,
                source,
                isRetry,
                parentWindow);
        }

        internal static string GetDisplayName(Uri packageSourceUri)
        {
            return packageSourceUri.GetComponents(
                UriComponents.SchemeAndServer | UriComponents.Path,
                UriFormat.SafeUnescaped);
        }
    }
}
