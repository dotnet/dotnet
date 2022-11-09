// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;

namespace Microsoft.DotNet.CodeAnalysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PinvokeAnalyzer : BaseAnalyzer
    {
        private const string Title = "Invalid Pinvoke call";
        private const string MessageFormat = @"{0} is not supported on one\more targeted platforms.{1}";
        private const string Description = "The Pinvoke call should be removed.";
        private const string AnalyzerName = "PinvokeAnalyzer";

        private static DiagnosticDescriptor InvalidPinvokeCall = new DiagnosticDescriptor(DiagnosticIds.BCL0015.ToString(), Title, MessageFormat, AnalyzerName, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(InvalidPinvokeCall); } }

        public override void OnCompilationStart(CompilationStartAnalysisContext obj)
        {
            _allowedPinvokeFile = obj.Options.AdditionalFiles.FirstOrDefault(f => Path.GetFileName(f.Path).IndexOf("PinvokeAnalyzer_", StringComparison.OrdinalIgnoreCase) >= 0);
            _exceptionFile = obj.Options.AdditionalFiles.FirstOrDefault(f => Path.GetFileName(f.Path).IndexOf("PinvokeAnalyzerExceptionList.analyzerdata", StringComparison.OrdinalIgnoreCase) >= 0);
            obj.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
        }

        private Dictionary<string, string> _allowedPinvokes;
        private Dictionary<string, string> _exceptionPinvokes;

        private Dictionary<string, string> AllowedPinvokes
        {
            get
            {
                if (_allowedPinvokes == null)
                {
                    _allowedPinvokes = ParseAdditionalFile(_allowedPinvokeFile);
                }

                return _allowedPinvokes;
            }
        }

        private Dictionary<string, string> ExceptionPinvokes
        {
            get
            {
                if (_exceptionPinvokes == null)
                {
                    _exceptionPinvokes = ParseAdditionalFile(_exceptionFile);
                }

                return _exceptionPinvokes;
            }
        }

        private AdditionalText _exceptionFile;
        private AdditionalText _allowedPinvokeFile;

        private HashSet<string> _isNotSupportedOnWin7 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);


        private Dictionary<string, string> ParseAdditionalFile(AdditionalText additionalFile)
        {
            Dictionary<string, string> parsedPinvokes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (additionalFile == null) return parsedPinvokes;

            SourceText fileContents = additionalFile.GetText();
            foreach (TextLine line in fileContents.Lines)
            {
                string lineStr = line.ToString();
                if (!string.IsNullOrWhiteSpace(lineStr) && !lineStr.StartsWith("<!--"))
                {
                    string[] splitCount = lineStr.Split('!');

                    if (splitCount.Length == 2 || splitCount.Length == 3)
                    {
                        parsedPinvokes[splitCount[1]] = splitCount[0];
                        if (splitCount.Length == 3)
                        {
                            _isNotSupportedOnWin7.Add(splitCount[1]);
                        }
                    }
                }
            }

            return parsedPinvokes;
        }

        private const string AltMsgString = @"Consider using {0} instead.";
        private const string NotSupportedOnWin7 = @"{0} is not supported on win7";


        // This method looks at the additional lists passed to ensure if the method call is valid.
        // This is the algorithm used by the method
        // 1. a. Check whether the moduleName!methodName is present as-is in the allowed list.
        //    b. If present validate whether it is also present on win7.
        // 2. Check if the moduleName!methodName is present in exception list.
        // 3. If not, check whether the methodName is present in another module than what is referenced.
        //    if so, make sure the error msg gives this as a suggestion to the user.
        private bool CheckIfMemberPresent(string methodName, string moduleName, ref string altMsg)
        {
            if (AllowedPinvokes.ContainsKey(methodName))
            {
                if (AllowedPinvokes[methodName].Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                {
                    if (_isNotSupportedOnWin7.Contains(methodName))
                    {
                        altMsg = String.Format(NotSupportedOnWin7, AllowedPinvokes[methodName] + "!" + methodName);
                        return false;
                    }

                    return true;
                }
                else
                {
                    if (String.IsNullOrEmpty(altMsg))
                        altMsg = String.Format(AltMsgString, AllowedPinvokes[methodName] + "!" + methodName);
                }
            }

            return (ExceptionPinvokes.ContainsKey(methodName) && ExceptionPinvokes[methodName].Equals(moduleName, StringComparison.OrdinalIgnoreCase));
        }

        private void AnalyzeMethod(SymbolAnalysisContext context)
        {
            var methodSymbol = context.Symbol as IMethodSymbol;
            if (methodSymbol == null) return;

            DllImportData data = methodSymbol.GetDllImportData();
            if (data == null) return;

            // Ignore QCall
            if (data.ModuleName.Equals("QCall", StringComparison.OrdinalIgnoreCase)) return;

            bool isPresent = false;
            string altMsg = string.Empty;

            // 1. If the method has an explicit entry point defined we validate if the moduleName!entryPoint is a valid combination.
            // 2. If not, we check whether the moduleName!methodName is valid or moduleName!methodNameW (we only support Unicode) is valid.
            if (data.EntryPointName != null)
            {
                isPresent = CheckIfMemberPresent(data.EntryPointName, data.ModuleName, ref altMsg);
            }
            else
            {
                isPresent = CheckIfMemberPresent(methodSymbol.Name, data.ModuleName, ref altMsg)
                    || CheckIfMemberPresent(methodSymbol.Name + 'W', data.ModuleName, ref altMsg);
            }

            if (!isPresent)
            {
                foreach (SyntaxReference synref in methodSymbol.DeclaringSyntaxReferences)
                {
                    context.ReportDiagnostic(Diagnostic.Create(InvalidPinvokeCall, synref.GetSyntax().GetLocation(), data.ModuleName + "!" + (data.EntryPointName ?? methodSymbol.Name), altMsg));
                }
            }
        }
    }
}
