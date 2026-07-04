// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MtpPureProject;

/// <summary>
/// Trivial code under test. The passing tests exercise <see cref="Add"/> and <see cref="Multiply"/>;
/// <see cref="Divide"/> is intentionally left uncovered so the coverage report shows a real
/// covered/uncovered split for this assembly.
/// </summary>
public static class Calculator
{
    public static int Add(int a, int b)
    {
        return a + b;
    }

    public static int Multiply(int a, int b)
    {
        int result = 0;
        for (int i = 0; i < b; i++)
        {
            result += a;
        }

        return result;
    }

    public static int Divide(int a, int b)
    {
        if (b == 0)
        {
            throw new System.DivideByZeroException();
        }

        return a / b;
    }
}
