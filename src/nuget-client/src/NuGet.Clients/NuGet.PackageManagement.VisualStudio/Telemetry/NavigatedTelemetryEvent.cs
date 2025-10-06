// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;
using ContractsItemFilter = NuGet.VisualStudio.Internal.Contracts.ItemFilter;

namespace NuGet.PackageManagement.Telemetry
{
    public class NavigatedTelemetryEvent : TelemetryEvent
    {
        internal const string NavigatedEventName = "Navigated";

        internal const string NavigationTypePropertyName = "NavigationType";
        internal const string OriginPropertyName = "Origin";
        internal const string HyperLinkTypePropertyName = "HyperlinkType";

        internal const string CurrentTabPropertyName = "CurrentTab";
        internal const string IsSolutionViewPropertyName = "IsSolutionView";

        internal const string PackageSourceMappingStatusPropertyName = "PackageSourceMappingStatus";

        internal const string SourcesCountPropertyName = "SourcesCount";
        internal const string IsGlobbingPropertyName = "IsGlobbing";
        internal const string IsUnifiedSettingsPropertyName = "IsUnifiedSettings";
        internal const string IsPromptCancelledPropertyName = "IsPromptCancelled";

        internal const string AlternativePackageIdPropertyName = "AlternativePackageId";

        /// <summary>
        /// General Navigation event with an origin specified.
        /// </summary>
        /// <param name="navigationType">Originating navigation type</param>
        /// <param name="navigationOrigin">Control which started this navigation</param>
        public NavigatedTelemetryEvent(NavigationType navigationType, NavigationOrigin navigationOrigin)
            : base(NavigatedEventName)
        {
            base[NavigationTypePropertyName] = navigationType;
            base[OriginPropertyName] = navigationOrigin;
        }

        /// <summary>
        /// When adding PackageSourceMapping, includes additional data about the new mapping.
        /// </summary>
        public static NavigatedTelemetryEvent CreateWithAddPackageSourceMapping(int sourcesCount, bool isGlobbing)
        {
            NavigationType navigationType = NavigationType.Button;
            NavigationOrigin navigationOrigin = NavigationOrigin.Options_PackageSourceMapping_Add;

            NavigatedTelemetryEvent navigatedTelemetryEvent = new(navigationType, navigationOrigin);
            navigatedTelemetryEvent[SourcesCountPropertyName] = sourcesCount;
            navigatedTelemetryEvent[IsGlobbingPropertyName] = isGlobbing;

            return navigatedTelemetryEvent;
        }

        /// <summary>
        /// Navigating to the Package Source Mapping VS Options dialog from the PM UI.
        /// </summary>
        /// <param name="currentTab">Active tab in the PM UI</param>
        /// <param name="isSolutionView">Whether the PM UI was in Solution (or Project) mode</param>
        /// <param name="packageSourceMappingStatus">Package Source Mapping status</param>
        public static NavigatedTelemetryEvent CreateWithPMUIConfigurePackageSourceMapping(
            ContractsItemFilter currentTab,
            bool isSolutionView,
            PackageSourceMappingStatus packageSourceMappingStatus)
        {
            NavigatedTelemetryEvent navigatedTelemetryEvent = new(NavigationType.Button, NavigationOrigin.PMUI_PackageSourceMapping_Configure);
            navigatedTelemetryEvent[CurrentTabPropertyName] = currentTab;
            navigatedTelemetryEvent[IsSolutionViewPropertyName] = isSolutionView;
            navigatedTelemetryEvent[PackageSourceMappingStatusPropertyName] = packageSourceMappingStatus;

            return navigatedTelemetryEvent;
        }

        /// <summary>
        /// Navigating an External hyperlink from the PM UI.
        /// </summary>
        /// <param name="hyperlinkType">Hyperlink origin</param>
        /// <param name="currentTab">Active tab in the PM UI</param>
        /// <param name="isSolutionView">Whether the PM UI was in Solution (or Project) mode</param>
        /// <returns></returns>
        public static NavigatedTelemetryEvent CreateWithExternalLink(
            HyperlinkType hyperlinkType,
            ContractsItemFilter currentTab,
            bool isSolutionView)
        {
            NavigatedTelemetryEvent navigatedTelemetryEvent = new(NavigationType.Hyperlink, NavigationOrigin.PMUI_ExternalLink);

            navigatedTelemetryEvent[HyperLinkTypePropertyName] = hyperlinkType;
            navigatedTelemetryEvent[CurrentTabPropertyName] = currentTab;
            navigatedTelemetryEvent[IsSolutionViewPropertyName] = isSolutionView;

            return navigatedTelemetryEvent;
        }

        /// <summary>
        /// Navigating from an Alternate Package hyperlink carries the Package ID which is PII.
        /// </summary>
        /// <param name="hyperlinkType">Hyperlink origin</param>
        /// <param name="currentTab">Active tab in the PM UI</param>
        /// <param name="isSolutionView">Whether the PM UI was in Solution (or Project) mode</param>
        /// <param name="alternativePackageId">Pii data. The alternate package ID selected.</param>
        /// <returns></returns>
        public static NavigatedTelemetryEvent CreateWithAlternatePackageNavigation(
            HyperlinkType hyperlinkType,
            ContractsItemFilter currentTab,
            bool isSolutionView,
            string alternativePackageId)
        {
            NavigatedTelemetryEvent navigatedTelemetryEvent = CreateWithExternalLink(hyperlinkType, currentTab, isSolutionView);
            navigatedTelemetryEvent.AddPiiData(AlternativePackageIdPropertyName, VSTelemetryServiceUtility.NormalizePackageId(alternativePackageId));

            return navigatedTelemetryEvent;
        }

        public static NavigatedTelemetryEvent CreateWithClearLocalsCommand(bool isUnifiedSettings, bool? isPromptCancelled = null)
        {
            NavigationType navigationType = NavigationType.Button;
            NavigationOrigin navigationOrigin = NavigationOrigin.Options_LocalsCommand_ClearAll;

            NavigatedTelemetryEvent navigatedTelemetryEvent = new(navigationType, navigationOrigin);
            navigatedTelemetryEvent[IsUnifiedSettingsPropertyName] = isUnifiedSettings;
            if (isUnifiedSettings && isPromptCancelled.HasValue)
            {
                navigatedTelemetryEvent[IsPromptCancelledPropertyName] = isPromptCancelled;
            }
            return navigatedTelemetryEvent;
        }
    }
}
