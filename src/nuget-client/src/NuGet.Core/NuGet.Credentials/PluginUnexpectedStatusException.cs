// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace NuGet.Credentials
{
    /// <summary>
    /// PluginUnexpectedStatusException results when a plugin credential provider returns an unexpected status,
    /// one not enumerated in PluginCredentialResponseExitCode.
    /// This typically occurs when a plugin throws a terminating exception.
    /// </summary>
    [Serializable]
    public class PluginUnexpectedStatusException : PluginException
    {
        public PluginUnexpectedStatusException() { }

        public PluginUnexpectedStatusException(string message) : base(message) { }

        public PluginUnexpectedStatusException(string message, Exception inner) : base(message, inner) { }
#if IS_DESKTOP
        protected PluginUnexpectedStatusException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        { }
#endif
        public static PluginException CreateUnexpectedStatusMessage(
            string path, PluginCredentialResponseExitCode status)
        {
            return new PluginUnexpectedStatusException(
                string.Format(CultureInfo.CurrentCulture, Resources.PluginException_UnexpectedStatus_Format, path, status));
        }
    }
}
