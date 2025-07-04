// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;

namespace Microsoft.CodeAnalysis.Razor.Cohost;

internal abstract class AbstractCohostDocumentEndpoint<TRequest, TResponse>(
    IIncompatibleProjectService incompatibleProjectService) : AbstractRazorCohostDocumentRequestHandler<TRequest, TResponse?>
{
    private readonly IIncompatibleProjectService _incompatibleProjectService = incompatibleProjectService;

    protected sealed override Task<TResponse?> HandleRequestAsync(TRequest request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        if (context.TextDocument is null)
        {
            _incompatibleProjectService.HandleMissingDocument(GetRazorTextDocumentIdentifier(request), context);

            return SpecializedTasks.Default<TResponse>();
        }

        if (context.TextDocument.Project.FilePath is null)
        {
            // If the project file path is null, we can't compute the hint name, so we can't handle the request.
            // This is likely a file in the misc files project, which we don't support yet anyway.
            // TODO: Expose context.TextDocument.Project.Solution.WorkspaceKind through our EA to confirm?
            _incompatibleProjectService.HandleMiscFilesDocument(context.TextDocument);
            return SpecializedTasks.Default<TResponse>();
        }

        return HandleRequestAsync(request, context, context.TextDocument, cancellationToken);
    }

    protected virtual Task<TResponse?> HandleRequestAsync(TRequest request, RazorCohostRequestContext context, TextDocument razorDocument, CancellationToken cancellationToken)
        => HandleRequestAsync(request, razorDocument, cancellationToken);

    protected abstract Task<TResponse?> HandleRequestAsync(TRequest request, TextDocument razorDocument, CancellationToken cancellationToken);
}
