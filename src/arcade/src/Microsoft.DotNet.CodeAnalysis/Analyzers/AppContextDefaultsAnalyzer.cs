// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.DotNet.CodeAnalysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AppContextDefaultsAnalyzer : BaseAnalyzer
    {
        private const string HowToDisableWarning = "If this is intentional consider using '#pragma warning disable {0}' to suppress the warning.";

        private static string s_title = @"Ensure AppContext defaults are correctly setup";
        private static string s_analyzerName = "AppContextDefaults";
        private static string s_description = @"Ensures AppContext default values are correctly setup.";
        private static string s_defaultValueNotInitializedToTrue = @"AppContext default value expected to be 'true' in the call: '{0}'.";
        private static string s_defaultValueInsideUnexpectedIfCondition = @"AppContext default value is defined inside an if statement that does not use the '<=' pattern.";
        private static string s_defaultValueDefinedOutsideIfCondition = @"AppContext default value should be defined inside an if statement at the root of the switch case.";

        private static readonly DiagnosticDescriptor s_appContextDefaultNotInitializedToTrueDiagnostic =
            new DiagnosticDescriptor(DiagnosticIds.BCL0010.ToString(), s_title, CreateDiagnosticMessage(s_defaultValueNotInitializedToTrue, DiagnosticIds.BCL0010.ToString()), s_analyzerName, DiagnosticSeverity.Error, isEnabledByDefault: true, description: s_description);

        private static readonly DiagnosticDescriptor s_appContextDefaultUsedUnexpectedIfStatementDiagnostic =
            new DiagnosticDescriptor(DiagnosticIds.BCL0011.ToString(), s_title, CreateDiagnosticMessage(s_defaultValueInsideUnexpectedIfCondition, DiagnosticIds.BCL0011.ToString()), s_analyzerName, DiagnosticSeverity.Error, isEnabledByDefault: true, description: s_description);

        private static readonly DiagnosticDescriptor s_appContextDefaultValueDefinedOutsideIfConditionDiagnostic =
            new DiagnosticDescriptor(DiagnosticIds.BCL0012.ToString(), s_title, CreateDiagnosticMessage(s_defaultValueDefinedOutsideIfCondition, DiagnosticIds.BCL0012.ToString()), s_analyzerName, DiagnosticSeverity.Error, isEnabledByDefault: true, description: s_description);

        /// <summary>
        /// This method combines information about what the actual error is with a suggested way on how to suppress the warning.
        /// </summary>
        private static string CreateDiagnosticMessage(string diagnosticMessage, string diagnosticId)
        {
            return string.Format("{0} {1}", diagnosticMessage, string.Format(HowToDisableWarning, diagnosticId));
        }


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    s_appContextDefaultNotInitializedToTrueDiagnostic,
                    s_appContextDefaultUsedUnexpectedIfStatementDiagnostic,
                    s_appContextDefaultValueDefinedOutsideIfConditionDiagnostic);
            }
        }

        public override void OnCompilationStart(CompilationStartAnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeCodeBlock, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeCodeBlock(SyntaxNodeAnalysisContext context)
        {
            InvocationExpressionSyntax call = context.Node as InvocationExpressionSyntax;
            if (call == null)
                return;

            // If we are not calling the DefineSwitchDefault methods on LocalAppContext then we can safely ignore this.
            if (!IsCallToDefineSwitchDefault(call, context.SemanticModel))
            {
                return;
            }

            // Validate that the second argument is true. 
            ArgumentSyntax args = call.ArgumentList.Arguments[1];
            if (args.Expression.Kind() != SyntaxKind.TrueLiteralExpression)
            {
                context.ReportDiagnostic(Diagnostic.Create(s_appContextDefaultNotInitializedToTrueDiagnostic, args.GetLocation(), call));
            }

            // check that we are doing this inside an if statement
            var containingIfStatement = call.Ancestors().FirstOrDefault(n => n.IsKind(SyntaxKind.IfStatement)) as IfStatementSyntax;
            if (containingIfStatement == null)
            {
                context.ReportDiagnostic(Diagnostic.Create(s_appContextDefaultValueDefinedOutsideIfConditionDiagnostic, args.GetLocation(), call));
            }
            else
            {
                // are we inside the switch? either as a block or as a switchcase?
                if (!(containingIfStatement.Parent.IsKind(SyntaxKind.SwitchSection) ||
                    containingIfStatement.Parent.Parent.IsKind(SyntaxKind.SwitchSection)))
                {
                    context.ReportDiagnostic(Diagnostic.Create(s_appContextDefaultValueDefinedOutsideIfConditionDiagnostic, args.GetLocation(), call));
                }
            }

            // Validate that the if statement is using the appropriate expression
            if (containingIfStatement.Condition.Kind() != SyntaxKind.LessThanOrEqualExpression)
            {
                context.ReportDiagnostic(Diagnostic.Create(s_appContextDefaultUsedUnexpectedIfStatementDiagnostic, containingIfStatement.GetLocation()));
            }
        }


        /// <summary>
        /// Check to see if the call is to this method: System.LocalAppContext.DefineSwitchDefault(string, boolean);
        /// </summary>
        private bool IsCallToDefineSwitchDefault(InvocationExpressionSyntax call, SemanticModel model)
        {
            // we are going to do a quick check to avoid doing a lot of computations at this point:
            // -- does the invocation expression have the 'DefineSwitchDefault' method name
            // -- does the call have 2 arguments
            if (call.Expression.ToString().IndexOf("DefineSwitchDefault") == -1)
                return false;

            if (call.ArgumentList.Arguments.Count != 2)
                return false;

            // If we got to this point, our fast checks indicate a possible real call that we need 
            // to further check
            var symInfo = model.GetSymbolInfo(call);

            if (symInfo.Symbol == null)
                return false;

            return IsMethodTheDefineSwitchDefaultOne(symInfo.Symbol as IMethodSymbol);
        }

        private bool IsMethodTheDefineSwitchDefaultOne(IMethodSymbol methodSym)
        {
            if (methodSym == null)
                return false;

            // We allow calls on both LocalAppContext (for non-mscorlib) and AppContext (for mscorlib)
            if (!StringComparer.Ordinal.Equals(methodSym.ContainingType.Name, "LocalAppContext") &&
                !StringComparer.Ordinal.Equals(methodSym.ContainingType.Name, "AppContext"))
                return false;

            if (!StringComparer.Ordinal.Equals(methodSym.ContainingNamespace.Name, "System"))
                return false;

            if (!StringComparer.Ordinal.Equals(methodSym.Name, "DefineSwitchDefault"))
                return false;

            //check parameter type.
            if (methodSym.Parameters.Length != 2)
                return false;

            if (methodSym.Parameters[0].Type.SpecialType != SpecialType.System_String)
                return false;

            if (methodSym.Parameters[1].Type.SpecialType != SpecialType.System_Boolean)
                return false;

            return true;
        }
    }
}
