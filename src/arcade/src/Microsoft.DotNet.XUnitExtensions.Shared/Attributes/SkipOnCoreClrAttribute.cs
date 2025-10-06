// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.SkipOnCoreClrDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SkipOnCoreClrAttribute : Attribute, ITraitAttribute
    {
#if USES_XUNIT_3
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
        private static readonly Lazy<bool> s_isStressTest = new Lazy<bool>(() => CoreClrConfigurationDetection.IsStressTest);

        private readonly TestPlatforms _testPlatforms = TestPlatforms.Any;
        private readonly RuntimeTestModes _testMode = RuntimeTestModes.Any;
        private readonly RuntimeConfiguration _runtimeConfiguration = RuntimeConfiguration.Any;

        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            if (!DiscovererHelpers.IsMonoRuntime)
            {
                if (DiscovererHelpers.TestPlatformApplies(_testPlatforms) && RuntimeConfigurationApplies(_runtimeConfiguration) && StressModeApplies(_testMode))
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
#endif
        internal SkipOnCoreClrAttribute() { }

        public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms)
        {
#if USES_XUNIT_3
            _testPlatforms = testPlatforms;
#endif
        }

        public SkipOnCoreClrAttribute(string reason, RuntimeTestModes testMode)
        {
#if USES_XUNIT_3
            _testMode = testMode;
#endif
        }

        public SkipOnCoreClrAttribute(string reason, RuntimeConfiguration runtimeConfigurations)
        {
#if USES_XUNIT_3
            _runtimeConfiguration = runtimeConfigurations;
#endif
        }

        public SkipOnCoreClrAttribute(string reason, RuntimeConfiguration runtimeConfigurations, RuntimeTestModes testModes)
        {
#if USES_XUNIT_3
            _runtimeConfiguration = runtimeConfigurations;
            _testMode = testModes;
#endif
        }

        public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeConfiguration runtimeConfigurations)
        {
#if USES_XUNIT_3
            _testPlatforms = testPlatforms;
            _runtimeConfiguration = runtimeConfigurations;
#endif
        }

        public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeTestModes testMode)
        {
#if USES_XUNIT_3
            _testPlatforms = testPlatforms;
            _testMode = testMode;
#endif
        }

        public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeConfiguration runtimeConfigurations, RuntimeTestModes testModes)
        {
#if USES_XUNIT_3
            _testPlatforms = testPlatforms;
            _runtimeConfiguration = runtimeConfigurations;
            _testMode = testModes;
#endif
        }

        public SkipOnCoreClrAttribute(string reason) { }
    }
}
