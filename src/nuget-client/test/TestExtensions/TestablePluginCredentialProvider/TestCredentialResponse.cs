// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Test.TestExtensions.TestablePluginCredentialProvider
{
    public class TestCredentialResponse
    {
        public const string ResponseMessage = "NUGET_TESTABLECREDENTIALPROVIDER_RESPONSEABORTMESSAGE";
        public const string ResponseDelaySeconds = "NUGET_TESTABLECREDENTIALPROVIDER_RESPONSEDELAYSECONDS";
        public const string ResponseExitCode = "NUGET_TESTABLECREDENTIALPROVIDER_RESPONSEEXITCODE";
        public const string ResponsePassword = "NUGET_TESTABLECREDENTIALPROVIDER_RESPONSEPASSWORD";
        public const string ResponseShouldThrow = "NUGET_TESTABLECREDENTIALPROVIDER_RESPONSESHOULDTHROW";
        public const string ResponseUserName = "NUGET_TESTABLECREDENTIALPROVIDER_RESPONSEUSERNAME";

        public const int SuccessExitCode = 0;
        public const int ProviderNotApplicableExitCode = 1;
        public const int FailureExitCode = 2;

        public static void ClearAllEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable(ResponseMessage, string.Empty);
            Environment.SetEnvironmentVariable(ResponseDelaySeconds, string.Empty);
            Environment.SetEnvironmentVariable(ResponseExitCode, string.Empty);
            Environment.SetEnvironmentVariable(ResponsePassword, string.Empty);
            Environment.SetEnvironmentVariable(ResponseShouldThrow, string.Empty);
            Environment.SetEnvironmentVariable(ResponseUserName, string.Empty);
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Message { get; set; }

    }
}
