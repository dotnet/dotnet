// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace vstest.ProgrammerTests.Fakes;

internal static class KnownFrameworkStrings
{
    public static string NetCore(int major, int minor = 0) => $".NETCoreApp,Version=v{major}.{minor}";
    private static string NetFramework(int major, int minor, int patch = 0) => $".NETFramework,Version=v{major}.{minor}{(patch != 0 ? $".{patch}" : null)}";

    public static string Netcoreapp1 = NetCore(1);
    public static string Netcoreapp2 = NetCore(2);
    public static string Netcoreapp21 = NetCore(2, 1);
    public static string Netcoreapp3 = NetCore(3);
    public static string Netcoreapp31 = NetCore(3, 1);
    public static string Net5 = NetCore(5);
    public static string Net6 = NetCore(6);
    public static string Net7 = NetCore(7);

    public static string Net462 = NetFramework(4, 6, 2);
    public static string Net47 = NetFramework(4, 7);
    public static string Net471 = NetFramework(4, 7, 1);
    public static string Net472 = NetFramework(4, 7, 2);
    public static string Net48 = NetFramework(4, 8);
}
