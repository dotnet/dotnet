// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !USES_XUNIT_3
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    public class SkipOnCoreClrDiscoverer : ITraitDiscoverer
    {
        private static readonly Lazy<bool> s_isJitStress = new Lazy<bool>(() => CoreClrConfigurationDetection.IsJitStress);
        private static readonly Lazy<bool> s_isJitStressRegs = new Lazy<bool>(() => CoreClrConfigurationDetection.IsJitStressRegs);
        private static readonly Lazy<bool> s_isJitMinOpts = new Lazy<bool>(() => CoreClrConfigurationDetection.IsJitMinOpts);
        private static readonly Lazy<bool> s_isTailCallStress = new Lazy<bool>(() => CoreClrConfigurationDetection.IsTailCallStress);
        private static readonly Lazy<bool> s_isZapDisable = new Lazy<bool>(() => CoreClrConfigurationDetection.IsZapDisable);
        private static readonly Lazy<bool> s_isGCStress3 = new Lazy<bool>(() => CoreClrConfigurationDetection.IsGCStress3);
        private static readonly Lazy<bool> s_isGCStressC = new Lazy<bool>(() => CoreClrConfigurationDetection.IsGCStressC);
        private static readonly Lazy<bool> s_isCheckedRuntime = new Lazy<bool>(() => CoreClrConfigurationDetection.IsCheckedRuntime);
        private static readonly Lazy<bool> s_isReleaseRuntime = new Lazy<bool>(() => CoreClrConfigurationDetection.IsReleaseRuntime);
        private static readonly Lazy<bool> s_isDebugRuntime = new Lazy<bool>(() => CoreClrConfigurationDetection.IsDebugRuntime);
        private static readonly Lazy<bool> s_isStressTest = new Lazy<bool>(() =>  CoreClrConfigurationDetection.IsStressTest);

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            if (!DiscovererHelpers.IsMonoRuntime)
            {
                TestPlatforms testPlatforms = TestPlatforms.Any;
                RuntimeTestModes stressMode = RuntimeTestModes.Any;
                RuntimeConfiguration runtimeConfigurations = RuntimeConfiguration.Any;
                foreach (object arg in traitAttribute.GetConstructorArguments().Skip(1)) // We skip the first one as it is the reason
                {
                    if (arg is TestPlatforms tp)
                    {
                        testPlatforms = tp;
                    }
                    else if (arg is RuntimeTestModes rtm)
                    {
                        stressMode = rtm;
                    }
                    else if (arg is RuntimeConfiguration rc)
                    {
                        runtimeConfigurations = rc;
                    }
                }

                if (DiscovererHelpers.TestPlatformApplies(testPlatforms) && RuntimeConfigurationApplies(runtimeConfigurations) && StressModeApplies(stressMode))
                {
                    return new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing) };
                }
            }

            return Array.Empty<KeyValuePair<string, string>>();
        }

        private static bool RuntimeConfigurationApplies(RuntimeConfiguration runtimeConfigurations) =>
            (runtimeConfigurations.HasFlag(RuntimeConfiguration.Checked) && s_isCheckedRuntime.Value) ||
            (runtimeConfigurations.HasFlag(RuntimeConfiguration.Release) && s_isReleaseRuntime.Value) ||
            (runtimeConfigurations.HasFlag(RuntimeConfiguration.Debug) && s_isDebugRuntime.Value);

        // Order here matters as some env variables may appear in multiple modes
        private static bool StressModeApplies(RuntimeTestModes stressMode) =>
            (stressMode.HasFlag(RuntimeTestModes.RegularRun) && !s_isStressTest.Value) ||
            (stressMode.HasFlag(RuntimeTestModes.GCStress3) && s_isGCStress3.Value) ||
            (stressMode.HasFlag(RuntimeTestModes.GCStressC) && s_isGCStressC.Value) ||
            (stressMode.HasFlag(RuntimeTestModes.ZapDisable) && s_isZapDisable.Value) ||
            (stressMode.HasFlag(RuntimeTestModes.TailcallStress) && s_isTailCallStress.Value) ||
            (stressMode.HasFlag(RuntimeTestModes.JitStressRegs) && s_isJitStressRegs.Value) ||
            (stressMode.HasFlag(RuntimeTestModes.JitStress) && s_isJitStress.Value) ||
            (stressMode.HasFlag(RuntimeTestModes.JitMinOpts) && s_isJitMinOpts.Value);
    }
}
#endif
