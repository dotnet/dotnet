// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace TestResources
{
    public static class ReleasePackages
    {
        public const string Version = "1.0.0";

        private static byte[] s_A;
        public static byte[] TestPackageA => ResourceLoader.GetOrCreateResource(ref s_A, NameA);
        public const string NameA = nameof(TestPackageA) + "." + Version + ".nupkg";

        private static byte[] s_B;
        public static byte[] TestPackageB => ResourceLoader.GetOrCreateResource(ref s_B, NameB);
        public const string NameB = nameof(TestPackageB) + "." + Version + ".nupkg";

        private static byte[] s_C;
        public static byte[] TestPackageC => ResourceLoader.GetOrCreateResource(ref s_C, NameC);
        public const string NameC = nameof(TestPackageC) + "." + Version + ".nupkg";

        private static byte[] s_D;
        public static byte[] TestPackageD => ResourceLoader.GetOrCreateResource(ref s_D, NameD);
        public const string NameD = nameof(TestPackageD) + "." + Version + ".nupkg";

        private static byte[] s_E;
        public static byte[] TestPackageE => ResourceLoader.GetOrCreateResource(ref s_E, NameE);
        public const string NameE = nameof(TestPackageE) + "." + Version + ".nupkg";

        private static byte[] s_F;
        public static byte[] TestPackageF => ResourceLoader.GetOrCreateResource(ref s_F, NameF);
        public const string NameF = nameof(TestPackageF) + "." + Version + ".nupkg";
    }

    public static class PreReleasePackages
    {
        public const string SemVer1 = "1.0.0-beta-final";
        public const string SemVer2 = "1.0.0-beta.final";

        private static byte[] s_A;
        public static byte[] TestPackageA => ResourceLoader.GetOrCreateResource(ref s_A, NameA);
        public const string NameA = nameof(TestPackageA) + "." + SemVer1 + ".nupkg";

        private static byte[] s_B;
        public static byte[] TestPackageB => ResourceLoader.GetOrCreateResource(ref s_B, NameB);
        public const string NameB = nameof(TestPackageB) + "." + SemVer1 + ".nupkg";

        private static byte[] s_C;
        public static byte[] TestPackageC => ResourceLoader.GetOrCreateResource(ref s_C, NameC);
        public const string NameC = nameof(TestPackageC) + "." + SemVer1 + ".nupkg";

        private static byte[] s_D;
        public static byte[] TestPackageD => ResourceLoader.GetOrCreateResource(ref s_D, NameD);
        public const string NameD = nameof(TestPackageD) + "." + SemVer1 + ".nupkg";

        private static byte[] s_E;
        public static byte[] TestPackageE => ResourceLoader.GetOrCreateResource(ref s_E, NameE);
        public const string NameE = nameof(TestPackageE) + "." + SemVer2 + ".nupkg";

        private static byte[] s_F;
        public static byte[] TestPackageF => ResourceLoader.GetOrCreateResource(ref s_F, NameF);
        public const string NameF = nameof(TestPackageF) + "." + SemVer2 + ".nupkg";
    }

    public static class DailyBuildPackages
    {
        public const string SemVer1 = "1.0.0-beta-12345-01";
        public const string SemVer2 = "1.0.0-beta.12345.1";

        private static byte[] s_A;
        public static byte[] TestPackageA => ResourceLoader.GetOrCreateResource(ref s_A, NameA);
        public const string NameA = nameof(TestPackageA) + "." + SemVer1 + ".nupkg";

        private static byte[] s_B;
        public static byte[] TestPackageB => ResourceLoader.GetOrCreateResource(ref s_B, NameB);
        public const string NameB = nameof(TestPackageB) + "." + SemVer1 + ".nupkg";

        private static byte[] s_C;
        public static byte[] TestPackageC => ResourceLoader.GetOrCreateResource(ref s_C, NameC);
        public const string NameC = nameof(TestPackageC) + "." + SemVer1 + ".nupkg";

        private static byte[] s_D;
        public static byte[] TestPackageD => ResourceLoader.GetOrCreateResource(ref s_D, NameD);
        public const string NameD = nameof(TestPackageD) + "." + SemVer1 + ".nupkg";

        private static byte[] s_E;
        public static byte[] TestPackageE => ResourceLoader.GetOrCreateResource(ref s_E, NameE);
        public const string NameE = nameof(TestPackageE) + "." + SemVer2 + ".nupkg";

        private static byte[] s_F;
        public static byte[] TestPackageF => ResourceLoader.GetOrCreateResource(ref s_F, NameF);
        public const string NameF = nameof(TestPackageF) + "." + SemVer2 + ".nupkg";
    }

    public static class MiscPackages
    {
        private static byte[] s_Signed;
        public static byte[] Signed => ResourceLoader.GetOrCreateResource(ref s_Signed, NameSigned);
        public const string NameSigned = "Signed.1.2.3.nupkg";

        private static byte[] s_DotneTool;
        public static byte[] DotnetTool => ResourceLoader.GetOrCreateResource(ref s_DotneTool, NameDotnetTool);
        public const string NameDotnetTool = "DotnetTool.1.0.0-beta-12345-01.nupkg";
    }
}
