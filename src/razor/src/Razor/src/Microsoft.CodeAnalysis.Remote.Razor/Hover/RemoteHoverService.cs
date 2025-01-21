// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Hover;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Roslyn.LanguageServer.Protocol;
using static Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Roslyn.LanguageServer.Protocol.Hover?>;
using static Microsoft.VisualStudio.LanguageServer.Protocol.ClientCapabilitiesExtensions;
using static Microsoft.VisualStudio.LanguageServer.Protocol.VsLspExtensions;
using static Roslyn.LanguageServer.Protocol.RoslynLspExtensions;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Range = Roslyn.LanguageServer.Protocol.Range;
using VsLsp = Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteHoverService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteHoverService
{
    internal sealed class Factory : FactoryBase<IRemoteHoverService>
    {
        protected override IRemoteHoverService CreateService(in ServiceArgs args)
            => new RemoteHoverService(in args);
    }

    private readonly IClientCapabilitiesService _clientCapabilitiesService = args.ExportProvider.GetExportedValue<IClientCapabilitiesService>();

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    public ValueTask<RemoteResponse<Hover?>> GetHoverAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        Position position,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetHoverAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<RemoteResponse<Hover?>> GetHoverAsync(
        RemoteDocumentContext context,
        Position position,
        CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (!codeDocument.Source.Text.TryGetAbsoluteIndex(position, out var hostDocumentIndex))
        {
            return NoFurtherHandling;
        }

        var clientCapabilities = _clientCapabilitiesService.ClientCapabilities;
        var positionInfo = GetPositionInfo(codeDocument, hostDocumentIndex, preferCSharpOverHtml: true);

        if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            var generatedDocument = await context.Snapshot
                .GetGeneratedDocumentAsync(cancellationToken)
                .ConfigureAwait(false);

            var csharpHover = await ExternalHandlers.Hover
                .GetHoverAsync(
                    generatedDocument,
                    positionInfo.Position.ToLinePosition(),
                    clientCapabilities.SupportsVisualStudioExtensions(),
                    clientCapabilities.SupportsMarkdown(),
                    cancellationToken)
                .ConfigureAwait(false);

            // Roslyn couldn't provide a hover, so we're done.
            if (csharpHover is null)
            {
                return NoFurtherHandling;
            }

            // Map the hover range back to the host document
            if (csharpHover.Range is { } range &&
                DocumentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), range.ToLinePositionSpan(), out var hostDocumentSpan))
            {
                csharpHover.Range = RoslynLspFactory.CreateRange(hostDocumentSpan);
            }

            return Results(csharpHover);
        }

        if (positionInfo.LanguageKind is not (RazorLanguageKind.Html or RazorLanguageKind.Razor))
        {
            Debug.Fail($"Encountered an unexpected {nameof(RazorLanguageKind)}: {positionInfo.LanguageKind}");
            return NoFurtherHandling;
        }

        // If this is Html or Razor, try to retrieve a hover from Razor.
        var options = HoverDisplayOptions.From(clientCapabilities);

        // In co-hosting, there isn't a singleton IComponentAvailabilityService in the MEF composition.
        // So, we construct one using the current solution snapshot.
        // All of this will change when solution snapshots are available in the core Razor project model.

        // TODO: Remove this when solution snapshots are available in the core Razor project model.
        var componentAvailabilityService = new ComponentAvailabilityService(context.Snapshot.ProjectSnapshot.SolutionSnapshot);

        var razorHover = await HoverFactory
            .GetHoverAsync(codeDocument, hostDocumentIndex, options, componentAvailabilityService, cancellationToken)
            .ConfigureAwait(false);

        // Roslyn couldn't provide a hover, so we're done.
        if (razorHover is null)
        {
            return CallHtml;
        }

        // Ensure that we convert our Hover to a Roslyn Hover.
        var resultHover = ConvertHover(razorHover);

        return Results(resultHover);
    }

    /// <summary>
    ///  Converts a <see cref="VsLsp.Hover"/> to a <see cref="Hover"/>.
    /// </summary>
    /// <remarks>
    ///  Once Razor moves wholly over to Roslyn.LanguageServer.Protocol, this method can be removed.
    /// </remarks>
    private Hover ConvertHover(VsLsp.Hover hover)
    {
        // Note: Razor only ever produces a Hover with MarkupContent or a VSInternalHover with RawContents.
        // Both variants return a Range.

        return hover switch
        {
            VsLsp.VSInternalHover { Range: var range, RawContent: { } rawContent } => new VSInternalHover()
            {
                Range = ConvertRange(range),
                Contents = string.Empty,
                RawContent = ConvertVsContent(rawContent)
            },
            VsLsp.Hover { Range: var range, Contents.Fourth: VsLsp.MarkupContent contents } => new Hover()
            {
                Range = ConvertRange(range),
                Contents = ConvertMarkupContent(contents)
            },
            _ => Assumed.Unreachable<Hover>(),
        };

        static Range? ConvertRange(VsLsp.Range? range)
        {
            return range is not null
                ? RoslynLspFactory.CreateRange(range.ToLinePositionSpan())
                : null;
        }

        static object ConvertVsContent(object obj)
        {
            return obj switch
            {
                VisualStudio.Core.Imaging.ImageId imageId => ConvertImageId(imageId),
                VisualStudio.Text.Adornments.ImageElement element => ConvertImageElement(element),
                VisualStudio.Text.Adornments.ClassifiedTextRun run => ConvertClassifiedTextRun(run),
                VisualStudio.Text.Adornments.ClassifiedTextElement element => ConvertClassifiedTextElement(element),
                VisualStudio.Text.Adornments.ContainerElement element => ConvertContainerElement(element),
                _ => Assumed.Unreachable<object>()
            };

            static Roslyn.Core.Imaging.ImageId ConvertImageId(VisualStudio.Core.Imaging.ImageId imageId)
            {
                return new(imageId.Guid, imageId.Id);
            }

            static Roslyn.Text.Adornments.ImageElement ConvertImageElement(VisualStudio.Text.Adornments.ImageElement element)
            {
                return new(ConvertImageId(element.ImageId), element.AutomationName);
            }

            static Roslyn.Text.Adornments.ClassifiedTextRun ConvertClassifiedTextRun(VisualStudio.Text.Adornments.ClassifiedTextRun run)
            {
                return new(
                    run.ClassificationTypeName,
                    run.Text,
                    (Roslyn.Text.Adornments.ClassifiedTextRunStyle)run.Style,
                    run.MarkerTagType,
                    run.NavigationAction,
                    run.Tooltip);
            }

            static Roslyn.Text.Adornments.ClassifiedTextElement ConvertClassifiedTextElement(VisualStudio.Text.Adornments.ClassifiedTextElement element)
            {
                return new(element.Runs.Select(ConvertClassifiedTextRun));
            }

            static Roslyn.Text.Adornments.ContainerElement ConvertContainerElement(VisualStudio.Text.Adornments.ContainerElement element)
            {
                return new((Roslyn.Text.Adornments.ContainerElementStyle)element.Style, element.Elements.Select(ConvertVsContent));
            }
        }

        static MarkupContent ConvertMarkupContent(VsLsp.MarkupContent value)
        {
            return new()
            {
                Kind = ConvertMarkupKind(value.Kind),
                Value = value.Value
            };
        }

        static MarkupKind ConvertMarkupKind(VsLsp.MarkupKind value)
        {
            return value == VsLsp.MarkupKind.Markdown
                ? MarkupKind.Markdown
                : MarkupKind.PlainText;
        }
    }
}
