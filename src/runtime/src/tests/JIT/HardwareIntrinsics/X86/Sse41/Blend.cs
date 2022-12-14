// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using Xunit;

namespace IntelHardwareIntrinsicTest._Sse41
{
    public partial class Program
    {
        [Fact]
        public static unsafe void Blend()
        {
            int testResult = Pass;

            if (Sse41.IsSupported)
            {
                using (TestTable_2Input<float> floatTable = new TestTable_2Input<float>(new float[4] { 1, -5, 100, 0 }, new float[4] { 22, -1, -50, 0 }, new float[4]))
                {
                    var vf1 = Unsafe.Read<Vector128<float>>(floatTable.inArray1Ptr);
                    var vf2 = Unsafe.Read<Vector128<float>>(floatTable.inArray2Ptr);

                    // SDDD
                    var vf3 = Sse41.Blend(vf1, vf2, 1);
                    Unsafe.Write(floatTable.outArrayPtr, vf3);

                    if (!floatTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                             (z[2] == x[2]) && (z[3] == x[3])))
                    {
                        Console.WriteLine("SSE41 Blend failed on float:");
                        foreach (var item in floatTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // DSDD
                    vf3 = Sse41.Blend(vf1, vf2, 2);
                    Unsafe.Write(floatTable.outArrayPtr, vf3);

                    if (!floatTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == y[1]) &&
                                                             (z[2] == x[2]) && (z[3] == x[3])))
                    {
                        Console.WriteLine("SSE41 Blend failed on float:");
                        foreach (var item in floatTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // DDSD
                    vf3 = Sse41.Blend(vf1, vf2, 4);
                    Unsafe.Write(floatTable.outArrayPtr, vf3);

                    if (!floatTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == x[1]) &&
                                                             (z[2] == y[2]) && (z[3] == x[3])))
                    {
                        Console.WriteLine("SSE41 Blend failed on float:");
                        foreach (var item in floatTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // SDSD
                    vf3 = Sse41.Blend(vf1, vf2, 85);
                    Unsafe.Write(floatTable.outArrayPtr, vf3);

                    if (!floatTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                             (z[2] == y[2]) && (z[3] == x[3])))
                    {
                        Console.WriteLine("SSE41 Blend failed on float:");
                        foreach (var item in floatTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                    
                    // SDDD
                    vf3 = (Vector128<float>)typeof(Sse41).GetMethod(nameof(Sse41.Blend), new Type[] { vf1.GetType(), vf2.GetType(), typeof(byte) }).Invoke(null, new object[] { vf1, vf2, (byte)(1) });
                    Unsafe.Write(floatTable.outArrayPtr, vf3);

                    if (!floatTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                             (z[2] == x[2]) && (z[3] == x[3])))
                    {
                        Console.WriteLine("SSE41 Blend failed on float:");
                        foreach (var item in floatTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable_2Input<double> doubleTable = new TestTable_2Input<double>(new double[2] { 1, -5 }, new double[2] { 22, -1 }, new double[2]))
                {
                    var vf1 = Unsafe.Read<Vector128<double>>(doubleTable.inArray1Ptr);
                    var vf2 = Unsafe.Read<Vector128<double>>(doubleTable.inArray2Ptr);

                    // DD
                    var vf3 = Sse41.Blend(vf1, vf2, 0);
                    Unsafe.Write(doubleTable.outArrayPtr, vf3);

                    if (!doubleTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == x[1])))
                    {
                        Console.WriteLine("SSE41 Blend failed on double:");
                        foreach (var item in doubleTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // SD
                    vf3 = Sse41.Blend(vf1, vf2, 1);
                    Unsafe.Write(doubleTable.outArrayPtr, vf3);

                    if (!doubleTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1])))
                    {
                        Console.WriteLine("SSE41 Blend failed on double:");
                        foreach (var item in doubleTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // DS
                    vf3 = Sse41.Blend(vf1, vf2, 2);
                    Unsafe.Write(doubleTable.outArrayPtr, vf3);

                    if (!doubleTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == y[1])))
                    {
                        Console.WriteLine("SSE41 Blend failed on double:");
                        foreach (var item in doubleTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // SS
                    vf3 = Sse41.Blend(vf1, vf2, 51);
                    Unsafe.Write(doubleTable.outArrayPtr, vf3);

                    if (!doubleTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == y[1])))
                    {
                        Console.WriteLine("SSE41 Blend failed on double:");
                        foreach (var item in doubleTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                    
                    // SDDD
                    vf3 = (Vector128<double>)typeof(Sse41).GetMethod(nameof(Sse41.Blend), new Type[] { vf1.GetType(), vf2.GetType(), typeof(byte) }).Invoke(null, new object[] { vf1, vf2, (byte)(0) });
                    Unsafe.Write(doubleTable.outArrayPtr, vf3);

                    if (!doubleTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == x[1])))
                    {
                        Console.WriteLine("SSE41 Blend failed on double:");
                        foreach (var item in doubleTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable_2Input<short> shortTable = new TestTable_2Input<short>(new short[8] { 1, -5, 100, 0, 1, -5, 100, 0 }, new short[8] { 22, -1, -50, 0, 22, -1, -50, 0 }, new short[8]))
                {
                    var vf1 = Unsafe.Read<Vector128<short>>(shortTable.inArray1Ptr);
                    var vf2 = Unsafe.Read<Vector128<short>>(shortTable.inArray2Ptr);

                    // SDDD DDDD
                    var vf3 = Sse41.Blend(vf1, vf2, 1);
                    Unsafe.Write(shortTable.outArrayPtr, vf3);

                    if (!shortTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                             (z[2] == x[2]) && (z[3] == x[3]) &&
                                                             (z[4] == x[4]) && (z[5] == x[5]) &&
                                                             (z[6] == x[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on short:");
                        foreach (var item in shortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // DSDD DDDD
                    vf3 = Sse41.Blend(vf1, vf2, 2);
                    Unsafe.Write(shortTable.outArrayPtr, vf3);

                    if (!shortTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == y[1]) &&
                                                             (z[2] == x[2]) && (z[3] == x[3]) &&
                                                             (z[4] == x[4]) && (z[5] == x[5]) &&
                                                             (z[6] == x[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on short:");
                        foreach (var item in shortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // DDSD DDDD
                    vf3 = Sse41.Blend(vf1, vf2, 4);
                    Unsafe.Write(shortTable.outArrayPtr, vf3);

                    if (!shortTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == x[1]) &&
                                                             (z[2] == y[2]) && (z[3] == x[3]) &&
                                                             (z[4] == x[4]) && (z[5] == x[5]) &&
                                                             (z[6] == x[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on short:");
                        foreach (var item in shortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // SDSD SDSD
                    vf3 = Sse41.Blend(vf1, vf2, 85);
                    Unsafe.Write(shortTable.outArrayPtr, vf3);

                    if (!shortTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                             (z[2] == y[2]) && (z[3] == x[3]) &&
                                                             (z[4] == y[4]) && (z[5] == x[5]) &&
                                                             (z[6] == y[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on short:");
                        foreach (var item in shortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                    
                    // SDDD DDDD
                    vf3 = (Vector128<short>)typeof(Sse41).GetMethod(nameof(Sse41.Blend), new Type[] { vf1.GetType(), vf2.GetType(), typeof(byte) }).Invoke(null, new object[] { vf1, vf2, (byte)(1) });
                    Unsafe.Write(shortTable.outArrayPtr, vf3);

                    if (!shortTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                             (z[2] == x[2]) && (z[3] == x[3]) &&
                                                             (z[4] == x[4]) && (z[5] == x[5]) &&
                                                             (z[6] == x[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on short:");
                        foreach (var item in shortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable_2Input<ushort> ushortTable = new TestTable_2Input<ushort>(new ushort[8] { 1, 5, 100, 0, 1, 5, 100, 0 }, new ushort[8] { 22, 1, 50, 0, 22, 1, 50, 0 }, new ushort[8]))
                {
                    var vf1 = Unsafe.Read<Vector128<ushort>>(ushortTable.inArray1Ptr);
                    var vf2 = Unsafe.Read<Vector128<ushort>>(ushortTable.inArray2Ptr);

                    // SDDD DDDD
                    var vf3 = Sse41.Blend(vf1, vf2, 1);
                    Unsafe.Write(ushortTable.outArrayPtr, vf3);

                    if (!ushortTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                              (z[2] == x[2]) && (z[3] == x[3]) &&
                                                              (z[4] == x[4]) && (z[5] == x[5]) &&
                                                              (z[6] == x[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on ushort:");
                        foreach (var item in ushortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // DSDD DDDD
                    vf3 = Sse41.Blend(vf1, vf2, 2);
                    Unsafe.Write(ushortTable.outArrayPtr, vf3);

                    if (!ushortTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == y[1]) &&
                                                              (z[2] == x[2]) && (z[3] == x[3]) &&
                                                              (z[4] == x[4]) && (z[5] == x[5]) &&
                                                              (z[6] == x[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on ushort:");
                        foreach (var item in ushortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // DDSD DDDD
                    vf3 = Sse41.Blend(vf1, vf2, 4);
                    Unsafe.Write(ushortTable.outArrayPtr, vf3);

                    if (!ushortTable.CheckResult((x, y, z) => (z[0] == x[0]) && (z[1] == x[1]) &&
                                                              (z[2] == y[2]) && (z[3] == x[3]) &&
                                                              (z[4] == x[4]) && (z[5] == x[5]) &&
                                                              (z[6] == x[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on ushort:");
                        foreach (var item in ushortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }

                    // SDSD SDSD
                    vf3 = Sse41.Blend(vf1, vf2, 85);
                    Unsafe.Write(ushortTable.outArrayPtr, vf3);

                    if (!ushortTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                              (z[2] == y[2]) && (z[3] == x[3]) &&
                                                              (z[4] == y[4]) && (z[5] == x[5]) &&
                                                              (z[6] == y[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on ushort:");
                        foreach (var item in ushortTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                    
                    // SDDD DDDD
                    vf3 = (Vector128<ushort>)typeof(Sse41).GetMethod(nameof(Sse41.Blend), new Type[] { vf1.GetType(), vf2.GetType(), typeof(byte) }).Invoke(null, new object[] { vf1, vf2, (byte)(1) });
                    Unsafe.Write(ushortTable.outArrayPtr, vf3);

                    if (!ushortTable.CheckResult((x, y, z) => (z[0] == y[0]) && (z[1] == x[1]) &&
                                                              (z[2] == x[2]) && (z[3] == x[3]) &&
                                                              (z[4] == x[4]) && (z[5] == x[5]) &&
                                                              (z[6] == x[6]) && (z[7] == x[7])))
                    {
                        Console.WriteLine("SSE41 Blend failed on ushort:");
                        foreach (var item in ushortTable.outArray)
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
