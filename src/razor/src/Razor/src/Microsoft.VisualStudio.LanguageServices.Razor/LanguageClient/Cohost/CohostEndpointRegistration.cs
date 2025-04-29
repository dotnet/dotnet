// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

/// <summary>
/// This class provides dynamic registration for Razor files, for LSP methods where the endpoint implementation
/// is provided by Roslyn
/// </summary>
#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[Export(typeof(IDynamicRegistrationProvider))]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostEndpointRegistration : IDynamicRegistrationProvider
{
    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        return [
            // Project Context, for the nav bar
            new Registration
            {
                Method = VSMethods.GetProjectContextsName,
                RegisterOptions = new TextDocumentRegistrationOptions()
            },
            // DidOpen, DidChange, DidClose, for document synchronization
            new Registration
            {
                Method = Methods.TextDocumentDidOpenName,
                RegisterOptions = new TextDocumentRegistrationOptions()
            },
            new Registration
            {
                Method = Methods.TextDocumentDidChangeName,
                RegisterOptions = new TextDocumentChangeRegistrationOptions()
                {
                    SyncKind = TextDocumentSyncKind.Incremental
                }
            },
            new Registration
            {
                Method = Methods.TextDocumentDidCloseName,
                RegisterOptions = new TextDocumentRegistrationOptions()
            },
        ];
    }
}
