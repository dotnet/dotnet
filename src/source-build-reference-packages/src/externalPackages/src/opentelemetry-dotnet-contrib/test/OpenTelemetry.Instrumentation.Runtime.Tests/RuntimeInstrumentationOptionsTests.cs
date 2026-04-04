// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Runtime.Tests;

internal class RuntimeInstrumentationOptionsTests
{
    /*
            [Fact]
            public void Enable_All_If_Nothing_Was_Defined()
            {
                var options = new RuntimeInstrumentationOptions();

                Assert.True(options.IsGcEnabled);
    #if NET
                Assert.True(options.IsJitEnabled);
                Assert.True(options.IsThreadingEnabled);
    #endif
                Assert.True(options.IsAssembliesEnabled);
                Assert.True(options.IsAllEnabled);
            }

            [Fact]
            public void Enable_Gc_Only()
            {
                var options = new RuntimeInstrumentationOptions { GcEnabled = true };

                Assert.True(options.IsGcEnabled);
    #if NET
                Assert.False(options.IsJitEnabled);
                Assert.False(options.IsThreadingEnabled);
    #endif
                Assert.False(options.IsAssembliesEnabled);
                Assert.False(options.IsAllEnabled);
            }

    #if NET
            [Fact]
            public void Enable_Jit_Only()
            {
                var options = new RuntimeInstrumentationOptions { JitEnabled = true };

                Assert.False(options.IsGcEnabled);
                Assert.True(options.IsJitEnabled);
                Assert.False(options.IsThreadingEnabled);
                Assert.False(options.IsAssembliesEnabled);
                Assert.False(options.IsAllEnabled);
            }
    #endif

    #if NET
            [Fact]
            public void Enable_Threading_Only()
            {
                var options = new RuntimeInstrumentationOptions { ThreadingEnabled = true };

                Assert.False(options.IsGcEnabled);
                Assert.False(options.IsJitEnabled);
                Assert.True(options.IsThreadingEnabled);
                Assert.False(options.IsAssembliesEnabled);
                Assert.False(options.IsAllEnabled);
            }
    #endif

            [Fact]
            public void Enable_Assemblies_Only()
            {
                var options = new RuntimeInstrumentationOptions { AssembliesEnabled = true };

                Assert.False(options.IsGcEnabled);
    #if NET
                Assert.False(options.IsJitEnabled);
                Assert.False(options.IsThreadingEnabled);
    #endif
                Assert.True(options.IsAssembliesEnabled);
                Assert.False(options.IsAllEnabled);
            }

            [Fact]
            public void Enable_Multiple()
            {
                var options = new RuntimeInstrumentationOptions { GcEnabled = true, AssembliesEnabled = true };

                Assert.True(options.IsGcEnabled);
    #if NET
                Assert.False(options.IsJitEnabled);
                Assert.False(options.IsThreadingEnabled);
    #endif
                Assert.True(options.IsAssembliesEnabled);
                Assert.False(options.IsAllEnabled);
            }
    */
}
