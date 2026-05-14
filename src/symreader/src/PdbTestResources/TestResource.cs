// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.
namespace Microsoft.DiaSymReader.Tools.UnitTests
{
    public struct TestResource
    {
        public readonly byte[] PE;
        public readonly byte[] Pdb;

        public TestResource(byte[] pe, byte[] pdb)
        {
            PE = pe;
            Pdb = pdb;
        }

        public override string ToString()
        {
            return "TR";
        }
    }
}
