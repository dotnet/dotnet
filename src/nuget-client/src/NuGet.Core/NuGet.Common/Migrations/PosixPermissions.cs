// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Common.Migrations
{
    internal struct PosixPermissions
    {
        private readonly int _mode;

        public PosixPermissions(int mode)
        {
            _mode = mode;
        }

        public override string ToString()
        {
            string mode = Convert.ToString(_mode, 8);
            return mode.PadLeft(3, '0');
        }

        public static PosixPermissions Parse(string input)
        {
            int mode = Convert.ToInt32(input, 8);
            return new PosixPermissions(mode);
        }

        public bool SatisfiesUmask(PosixPermissions umask)
        {
            int combined = _mode & umask._mode;
            return combined == 0;
        }

        public PosixPermissions WithUmask(PosixPermissions umask)
        {
            int mode = _mode & (~umask._mode);
            return new PosixPermissions(mode);
        }
    }
}
