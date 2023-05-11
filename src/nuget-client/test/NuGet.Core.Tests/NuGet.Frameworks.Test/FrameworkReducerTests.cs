// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.Test
{
    public class FrameworkReducerTests
    {
        [Theory]
        // Generation fall-through patterns of uap10.0: 
        // 1. lib/uap10, 
        // 2. then lib/win81, 
        // 3. then lib/wpa81, 
        // 4. then lib/netstandard1.2, 
        // 5. then lib/dotnet5.3, 
        // 6. then portable-win81+*, etc
        [InlineData("uap10.0", "netcore50,win81,wpa81,dotnet5.4,portable-win81+net45", "netcore50")]
        [InlineData("uap10.0", "uap10.0,win81,wpa81,dotnet5.4,portable-win81+net45", "uap10.0")]
        [InlineData("uap10.0", "win81,wpa81,dotnet5.4,portable-win81+net45", "win81")]
        [InlineData("uap10.0", "wpa81,dotnet5.4,portable-win81+net45", "wpa81")]
        [InlineData("uap10.0", "dotnet5.4,portable-win81+net45", "dotnet5.4")]
        [InlineData("uap10.0", "portable-win81+net45", "portable-win81+net45")]
        [InlineData("uap10.0", "dotnet5.6,dotnet5.5,dotnet5.4,portable-win81+net45", "dotnet5.5")]
        [InlineData("uap10.0", "netstandard1.5,dotnet5.6,dotnet5.5,dotnet5.4,portable-win81+net45", "dotnet5.5")]
        [InlineData("uap10.0", "netstandard1.4,dotnet5.6,dotnet5.5,dotnet5.4,portable-win81+net45", "netstandard1.4")]
        [InlineData("uap10.0", "netstandard1.3,dotnet5.6,dotnet5.5,dotnet5.4,portable-win81+net45", "netstandard1.3")]
        [InlineData("uap10.0", "dotnet5.4,dotnet,portable-win81+net45", "dotnet5.4")]
        [InlineData("uap10.0", "dotnet,portable-win81+net45", "dotnet")]
        [InlineData("uap10.0", "wpa81,dotnet5.3,portable-win81+net45", "wpa81")]
        [InlineData("uap10.0", "dotnet5.2,portable-win81+net45", "dotnet5.2")]
        [InlineData("uap10.0", "wpa81,dotnet5.2,portable-win81+net45", "wpa81")]
        [InlineData("uap10.0.15064.0", "netcore50,win81,wpa81,dotnet5.4,portable-win81+net45,netstandard2.0", "netcore50")]
        [InlineData("uap10.0.15064.0", "netstandard2.0, netstandard2.1, netstandard1.9, net45, net461", "netstandard2.0")]
        [InlineData("uap10.0.15064.0", "netstandard2.0, netcoreapp1.0, netcore2.0", "netstandard2.0")]
        // Take the most specific PCL profile
        [InlineData("uap10.0", "dotnet6.0,portable-win81+net45", "portable-win81+net45")]
        [InlineData("uap10.0", "dotnet6.0,portable-win81+net45+sl5,portable-win81+net45", "portable-win81+net45")]
        [InlineData("uap10.0", "dotnet6.0,portable-win81+net45+sl5,portable-uap11.0", "portable-win81+net45+sl5")]
        [InlineData("uap10.0", "netstandard7.0,dotnet6.0,portable-win81+net45", "portable-win81+net45")]
        [InlineData("uap10.0", "netstandard7.0,dotnet6.0,portable-win81+net45+sl5,portable-win81+net45", "portable-win81+net45")]
        [InlineData("uap10.0", "netstandard7.0,dotnet6.0,portable-win81+net45+sl5,portable-uap11.0", "portable-win81+net45+sl5")]
        // Same TFM wins
        [InlineData("net461", "net462,net461,net46,net45,net4,net2,dotnet6.0,dotnet5.5,dotnet5.4,dotnet5.3,dotnet,portable-net45+win8,portable-net45+win8+wpa81", "net461")]
        [InlineData("net461", "net46,net45,net4,net2,dotnet6.0,dotnet5.5,dotnet5.4,dotnet5.3,dotnet,portable-net45+win8,portable-net45+win8+wpa81", "net46")]
        [InlineData("net461", "net45,net4,net2,dotnet6.0,dotnet5.5,dotnet5.4,dotnet5.3,dotnet,portable-net45+win8,portable-net45+win8+wpa81", "net45")]
        [InlineData("net461", "net4,net2,dotnet6.0,dotnet5.5,dotnet5.4,dotnet5.3,dotnet,portable-net45+win8,portable-net45+win8+wpa81", "net4")]
        [InlineData("net461", "net2,dotnet6.0,dotnet5.5,dotnet5.4,dotnet5.3,dotnet,portable-net45+win8,portable-net45+win8+wpa81", "net2")]
        // Use a compatible TFM if there are no framework matches
        [InlineData("net461", "dotnet6.0,dotnet5.5,dotnet5.4,dotnet5.3,dotnet,portable-net45+win8,portable-net45+win8+wpa81", "dotnet5.5")]
        [InlineData("net461", "dotnet5.4,dotnet,portable-net45+win8,portable-net45+win8+wpa81", "dotnet5.4")]
        [InlineData("net461", "dotnet5.4.9,dotnet,portable-net45+win8,portable-net45+win8+wpa81", "dotnet5.4.9")]
        [InlineData("net461", "dotnet,portable-net45+win8,portable-net45+win8+wpa81", "dotnet")]
        [InlineData("net461", "portable-net45+win8,portable-net45+win8+wpa81", "portable-net45+win8")]
        [InlineData("net461", "portable-net45+win8+wpa81,native", "portable-net45+win8+wpa81")]
        [InlineData("net49", "dotnet6.0,dotnet5.6,dotnet5.5,dotnet5.4,portable-net45+win8", "dotnet5.6")]
        // netstandard
        [InlineData("netstandard1.5", "net4,netstandard7.0,netstandard1.4", "netstandard1.4")]
        [InlineData("netstandard1.5", "net4,netstandard7.0,netstandard1.5", "netstandard1.5")]
        [InlineData("netstandard1.5", "net4,netstandard7.0,netstandard1.6", null)]
        [InlineData("netstandard1.5", "net4,netstandard7.0,netstandard0.0", "netstandard")]
        [InlineData("netstandard1.5", "dotnet5.6,netstandard1.4", "netstandard1.4")]
        [InlineData("netstandard1.5", "dotnet1.3,netstandard1.4", "netstandard1.4")]
        [InlineData("netstandard1.5", "dotnet5.6,netstandard0.1", "netstandard0.1")]
        [InlineData("netstandard1.5", "dotnet5.6,netstandard0.0", "netstandard")]
        [InlineData("netstandard1.5", "dotnet5.6", null)]
        [InlineData("netstandard7.0", "netstandard6.0,netstandard1.0", "netstandard6.0")]
        // netstandardapp
        [InlineData("netstandardapp1.5", "net4,netstandardapp1.5,netstandardapp1.4", "netstandardapp1.5")]
        [InlineData("netstandardapp1.5", "net4,netstandard1.4,netstandardapp1.3,dotnet5.6", "netstandardapp1.3")]
        [InlineData("netstandardapp1.5", "dotnet5.6,netstandard1.4", "netstandard1.4")]
        [InlineData("netstandardapp1.5", "dotnet5.6,netstandard1.3,netstandard1.2", "netstandard1.3")]
        [InlineData("netstandardapp1.5", "net4,dotnet5.3", null)]
        [InlineData("netstandardapp1.5", "net4", null)]
        [InlineData("netstandardapp1.0", "net4,dotnet5.3", null)]
        [InlineData("netstandardapp1.2", "netstandardapp1.2,netstandardapp1.1", "netstandardapp1.2")]
        [InlineData("netstandardapp1.0", "netstandardapp1.2,netstandardapp1.0", "netstandardapp1.0")]
        [InlineData("netstandardapp1.5", "netstandardapp1.2,netstandardapp1.1", "netstandardapp1.2")]
        [InlineData("netstandardapp1.0", "dotnet5.2,dotnet", null)]
        // PCL
        [InlineData("portable-net45", "portable-net45+netcore45,portable-net45+netcore45+wpa81", "portable-net45+netcore45")]
        [InlineData("portable-net45+netcore45", "portable-net45+netcore45,netstandard1.2", "portable-net45+netcore45")]
        [InlineData("portable-net45+netcore45", "portable-net45+netcore45,netstandard1.1.1", "portable-net45+netcore45")]
        [InlineData("portable-net45+netcore45", "portable-net45+netcore45,dotnet5.2", "portable-net45+netcore45")]
        [InlineData("portable-net45+netcore45", "portable-net45+netcore45,dotnet5.1", "portable-net45+netcore45")]
        [InlineData("portable-net45+netcore45", "portable-net45+netcore45+wpa81,netstandard1.1,dotnet5.1", "portable-net45+netcore45+wpa81")]
        [InlineData("portable-net45+netcore45", "portable-net45+netcore45+wpa81,netstandard1.1", "portable-net45+netcore45+wpa81")]
        [InlineData("portable-net45+netcore45", "portable-net45+netcore45+wpa81,netstandard1.0", "portable-net45+netcore45+wpa81")]
        [InlineData("portable-net45+netcore45", "netstandard1.1,dotnet5.1", "netstandard1.1")]
        [InlineData("portable-net45+netcore45+bad", "netstandard1.0", null)]
        // net5.0, generational compatibility is preferred over platform compatibility
        [InlineData("net5.0", "net5.0,netcoreapp3.0", "net5.0")]
        [InlineData("net6.0", "net5.0,netcoreapp3.0", "net5.0")]
        [InlineData("net5.0-ios", "net5.0,netcoreapp3.0", "net5.0")]
        [InlineData("net5.0-ios", "net5.0,net5.0-ios", "net5.0-ios")]
        [InlineData("net6.0-ios", "net5.0,net5.0-ios", "net5.0-ios")]
        [InlineData("net6.0", "net5.0,net5.0-ios", "net5.0")]
        [InlineData("net6.0-ios", "net6.0,net5.0-ios,netstandard2.1", "net6.0")]
        [InlineData("net6.0-ios", "net5.0-ios,net6.0,netstandard2.1", "net6.0")]
        [InlineData("net6.0-ios", "net6.0-android,net7.0-ios,net6.1", null)]
        [InlineData("net6.0-ios10.0", "net6.0-ios11.0,net6.0,net6.1", "net6.0")]
        [InlineData("net6.0-ios10.0", "net6.0-ios11.0,net6.0,net6.0-android10.0", "net6.0")]
        [InlineData("net6.0-ios10.0", "net6.0-ios11.0,net6.0,net6.0-ios9.0", "net6.0-ios9.0")]
        [InlineData("net6.0-ios11.0", "net6.0-ios10.0,net6.0,net6.0-ios9.0", "net6.0-ios10.0")]
        [InlineData("net6.0-ios11.0", "net6.0-ios12.0,net6.0,net6.0-ios13.0", "net6.0")]
        [InlineData("net7.0-ios", "net6.0,net5.0-ios,netstandard2.1", "net6.0")]
        [InlineData("net7.0-ios", "net6.0,net6.0-ios,netstandard2.1", "net6.0-ios")]
        // Some net6.0 platforms have "special" fallbacks to xamarin over net5.0
        [InlineData("net6.0-ios", "xamarin.mac,net5.0", "net5.0")]
        [InlineData("net6.0-android", "xamarin.mac,net7.0,net5.0,monoandroid12.0", "monoandroid12.0")]
        [InlineData("net6.0-tizen", "xamarin.mac,net7.0,net5.0,tizen9.0,netcoreapp3.1", "tizen9.0")]
        [InlineData("net6.0-ios", "xamarin.mac", null)]
        [InlineData("net6.0-maccatalyst", "xamarin.mac", null)]
        [InlineData("net6.0-mac", "xamarin.mac", null)] // the correct platform is "macos"
        [InlineData("net6.0-whatever", "xamarin.whatever", null)]
        // Special net6.0 platform fallbacks do not apply to xamarin.* frameworks
        [InlineData("net6.0-ios", "xamarin.ios,xamarin.mac,net5.0", "net5.0")]
        [InlineData("net6.0-maccatalyst", "xamarin.ios,xamarin.mac,net5.0", "net5.0")]
        [InlineData("net6.0-macos", "xamarin.mac,xamarin.ios,net5.0", "net5.0")]
        [InlineData("net6.0-tvos", "xamarin.tvos,xamarin.ios,net5.0", "net5.0")]
        [InlineData("net6.0-ios", "xamarin.mac,xamarin.ios,net5.0", "net5.0")]
        [InlineData("net7.0-ios", "xamarin.mac,net5.0,net6.0", "net6.0")]
        [InlineData("net7.0-ios", "net6.0,xamarin.ios,xamarin.mac,net5.0", "net6.0")]
        [InlineData("net7.0-ios", "net7.0-macos,xamarin.mac,xamarin.ios,net6.0,net6.0-ios,net6.0-macos,net5.0", "net6.0-ios")]
        [InlineData("net7.0-macos", "xamarin.ios,net5.0,net6.0", "net6.0")]
        [InlineData("net7.0-macos", "net6.0,xamarin.ios,xamarin.tvos,net5.0", "net6.0")]
        [InlineData("net7.0-macos", "net7.0-ios,xamarin.ios,net6.0,net6.0-ios,net6.0-macos,net5.0", "net6.0-macos")]
        [InlineData("net7.0-tvos", "xamarin.ios,net5.0,net6.0", "net6.0")]
        [InlineData("net7.0-tvos", "net6.0,xamarin.ios,xamarin.mac,net5.0", "net6.0")]
        [InlineData("net7.0-tvos", "net7.0-ios,xamarin.ios,net6.0,net6.0-ios,net6.0-tvos,net5.0", "net6.0-tvos")]
        [InlineData("net7.0-maccatalyst", "xamarin.mac,net5.0,net6.0", "net6.0")]
        [InlineData("net7.0-maccatalyst", "net6.0,xamarin.tvos,xamarin.mac,net5.0", "net6.0")]
        [InlineData("net7.0-maccatalyst", "net7.0-ios,xamarin.mac,net6.0,net6.0-maccatalyst,net6.0-ios,net5.0", "net6.0-maccatalyst")]
        [InlineData("net7.0-tizen", "xamarin.mac,net6.0,net6.0-android,tizen9.0,net5.0,netcoreapp3.1", "net6.0")]
        [InlineData("net7.0-tizen", "xamarin.mac,net7.0,net6.0-android,tizen9.0,net5.0,netcoreapp3.1", "net7.0")]
        [InlineData("net7.0-tizen", "xamarin.mac,net7.0-android,net6.0-android,", null)]
        [InlineData("net7.0-android", "xamarin.mac,net6.0,net6.0-tizen,monoandroid,net5.0,netcoreapp3.1", "net6.0")]
        [InlineData("net7.0-android", "xamarin.mac,net7.0,net6.0-android,monoandroid,net5.0,netcoreapp3.1", "net7.0")]
        [InlineData("net7.0-android", "xamarin.mac,net7.0-tizen,net6.0-macos,", null)]
        // Additional tests
        [InlineData("dotnet5.5", "dotnet6.0,dotnet5.4,portable-net45+win8", "dotnet5.4")]
        [InlineData("dotnet7", "dotnet6.0,dotnet5.4,portable-net45+win8", "dotnet6.0")]
        [InlineData("dnxcore50", "dotnet6.0,dotnet5.5,portable-net45+win8", "dotnet5.5")]
        [InlineData("dotnet", "dotnet5.1,native,uap10.1", null)]
        [InlineData("uap10.0", "uap10.1,dotnet5.7,dotnet5.6.0.1,dotnet6.0,dnxcore50,native", null)]
        [InlineData("dotnet", "", null)]
        [InlineData("dotnet5.4", "", null)]
        [InlineData("netstandard", "", null)]
        [InlineData("netstandard1.0", "", null)]
        [InlineData("netstandard1.5", "", null)]
        [InlineData("uap10.0", "", null)]
        [InlineData("net461", "", null)]
        [InlineData("unsupported", "any", "any")]
        [InlineData("any", "any", "any")]
        [InlineData("any", "unsupported", "unsupported")]
        [InlineData("netnano1.0", "net48,netstandard1.0,netcoreapp1.0", null)]
        public void FrameworkReducer_GetNearestWithGenerations(
            string projectFramework,
            string packageFrameworks,
            string expected)
        {
            // Arrange
            var project = NuGetFramework.Parse(projectFramework);

            var frameworks = packageFrameworks.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => NuGetFramework.Parse(s)).ToList();

            var expectedFramework = expected == null ? null : NuGetFramework.Parse(expected);

            FrameworkReducer reducer = new FrameworkReducer();

            // Act
            var nearest = reducer.GetNearest(project, frameworks);

            // Assert
            Assert.Equal(expectedFramework, nearest);
        }

        [Theory]
        [InlineData("uap10.0", "portable-aspnetcore50+net45+win8+wp8+wpa81")]
        [InlineData("netcore50", "portable-aspnetcore50+net45+win8+wp8+wpa81")]
        [InlineData("dnx452", "net45")]
        [InlineData("dnxcore50", "portable-aspnetcore50+net45+win8+wp8+wpa81")]
        [InlineData("net20", "net20")]
        [InlineData("net451", "net45")]
        [InlineData("sl5", "portable-net40+sl5+win8+wp8+wpa81")]
        [InlineData("wp81", "portable-aspnetcore50+net45+win8+wp8+wpa81")]
        [InlineData("win81", "portable-aspnetcore50+net45+win8+wp8+wpa81")]
        public void FrameworkReducer_JsonNetGetNearestLibGroup(string projectFramework, string expectedFramework)
        {
            // Arrange

            // Get nearest lib group for newtonsoft.json 7.0.1-beta2
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse(projectFramework);

            List<NuGetFramework> frameworks = new List<NuGetFramework>()
            {
                NuGetFramework.Parse("net20"),
                NuGetFramework.Parse("net35"),
                NuGetFramework.Parse("net40"),
                NuGetFramework.Parse("net45"),
                NuGetFramework.Parse("portable-net40+wp80+win8+wpa81+sl5"),
                NuGetFramework.Parse("portable-net45+wp80+win8+wpa81+aspnetcore50")
            };

            // Act
            var result = reducer.GetNearest(project, frameworks);

            // Assert
            Assert.Equal(expectedFramework, result!.GetShortFolderName());
        }

        [Fact]
        public void FrameworkReducer_GetNearestUAPTie()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse("UAP10.0");

            var win81 = NuGetFramework.Parse("win81");
            var wpa81 = NuGetFramework.Parse("wpa81");
            var native = NuGetFramework.Parse("native");
            var netcore = NuGetFramework.Parse("netcore");
            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { native, netcore, win81, net45, wpa81 };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(win81, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestUAPTie2()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse("UAP10.0");

            var win = NuGetFramework.Parse("win");
            var wpa81 = NuGetFramework.Parse("wpa81");
            var native = NuGetFramework.Parse("native");
            var netcore = NuGetFramework.Parse("netcore");
            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { native, netcore, win, net45, wpa81 };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(netcore, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestUAP()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse("UAP10.0");

            var win81 = NuGetFramework.Parse("win81");
            var win = NuGetFramework.Parse("win");
            var native = NuGetFramework.Parse("native");
            var netcore = NuGetFramework.Parse("netcore");
            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { native, netcore, win81, net45, win };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(win81, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestUAPCore()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse("UAP10.0");

            var native = NuGetFramework.Parse("native");
            var dotnet = NuGetFramework.Parse("dotnet");
            var dnx451 = NuGetFramework.Parse("dnx451");
            var dnxcore50 = NuGetFramework.Parse("dnxcore50");
            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { native, dotnet, dnx451, dnxcore50, net45 };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(dotnet, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestUAPCore50()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse("UAP10.0");

            var dotnet = NuGetFramework.Parse("dotnet");
            var dnx451 = NuGetFramework.Parse("dnx451");
            var dnxcore50 = NuGetFramework.Parse("dnxcore50");
            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { dotnet, dnx451, dnxcore50, net45 };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(dotnet, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestNetCore()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var win81 = NuGetFramework.Parse("win81");
            var native = NuGetFramework.Parse("native");
            var netcore = NuGetFramework.Parse("netcore");

            var project = NuGetFramework.Parse("net451");

            var all = new NuGetFramework[] { native, netcore };

            var result = reducer.GetNearest(win81, all);

            Assert.Equal(netcore, result);
        }

        [Theory]
        [InlineData("dnx452", "aspnet50")]
        [InlineData("dnx451", "aspnet50")]
        [InlineData("dnx45", "aspnet50")]
        [InlineData("net45", "net40")]
        [InlineData("dnxcore5", "aspnetcore50")]
        [InlineData("win8", "portable-net40+sl5+win8+wp8")]
        [InlineData("MonoAndroid40", "monoandroid")]
        [InlineData("win81", "win81")]
        [InlineData("wpa81", "wpa81")]
        [InlineData("sl5", "sl5")]
        public void FrameworkReducer_AutoMapperGetNearestLibGroup(string projectFramework, string expectedFramework)
        {
            // Arrange

            // Get nearest lib group for AutoMapper 4.0.0-ci1026
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse(projectFramework);

            List<NuGetFramework> frameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("aspnet50"),
                    NuGetFramework.Parse("aspnetcore50"),
                    NuGetFramework.Parse("MonoAndroid"),
                    NuGetFramework.Parse("MonoTouch"),
                    NuGetFramework.Parse("net40"),
                    NuGetFramework.Parse("portable-windows8%2Bnet40%2Bwp8%2Bsl5%2BMonoAndroid%2BMonoTouch"),
                    NuGetFramework.Parse("portable-windows8%2Bnet40%2Bwp8%2Bwpa81%2Bsl5%2BMonoAndroid%2BMonoTouch"),
                    NuGetFramework.Parse("sl5"),
                    NuGetFramework.Parse("windows81"),
                    NuGetFramework.Parse("wpa81"),
                    NuGetFramework.Parse("Xamarin.iOS10")
                };

            // Act
            var result = reducer.GetNearest(project, frameworks);

            // Assert
            Assert.Equal(expectedFramework, result!.GetShortFolderName());
        }

        [Fact]
        public void FrameworkReducer_GetNearestDuplicatePCL()
        {
            // Verify duplicate PCLs in a folder are reduced correctly
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse("wp8");

            var net35 = NuGetFramework.Parse("net35");
            var pcl1 = NuGetFramework.Parse("portable-net403%2Bsl5%2Bnetcore45%2Bwp8");
            var pcl2 = NuGetFramework.Parse("portable-net403%2Bsl5%2Bnetcore45%2Bwp8%2BMonoAndroid1%2BMonoTouch1");

            var all = new NuGetFramework[] { net35, pcl1, pcl2 };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(pcl1, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestProfile()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var project = NuGetFramework.Parse("net45-client");

            var net40 = NuGetFramework.Parse("net40");
            var net40client = NuGetFramework.Parse("net40-client");

            var all = new NuGetFramework[] { net40, net40client };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(net40client, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestWinRT()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var win81 = NuGetFramework.Parse("win81");
            var native = NuGetFramework.Parse("native");
            var winrt45 = NuGetFramework.Parse("winrt45");

            var project = NuGetFramework.Parse("net451");

            var all = new NuGetFramework[] { native, winrt45 };

            var result = reducer.GetNearest(win81, all);

            Assert.Equal(winrt45, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestWin()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var win81 = NuGetFramework.Parse("win81");
            var win8 = NuGetFramework.Parse("win8");
            var netcore = NuGetFramework.Parse("netcore");
            var nfcore = NuGetFramework.Parse("nfcore");
            var native = NuGetFramework.Parse("native");
            var netcore451 = NuGetFramework.Parse("netcore451");
            var winrt45 = NuGetFramework.Parse("winrt45");

            var project = NuGetFramework.Parse("net451");

            var all = new NuGetFramework[] { win8, netcore, nfcore, native, netcore451, winrt45 };

            var result = reducer.GetNearest(win81, all);

            Assert.Equal(netcore451, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestAny()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var dnxcore50 = NuGetFramework.Parse("dnxcore50");
            var dotnet = NuGetFramework.Parse("dotnet");

            var all = new NuGetFramework[] { dnxcore50, dotnet };

            var result = reducer.GetNearest(NuGetFramework.AnyFramework, all);

            Assert.Equal(dnxcore50, result);
        }

        [Fact]
        public void FrameworkReducer_ReduceToHighest()
        {
            // both frameworks are equivalent
            var fw1 = new NuGetFramework(FrameworkConstants.FrameworkIdentifiers.Windows, FrameworkConstants.EmptyVersion);
            var fw2 = FrameworkConstants.CommonFrameworks.Win8;

            var packageFrameworks = new List<NuGetFramework>()
                {
                    fw1,
                    fw2
                };

            FrameworkReducer reducer = new FrameworkReducer();

            // the non-zero version should win in both cases
            var upwards = reducer.ReduceUpwards(packageFrameworks).Single();
            var downwards = reducer.ReduceUpwards(packageFrameworks).Single();

            Assert.Equal(fw2, upwards);
            Assert.Equal(fw2, downwards);
        }

        [Fact]
        public void FrameworkReducer_GetNearestPackagesBasedWithPCL()
        {
            // Arrange
            var project = NuGetFramework.Parse("net46");

            var packageFrameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("portable-net45+win8"),
                    NuGetFramework.Parse("dotnet"),
                };

            FrameworkReducer reducer = new FrameworkReducer();

            // Act
            var nearest = reducer.GetNearest(project, packageFrameworks);

            // Assert
            Assert.Equal(packageFrameworks[1], nearest);
        }

        [Fact]
        public void FrameworkReducer_GetNearestPackagesBasedWithFullFramework()
        {
            // Arrange
            var project = NuGetFramework.Parse("uap10.0");

            var packageFrameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("win8"),
                    NuGetFramework.Parse("dotnet5.3"),
                };

            FrameworkReducer reducer = new FrameworkReducer();

            // Act
            var nearest = reducer.GetNearest(project, packageFrameworks);

            // Assert
            Assert.Equal(packageFrameworks[0], nearest);
        }

        [Fact]
        public void FrameworkReducer_GetNearestPCLtoPCL2()
        {
            var project = NuGetFramework.Parse("portable-net45+sl5+monotouch+monoandroid");

            var packageFrameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("portable-net45+sl5"),
                    NuGetFramework.Parse("portable-net40+sl5+monotouch"),
                    NuGetFramework.Parse("portable-net40+sl4+monotouch+monoandroid"),
                    NuGetFramework.Parse("portable-net40+sl4+monotouch+monoandroid+wp71"),
                };

            FrameworkReducer reducer = new FrameworkReducer();

            var nearest = reducer.GetNearest(project, packageFrameworks);

            Assert.Equal(packageFrameworks[0], nearest);
        }

        [Fact]
        public void FrameworkReducer_GetNearestPCLtoPCL()
        {
            // Arrange
            var project = NuGetFramework.Parse("portable-net45+win81");

            var packageFrameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("portable-net45+win8"),
                    NuGetFramework.Parse("portable-net40+win8"),
                    NuGetFramework.Parse("portable-net40+win81"),
                };

            FrameworkReducer reducer = new FrameworkReducer();

            // Act
            var nearest = reducer.GetNearest(project, packageFrameworks);

            // Assert
            // net45+win8 is nearest since net45 wins over net40
            Assert.Equal(packageFrameworks[0], nearest);
        }

        [Fact]
        public void FrameworkReducer_GetNearestPCLtoPCLVersions()
        {
            var project = NuGetFramework.Parse("portable-net45+win81");

            var packageFrameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("portable-net40+win81+sl5"),
                    NuGetFramework.Parse("portable-net45+win8+sl5"),
                    NuGetFramework.Parse("portable-net45+win81+wpa81+monotouch+monoandroid"),
                };

            FrameworkReducer reducer = new FrameworkReducer();

            var nearest = reducer.GetNearest(project, packageFrameworks);

            // portable-net45+win81+wpa81+monotouch+monoandroid is nearest to the original
            Assert.Equal(packageFrameworks[2], nearest);
        }

        [Fact]
        public void FrameworkReducer_GetNearestNonPCLtoPCL()
        {
            var project = NuGetFramework.Parse("win9");

            var packageFrameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("portable-net45+win8"),
                    NuGetFramework.Parse("portable-net45+win82"),
                    NuGetFramework.Parse("portable-net45+win81"),
                    NuGetFramework.Parse("portable-net45+win91"),
                };

            FrameworkReducer reducer = new FrameworkReducer();

            var nearest = reducer.GetNearest(project, packageFrameworks);

            // win82 is the best match for win9
            Assert.Equal(packageFrameworks[1], nearest);
        }

        [Fact]
        public void FrameworkReducer_GetNearestNonPCLtoPCLBasedOnOtherVersions()
        {
            // Arrange
            var project = NuGetFramework.Parse("win8");

            var packageFrameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("portable-net45+win8+wpa81"),
                    NuGetFramework.Parse("portable-net45+win8+wpa82"),
                    NuGetFramework.Parse("portable-net45+win8+wpa9"),
                    NuGetFramework.Parse("portable-net45+win8+wpa10.0"),
                    NuGetFramework.Parse("portable-net45+win8+wpa11.0+sl5")
                };

            FrameworkReducer reducer = new FrameworkReducer();

            // Act
            var nearest = reducer.GetNearest(project, packageFrameworks);

            // Assert
            // portable-net45+win8+wpa10.0 is the best match since it has the highest
            // version of WPA, and the least frameworks
            Assert.Equal(packageFrameworks[3], nearest);
        }

        [Fact]
        public void FrameworkReducer_GetNearestNonPCLtoPCLUncertain()
        {
            // Arrange
            var project = NuGetFramework.Parse("win8");

            var packageFrameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("portable-net45+win8+sl6"),
                    NuGetFramework.Parse("portable-net45+win8+dnxcore50"),
                    NuGetFramework.Parse("portable-net45+win8+native"),
                };

            FrameworkReducer reducer = new FrameworkReducer();

            // Act
            var nearest = reducer.GetNearest(project, packageFrameworks);

            // Assert
            // There is no certain way to relate these frameworks to each other, but the same one
            // should always come back from this compare.
            Assert.Equal(packageFrameworks[1], nearest);
        }

        [Fact]
        public void FrameworkReducer_GetNearestChooseFrameworkName()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("net40");
            var framework2 = NuGetFramework.Parse("nfcore45");

            var project = NuGetFramework.Parse("net451");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(framework1, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestEqual()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("net40");
            var framework2 = NuGetFramework.Parse("net40-client");

            var project = NuGetFramework.Parse("net40-client");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(framework2, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestEquivalent()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("net40");
            var framework2 = NuGetFramework.Parse("net40-client");

            var project = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.GetNearest(project, all);

            Assert.Equal(framework1, result);
        }

        [Fact]
        public void FrameworkReducer_ReduceUpEquivalent()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("net40");
            var framework2 = NuGetFramework.Parse("net40-client");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.ReduceUpwards(all).ToArray();

            Assert.Equal(2, result.Length);
            Assert.Equal(framework1, result.First());
            Assert.Equal(framework2, result.Last());
        }

        [Fact]
        public void FrameworkReducer_ReduceUpEqual()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("net40");
            var framework2 = NuGetFramework.Parse("net40");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.ReduceUpwards(all);

            Assert.Equal(framework2, result.Single());
        }

        [Fact]
        public void FrameworkReducer_ReduceDownPCL2()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("portable-net45+win8");
            var framework2 = NuGetFramework.Parse("portable-net45+win8+wp8");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.ReduceDownwards(all);

            Assert.Equal(framework2, result.Single());
        }

        [Fact]
        public void FrameworkReducer_ReduceDownPCL()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("portable-net45+win8");
            var framework2 = NuGetFramework.Parse("portable-net451+win81");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.ReduceDownwards(all);

            Assert.Equal(framework1, result.Single());
        }

        [Fact]
        public void FrameworkReducer_ReduceUpPCL2()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("portable-net45+win8");
            var framework2 = NuGetFramework.Parse("portable-net45+win8+wp8");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.ReduceUpwards(all);

            Assert.Equal(framework1, result.Single());
        }

        [Fact]
        public void FrameworkReducer_ReduceUpPCL()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("portable-net45+win8");
            var framework2 = NuGetFramework.Parse("portable-net451+win81");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.ReduceUpwards(all);

            Assert.Equal(framework2, result.Single());
        }

        [Fact]
        public void FrameworkReducer_ReducePCL()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var framework1 = NuGetFramework.Parse("portable-net45+win8");
            var framework2 = NuGetFramework.Parse("portable-win+net45");

            var all = new NuGetFramework[] { framework1, framework2 };

            var result = reducer.ReduceEquivalent(all);

            Assert.Equal(framework1, result.Single());
        }

        [Fact]
        public void FrameworkReducer_ReduceNonSingle()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var sl3wp = NuGetFramework.Parse("sl3-wp");
            var wp7 = NuGetFramework.Parse("wp7");
            var win81 = NuGetFramework.Parse("win81");

            var all = new NuGetFramework[] { sl3wp, wp7, win81 };

            var result = reducer.ReduceEquivalent(all);

            Assert.Equal(2, result.Count());
            Assert.Equal(win81, result.First());
            Assert.Equal(wp7, result.ElementAt(1));
        }

        [Fact]
        public void FrameworkReducer_ReducePrecedence()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var win = NuGetFramework.Parse("win");
            var win8 = NuGetFramework.Parse("win8");
            var netcore45 = NuGetFramework.Parse("netcore45");
            var winrt45 = NuGetFramework.Parse("winrt45");
            var winrt = NuGetFramework.Parse("netcore");
            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { win, win8, netcore45, winrt45, winrt, net45 };

            var result = reducer.ReduceEquivalent(all);

            Assert.Equal(2, result.Count());
            Assert.Equal(win8, result.First());
            Assert.Equal(net45, result.ElementAt(1));
        }

        [Fact]
        public void FrameworkReducer_ReduceSingle()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var sl3wp = NuGetFramework.Parse("sl3-wp");
            var wp7 = NuGetFramework.Parse("wp7");

            var all = new NuGetFramework[] { sl3wp, wp7 };

            var result = reducer.ReduceEquivalent(all);

            Assert.Equal(wp7, result.Single());
        }

        [Fact]
        public void FrameworkReducer_ReduceUpwardsNonSingle()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var net35 = NuGetFramework.Parse("net35");
            var net40 = NuGetFramework.Parse("net40");
            var net45 = NuGetFramework.Parse("net45");
            var net451 = NuGetFramework.Parse("net451");
            var net453 = NuGetFramework.Parse("net453");
            var wp8 = NuGetFramework.Parse("wp8");
            var wp81 = NuGetFramework.Parse("wp81");

            var all = new NuGetFramework[] { net35, net40, net45, net451, net453, wp8, wp81 };

            var result = reducer.ReduceUpwards(all);

            Assert.Equal(net453, result.First());
            Assert.Equal(wp81, result.Skip(1).First());
        }

        [Fact]
        public void FrameworkReducer_ReduceUpwardsBasic()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var net35 = NuGetFramework.Parse("net35");
            var net40 = NuGetFramework.Parse("net40");
            var net45 = NuGetFramework.Parse("net45");
            var net451 = NuGetFramework.Parse("net451");
            var net453 = NuGetFramework.Parse("net453");

            var all = new NuGetFramework[] { net35, net40, net45, net451, net453 };

            var result = reducer.ReduceUpwards(all);

            Assert.Equal(net453, result.Single());
        }

        [Fact]
        public void FrameworkReducer_ReduceDownwardsBasic()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var net35 = NuGetFramework.Parse("net35");
            var net40 = NuGetFramework.Parse("net40");
            var net45 = NuGetFramework.Parse("net45");
            var net451 = NuGetFramework.Parse("net451");
            var net453 = NuGetFramework.Parse("net453");

            var all = new NuGetFramework[] { net35, net40, net45, net451, net453 };

            var result = reducer.ReduceDownwards(all);

            Assert.Equal(net35, result.Single());
        }

        [Fact]
        public void FrameworkReducer_GetNearest()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var net35 = NuGetFramework.Parse("net35");
            var net40 = NuGetFramework.Parse("net40");
            var net45 = NuGetFramework.Parse("net45");
            var net451 = NuGetFramework.Parse("net451");
            var net453 = NuGetFramework.Parse("net453");

            var all = new NuGetFramework[] { net35, net40, net45, net453 };

            var result = reducer.GetNearest(net451, all);

            Assert.Equal(net45, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearest2()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var net35 = NuGetFramework.Parse("net35");
            var net40 = NuGetFramework.Parse("net40");
            var net45 = NuGetFramework.Parse("net45");
            var net451 = NuGetFramework.Parse("net451");
            var net453 = NuGetFramework.Parse("net453");

            var all = new NuGetFramework[] { net35, net40, net45, net451, net453 };

            var result = reducer.GetNearest(net451, all);

            Assert.Equal(net451, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestWithEmptyList()
        {
            // Arrange
            FrameworkReducer reducer = new FrameworkReducer();

            // Act
            var result = reducer.GetNearest(NuGetFramework.Parse("net35"), new NuGetFramework[0]);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestWithAny()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var net40 = NuGetFramework.Parse("net40");
            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { NuGetFramework.AnyFramework, net40, net45 };

            var result = reducer.GetNearest(net45, all);

            Assert.Equal(net45, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestWithUnsupported()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { NuGetFramework.AnyFramework, NuGetFramework.UnsupportedFramework };

            var result = reducer.GetNearest(net45, all);

            Assert.Equal(NuGetFramework.AnyFramework, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestWithUnsupported2()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var all = new NuGetFramework[] { NuGetFramework.AnyFramework, NuGetFramework.UnsupportedFramework };

            var result = reducer.GetNearest(NuGetFramework.UnsupportedFramework, all);

            Assert.Equal(NuGetFramework.AnyFramework, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestWithUnsupported3()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var all = new NuGetFramework[] { NuGetFramework.UnsupportedFramework };

            var result = reducer.GetNearest(NuGetFramework.UnsupportedFramework, all);

            Assert.Equal(NuGetFramework.UnsupportedFramework, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestWithAnyOnly()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            var net45 = NuGetFramework.Parse("net45");

            var all = new NuGetFramework[] { NuGetFramework.AnyFramework };

            var result = reducer.GetNearest(net45, all);

            Assert.Equal(NuGetFramework.AnyFramework, result);
        }

        [Fact]
        public void FrameworkReducer_GetNearestAzureRepro()
        {
            FrameworkReducer reducer = new FrameworkReducer();

            List<NuGetFramework> frameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse("net40"),
                    NuGetFramework.Parse("portable-net45+wp8+win8+wpa"),
                    NuGetFramework.Parse("sl4")
                };

            NuGetFramework projectFramework = NuGetFramework.Parse("net45");

            var result = reducer.GetNearest(projectFramework, frameworks);

            Assert.Equal("net40", result!.GetShortFolderName());
        }

        [Theory]
        [InlineData("dnx451", "aspnet50")]
        [InlineData("dnx451", "aspnet5")]
        [InlineData("dnx451", "aspnet")]
        [InlineData("dnx", "aspnet50")]
        [InlineData("dnx", "net45")]
        [InlineData("dnx", "portable-net45+win8")]
        [InlineData("dnx", "portable-win8+net45")]
        [InlineData("dnx", "portable-win8+net45+sl4")]
        [InlineData("dnx50", "dnx50")]
        [InlineData("dnx50", "dnx5")]
        [InlineData("dnx50", "dnx")]
        public void FrameworkReducer_GetNearestDnx(string project, string framework)
        {
            FrameworkReducer reducer = new FrameworkReducer();

            List<NuGetFramework> frameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse(framework),
                };

            NuGetFramework projectFramework = NuGetFramework.Parse(project);

            var result = reducer.GetNearest(projectFramework, frameworks);

            Assert.True(NuGetFramework.Parse(framework).Equals(result));
        }

        [Theory]
        [InlineData("aspnet", "aspnetcore")]
        [InlineData("aspnetcore", "net45")]
        [InlineData("aspnetcore", "portable-net403+win8")]
        public void FrameworkReducer_GetNearestAspNeg(string project, string framework)
        {
            FrameworkReducer reducer = new FrameworkReducer();

            List<NuGetFramework> frameworks = new List<NuGetFramework>()
                {
                    NuGetFramework.Parse(framework),
                };

            NuGetFramework projectFramework = NuGetFramework.Parse(project);

            var result = reducer.GetNearest(projectFramework, frameworks);

            Assert.Null(result);
        }
    }
}
