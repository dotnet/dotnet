// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Xunit;

namespace IntelHardwareIntrinsicTest.SSE2
{
    public partial class Program
    {
        [Fact]
        public static unsafe void SqrtScalar()
        {
            int testsCount = 21;
            string methodUnderTestName = nameof(Sse2.SqrtScalar);

            if (Sse2.IsSupported)
            {
                using (var doubleTable = TestTableScalarSse2<double, double>.Create(testsCount))
                {
                    for (int i = 0; i < testsCount; i++)
                    {
                        (Vector128<double>, Vector128<double>) value = doubleTable[i];
                        Vector128<double> result = Sse2.SqrtScalar(value.Item1);
                        doubleTable.SetOutArray(result);
                    }

                    CheckMethodEight<double, double> checkDouble = (Span<double> x, Span<double> y, Span<double> z, Span<double> a) =>
                    {
                        a[0] = Math.Sqrt(x[0]);
                        a[1] = x[1];
                        return a[0] == z[0] && a[1] == z[1];
                    };

                    if (!doubleTable.CheckResult(checkDouble))
                    {
                        PrintError(doubleTable, methodUnderTestName, "(Span<double> x, Span<double> y, Span<double> z, Span<double> a) => SqrtScalar", checkDouble);
                        Assert.Fail("");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Sse2.IsSupported: {Sse2.IsSupported}, skipped tests of {typeof(Sse2)}.{methodUnderTestName}");
            }
        }
    }
}
