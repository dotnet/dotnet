// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.Test
{
    public class FrameworkExpanderTests
    {
        [Fact]
        public void FrameworkExpander_UAPWPA()
        {
            NuGetFramework framework = NuGetFramework.Parse("UAP10.0");
            NuGetFramework indirect = new NuGetFramework("WindowsPhoneApp", new Version(8, 1, 0, 0));

            FrameworkExpander expander = new FrameworkExpander();
            var expanded = expander.Expand(framework).ToArray();

            Assert.True(expanded.Contains(indirect, NuGetFramework.Comparer), String.Join("|", expanded.Select(e => e.ToString())));
        }

        [Fact]
        public void FrameworkExpander_UAPWIN()
        {
            NuGetFramework framework = NuGetFramework.Parse("UAP10.0");
            NuGetFramework indirect = new NuGetFramework("Windows", new Version(8, 1, 0, 0));

            FrameworkExpander expander = new FrameworkExpander();
            var expanded = expander.Expand(framework).ToArray();

            Assert.True(expanded.Contains(indirect, NuGetFramework.Comparer), String.Join("|", expanded.Select(e => e.ToString())));
        }

        [Fact]
        public void FrameworkExpander_UAP()
        {
            NuGetFramework framework = NuGetFramework.Parse("UAP10.0");
            NuGetFramework indirect = new NuGetFramework(".NETCore", new Version(5, 0, 0, 0));

            FrameworkExpander expander = new FrameworkExpander();
            var expanded = expander.Expand(framework).ToArray();

            Assert.True(expanded.Contains(indirect, NuGetFramework.Comparer), String.Join("|", expanded.Select(e => e.ToString())));
        }

        [Fact]
        public void FrameworkExpander_Indirect()
        {
            NuGetFramework framework = NuGetFramework.Parse("win9");
            NuGetFramework indirect = new NuGetFramework(".NETCore", new Version(4, 5));

            FrameworkExpander expander = new FrameworkExpander();
            var expanded = expander.Expand(framework).ToArray();

            Assert.True(expanded.Contains(indirect, NuGetFramework.Comparer), String.Join("|", expanded.Select(e => e.ToString())));
        }

        [Fact]
        public void FrameworkExpander_Basic()
        {
            NuGetFramework framework = NuGetFramework.Parse("net45");

            FrameworkExpander expander = new FrameworkExpander();
            var expanded = expander
                .Expand(framework)
                .OrderBy(f => f, NuGetFrameworkSorter.Instance)
                .ToArray();

            Assert.Equal(7, expanded.Length);
            Assert.Equal(".NETFramework,Version=v4.5,Profile=Client", expanded[0].ToString());
            Assert.Equal(".NETFramework,Version=v4.5,Profile=Full", expanded[1].ToString());
            Assert.Equal(".NETPlatform,Version=v5.0", expanded[2].ToString()); // dotnet
            Assert.Equal(new Version(0, 0, 0, 0), expanded[2].Version);
            Assert.Equal(".NETPlatform,Version=v5.0", expanded[3].ToString()); // dotnet5
            Assert.Equal(new Version(5, 0, 0, 0), expanded[3].Version);
            Assert.Equal(".NETPlatform,Version=v5.2", expanded[4].ToString());
            Assert.Equal(".NETStandard,Version=v1.0", expanded[5].ToString());
            Assert.Equal(".NETStandard,Version=v1.1", expanded[6].ToString());
        }

        [Fact]
        public void FrameworkExpander_Win()
        {
            NuGetFramework framework = NuGetFramework.Parse("win");

            FrameworkExpander expander = new FrameworkExpander();
            var expanded = expander
                .Expand(framework)
                .OrderBy(f => f, NuGetFrameworkSorter.Instance)
                .ToArray();

            Assert.Equal(10, expanded.Length);
            Assert.Equal(".NETCore,Version=v0.0", expanded[0].ToString());
            Assert.Equal(".NETCore,Version=v4.5", expanded[1].ToString());
            Assert.Equal(".NETPlatform,Version=v5.0", expanded[2].ToString()); // dotnet
            Assert.Equal(new Version(0, 0, 0, 0), expanded[2].Version);
            Assert.Equal(".NETPlatform,Version=v5.0", expanded[3].ToString()); // dotnet5
            Assert.Equal(new Version(5, 0, 0, 0), expanded[3].Version);
            Assert.Equal(".NETPlatform,Version=v5.2", expanded[4].ToString());
            Assert.Equal(".NETStandard,Version=v1.0", expanded[5].ToString());
            Assert.Equal(".NETStandard,Version=v1.1", expanded[6].ToString());
            Assert.Equal("Windows,Version=v8.0", expanded[7].ToString());
            Assert.Equal("WinRT,Version=v0.0", expanded[8].ToString());
            Assert.Equal("WinRT,Version=v4.5", expanded[9].ToString());
        }

        [Fact]
        public void FrameworkExpander_NetCore45()
        {
            NuGetFramework framework = NuGetFramework.Parse("nfcore45");

            FrameworkExpander expander = new FrameworkExpander();
            var expanded = expander.Expand(framework).ToArray();

            Assert.Equal(0, expanded.Length);
        }
    }
}
