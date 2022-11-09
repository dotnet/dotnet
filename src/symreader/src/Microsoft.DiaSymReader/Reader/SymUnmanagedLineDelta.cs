// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.


namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// Line deltas allow a compiler to omit functions that have not been modified from
    /// the pdb stream provided the line information meets the following condition.
    /// The correct line information can be determined with the old pdb line info and
    /// one delta for all lines in the function.
    /// </summary>
    public struct SymUnmanagedLineDelta
    {
        public readonly int MethodToken;
        public readonly int Delta;

        public SymUnmanagedLineDelta(int methodToken, int delta)
        {
            MethodToken = methodToken;
            Delta = delta;
        }
    }
}
