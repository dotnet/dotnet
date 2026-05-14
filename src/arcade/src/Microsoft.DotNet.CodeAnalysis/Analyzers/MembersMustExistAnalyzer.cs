// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.DotNet.CodeAnalysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MembersMustExistAnalyzer : BaseAnalyzer
    {
        private static string s_title = @"Ensure minimum API surface is respected";
        private static string s_analyzerName = "MembersMustExist";
        private static string s_messageFormat = @"Expected member '{0}' was not found. This member was identified as being a part of a contract between the .NET team and their partners. The '" + s_analyzerName + ".analyzerData' file will contain more information about the team that required this API.";
        private static string s_description = @"Ensures all the APIs in the '" + s_analyzerName + "' file are present.";
        private const string CommentDelimiter = "#";

        private HashSet<string> _apisToEnsureExist = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private static readonly DiagnosticDescriptor s_memberMustExistDiagnostic =
            new DiagnosticDescriptor(DiagnosticIds.BCL0001.ToString(), s_title, s_messageFormat, s_analyzerName, DiagnosticSeverity.Error, isEnabledByDefault: true, description: s_description, customTags: WellKnownDiagnosticTags.CompilationEnd);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(s_memberMustExistDiagnostic); } }

        private void OnCompilationEnd(CompilationAnalysisContext context)
        {
            lock (_apisToEnsureExist)
            {
                if (_apisToEnsureExist.Count != 0)
                {
                    // If we have not cleared the list of APIs that must exist then we need to give errors about them
                    foreach (var missingAPI in _apisToEnsureExist)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(s_memberMustExistDiagnostic, Location.None, missingAPI));
                    }
                }
            }
        }

        public override void OnCompilationStart(CompilationStartAnalysisContext context)
        {
            // Read the file line-by-line to get the terms.
            var additionalAnalyzerFiles = context.Options.AdditionalFiles.Where(af => af.Path.IndexOf(s_analyzerName, 0, StringComparison.OrdinalIgnoreCase) >= 0);

            if (!additionalAnalyzerFiles.Any())
                return;

            lock (_apisToEnsureExist)
            {
                foreach (string api in ReadRequiredAPIsFromFiles(additionalAnalyzerFiles))
                {
                    _apisToEnsureExist.Add(api);
                }
            }

            context.RegisterCompilationEndAction(OnCompilationEnd);
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method, SymbolKind.Event);
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field, SymbolKind.Event);
        }

        private static IEnumerable<string> ReadRequiredAPIsFromFiles(IEnumerable<AdditionalText> additionalAnalyzerFiles)
        {
            // We might have multiple files passed in. We need to make sure we read all of them
            foreach (var additionalFile in additionalAnalyzerFiles)
            {
                SourceText fileContents = additionalFile.GetText();
                foreach (TextLine line in fileContents.Lines)
                {
                    string lineStr = line.ToString();
                    if (!string.IsNullOrWhiteSpace(lineStr) && !lineStr.StartsWith(CommentDelimiter))
                    {
                        yield return lineStr;
                    }
                }
            }
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            string apiDef = Helpers.GetMemberName(context.Symbol);

            lock (_apisToEnsureExist)
            {
                // The only thing we need to do when we identify a symbol is to remove that signature from the list of APIs we are looking for
                _apisToEnsureExist.Remove(apiDef);
            }
        }
    }
}
