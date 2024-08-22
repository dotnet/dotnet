// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Xml.Linq;

namespace NuGet.Packaging
{
    [Obsolete("This class is obsolete and will be removed in a future release.")]
    public static class XmlUtility
    {
        public static XDocument LoadSafe(Stream input)
        {
            return Shared.XmlUtility.Load(input);
        }

        public static XDocument LoadSafe(Stream input, bool ignoreWhiteSpace)
        {
            if (ignoreWhiteSpace)
                return Shared.XmlUtility.Load(input);

            return Shared.XmlUtility.Load(input, LoadOptions.PreserveWhitespace);
        }
    }
}
