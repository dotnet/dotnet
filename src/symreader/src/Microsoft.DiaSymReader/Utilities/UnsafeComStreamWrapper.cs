// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

#if NET9_0_OR_GREATER
using System.Runtime.InteropServices.Marshalling;
#endif

namespace Microsoft.DiaSymReader.Utilities;

/// <summary>
/// Provides an <see cref="IStream"/>-like wrapper around an <see cref="IUnsafeComStream"/> to handle incompatibilities
/// introduced with source-generated ComWrappers.
/// </summary>
/// <remarks>
/// <see cref="ComStreamWrapper"/> is a wrapper around a <see cref="Stream"/> that implements both interface definitions.
/// This is a wrapper around a marshalled <see cref="IUnsafeComStream"/> that also implements the <see cref="IStream"/> interface./>
/// </remarks>
#if NET9_0_OR_GREATER
[GeneratedComClass]
#endif
internal partial class UnsafeComStreamWrapper : IUnsafeComStream, IStream
{
    private readonly IUnsafeComStream _stream;

    public UnsafeComStreamWrapper(IUnsafeComStream stream)
    {
        _stream = stream;
    }

    #region IUnsafeComStream

    public unsafe void Read(byte* pv, int cb, int* pcbRead) => _stream.Read(pv, cb, pcbRead);

    public unsafe void Write(byte* pv, int cb, int* pcbWritten) => _stream.Write(pv, cb, pcbWritten);

    public unsafe void Seek(long dlibMove, int dwOrigin, long* plibNewPosition) => _stream.Seek(dlibMove, dwOrigin, plibNewPosition);

    public void SetSize(long libNewSize) => _stream.SetSize(libNewSize);

    public unsafe void CopyTo(IntPtr pstm, long cb, int* pcbRead, int* pcbWritten) => _stream.CopyTo(pstm, cb, pcbRead, pcbWritten);

    public void Commit(int grfCommitFlags) => _stream.Commit(grfCommitFlags);

    public void Revert() => _stream.Revert();

    public void LockRegion(long libOffset, long cb, int dwLockType) => _stream.LockRegion(libOffset, cb, dwLockType);

    public void UnlockRegion(long libOffset, long cb, int dwLockType) => _stream.UnlockRegion(libOffset, cb, dwLockType);

    public void Stat(out STATSTG pstatstg, int grfStatFlag) => _stream.Stat(out pstatstg, grfStatFlag);

    public void Clone(out IntPtr ppstm) => _stream.Clone(out ppstm);

    #endregion

    #region IStream

    public void Clone(out IStream ppstm) => throw new NotSupportedException("Clone is not supported in this version.");

    public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten) => throw new NotImplementedException("CopyTo is not implemented in this version.");

    public unsafe void Read(byte[] pv, int cb, IntPtr pcbRead)
    {
        fixed (byte* pByte = pv)
        {
            _stream.Read(pByte, cb, (int*)pcbRead);
        }
    }

    public unsafe void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition) => _stream.Seek(dlibMove, dwOrigin, (long*)plibNewPosition);

    public unsafe void Write(byte[] pv, int cb, IntPtr pcbWritten)
    {
        fixed (byte* pByte = pv)
        {
            _stream.Write(pByte, cb, (int*)pcbWritten);
        }
    }

    void System.Runtime.InteropServices.ComTypes.IStream.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
    {
        _stream.Stat(out var unsafeSTASTG, grfStatFlag);

        pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG()
        {
            cbSize = unsafeSTASTG.cbSize,
        };
    }

    #endregion
}