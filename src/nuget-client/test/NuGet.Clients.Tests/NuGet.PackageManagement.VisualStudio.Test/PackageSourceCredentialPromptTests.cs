// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Net;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class PackageSourceCredentialPromptTests
    {
        private const int ErrorAccessDenied = 5;
        private const int ErrorNoSuchLogonSession = 1312;

        [Theory]
        [InlineData("https://unit.test/v3/index.json", "NuGet:https://unit.test")]
        [InlineData("https://unit.test:8443/other/index.json", "NuGet:https://unit.test:8443")]
        [InlineData("http://unit.test/v3/index.json", "NuGet:http://unit.test")]
        public void PromptForCredentials_WithSavedCredentials_DoesNotPrompt(string source, string expectedTarget)
        {
            var savedCredentials = new NetworkCredential("user", "password");
            var prompt = new PackageSourceCredentialPrompt(
                readCredentials: target =>
                {
                    Assert.Equal(expectedTarget, target);
                    return savedCredentials;
                },
                promptForCredentials: UnexpectedPrompt,
                writeCredentials: (_, _) => Assert.Fail("Saved credentials should not be written again."),
                logger: NullLogger.Instance);

            NetworkCredential? result = prompt.PromptForCredentials(new Uri(source), isRetry: false, IntPtr.Zero);

            Assert.Same(savedCredentials, result);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void PromptForCredentials_PersistsOnlyWhenSelected(bool isRetry, bool save)
        {
            var credentials = new NetworkCredential("user", "password");
            int reads = 0;
            int writes = 0;
            var parentWindow = new IntPtr(123);
            var logger = new TestLogger();
            var prompt = new PackageSourceCredentialPrompt(
                readCredentials: _ =>
                {
                    reads++;
                    return null;
                },
                promptForCredentials: (string caption, string message, string target, bool retry, IntPtr owner, out bool saveCredentials) =>
                {
                    Assert.Equal("NuGet:https://unit.test:8443", target);
                    Assert.Contains("https://unit.test:8443/v3/index.json", message);
                    Assert.DoesNotContain("secret", message);
                    Assert.Equal(isRetry, retry);
                    Assert.Equal(parentWindow, owner);
                    saveCredentials = save;
                    return credentials;
                },
                writeCredentials: (target, value) =>
                {
                    Assert.Equal("NuGet:https://unit.test:8443", target);
                    Assert.Same(credentials, value);
                    writes++;
                },
                logger: logger);

            NetworkCredential? result = prompt.PromptForCredentials(
                new Uri("https://user:secret@unit.test:8443/v3/index.json?token=secret"),
                isRetry,
                parentWindow);

            Assert.Same(credentials, result);
            Assert.Equal(isRetry ? 0 : 1, reads);
            Assert.Equal(save ? 1 : 0, writes);
            Assert.Empty(logger.WarningMessages);
        }

        [Fact]
        public void PromptForCredentials_WhenCanceled_DoesNotSave()
        {
            var prompt = new PackageSourceCredentialPrompt(
                readCredentials: _ => null,
                promptForCredentials: (string caption, string message, string target, bool retry, IntPtr owner, out bool saveCredentials) =>
                {
                    saveCredentials = true;
                    return null;
                },
                writeCredentials: (_, _) => Assert.Fail("Canceled credentials should not be saved."),
                logger: NullLogger.Instance);

            Assert.Null(prompt.PromptForCredentials(new Uri("https://unit.test"), isRetry: false, IntPtr.Zero));
        }

        [Theory]
        [InlineData(ErrorAccessDenied)]
        [InlineData(ErrorNoSuchLogonSession)]
        public void PromptForCredentials_WhenReadingFails_PromptsAndLogsWarning(int errorCode)
        {
            var credentials = new NetworkCredential("user", "password");
            var logger = new TestLogger();
            var prompt = new PackageSourceCredentialPrompt(
                readCredentials: _ => throw new Win32Exception(errorCode),
                promptForCredentials: (string caption, string message, string target, bool retry, IntPtr owner, out bool saveCredentials) =>
                {
                    saveCredentials = false;
                    return credentials;
                },
                writeCredentials: (_, _) => Assert.Fail("Credentials should not be saved when save was not selected."),
                logger: logger);

            NetworkCredential? result = prompt.PromptForCredentials(new Uri("https://unit.test"), isRetry: false, IntPtr.Zero);

            Assert.Same(credentials, result);
            string warning = Assert.Single(logger.WarningMessages);
            Assert.Contains("Failed to read", warning);
            Assert.Contains($"error {errorCode}", warning);
        }

        [Theory]
        [InlineData(ErrorAccessDenied, false)]
        [InlineData(ErrorNoSuchLogonSession, false)]
        [InlineData(ErrorNoSuchLogonSession, true)]
        public void PromptForCredentials_WhenSavingFails_ReturnsCredentialsAndLogsWarning(int errorCode, bool readAlsoFails)
        {
            var credentials = new NetworkCredential("user", "password");
            var logger = new TestLogger();
            var prompt = new PackageSourceCredentialPrompt(
                readCredentials: _ => readAlsoFails ? throw new Win32Exception(errorCode) : null,
                promptForCredentials: (string caption, string message, string target, bool retry, IntPtr owner, out bool saveCredentials) =>
                {
                    saveCredentials = true;
                    return credentials;
                },
                writeCredentials: (_, _) => throw new Win32Exception(errorCode),
                logger: logger);

            NetworkCredential? result = prompt.PromptForCredentials(new Uri("https://unit.test"), isRetry: false, IntPtr.Zero);

            Assert.Same(credentials, result);
            Assert.Equal(readAlsoFails ? 2 : 1, logger.WarningMessages.Count);
            Assert.Contains(logger.WarningMessages, warning =>
                warning.Contains("Failed to save") && warning.Contains($"error {errorCode}"));
            if (readAlsoFails)
            {
                Assert.Contains(logger.WarningMessages, warning =>
                    warning.Contains("Failed to read") && warning.Contains($"error {errorCode}"));
            }
        }

        [Fact]
        public void PromptForCredentials_WhenPromptFails_ReportsError()
        {
            var error = new Win32Exception(ErrorAccessDenied);
            var logger = new TestLogger();
            var prompt = new PackageSourceCredentialPrompt(
                readCredentials: _ => null,
                promptForCredentials: (string caption, string message, string target, bool retry, IntPtr owner, out bool saveCredentials) =>
                    throw error,
                writeCredentials: (_, _) => Assert.Fail("Credentials should not be saved when the prompt fails."),
                logger: logger);

            Assert.Same(error, Assert.Throws<Win32Exception>(
                () => prompt.PromptForCredentials(new Uri("https://unit.test"), isRetry: false, IntPtr.Zero)));
            Assert.Empty(logger.WarningMessages);
        }

        private static NetworkCredential? UnexpectedPrompt(
            string caption,
            string message,
            string target,
            bool retry,
            IntPtr owner,
            out bool saveCredentials)
        {
            saveCredentials = false;
            Assert.Fail("Saved credentials should be returned without showing the dialog.");
            return null;
        }

        [Fact]
        public void GetDisplayName_WithCredentialsAndQuery_DoesNotExposeSecrets()
        {
            var uri = new Uri("https://user:secret@unit.test/v3/index.json?token=secret");

            string source = PackageSourceCredentialPrompt.GetDisplayName(uri);

            Assert.Equal("https://unit.test/v3/index.json", source);
        }
    }
}
