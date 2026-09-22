// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;

namespace NuGet.PackageManagement.VisualStudio
{
    internal interface IPackageSourceCredentialPrompt
    {
        NetworkCredential? PromptForCredentials(Uri packageSourceUri, bool isRetry, IntPtr parentWindow);
    }
}
