// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

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

        public string Username { get; set; }

        public string Password { get; set; }

        public string Message { get; set; }

    }
}
