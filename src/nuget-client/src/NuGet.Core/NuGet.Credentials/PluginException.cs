// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace NuGet.Credentials
{
    [Serializable]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PluginException : Exception
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private const string RedactedPassword = "********";

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PluginException() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PluginException(string message) : base(message) { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PluginException(string message, Exception inner) : base(message, inner) { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#if IS_DESKTOP
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected PluginException(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
#endif
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static PluginException Create(string path, Exception inner)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return new PluginException(
                string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Exception_Format, path, inner.GetType().Name),
                inner);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static PluginException CreateTimeoutMessage(string path, int timeoutMillis)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return new PluginException(
                string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Timeout_Format, path, timeoutMillis));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static PluginException CreateNotStartedMessage(string path)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_NotStarted_Format, path));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static PluginException CreatePathNotFoundMessage(string path, string attempted)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_PathNotFound_Format, path,
                attempted));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static PluginException CreateAbortMessage(string path, string message)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return new PluginException(string.Format(CultureInfo.CurrentCulture, Resources.PluginException_Abort_Format, path, message));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static PluginException CreateUnreadableResponseExceptionMessage(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            string path,
            PluginCredentialResponseExitCode status)
        {
            return new PluginException(string.Format(
                CultureInfo.CurrentCulture,
                Resources.PluginException_UnreadableResponse_Format,
                path,
                status));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static PluginException CreateInvalidResponseExceptionMessage(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
