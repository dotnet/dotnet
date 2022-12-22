// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    public class SourceValidationResult
    {
        private static readonly SourceValidationResult _none = new SourceValidationResult(
                SourceValidity.None,
                source: null,
                sourceRepository: null);

        private SourceValidationResult(SourceValidity validity, string source, SourceRepository sourceRepository)
        {
            Validity = validity;
            Source = source;
            SourceRepository = sourceRepository;
        }

        public static SourceValidationResult None => _none;

        public static SourceValidationResult Valid(string source, SourceRepository sourceRepository)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (sourceRepository == null)
            {
                throw new ArgumentNullException(nameof(sourceRepository));
            }

            return new SourceValidationResult(
                SourceValidity.Valid,
                source,
                sourceRepository);
        }

        public static SourceValidationResult UnknownSource(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new SourceValidationResult(
                SourceValidity.UnknownSource,
                source: source,
                sourceRepository: null);
        }

        public static SourceValidationResult UnknownSourceType(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new SourceValidationResult(
                SourceValidity.UnknownSourceType,
                source: source,
                sourceRepository: null);
        }

        public SourceValidity Validity { get; }
        public string Source { get; }
        public SourceRepository SourceRepository { get; }
    }
}
