// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal static class RazorServices
{
    private const string ComponentName = "Razor";

    public static readonly RazorServiceDescriptorsWrapper Descriptors = new(
        ComponentName,
        featureDisplayNameProvider: feature => $"Razor {feature} Feature",
        additionalFormatters: [],
        additionalResolvers: TopLevelResolvers.All,
        interfaces:
        [
            (typeof(IRemoteLinkedEditingRangeService), null),
            (typeof(IRemoteTagHelperProviderService), null),
            (typeof(IRemoteClientInitializationService), null),
            (typeof(IRemoteSemanticTokensService), null),
            (typeof(IRemoteHtmlDocumentService), null),
            (typeof(IRemoteUriPresentationService), null),
            (typeof(IRemoteFoldingRangeService), null)
        ]);

    public static readonly RazorServiceDescriptorsWrapper JsonDescriptors = new(
        ComponentName, // Needs to match the above because so much of our ServiceHub infrastructure is convention based
        featureDisplayNameProvider: feature => $"Razor {feature} Feature",
        jsonConverters: RazorServiceDescriptorsWrapper.GetLspConverters(),
        interfaces:
        [
            (typeof(IRemoteSignatureHelpService), null),
        ]);
}
