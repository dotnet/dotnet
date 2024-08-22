// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

namespace NuGet.Commands
{
    /// <summary>
    /// Wrapper for RestoreRequest
    /// </summary>
    public class RestoreSummaryRequest
    {
        public RestoreRequest Request { get; }

        public IEnumerable<string> ConfigFiles { get; }

        public IReadOnlyList<SourceRepository> Sources { get; }

        public string InputPath { get; }

        public RestoreSummaryRequest(
            RestoreRequest request,
            string inputPath,
            IEnumerable<string> configFiles,
            IReadOnlyList<SourceRepository> sources)
        {
            Request = request;
            ConfigFiles = configFiles;
            Sources = sources;
            InputPath = inputPath;
        }
    }
}
