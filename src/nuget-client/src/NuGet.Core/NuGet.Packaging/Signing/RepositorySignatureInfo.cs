// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    public class RepositorySignatureInfo
    {
        public bool AllRepositorySigned { get; }

        public IEnumerable<IRepositoryCertificateInfo> RepositoryCertificateInfos { get; }

        public RepositorySignatureInfo(bool allRepositorySigned, IEnumerable<IRepositoryCertificateInfo> repositoryCertificateInfos)
        {
            AllRepositorySigned = allRepositorySigned;
            RepositoryCertificateInfos = repositoryCertificateInfos;
        }
    }
}
