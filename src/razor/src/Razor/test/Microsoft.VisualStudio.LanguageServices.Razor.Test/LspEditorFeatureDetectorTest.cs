// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.Razor.Logging;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor;

public class LspEditorFeatureDetectorTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    public static TheoryData<bool, bool, bool> IsLspEditorEnabledTestData { get; } = new()
    {
        // legacyEditorFeatureFlag, legacyEditorSetting, expectedResult
        { false, false, true },
        { false, true, false },
        { true, false, false },
        { true, true, false }
    };

    [UITheory]
    [MemberData(nameof(IsLspEditorEnabledTestData))]
    public void IsLspEditorEnabled(bool legacyEditorFeatureFlag, bool legacyEditorSetting, bool expectedResult)
    {
        // Arrange
        var featureDetector = CreateLspEditorFeatureDetector(legacyEditorFeatureFlag, legacyEditorSetting);

        // Act
        var result = featureDetector.IsLspEditorEnabledAndAvailable(@"c:\TestProject\TestFile.cshtml");

        // Assert
        Assert.Equal(expectedResult, result);
    }

    public static TheoryData<bool, bool, bool, bool, bool> IsLspEditorAvailableTestData { get; } = new()
    {
        // legacyEditorFeatureFlag, legacyEditorSetting, hasLegacyRazorEditorCapability, hasDotNetCoreCSharpCapability, expectedResult
        { false, false, true, false, false }, // .Net Framework project - always non-LSP
        { false, false, false, true, true },  // .Net Core project
        { false, false, true, true, false },  // .Net Core project opts-in into legacy razor editor (exists in reality?)
        { true, false, false, true, false },  // .Net Core project but legacy editor via feature flag
        { false, true, false, true, false },  // .Net Core project but legacy editor via editor option
    };

    [UITheory]
    [MemberData(nameof(IsLspEditorAvailableTestData))]
    public void IsLspEditorAvailable(
        bool legacyEditorFeatureFlag,
        bool legacyEditorSetting,
        bool hasLegacyRazorEditorCapability,
        bool hasDotNetCoreCSharpCapability,
        bool expectedResult)
    {
        // Arrange
        var featureDetector = CreateLspEditorFeatureDetector(legacyEditorFeatureFlag, legacyEditorSetting, hasLegacyRazorEditorCapability, hasDotNetCoreCSharpCapability);

        // Act
        var result = featureDetector.IsLspEditorEnabledAndAvailable(@"c:\TestProject\TestFile.cshtml");

        // Assert
        Assert.Equal(expectedResult, result);
    }

    public static TheoryData<bool, bool, bool, bool> IsRemoteClientTestData { get; } = new()
    {
        // isLiveShareHostActive, isLiveShareGuestActive, cloudEnvironmentConnectedActive, expectedResult
        { false, false, false, false },
        { true, false, false, false },
        { false, true, false, true },
        { false, false, true, true },
        { true, false, true, true },
        { true, true, true, true }
    };

    [UITheory]
    [MemberData(nameof(IsRemoteClientTestData))]
    public void IsRemoteClient(bool liveShareHostActive, bool liveShareGuestActive, bool cloudEnvironmentConnectedActive, bool expectedResult)
    {
        // Arrange
        var uiContextService = CreateUIContextService(liveShareHostActive, liveShareGuestActive, cloudEnvironmentConnectedActive);
        var featureDetector = CreateLspEditorFeatureDetector(uiContextService);

        // Act
        var result = featureDetector.IsRemoteClient();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    public static TheoryData<bool, bool, bool, bool> IsLiveShareHostTestData { get; } = new()
    {
        // isLiveShareHostActive, isLiveShareGuestActive, cloudEnvironmentConnectedActive, expectedResult
        { false, false, false, false },
        { true, false, false, true },
        { false, true, false, false },
        { false, false, true, false },
        { true, false, true, true },
        { true, true, true, true }
    };

    [UITheory]
    [MemberData(nameof(IsLiveShareHostTestData))]
    public void IsLiveShareHost(bool liveShareHostActive, bool liveShareGuestActive, bool cloudEnvironmentConnectedActive, bool expectedResult)
    {
        // Arrange
        var uiContextService = CreateUIContextService(liveShareHostActive, liveShareGuestActive, cloudEnvironmentConnectedActive);
        var featureDetector = CreateLspEditorFeatureDetector(uiContextService);

        // Act
        var result = featureDetector.IsLiveShareHost();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    private ILspEditorFeatureDetector CreateLspEditorFeatureDetector(IUIContextService uiContextService)
        => CreateLspEditorFeatureDetector(legacyEditorFeatureFlag: false, legacyEditorSetting: false, uiContextService, hasLegacyRazorEditorCapability: false, hasDotNetCoreCSharpCapability: true);

    private ILspEditorFeatureDetector CreateLspEditorFeatureDetector(
        bool legacyEditorFeatureFlag = false,
        bool legacyEditorSetting = false,
        bool hasLegacyRazorEditorCapability = false,
        bool hasDotNetCoreCSharpCapability = true)
    {
        return CreateLspEditorFeatureDetector(legacyEditorFeatureFlag, legacyEditorSetting, CreateUIContextService(), hasLegacyRazorEditorCapability, hasDotNetCoreCSharpCapability);
    }

    private ILspEditorFeatureDetector CreateLspEditorFeatureDetector(
        bool legacyEditorFeatureFlag,
        bool legacyEditorSetting,
        IUIContextService uiContextService,
        bool hasLegacyRazorEditorCapability,
        bool hasDotNetCoreCSharpCapability)
    {
        uiContextService ??= CreateUIContextService();

        var featureDetector = new LspEditorFeatureDetector(
            CreateAggregateProjectCapabilityResolver(hasLegacyRazorEditorCapability, hasDotNetCoreCSharpCapability),
            CreateVsFeatureFlagsService(legacyEditorFeatureFlag),
            CreateVsSettingsManagerService(legacyEditorSetting),
            uiContextService,
            JoinableTaskContext,
            CreateRazorActivityLog(),
            CreateVSUIShellOpenDocument());

        AddDisposable(featureDetector);

        return featureDetector;
    }

    private static IAggregateProjectCapabilityResolver CreateAggregateProjectCapabilityResolver(bool hasLegacyRazorEditorCapability, bool hasDotNetCoreCSharpCapability)
    {
        var aggregateProjectCapabilityResolverMock = new StrictMock<IAggregateProjectCapabilityResolver>();
        aggregateProjectCapabilityResolverMock
            .Setup(x => x.HasCapability(It.IsAny<string>(), It.IsAny<object>(), LspEditorFeatureDetector.LegacyRazorEditorCapability))
            .Returns(hasLegacyRazorEditorCapability);
        aggregateProjectCapabilityResolverMock
            .Setup(x => x.HasCapability(It.IsAny<string>(), It.IsAny<object>(), LspEditorFeatureDetector.DotNetCoreCSharpCapability))
            .Returns(hasDotNetCoreCSharpCapability);

        return aggregateProjectCapabilityResolverMock.Object;
    }

    private static Lazy<IVsUIShellOpenDocument> CreateVSUIShellOpenDocument()
    {
        var vsUIShellOpenDocumentMock = new StrictMock<IVsUIShellOpenDocument>();
        var hierarchy = new StrictMock<IVsUIHierarchy>().Object;
        vsUIShellOpenDocumentMock
            .Setup(x => x.IsDocumentInAProject(It.IsAny<string>(), out hierarchy, out It.Ref<uint>.IsAny, out It.Ref<OLE.Interop.IServiceProvider>.IsAny, out It.Ref<int>.IsAny))
            .Returns(VSConstants.S_OK);

        return new Lazy<IVsUIShellOpenDocument>(() => vsUIShellOpenDocumentMock.Object);
    }

    private static IVsService<SVsFeatureFlags, IVsFeatureFlags> CreateVsFeatureFlagsService(bool useLegacyEditor)
    {
        var vsFeatureFlagsMock = new StrictMock<IVsFeatureFlags>();
        vsFeatureFlagsMock
            .Setup(x => x.IsFeatureEnabled(WellKnownFeatureFlagNames.UseLegacyRazorEditor, It.IsAny<bool>()))
            .Returns(useLegacyEditor);
        return VsMocks.CreateVsService<SVsFeatureFlags, IVsFeatureFlags>(vsFeatureFlagsMock);
    }

    private static IVsService<SVsSettingsPersistenceManager, ISettingsManager> CreateVsSettingsManagerService(bool useLegacyEditor)
    {
        var vsSettingsManagerMock = new StrictMock<ISettingsManager>();
        vsSettingsManagerMock
            .Setup(x => x.GetValueOrDefault(WellKnownSettingNames.UseLegacyASPNETCoreEditor, It.IsAny<bool>()))
            .Returns(useLegacyEditor);

        return VsMocks.CreateVsService<SVsSettingsPersistenceManager, ISettingsManager>(vsSettingsManagerMock);
    }

    private static IUIContextService CreateUIContextService(
        bool liveShareHostActive = false,
        bool liveShareGuestActive = false,
        bool cloudEnvironmentConnectedActive = false)
    {
        var mock = new StrictMock<IUIContextService>();

        mock.Setup(x => x.IsActive(Guids.LiveShareHostUIContextGuid))
            .Returns(liveShareHostActive);

        mock.Setup(x => x.IsActive(Guids.LiveShareGuestUIContextGuid))
            .Returns(liveShareGuestActive);

        mock.Setup(x => x.IsActive(VSConstants.UICONTEXT.CloudEnvironmentConnected_guid))
            .Returns(cloudEnvironmentConnectedActive);

        return mock.Object;
    }
    private RazorActivityLog CreateRazorActivityLog()
    {
        var vsActivityLogMock = new StrictMock<IVsActivityLog>();
        vsActivityLogMock
            .Setup(x => x.LogEntry(It.IsAny<uint>(), "Razor", It.IsAny<string>()))
            .Callback((uint entryType, string source, string description) =>
            {
                switch ((__ACTIVITYLOG_ENTRYTYPE)entryType)
                {
                    case __ACTIVITYLOG_ENTRYTYPE.ALE_ERROR:
                        Logger.LogError($"Error:{description}");
                        break;

                    case __ACTIVITYLOG_ENTRYTYPE.ALE_WARNING:
                        Logger.LogError($"Warning:{description}");
                        break;

                    case __ACTIVITYLOG_ENTRYTYPE.ALE_INFORMATION:
                        Logger.LogError($"Info:{description}");
                        break;

                    default:
                        Assumed.Unreachable();
                        break;
                }
            })
            .Returns(VSConstants.S_OK);

        var serviceProviderMock = new StrictMock<IAsyncServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetServiceAsync(typeof(SVsActivityLog)))
            .ReturnsAsync(vsActivityLogMock.Object);

        var activityLog = new RazorActivityLog(serviceProviderMock.Object, JoinableTaskContext);

        AddDisposable(activityLog);

        return activityLog;
    }
}
