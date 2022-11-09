// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using Xunit;

namespace NuGet.Test
{
    public class CompatibilityPCLTests
    {
        [Fact]
        public void CompatibilityPCL_NetNeg()
        {
            var framework1 = NuGetFramework.Parse("net40");
            var framework2 = NuGetFramework.Parse("portable-win8+net45");

            var compat = DefaultCompatibilityProvider.Instance;

            Assert.False(compat.IsCompatible(framework1, framework2));
            Assert.False(compat.IsCompatible(framework2, framework1));
        }

        [Fact]
        public void CompatibilityPCL_Net()
        {
            var framework1 = NuGetFramework.Parse("net45");
            var framework2 = NuGetFramework.Parse("portable-win8+net45");

            var compat = DefaultCompatibilityProvider.Instance;

            Assert.True(compat.IsCompatible(framework1, framework2));
            Assert.False(compat.IsCompatible(framework2, framework1));
        }

        [Fact]
        public void CompatibilityPCL_WithOneTfm()
        {
            var framework1 = NuGetFramework.Parse("net45");
            var framework2 = NuGetFramework.Parse("portable-net45");

            var compat = DefaultCompatibilityProvider.Instance;

            Assert.True(compat.IsCompatible(framework1, framework2));
            Assert.False(compat.IsCompatible(framework2, framework1));
        }

        [Fact]
        public void CompatibilityPCL_ZeroTfmIntoNonPclProject()
        {
            var framework1 = NuGetFramework.Parse("net45");
            var framework2 = NuGetFramework.Parse("portable");

            var compat = DefaultCompatibilityProvider.Instance;

            Assert.False(compat.IsCompatible(framework1, framework2));
            Assert.False(compat.IsCompatible(framework2, framework1));
        }

        [Fact]
        public void CompatibilityPCL_ZeroTfm()
        {
            var framework1 = NuGetFramework.Parse("portable");
            var framework2 = NuGetFramework.Parse("portable");

            var compat = DefaultCompatibilityProvider.Instance;

            Assert.True(compat.IsCompatible(framework1, framework2));
            Assert.True(compat.IsCompatible(framework2, framework1));
        }

        [Theory]
        // supported profiles
        [InlineData("portable-net45+netcore45", "netstandard6.0", false)]          // Profile7 -> netstandard 1.0 - 1.1
        [InlineData("portable-net45+netcore45", "netstandard1.2", false)]
        [InlineData("portable-net45+netcore45", "netstandard1.1.1", false)]
        [InlineData("portable-net45+netcore45", "netstandard1.1.0", true)]
        [InlineData("portable-net45+netcore45", "netstandard1.1", true)]
        [InlineData("portable-net45+netcore45", "netstandard1.0.1", true)]
        [InlineData("portable-net45+netcore45", "netstandard1.0.0", true)]
        [InlineData("portable-net45+netcore45", "netstandard1.0", true)]
        [InlineData("portable-net45+netcore45", "netstandard0.9.1", true)]
        [InlineData("portable-net45+netcore45", "netstandard0.9", true)]
        [InlineData("portable-net45+netcore45", "netstandard0.0", true)]
        [InlineData("portable-net45+netcore45", "netstandard", true)]
        [InlineData("portable-netcore451+wp81", "netstandard1.0", true)]           // Profile31 -> netstandard 1.0
        [InlineData("portable-netcore451+wpa81", "netstandard1.2", true)]          // Profile32 -> netstandard 1.0 - 1.2
        [InlineData("portable-net451+netcore451", "netstandard1.2", true)]         // Profile44 -> netstandard 1.0 - 1.2
        [InlineData("portable-net45+wp8", "netstandard1.0", true)]                 // Profile49 -> netstandard 1.0
        [InlineData("portable-net45+netcore45+wp8", "netstandard1.0", true)]       // Profile78 -> netstandard 1.0
        [InlineData("portable-wpa81+wp81", "netstandard1.0", true)]                // Profile84 -> netstandard 1.0
        [InlineData("portable-net45+netcore45+wpa81", "netstandard1.1", true)]     // Profile111 -> netstandard 1.0 - 1.1
        [InlineData("portable-net451+netcore451+wpa81", "netstandard1.2", true)]   // Profile151 -> netstandard 1.0 - 1.2
        [InlineData("portable-netcore451+wpa81+wp81", "netstandard1.0", true)]     // Profile157 -> netstandard 1.0
        [InlineData("portable-net45+netcore45+wpa81+wp8", "netstandard1.0", true)] // Profile259 -> netstandard 1.0

        // unsupported profiles
        [InlineData("portable-net4+sl40+netcore45+wp70", "netstandard1.0", false)]        // Profile2
        [InlineData("portable-net4+sl40", "netstandard1.0", false)]                       // Profile3
        [InlineData("portable-net45+sl40+netcore45+wp70", "netstandard1.0", false)]       // Profile4
        [InlineData("portable-net4+netcore45", "netstandard1.0", false)]                  // Profile5
        [InlineData("portable-net403+netcore45", "netstandard1.0", false)]                // Profile6
        [InlineData("portable-net4+sl50", "netstandard1.0", false)]                       // Profile14
        [InlineData("portable-net403+sl40", "netstandard1.0", false)]                     // Profile18
        [InlineData("portable-net403+sl50", "netstandard1.0", false)]                     // Profile19
        [InlineData("portable-net45+sl40", "netstandard1.0", false)]                      // Profile23
        [InlineData("portable-net45+sl50", "netstandard1.0", false)]                      // Profile24
        [InlineData("portable-net4+sl40+netcore45+wp8", "netstandard1.0", false)]         // Profile36
        [InlineData("portable-net4+sl50+netcore45", "netstandard1.0", false)]             // Profile37
        [InlineData("portable-net403+sl40+netcore45", "netstandard1.0", false)]           // Profile41
        [InlineData("portable-net403+sl50+netcore45", "netstandard1.0", false)]           // Profile42
        [InlineData("portable-net45+sl40+netcore45", "netstandard1.0", false)]            // Profile46
        [InlineData("portable-net45+sl50+netcore45", "netstandard1.0", false)]            // Profile47
        [InlineData("portable-net4+sl40+netcore45+wp71", "netstandard1.0", false)]        // Profile88
        [InlineData("portable-net4+netcore45+wpa81", "netstandard1.0", false)]            // Profile92
        [InlineData("portable-net403+sl40+netcore45+wp70", "netstandard1.0", false)]      // Profile95
        [InlineData("portable-net403+sl40+netcore45+wp71", "netstandard1.0", false)]      // Profile96
        [InlineData("portable-net403+netcore45+wpa81", "netstandard1.0", false)]          // Profile102
        [InlineData("portable-net45+sl40+netcore45+wp71", "netstandard1.0", false)]       // Profile104
        [InlineData("portable-net4+sl50+netcore45+wp8", "netstandard1.0", false)]         // Profile136
        [InlineData("portable-net403+sl40+netcore45+wp8", "netstandard1.0", false)]       // Profile143
        [InlineData("portable-net403+sl50+netcore45+wp8", "netstandard1.0", false)]       // Profile147
        [InlineData("portable-net45+sl40+netcore45+wp8", "netstandard1.0", false)]        // Profile154
        [InlineData("portable-net45+sl50+netcore45+wp8", "netstandard1.0", false)]        // Profile158
        [InlineData("portable-net4+sl50+netcore45+wpa81", "netstandard1.0", false)]       // Profile225
        [InlineData("portable-net403+sl50+netcore45+wpa81", "netstandard1.0", false)]     // Profile240
        [InlineData("portable-net45+sl50+netcore45+wpa81", "netstandard1.0", false)]      // Profile255
        [InlineData("portable-net4+sl50+netcore45+wpa81+wp8", "netstandard1.0", false)]   // Profile328
        [InlineData("portable-net403+sl50+netcore45+wpa81+wp8", "netstandard1.0", false)] // Profile336
        [InlineData("portable-net45+sl50+netcore45+wpa81+wp8", "netstandard1.0", false)]  // Profile344
        [InlineData("portable-net4+sl40+bad", "netstandard1.0", false)]                   // Invalid
        public void CompatibilityPCL_SomePclSupportsNetStandard(string portable, string netStandard, bool isCompatible)
        {
            var portableFramework = NuGetFramework.Parse(portable);
            var netStandardFramework = NuGetFramework.Parse(netStandard);

            var compat = DefaultCompatibilityProvider.Instance;

            Assert.Equal(isCompatible, compat.IsCompatible(portableFramework, netStandardFramework));

            // NetStandard does not support PCL
            Assert.False(compat.IsCompatible(netStandardFramework, portableFramework));
        }

        [Theory]
        [InlineData("portable-net45+netcore45", "netstandardapp6.0")]           // Profile7
        [InlineData("portable-netcore451+wp81", "netstandardapp1.0")]           // Profile31
        [InlineData("portable-netcore451+wpa81", "netstandardapp1.2")]          // Profile32
        [InlineData("portable-net451+netcore451", "netstandardapp1.2")]         // Profile44
        [InlineData("portable-net45+wp8", "netstandardapp1.0")]                 // Profile49
        [InlineData("portable-net45+netcore45+wp8", "netstandardapp1.0")]       // Profile78
        [InlineData("portable-wpa81+wp81", "netstandardapp1.0")]                // Profile84
        [InlineData("portable-net45+netcore45+wpa81", "netstandardapp1.1")]     // Profile111
        [InlineData("portable-net451+netcore451+wpa81", "netstandardapp1.2")]   // Profile151
        [InlineData("portable-netcore451+wpa81+wp81", "netstandardapp1.0")]     // Profile157
        [InlineData("portable-net45+netcore45+wpa81+wp8", "netstandardapp1.0")] // Profile259
        [InlineData("portable-net4+sl40+bad", "netstandardapp1.0")]             // Invalid
        public void CompatibilityPCL_PclDoesNotSupportNetStandardApp(string portable, string netStandardApp)
        {
            var portableFramework = NuGetFramework.Parse(portable);
            var netStandardAppFramework = NuGetFramework.Parse(netStandardApp);

            var compat = DefaultCompatibilityProvider.Instance;

            // PCL does not support NetStandardApp
            Assert.False(compat.IsCompatible(portableFramework, netStandardAppFramework));

            // NetStandardApp does not support PCL
            Assert.False(compat.IsCompatible(netStandardAppFramework, portableFramework));
        }

        [Theory]
        [InlineData("portable-net45+netcore45", "netcoreapp1.0")]           // Profile7
        [InlineData("portable-netcore451+wp81", "netcoreapp1.0")]           // Profile31
        [InlineData("portable-netcore451+wpa81", "netcoreapp1.0")]          // Profile32
        [InlineData("portable-net451+netcore451", "netcoreapp1.0")]         // Profile44
        [InlineData("portable-net45+wp8", "netcoreapp1.0")]                 // Profile49
        [InlineData("portable-net45+netcore45+wp8", "netcoreapp1.0")]       // Profile78
        [InlineData("portable-wpa81+wp81", "netcoreapp1.0")]                // Profile84
        [InlineData("portable-net45+netcore45+wpa81", "netcoreapp1.0")]     // Profile111
        [InlineData("portable-net451+netcore451+wpa81", "netcoreapp1.0")]   // Profile151
        [InlineData("portable-netcore451+wpa81+wp81", "netcoreapp1.0")]     // Profile157
        [InlineData("portable-net45+netcore45+wpa81+wp8", "netcoreapp1.0")] // Profile259
        [InlineData("portable-net4+sl40+bad", "netcoreapp1.0")]             // Invalid
        public void CompatibilityPCL_PclDoesNotSupportNetCoreApp(string portable, string netStandardApp)
        {
            var portableFramework = NuGetFramework.Parse(portable);
            var netStandardAppFramework = NuGetFramework.Parse(netStandardApp);

            var compat = DefaultCompatibilityProvider.Instance;

            // PCL does not support NetStandardApp
            Assert.False(compat.IsCompatible(portableFramework, netStandardAppFramework));

            // NetStandardApp does not support PCL
            Assert.False(compat.IsCompatible(netStandardAppFramework, portableFramework));
        }

        [Theory]
        [InlineData("portable-net45+win8+monoandroid", "portable-net45+win8+unk8+monoandroid")]
        [InlineData("portable-net45+win8", "portable-net45+win8+unk8+monoandroid+monotouch")]
        [InlineData("portable-net45+win8", "portable-net45+win8+unk8+monoandroid1+monotouch1")]
        [InlineData("portable-net45+win8+monoandroid+monotouch", "portable-net45+win8+unk8")]
        [InlineData("portable-net45+win8+monoandroid1+monotouch1", "portable-net45+win8+unk8")]
        public void CompatibilityPCL_OptionalUnk(string fw1, string fw2)
        {
            var framework1 = NuGetFramework.Parse(fw1);
            var framework2 = NuGetFramework.Parse(fw2);

            var compat = DefaultCompatibilityProvider.Instance;

            // verify that compatibility is inferred across all the mappings
            Assert.True(compat.IsCompatible(framework1, framework2));

            // verify that this was a one way mapping
            Assert.False(compat.IsCompatible(framework2, framework1));
        }

        [Theory]
        [InlineData("portable-net45+win8+monoandroid", "portable-net45+win8+wp8+monoandroid")]
        [InlineData("portable-net45+win8", "portable-net45+win8+wp8+monoandroid+monotouch")]
        [InlineData("portable-net45+win8", "portable-net45+win8+wp8+monoandroid1+monotouch1")]
        [InlineData("portable-net45+win8+monoandroid+monotouch", "portable-net45+win8+wp8")]
        [InlineData("portable-net45+win8+monoandroid1+monotouch1", "portable-net45+win8+wp8")]
        [InlineData("monoandroid10", "portable-net45+win8")]              //Profile7
        [InlineData("monoandroid10", "portable-net451+win81")]            //Profile44
        [InlineData("monoandroid10", "portable-net451+win81+wpa81")]      //Profile151
        [InlineData("monotouch10", "portable-net45+win8")]                //Profile7
        [InlineData("monotouch10", "portable-net451+win81")]              //Profile44
        [InlineData("monotouch10", "portable-net451+win81+wpa81")]        //Profile151
        [InlineData("xamarinios10", "portable-net45+win8")]               //Profile7
        [InlineData("xamarinios10", "portable-net451+win81")]             //Profile44
        [InlineData("xamarinios10", "portable-net451+win81+wpa81")]       //Profile151
        [InlineData("xamarintvos10", "portable-net45+win8")]              //Profile7
        [InlineData("xamarintvos10", "portable-net451+win81")]            //Profile44
        [InlineData("xamarintvos10", "portable-net451+win81+wpa81")]      //Profile151
        [InlineData("xamarinwatchos10", "portable-net45+win8")]           //Profile7
        [InlineData("xamarinwatchos10", "portable-net451+win81")]         //Profile44
        [InlineData("xamarinwatchos10", "portable-net451+win81+wpa81")]   //Profile151
        [InlineData("xamarinmac20", "portable-net45+win8")]               //Profile7
        [InlineData("xamarinmac20", "portable-net451+win81")]             //Profile44
        [InlineData("xamarinmac20", "portable-net451+win81+wpa81")]       //Profile151
        public void CompatibilityPCL_Optional(string fw1, string fw2)
        {
            var framework1 = NuGetFramework.Parse(fw1);
            var framework2 = NuGetFramework.Parse(fw2);

            var compat = DefaultCompatibilityProvider.Instance;

            // verify that compatibility is inferred across all the mappings
            Assert.True(compat.IsCompatible(framework1, framework2));

            // verify that this was a one way mapping
            Assert.False(compat.IsCompatible(framework2, framework1));
        }

        [Fact]
        public void CompatibilityPCL_Basic()
        {
            // win9 -> win8 -> netcore45, win8 -> netcore45
            var framework1 = NuGetFramework.Parse("portable-net451+win81");
            var framework2 = NuGetFramework.Parse("portable-win8+net45");

            var compat = DefaultCompatibilityProvider.Instance;

            // verify that compatibility is inferred across all the mappings
            Assert.True(compat.IsCompatible(framework1, framework2));

            // verify that this was a one way mapping
            Assert.False(compat.IsCompatible(framework2, framework1));
        }

        [Fact]
        public void CompatibilityPCL_Same()
        {
            // win9 -> win8 -> netcore45, win8 -> netcore45
            var framework1 = NuGetFramework.Parse("portable-net45+win8");
            var framework2 = NuGetFramework.Parse("portable-win8+net45");

            var compat = DefaultCompatibilityProvider.Instance;

            // verify that compatibility is inferred across all the mappings
            Assert.True(compat.IsCompatible(framework1, framework2));

            // verify that this was a one way mapping
            Assert.True(compat.IsCompatible(framework2, framework1));
        }
    }
}
