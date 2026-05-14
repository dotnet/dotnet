// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Versioning;

namespace vstest.ProgrammerTests.Fakes;

internal static class KnownFrameworkNames
{
    public static FrameworkName Netcoreapp1 = new(KnownFrameworkStrings.Netcoreapp1);
    public static FrameworkName Netcoreapp2 = new(KnownFrameworkStrings.Netcoreapp2);
    public static FrameworkName Netcoreapp21 = new(KnownFrameworkStrings.Netcoreapp21);
    public static FrameworkName Netcoreapp3 = new(KnownFrameworkStrings.Netcoreapp3);
    public static FrameworkName Netcoreapp31 = new(KnownFrameworkStrings.Netcoreapp31);
    public static FrameworkName Net5 = new(KnownFrameworkStrings.Net5);
    public static FrameworkName Net6 = new(KnownFrameworkStrings.Net6);
    public static FrameworkName Net7 = new(KnownFrameworkStrings.Net7);
    public static FrameworkName Net462 = new(KnownFrameworkStrings.Net462);
    public static FrameworkName Net47 = new(KnownFrameworkStrings.Net47);
    public static FrameworkName Net471 = new(KnownFrameworkStrings.Net471);
    public static FrameworkName Net472 = new(KnownFrameworkStrings.Net472);
    public static FrameworkName Net48 = new(KnownFrameworkStrings.Net48);
}
