// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Packaging.PackageExtraction
{
    public static class PackageExtractionBehavior
    {
        private const string XmlDocFileSaveModeEnvironmentKey = "NUGET_XMLDOC_MODE";
        private static XmlDocFileSaveMode? _xmlDocFileSaveMode;

        internal static IEnvironmentVariableReader environmentVariableReader { get; set; } = EnvironmentVariableWrapper.Instance;

        /// <summary>
        /// Gets or sets the <see cref="PackageExtraction.XmlDocFileSaveMode"/>.
        /// </summary>
        public static XmlDocFileSaveMode XmlDocFileSaveMode
        {
            get
            {
                if (_xmlDocFileSaveMode == null)
                {
                    var xmlDocFileMode = environmentVariableReader.GetEnvironmentVariable(XmlDocFileSaveModeEnvironmentKey);
                    if (string.Equals(xmlDocFileMode, XmlDocFileSaveMode.Compress.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        _xmlDocFileSaveMode = XmlDocFileSaveMode.Compress;
                    }
                    else if (string.Equals(xmlDocFileMode, XmlDocFileSaveMode.Skip.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        _xmlDocFileSaveMode = XmlDocFileSaveMode.Skip;
                    }
                    else
                    {
                        _xmlDocFileSaveMode = XmlDocFileSaveMode.None;
                    }
                }

                return _xmlDocFileSaveMode.Value;
            }
            set
            {
                _xmlDocFileSaveMode = value;
            }
        }
    }
}
