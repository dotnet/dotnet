// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    internal sealed class DotnetVersionChecker : IDotnetVersionChecker
    {
        public static readonly DotnetVersionChecker Instance = new DotnetVersionChecker();

        private int _cachedVersion;

        public int DotnetVersion
        {
            get
            {
                if (_cachedVersion == 0)
                {
                    _cachedVersion = Environment.Version.Major;
                }

                return _cachedVersion;
            }
        }
    }
}
