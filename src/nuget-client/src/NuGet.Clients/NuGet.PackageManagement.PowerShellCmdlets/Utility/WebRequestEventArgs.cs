// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    public class WebRequestEventArgs : EventArgs
    {
        public WebRequest Request { get; private set; }

        public WebRequestEventArgs(WebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Request = request;
        }
    }
}
