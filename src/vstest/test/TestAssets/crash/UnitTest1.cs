// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Runtime.CompilerServices;

#pragma warning disable IDE1006 // Naming Styles
namespace timeout
#pragma warning restore IDE1006 // Naming Styles
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Recurse until the stack is exhausted. One huge stackalloc is quicker, but on threads other than the main one it can
            // end the process with an access violation before the runtime reports "Stack overflow." or writes a crash dump.
            Recurse(0);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int Recurse(int depth) => Recurse(depth + 1) + 1;
    }
}
