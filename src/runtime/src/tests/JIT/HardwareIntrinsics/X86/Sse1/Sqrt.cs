// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using Xunit;

namespace IntelHardwareIntrinsicTest._Sse1
{
    public partial class Program
    {
        [Fact]
        public static unsafe void Sqrt()
        {
            int testResult = Pass;

            if (Sse.IsSupported)
            {
                using (TestTable<float> floatTable = new TestTable<float>(new float[4] { 1, -5, 100, 0 }, new float[4]))
                {

                    var vf1 = Unsafe.Read<Vector128<float>>(floatTable.inArrayPtr);
                    var vf2 = Sse.Sqrt(vf1);
                    Unsafe.Write(floatTable.outArrayPtr, vf2);

                    if (!floatTable.CheckResult((x, y) => {
                        var expected = MathF.Sqrt(x);
                        return (expected == y)
                            || (float.IsNaN(expected) && float.IsNaN(y));
                    }))
                    {
                        Console.WriteLine("SSE Sqrt failed on float:");
                        foreach (var item in floatTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }
            }


            Assert.Equal(Pass, testResult);
        }
    }
}
