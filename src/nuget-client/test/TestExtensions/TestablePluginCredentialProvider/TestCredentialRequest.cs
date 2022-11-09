// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Test.TestExtensions.TestablePluginCredentialProvider
{
    public class TestCredentialRequest
    {
        public string Uri { get; set; }

        public bool NonInteractive { get; set; }

        public bool IsRetry { get; set; }

    }
}
