// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace NuGet.Credentials
{
    [Serializable]
    public class PluginException : Exception
    {
        private const string RedactedPassword = "********";

        public PluginException() { }

        public PluginException(string message) : base(message) { }

        public PluginException(string message, Exception inner) : base(message, inner) { }
#if IS_DESKTOP
        protected PluginException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
#endif
        public static PluginException Create(string path, Exception inner)
        {
            return new PluginException(
                string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Exception_Format, path, inner.GetType().Name),
                inner);
        }

        public static PluginException CreateTimeoutMessage(string path, int timeoutMillis)
        {
            return new PluginException(
                string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Timeout_Format, path, timeoutMillis));
        }

        public static PluginException CreateNotStartedMessage(string path)
        {
            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_NotStarted_Format, path));
        }

        public static PluginException CreatePathNotFoundMessage(string path, string attempted)
        {
            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_PathNotFound_Format, path,
                attempted));
        }

        public static PluginException CreateAbortMessage(string path, string message)
        {
            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Abort_Format, path, message));
        }

        public static PluginException CreateUnreadableResponseExceptionMessage(
            string path,
            PluginCredentialResponseExitCode status)
        {
            return new PluginException(string.Format(
                CultureInfo.CurrentCulture,
                Resources.PluginException_UnreadableResponse_Format,
                path,
                status));
        }

        public static PluginException CreateInvalidResponseExceptionMessage(
            string path,
            PluginCredentialResponseExitCode status,
            PluginCredentialResponse response)
        {
            return new PluginException(string.Format(
                CultureInfo.CurrentCulture,
                Resources.PluginException_InvalidResponse_Format,
                path,
                status,
                response.Username,
                response.Password == null ? string.Empty : RedactedPassword,
                response.AuthTypes == null ? string.Empty : string.Join(", ", response.AuthTypes),
                response.Message));
        }
    }
}
