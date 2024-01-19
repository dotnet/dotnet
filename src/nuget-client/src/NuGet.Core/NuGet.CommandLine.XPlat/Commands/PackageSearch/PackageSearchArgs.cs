// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace NuGet.CommandLine.XPlat
{
    internal class PackageSearchArgs
    {
        private const int DefaultSkip = 0;
        private const int DefaultTake = 20;
        public List<string> Sources { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool Prerelease { get; set; }
        public bool ExactMatch { get; set; }
        public bool Interactive { get; set; }
        public ILoggerWithColor Logger { get; set; }
        public string SearchTerm { get; set; }
        public PackageSearchVerbosity Verbosity { get; set; } = PackageSearchVerbosity.Normal;
        public PackageSearchFormat Format { get; set; } = PackageSearchFormat.Table;

        public PackageSearchArgs(string skip, string take, string format, string verbosity)
        {
            Skip = VerifyInt(skip, DefaultSkip, "--skip");
            Take = VerifyInt(take, DefaultTake, "--take");
            Format = GetFormatFromOption(format);
            Verbosity = GetVerbosityFromOption(verbosity);
        }

        public PackageSearchArgs() { }

        private int VerifyInt(string number, int defaultValue, string option)
        {
            if (string.IsNullOrEmpty(number))
            {
                return defaultValue;
            }

            if (int.TryParse(number, out int verifiedNumber))
            {
                return verifiedNumber;
            }

            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidOptionValue, number, option));
        }

        private PackageSearchFormat GetFormatFromOption(string format)
        {
            PackageSearchFormat packageSearchFormat = PackageSearchFormat.Table;

            if (!string.IsNullOrEmpty(format) && !Enum.TryParse(format, ignoreCase: true, out packageSearchFormat))
            {
                packageSearchFormat = PackageSearchFormat.Table;
            }

            return packageSearchFormat;
        }

        private PackageSearchVerbosity GetVerbosityFromOption(string verbosity)
        {
            PackageSearchVerbosity packageSearchVerbosity = PackageSearchVerbosity.Normal;

            if (!string.IsNullOrEmpty(verbosity) && !Enum.TryParse(verbosity, ignoreCase: true, out packageSearchVerbosity))
            {
                packageSearchVerbosity = PackageSearchVerbosity.Normal;
            }

            return packageSearchVerbosity;
        }
    }
}
