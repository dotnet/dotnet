// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NuGet.Packaging.Signing
{
    internal sealed class HeapBlockRetainer : IDisposable
    {
        public HeapBlockRetainer()
        {
            _blocks = new List<SafeLocalAllocHandle>();
        }

        public IntPtr Alloc(int cbSize)
        {
            if (cbSize < 0)
            {
                throw new OverflowException();
            }
            var hBlock = new SafeLocalAllocHandle(Marshal.AllocHGlobal(cbSize));
            _blocks.Add(hBlock);
            return hBlock.DangerousGetHandle();
        }

        public IntPtr AllocAsciiString(string s)
        {
            var b = Encoding.ASCII.GetBytes(s);
            var pb = Alloc(b.Length + 1);
            Marshal.Copy(b, 0, pb, b.Length);
            unsafe
            {
                ((byte*)pb)[b.Length] = 0; // NUL termination.
            }
            return pb;
        }

        public void Dispose()
        {
            if (_blocks != null)
            {
                foreach (var h in _blocks)
                {
                    h.Dispose();
                }
            }
            _blocks = null;
        }

        private List<SafeLocalAllocHandle> _blocks;
    }
}
