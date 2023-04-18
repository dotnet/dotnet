// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.Tracing;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    [EventSource(Name = "Microsoft-DotNet-SDK-Razor-SourceGenerator")]
    internal sealed class RazorSourceGeneratorEventSource : EventSource
    {
        public static readonly RazorSourceGeneratorEventSource Log = new();

        private RazorSourceGeneratorEventSource() { }

        private const int ComputeRazorSourceGeneratorOptionsId = 1;
        [Event(ComputeRazorSourceGeneratorOptionsId, Level = EventLevel.Informational)]
        public void ComputeRazorSourceGeneratorOptions() => WriteEvent(ComputeRazorSourceGeneratorOptionsId);

        private const int GenerateDeclarationCodeStartId = 2;
        [Event(GenerateDeclarationCodeStartId, Level = EventLevel.Informational)]
        public void GenerateDeclarationCodeStart(string filePath) => WriteEvent(GenerateDeclarationCodeStartId, filePath);

        private const int GenerateDeclarationCodeStopId = 4;
        [Event(GenerateDeclarationCodeStopId, Level = EventLevel.Informational)]
        public void GenerateDeclarationCodeStop(string filePath) => WriteEvent(GenerateDeclarationCodeStopId, filePath);

        private const int DiscoverTagHelpersFromCompilationStartId = 6;
        [Event(DiscoverTagHelpersFromCompilationStartId, Level = EventLevel.Informational)]
        public void DiscoverTagHelpersFromCompilationStart() => WriteEvent(DiscoverTagHelpersFromCompilationStartId);

        private const int DiscoverTagHelpersFromCompilationStopId = 7;
        [Event(DiscoverTagHelpersFromCompilationStopId, Level = EventLevel.Informational)]
        public void DiscoverTagHelpersFromCompilationStop() => WriteEvent(DiscoverTagHelpersFromCompilationStopId);

        private const int DiscoverTagHelpersFromReferencesStartId = 8;
        [Event(DiscoverTagHelpersFromReferencesStartId, Level = EventLevel.Informational)]
        public void DiscoverTagHelpersFromReferencesStart() => WriteEvent(DiscoverTagHelpersFromReferencesStartId);

        private const int DiscoverTagHelpersFromReferencesStopId = 9;
        [Event(DiscoverTagHelpersFromReferencesStopId, Level = EventLevel.Informational)]
        public void DiscoverTagHelpersFromReferencesStop() => WriteEvent(DiscoverTagHelpersFromReferencesStopId);

        private const int RazorCodeGenerateStartId = 10;
        [Event(RazorCodeGenerateStartId, Level = EventLevel.Informational)]
        public void RazorCodeGenerateStart(string file) => WriteEvent(RazorCodeGenerateStartId, file);

        private const int RazorCodeGenerateStopId = 11;
        [Event(RazorCodeGenerateStopId, Level = EventLevel.Informational)]
        public void RazorCodeGenerateStop(string file) => WriteEvent(RazorCodeGenerateStopId, file);

        private const int AddSyntaxTreesId = 12;
        [Event(AddSyntaxTreesId, Level = EventLevel.Informational)]
        public void AddSyntaxTrees(string file) => WriteEvent(AddSyntaxTreesId, file);

        private const int GenerateDeclarationSyntaxTreeStartId = 13;
        [Event(GenerateDeclarationSyntaxTreeStartId, Level = EventLevel.Informational)]
        public void GenerateDeclarationSyntaxTreeStart() => WriteEvent(GenerateDeclarationSyntaxTreeStartId);

        private const int GenerateDeclarationSyntaxTreeStopId = 14;
        [Event(GenerateDeclarationSyntaxTreeStopId, Level = EventLevel.Informational)]
        public void GenerateDeclarationSyntaxTreeStop() => WriteEvent(GenerateDeclarationSyntaxTreeStopId);

        private const int DiscoverTagHelpersFromComponentStartId = 15;
        [Event(DiscoverTagHelpersFromComponentStartId, Level = EventLevel.Informational)]
        public void DiscoverTagHelpersFromComponentStart(string filePath) => WriteEvent(DiscoverTagHelpersFromComponentStartId, filePath);

        private const int DiscoverTagHelpersFromComponentStopId = 16;
        [Event(DiscoverTagHelpersFromComponentStopId, Level = EventLevel.Informational)]
        public void DiscoverTagHelpersFromComponentStop(string filePath) => WriteEvent(DiscoverTagHelpersFromComponentStopId, filePath);

        private const int ParseRazorDocumentStartId = 17;
        [Event(ParseRazorDocumentStartId, Level = EventLevel.Informational)]
        public void ParseRazorDocumentStart(string file) => WriteEvent(ParseRazorDocumentStartId, file);

        private const int ParseRazorDocumentStopId = 18;
        [Event(ParseRazorDocumentStopId, Level = EventLevel.Informational)]
        public void ParseRazorDocumentStop(string file) => WriteEvent(ParseRazorDocumentStopId, file);

        private const int RewriteTagHelpersStartId = 19;
        [Event(RewriteTagHelpersStartId, Level = EventLevel.Informational)]
        public void RewriteTagHelpersStart(string file) => WriteEvent(RewriteTagHelpersStartId, file);

        private const int RewriteTagHelpersStopId = 20;
        [Event(RewriteTagHelpersStopId, Level = EventLevel.Informational)]
        public void RewriteTagHelpersStop(string file) => WriteEvent(RewriteTagHelpersStopId, file);

        private const int CheckAndRewriteTagHelpersStartId = 21;
        [Event(CheckAndRewriteTagHelpersStartId, Level = EventLevel.Informational)]
        public void CheckAndRewriteTagHelpersStart(string file) => WriteEvent(CheckAndRewriteTagHelpersStartId, file);

        private const int CheckAndRewriteTagHelpersStopId = 22;
        [Event(CheckAndRewriteTagHelpersStopId, Level = EventLevel.Informational)]
        public void CheckAndRewriteTagHelpersStop(string file) => WriteEvent(CheckAndRewriteTagHelpersStopId, file);
    }
}
