// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace NuGet.PackageManagement.VisualStudio
{
    internal static class NativeMethods
    {
        public const int IDCANCEL = 2;
        public const int IDYES = 6;
        public const int IDNO = 7;

        private const int ErrorCancelled = 1223;
        private const int MaxPasswordLength = 256;
        private const int MaxUserNameLength = 513;

        [Flags]
        private enum CredUiFlags
        {
            IncorrectPassword = 0x1,
            DoNotPersist = 0x2,
            AlwaysShowUi = 0x80,
            GenericCredentials = 0x40000,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CredUiInfo
        {
            public int Size;
            public IntPtr ParentWindow;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string MessageText;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string CaptionText;

            public IntPtr Banner;
        }

        [DllImport("credui.dll", EntryPoint = "CredUIPromptForCredentialsW", CharSet = CharSet.Unicode)]
        private static extern int CredUIPromptForCredentials(
            ref CredUiInfo info,
            string targetName,
            IntPtr reserved,
            int authenticationError,
            StringBuilder userName,
            int maxUserNameCharacters,
            StringBuilder password,
            int maxPasswordCharacters,
            ref int save,
            CredUiFlags flags);

        internal static NetworkCredential? PromptForCredentials(
            string caption,
            string message,
            string targetName,
            bool isRetry,
            IntPtr parentWindow)
        {
            var info = new CredUiInfo
            {
                Size = Marshal.SizeOf<CredUiInfo>(),
                ParentWindow = parentWindow,
                MessageText = message,
                CaptionText = caption,
            };
            var userName = new StringBuilder(MaxUserNameLength);
            var password = new StringBuilder(MaxPasswordLength);
            var save = 0;
            var flags = CredUiFlags.AlwaysShowUi | CredUiFlags.DoNotPersist | CredUiFlags.GenericCredentials;

            if (isRetry)
            {
                flags |= CredUiFlags.IncorrectPassword;
            }

            try
            {
                int result = CredUIPromptForCredentials(
                    ref info,
                    targetName,
                    IntPtr.Zero,
                    authenticationError: 0,
                    userName,
                    MaxUserNameLength,
                    password,
                    MaxPasswordLength,
                    ref save,
                    flags);

                if (result == ErrorCancelled)
                {
                    return null;
                }

                if (result != 0)
                {
                    throw new Win32Exception(result);
                }

                using (var securePassword = new SecureString())
                {
                    for (var index = 0; index < password.Length; index++)
                    {
                        securePassword.AppendChar(password[index]);
                    }

                    securePassword.MakeReadOnly();
                    return new NetworkCredential(userName.ToString(), securePassword);
                }
            }
            finally
            {
                for (var index = 0; index < password.Length; index++)
                {
                    password[index] = '\0';
                }
            }
        }
    }
}
