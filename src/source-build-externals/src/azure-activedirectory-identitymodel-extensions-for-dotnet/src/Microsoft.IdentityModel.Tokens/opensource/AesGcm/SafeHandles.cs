//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Microsoft.IdentityModel.Tokens.Interop;

namespace Microsoft.IdentityModel.Tokens
{
    internal sealed class SafeAlgorithmHandle : SafeBCryptHandle
    {
        protected sealed override bool ReleaseHandle()
        {
            uint ntStatus = BCryptCloseAlgorithmProvider(handle, 0);
            return ntStatus == 0;
        }

        [DllImport(Libraries.BCrypt)]
        private static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, int dwFlags);
    }

    internal abstract class SafeBCryptHandle : SafeHandle
    {
        protected SafeBCryptHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public sealed override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected abstract override bool ReleaseHandle();
    }

    internal sealed class SafeKeyHandle : SafeBCryptHandle
    {
        private SafeAlgorithmHandle _parentHandle;

        public void SetParentHandle(SafeAlgorithmHandle parentHandle)
        {
            Debug.Assert(_parentHandle == null);
            Debug.Assert(parentHandle != null);
            Debug.Assert(!parentHandle.IsInvalid);

            bool ignore = false;
            parentHandle.DangerousAddRef(ref ignore);

            _parentHandle = parentHandle;
        }

        protected sealed override bool ReleaseHandle()
        {
            if (_parentHandle != null)
            {
                _parentHandle.DangerousRelease();
                _parentHandle = null;
            }

            uint ntStatus = BCryptDestroyKey(handle);
            return ntStatus == 0;
        }

        [DllImport(Libraries.BCrypt)]
        private static extern uint BCryptDestroyKey(IntPtr hKey);
    }
}
