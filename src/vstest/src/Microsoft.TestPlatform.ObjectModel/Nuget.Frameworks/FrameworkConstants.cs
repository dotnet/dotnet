// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGetClone.Frameworks
{
    internal static class FrameworkConstants
    {
        public static readonly Version EmptyVersion = new(0, 0, 0, 0);
        public static readonly Version MaxVersion = new(int.MaxValue, 0, 0, 0);
        public static readonly Version Version5 = new(5, 0, 0, 0);
        public static readonly Version Version6 = new(6, 0, 0, 0);
        public static readonly Version Version7 = new(7, 0, 0, 0);
        public static readonly Version Version10 = new(10, 0, 0, 0);
        public static readonly FrameworkRange DotNetAll = new(
                        new NuGetFramework(FrameworkIdentifiers.NetPlatform, FrameworkConstants.EmptyVersion),
                        new NuGetFramework(FrameworkIdentifiers.NetPlatform, FrameworkConstants.MaxVersion));

        internal static class SpecialIdentifiers
        {
            public const string Any = "Any";
            public const string Agnostic = "Agnostic";
            public const string Unsupported = "Unsupported";
        }

        internal static class PlatformIdentifiers
        {
            public const string WindowsPhone = "WindowsPhone";
            public const string Windows = "Windows";
        }

        internal static class FrameworkIdentifiers
        {
            public const string NetCoreApp = ".NETCoreApp";
            public const string NetStandardApp = ".NETStandardApp";
            public const string NetStandard = ".NETStandard";
            public const string NetPlatform = ".NETPlatform";
            public const string DotNet = "dotnet";
            public const string Net = ".NETFramework";
            public const string NetCore = ".NETCore";
            public const string WinRT = "WinRT"; // deprecated
            public const string NetMicro = ".NETMicroFramework";
            public const string Portable = ".NETPortable";
            public const string WindowsPhone = "WindowsPhone";
            public const string Windows = "Windows";
            public const string WindowsPhoneApp = "WindowsPhoneApp";
            public const string Dnx = "DNX";
            public const string DnxCore = "DNXCore";
            public const string AspNet = "ASP.NET";
            public const string AspNetCore = "ASP.NETCore";
            public const string Silverlight = "Silverlight";
            public const string Native = "native";
            public const string MonoAndroid = "MonoAndroid";
            public const string MonoTouch = "MonoTouch";
            public const string MonoMac = "MonoMac";
            public const string XamarinIOs = "Xamarin.iOS";
            public const string XamarinMac = "Xamarin.Mac";
            public const string XamarinPlayStation3 = "Xamarin.PlayStation3";
            public const string XamarinPlayStation4 = "Xamarin.PlayStation4";
            public const string XamarinPlayStationVita = "Xamarin.PlayStationVita";
            public const string XamarinWatchOS = "Xamarin.WatchOS";
            public const string XamarinTVOS = "Xamarin.TVOS";
            public const string XamarinXbox360 = "Xamarin.Xbox360";
            public const string XamarinXboxOne = "Xamarin.XboxOne";
            public const string UAP = "UAP";
            public const string Tizen = "Tizen";
            public const string NanoFramework = ".NETnanoFramework";
        }

        /// <summary>
        /// Interned frameworks that are commonly used in NuGet
        /// </summary>
        internal static class CommonFrameworks
        {
            public static readonly NuGetFramework Net11 = new(FrameworkIdentifiers.Net, new Version(1, 1, 0, 0));
            public static readonly NuGetFramework Net2 = new(FrameworkIdentifiers.Net, new Version(2, 0, 0, 0));
            public static readonly NuGetFramework Net35 = new(FrameworkIdentifiers.Net, new Version(3, 5, 0, 0));
            public static readonly NuGetFramework Net4 = new(FrameworkIdentifiers.Net, new Version(4, 0, 0, 0));
            public static readonly NuGetFramework Net403 = new(FrameworkIdentifiers.Net, new Version(4, 0, 3, 0));
            public static readonly NuGetFramework Net45 = new(FrameworkIdentifiers.Net, new Version(4, 5, 0, 0));
            public static readonly NuGetFramework Net451 = new(FrameworkIdentifiers.Net, new Version(4, 5, 1, 0));
            public static readonly NuGetFramework Net452 = new(FrameworkIdentifiers.Net, new Version(4, 5, 2, 0));
            public static readonly NuGetFramework Net46 = new(FrameworkIdentifiers.Net, new Version(4, 6, 0, 0));
            public static readonly NuGetFramework Net461 = new(FrameworkIdentifiers.Net, new Version(4, 6, 1, 0));
            public static readonly NuGetFramework Net462 = new(FrameworkIdentifiers.Net, new Version(4, 6, 2, 0));
            public static readonly NuGetFramework Net463 = new(FrameworkIdentifiers.Net, new Version(4, 6, 3, 0));
            public static readonly NuGetFramework Net47 = new(FrameworkIdentifiers.Net, new Version(4, 7, 0, 0));
            public static readonly NuGetFramework Net471 = new(FrameworkIdentifiers.Net, new Version(4, 7, 1, 0));
            public static readonly NuGetFramework Net472 = new(FrameworkIdentifiers.Net, new Version(4, 7, 2, 0));

            public static readonly NuGetFramework NetCore45 = new(FrameworkIdentifiers.NetCore, new Version(4, 5, 0, 0));
            public static readonly NuGetFramework NetCore451 = new(FrameworkIdentifiers.NetCore, new Version(4, 5, 1, 0));
            public static readonly NuGetFramework NetCore50 = new(FrameworkIdentifiers.NetCore, new Version(5, 0, 0, 0));

            public static readonly NuGetFramework Win8 = new(FrameworkIdentifiers.Windows, new Version(8, 0, 0, 0));
            public static readonly NuGetFramework Win81 = new(FrameworkIdentifiers.Windows, new Version(8, 1, 0, 0));
            public static readonly NuGetFramework Win10 = new(FrameworkIdentifiers.Windows, new Version(10, 0, 0, 0));

            public static readonly NuGetFramework SL4 = new(FrameworkIdentifiers.Silverlight, new Version(4, 0, 0, 0));
            public static readonly NuGetFramework SL5 = new(FrameworkIdentifiers.Silverlight, new Version(5, 0, 0, 0));

            public static readonly NuGetFramework WP7 = new(FrameworkIdentifiers.WindowsPhone, new Version(7, 0, 0, 0));
            public static readonly NuGetFramework WP75 = new(FrameworkIdentifiers.WindowsPhone, new Version(7, 5, 0, 0));
            public static readonly NuGetFramework WP8 = new(FrameworkIdentifiers.WindowsPhone, new Version(8, 0, 0, 0));
            public static readonly NuGetFramework WP81 = new(FrameworkIdentifiers.WindowsPhone, new Version(8, 1, 0, 0));
            public static readonly NuGetFramework WPA81 = new(FrameworkIdentifiers.WindowsPhoneApp, new Version(8, 1, 0, 0));

            public static readonly NuGetFramework Tizen3 = new(FrameworkIdentifiers.Tizen, new Version(3, 0, 0, 0));
            public static readonly NuGetFramework Tizen4 = new(FrameworkIdentifiers.Tizen, new Version(4, 0, 0, 0));
            public static readonly NuGetFramework Tizen6 = new(FrameworkIdentifiers.Tizen, new Version(6, 0, 0, 0));

            public static readonly NuGetFramework AspNet = new(FrameworkIdentifiers.AspNet, EmptyVersion);
            public static readonly NuGetFramework AspNetCore = new(FrameworkIdentifiers.AspNetCore, EmptyVersion);
            public static readonly NuGetFramework AspNet50 = new(FrameworkIdentifiers.AspNet, Version5);
            public static readonly NuGetFramework AspNetCore50 = new(FrameworkIdentifiers.AspNetCore, Version5);

            public static readonly NuGetFramework Dnx = new(FrameworkIdentifiers.Dnx, EmptyVersion);
            public static readonly NuGetFramework Dnx45 = new(FrameworkIdentifiers.Dnx, new Version(4, 5, 0, 0));
            public static readonly NuGetFramework Dnx451 = new(FrameworkIdentifiers.Dnx, new Version(4, 5, 1, 0));
            public static readonly NuGetFramework Dnx452 = new(FrameworkIdentifiers.Dnx, new Version(4, 5, 2, 0));
            public static readonly NuGetFramework DnxCore = new(FrameworkIdentifiers.DnxCore, EmptyVersion);
            public static readonly NuGetFramework DnxCore50 = new(FrameworkIdentifiers.DnxCore, Version5);

            public static readonly NuGetFramework DotNet
                = new(FrameworkIdentifiers.NetPlatform, EmptyVersion);
            public static readonly NuGetFramework DotNet50
                = new(FrameworkIdentifiers.NetPlatform, Version5);
            public static readonly NuGetFramework DotNet51
                = new(FrameworkIdentifiers.NetPlatform, new Version(5, 1, 0, 0));
            public static readonly NuGetFramework DotNet52
                = new(FrameworkIdentifiers.NetPlatform, new Version(5, 2, 0, 0));
            public static readonly NuGetFramework DotNet53
                = new(FrameworkIdentifiers.NetPlatform, new Version(5, 3, 0, 0));
            public static readonly NuGetFramework DotNet54
                = new(FrameworkIdentifiers.NetPlatform, new Version(5, 4, 0, 0));
            public static readonly NuGetFramework DotNet55
                = new(FrameworkIdentifiers.NetPlatform, new Version(5, 5, 0, 0));
            public static readonly NuGetFramework DotNet56
                = new(FrameworkIdentifiers.NetPlatform, new Version(5, 6, 0, 0));

            public static readonly NuGetFramework NetStandard
                = new(FrameworkIdentifiers.NetStandard, EmptyVersion);
            public static readonly NuGetFramework NetStandard10
                = new(FrameworkIdentifiers.NetStandard, new Version(1, 0, 0, 0));
            public static readonly NuGetFramework NetStandard11
                = new(FrameworkIdentifiers.NetStandard, new Version(1, 1, 0, 0));
            public static readonly NuGetFramework NetStandard12
                = new(FrameworkIdentifiers.NetStandard, new Version(1, 2, 0, 0));
            public static readonly NuGetFramework NetStandard13
                = new(FrameworkIdentifiers.NetStandard, new Version(1, 3, 0, 0));
            public static readonly NuGetFramework NetStandard14
                = new(FrameworkIdentifiers.NetStandard, new Version(1, 4, 0, 0));
            public static readonly NuGetFramework NetStandard15
                = new(FrameworkIdentifiers.NetStandard, new Version(1, 5, 0, 0));
            public static readonly NuGetFramework NetStandard16
                = new(FrameworkIdentifiers.NetStandard, new Version(1, 6, 0, 0));
            public static readonly NuGetFramework NetStandard17
                = new(FrameworkIdentifiers.NetStandard, new Version(1, 7, 0, 0));
            public static readonly NuGetFramework NetStandard20
                = new(FrameworkIdentifiers.NetStandard, new Version(2, 0, 0, 0));
            public static readonly NuGetFramework NetStandard21
                = new(FrameworkIdentifiers.NetStandard, new Version(2, 1, 0, 0));

            public static readonly NuGetFramework NetStandardApp15
                = new(FrameworkIdentifiers.NetStandardApp, new Version(1, 5, 0, 0));

            public static readonly NuGetFramework UAP10
                = new(FrameworkIdentifiers.UAP, Version10);

            public static readonly NuGetFramework NetCoreApp10
                = new(FrameworkIdentifiers.NetCoreApp, new Version(1, 0, 0, 0));
            public static readonly NuGetFramework NetCoreApp11
                = new(FrameworkIdentifiers.NetCoreApp, new Version(1, 1, 0, 0));
            public static readonly NuGetFramework NetCoreApp20
                = new(FrameworkIdentifiers.NetCoreApp, new Version(2, 0, 0, 0));
            public static readonly NuGetFramework NetCoreApp21
                = new(FrameworkIdentifiers.NetCoreApp, new Version(2, 1, 0, 0));
            public static readonly NuGetFramework NetCoreApp22
                = new(FrameworkIdentifiers.NetCoreApp, new Version(2, 2, 0, 0));
            public static readonly NuGetFramework NetCoreApp30
                = new(FrameworkIdentifiers.NetCoreApp, new Version(3, 0, 0, 0));
            public static readonly NuGetFramework NetCoreApp31
                = new(FrameworkIdentifiers.NetCoreApp, new Version(3, 1, 0, 0));

            // .NET 5.0 and later has NetCoreApp identifier
            public static readonly NuGetFramework Net50 = new(FrameworkIdentifiers.NetCoreApp, Version5);
            public static readonly NuGetFramework Net60 = new(FrameworkIdentifiers.NetCoreApp, Version6);
            public static readonly NuGetFramework Net70 = new(FrameworkIdentifiers.NetCoreApp, Version7);

            public static readonly NuGetFramework Native = new(FrameworkIdentifiers.Native, new Version(0, 0, 0, 0));
        }
    }
}
