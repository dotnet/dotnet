// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using Xunit;

namespace IntelHardwareIntrinsicTest.SSE2
{
    public partial class Program
    {
        [Fact]
        public static unsafe void Store()
        {
            int testResult = Pass;

            if (Sse2.IsSupported)
            {
                using (TestTable<double> doubleTable = new TestTable<double>(new double[2] { 1, -5 }, new double[2]))
                {
                    var vf = Unsafe.Read<Vector128<double>>(doubleTable.inArrayPtr);
                    Sse2.Store((double*)(doubleTable.outArrayPtr), vf);

                    if (!doubleTable.CheckResult((x, y) => BitConverter.DoubleToInt64Bits(x) == BitConverter.DoubleToInt64Bits(y)))
                    {
                        Console.WriteLine("Sse2 Store failed on double:");
                        foreach (var item in doubleTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable<long> intTable = new TestTable<long>(new long[2] { 1, -5 }, new long[2]))
                {
                    var vf = Unsafe.Read<Vector128<long>>(intTable.inArrayPtr);
                    Sse2.Store((long*)(intTable.outArrayPtr), vf);

                    if (!intTable.CheckResult((long x, long y) => x == y))
                    {
                        Console.WriteLine("Sse2 Store failed on long:");
                        foreach (var item in intTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable<ulong> intTable = new TestTable<ulong>(new ulong[2] { 1, 5 }, new ulong[2]))
                {
                    var vf = Unsafe.Read<Vector128<ulong>>(intTable.inArrayPtr);
                    Sse2.Store((ulong*)(intTable.outArrayPtr), vf);

                    if (!intTable.CheckResult((ulong x, ulong y) => x == y))
                    {
                        Console.WriteLine("Sse2 Store failed on ulong:");
                        foreach (var item in intTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable<int> intTable = new TestTable<int>(new int[4] { 1, -5, 100, 0 }, new int[4]))
                {
                    var vf = Unsafe.Read<Vector128<int>>(intTable.inArrayPtr);
                    Sse2.Store((int*)(intTable.outArrayPtr), vf);

                    if (!intTable.CheckResult((int x, int y) => x == y))
                    {
                        Console.WriteLine("Sse2 Store failed on int:");
                        foreach (var item in intTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable<uint> intTable = new TestTable<uint>(new uint[4] { 1, 5, 100, 0 }, new uint[4]))
                {
                    var vf = Unsafe.Read<Vector128<uint>>(intTable.inArrayPtr);
                    Sse2.Store((uint*)(intTable.outArrayPtr), vf);

                    if (!intTable.CheckResult((uint x, uint y) => x == y))
                    {
                        Console.WriteLine("Sse2 Store failed on uint:");
                        foreach (var item in intTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable<short> intTable = new TestTable<short>(new short[8] { 1, -5, 100, 0, 1, 2, 3, 4 }, new short[8]))
                {
                    var vf = Unsafe.Read<Vector128<short>>(intTable.inArrayPtr);
                    Sse2.Store((short*)(intTable.outArrayPtr), vf);

                    if (!intTable.CheckResult((short x, short y) => x == y))
                    {
                        Console.WriteLine("Sse2 Store failed on short:");
                        foreach (var item in intTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable<ushort> intTable = new TestTable<ushort>(new ushort[8] { 1, 5, 100, 0, 1, 2, 3, 4 }, new ushort[8]))
                {
                    var vf = Unsafe.Read<Vector128<ushort>>(intTable.inArrayPtr);
                    Sse2.Store((ushort*)(intTable.outArrayPtr), vf);

                    if (!intTable.CheckResult((ushort x, ushort y) => x == y))
                    {
                        Console.WriteLine("Sse2 Store failed on ushort:");
                        foreach (var item in intTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable<sbyte> intTable = new TestTable<sbyte>(new sbyte[16] { 1, -5, 100, 0, 1, 2, 3, 4, 1, -5, 100, 0, 1, 2, 3, 4 }, new sbyte[16]))
                {
                    var vf = Unsafe.Read<Vector128<sbyte>>(intTable.inArrayPtr);
                    Sse2.Store((sbyte*)(intTable.outArrayPtr), vf);

                    if (!intTable.CheckResult((sbyte x, sbyte y) => x == y))
                    {
                        Console.WriteLine("Sse2 Store failed on sbyte:");
                        foreach (var item in intTable.outArray)
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine();
                        testResult = Fail;
                    }
                }

                using (TestTable<byte> intTable = new TestTable<byte>(new byte[16] { 1, 5, 100, 0, 1, 2, 3, 4, 1, 5, 100, 0, 1, 2, 3, 4 }, new byte[16]))
                {
                    var vf = Unsafe.Read<Vector128<byte>>(intTable.inArrayPtr);
                    Sse2.Store((byte*)(intTable.outArrayPtr), vf);

                    if (!intTable.CheckResult((byte x, byte y) => x == y))
                    {
                        Console.WriteLine("Sse2 Store failed on byte:");
                        foreach (var item in intTable.outArray)
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
