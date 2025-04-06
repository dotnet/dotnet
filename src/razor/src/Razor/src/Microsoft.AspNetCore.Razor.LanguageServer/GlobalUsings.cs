// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

// This file is shared, but not all of the usings are needed for all files, so Roslyn seems keen to flag them as unused in this file
#pragma warning disable IDE0005 // Using directive is unnecessary.

// The <Using> item doesn't support aliases so we need to define aliased global usings in a .cs file not in the .csproj
// https://github.com/dotnet/sdk/issues/37814
extern alias RLSP;
global using RLSP::Roslyn.LanguageServer.Protocol;

// Avoid extern alias in every file that needs to disambiguate common LSP type names
global using LspColorPresentation = RLSP::Roslyn.LanguageServer.Protocol.ColorPresentation;
global using LspDiagnostic = RLSP::Roslyn.LanguageServer.Protocol.Diagnostic;
global using LspDiagnosticSeverity = RLSP::Roslyn.LanguageServer.Protocol.DiagnosticSeverity;
global using LspDocumentHighlight = RLSP::Roslyn.LanguageServer.Protocol.DocumentHighlight;
global using LspHover = RLSP::Roslyn.LanguageServer.Protocol.Hover;
global using LspLocation = RLSP::Roslyn.LanguageServer.Protocol.Location;
global using LspRange = RLSP::Roslyn.LanguageServer.Protocol.Range;
global using LspSignatureHelp = RLSP::Roslyn.LanguageServer.Protocol.SignatureHelp;

// Avoid ambiguity errors because of our global using above
global using Range = System.Range;

// We put our extensions on Roslyn's LSP types in the same namespace, for convenience, but of course without the alias,
// so to prevent confusion at not needing a using directive to access types, but needing one for extensions, we just
// global using the our extensions (which of course means they didn't need to be in the same namespace for convenience!)
global using Roslyn.LanguageServer.Protocol;
