// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using SdkResultBase = Microsoft.Build.Framework.SdkResult;

namespace Microsoft.Build.NuGetSdkResolver.Test
{
    /// <summary>
    /// Represents a mock implementation of <see cref="Microsoft.Build.Framework.SdkResult" />.
    /// </summary>
    internal class MockSdkResult : SdkResultBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockSdkResult" /> class as unsuccessful.
        /// </summary>
        /// <param name="errors">An <see cref="IEnumerable{T}" /> representing any errors that were logged.</param>
        /// <param name="warnings">An <see cref="IEnumerable{T}" /> representing any warnings that were logged.</param>
        public MockSdkResult(IEnumerable<string> errors, IEnumerable<string> warnings)
        {
            Success = false;
            Errors = errors ?? Enumerable.Empty<string>();
            Warnings = warnings ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockSdkResult" /> class as successful.
        /// </summary>
        /// <param name="path">The path to the resolved SDK.</param>
        /// <param name="version">The version of the resolved SDK.</param>
        /// <param name="warnings">An <see cref="IEnumerable{T}" /> representing any warnings that were logged.</param>
        public MockSdkResult(string path, string version, IEnumerable<string> warnings)
        {
            Success = true;
            Path = path;
            Version = version;
            Warnings = warnings ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}" /> representing any errors that were logged.
        /// </summary>
        public IEnumerable<string> Errors { get; } = Enumerable.Empty<string>();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}" /> representing any warnings that were logged.
        /// </summary>
        public IEnumerable<string> Warnings { get; } = Enumerable.Empty<string>();
    }
}
