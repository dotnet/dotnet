﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Microsoft.CodeAnalysis.UseConditionalExpression;

internal abstract class AbstractUseConditionalExpressionForReturnDiagnosticAnalyzer<
    TIfStatementSyntax>(LocalizableResourceString message)
    : AbstractUseConditionalExpressionDiagnosticAnalyzer<TIfStatementSyntax>(
        IDEDiagnosticIds.UseConditionalExpressionForReturnDiagnosticId,
        EnforceOnBuildValues.UseConditionalExpressionForReturn,
        message,
        CodeStyleOptions2.PreferConditionalExpressionOverReturn)
    where TIfStatementSyntax : SyntaxNode
{
    protected sealed override CodeStyleOption2<bool> GetStylePreference(OperationAnalysisContext context)
        => context.GetAnalyzerOptions().PreferConditionalExpressionOverReturn;

    protected sealed override (bool matched, bool canSimplify) TryMatchPattern(
        IConditionalOperation ifOperation, ISymbol containingSymbol, CancellationToken cancellationToken)
    {
        if (!UseConditionalExpressionForReturnHelpers.TryMatchPattern(
                GetSyntaxFacts(), ifOperation, containingSymbol, cancellationToken,
                out var isRef, out var trueStatement, out var falseStatement, out var trueReturn, out var falseReturn))
        {
            return default;
        }

        if (!IsStatementSupported(trueStatement) ||
            !IsStatementSupported(falseStatement))
        {
            return default;
        }

        var canSimplify = UseConditionalExpressionHelpers.CanSimplify(
            trueReturn?.ReturnedValue ?? trueStatement,
            falseReturn?.ReturnedValue ?? falseStatement,
            isRef,
            out _);

        return (matched: true, canSimplify);
    }

    protected virtual bool IsStatementSupported(IOperation statement)
        => true;
}
