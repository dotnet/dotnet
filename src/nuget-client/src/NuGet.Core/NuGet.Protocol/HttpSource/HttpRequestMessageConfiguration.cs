// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.Protocol
{
    public class HttpRequestMessageConfiguration
    {
        public static readonly HttpRequestMessageConfiguration Default =
            new HttpRequestMessageConfiguration();

        public HttpRequestMessageConfiguration(
            ILogger logger = null,
            bool promptOn403 = true)
        {
            Logger = logger ?? NullLogger.Instance;
            PromptOn403 = promptOn403;
        }

        public ILogger Logger { get; }
        public bool PromptOn403 { get; }
    }
}
