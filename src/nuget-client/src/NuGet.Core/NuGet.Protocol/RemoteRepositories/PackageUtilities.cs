// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    internal static class PackageUtilities
    {
        /// <summary>
        /// Create a <see cref="NuspecReader"/> from a nupkg stream.
        /// </summary>
        internal static NuspecReader OpenNuspecFromNupkg(string id, Stream nupkgStream, ILogger log)
        {
            try
            {
                using (var reader = new PackageArchiveReader(nupkgStream, leaveStreamOpen: true))
                using (var nuspecStream = reader.GetNuspec())
                {
                    if (nupkgStream == null)
                    {
                        throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToGetNuspecStream, id));
                    }

                    return new NuspecReader(nuspecStream);
                }
            }
            catch (Exception exception) when (exception is PackagingException ||
                                              exception is InvalidDataException)
            {
                var fileStream = nupkgStream as FileStream;
                if (fileStream != null)
                {
                    log.LogWarning(string.Format(CultureInfo.CurrentCulture, Strings.Log_FileIsCorrupt, fileStream.Name));
                }

                throw;
            }
        }
    }
}
