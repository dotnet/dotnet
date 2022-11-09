// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands.SignCommand;

namespace NuGet.CommandLine
{
    /// <summary>
    /// Allows requesting a user to input their password through Console.
    /// </summary>
    internal class ConsolePasswordProvider : IPasswordProvider
    {
        private IConsole _console;

        public ConsolePasswordProvider(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

#if IS_DESKTOP
        /// <summary>
        /// Requests user to input password and returns it as a SecureString on Console.
        /// </summary>
        /// <param name="filePath">Path to the file that needs a password to open.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>SecureString containing the user input password. The SecureString should be disposed after use.</returns>
        public Task<SecureString> GetPassword(string filePath, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var password = new SecureString();

            _console.WriteLine(string.Format(CultureInfo.CurrentCulture, NuGetResources.ConsolePasswordProvider_DisplayFile, filePath));
            _console.Write(NuGetResources.ConsolePasswordProvider_PromptForPassword);
            _console.ReadSecureString(password);

            return Task.FromResult(password);
        }
#endif
    }
}
