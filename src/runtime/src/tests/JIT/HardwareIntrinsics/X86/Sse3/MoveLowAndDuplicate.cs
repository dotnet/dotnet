// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using Xunit;

namespace IntelHardwareIntrinsicTest._Sse3
{
    public static partial class Program
    {
        [Fact]
        public static unsafe void MoveLowAndDuplicate()
        {
            int testResult = Pass;

            if (Sse3.IsSupported)
            {
                using (TestTable<float> floatTable = new TestTable<float>(new float[4] { 1, -5, 100, 0 }, new float[4]))
                {
                    var vf1 = Sse.LoadVector128((float*)(floatTable.inArrayPtr));
                    var vf2 = Sse3.MoveLowAndDuplicate(vf1);
                    Unsafe.Write(floatTable.outArrayPtr, vf2);

                    if (BitConverter.SingleToInt32Bits(floatTable.inArray[0]) != BitConverter.SingleToInt32Bits(floatTable.outArray[0]) || 
                        BitConverter.SingleToInt32Bits(floatTable.inArray[0]) != BitConverter.SingleToInt32Bits(floatTable.outArray[1]) ||
                        BitConverter.SingleToInt32Bits(floatTable.inArray[2]) != BitConverter.SingleToInt32Bits(floatTable.outArray[2]) ||
                        BitConverter.SingleToInt32Bits(floatTable.inArray[2]) != BitConverter.SingleToInt32Bits(floatTable.outArray[3]))
                    {
                        Console.WriteLine("Sse3 MoveLowAndDuplicate failed on float:");
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
