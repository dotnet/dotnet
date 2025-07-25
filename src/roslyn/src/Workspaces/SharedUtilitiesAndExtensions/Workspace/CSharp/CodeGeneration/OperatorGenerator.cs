﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis.CodeGeneration;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.PooledObjects;

namespace Microsoft.CodeAnalysis.CSharp.CodeGeneration;

using static CodeGenerationHelpers;
using static CSharpCodeGenerationHelpers;
using static CSharpSyntaxTokens;
using static SyntaxFactory;

internal static class OperatorGenerator
{
    internal static TypeDeclarationSyntax AddOperatorTo(
        TypeDeclarationSyntax destination,
        IMethodSymbol method,
        CSharpCodeGenerationContextInfo info,
        IList<bool>? availableIndices,
        CancellationToken cancellationToken)
    {
        var methodDeclaration = GenerateOperatorDeclaration(method, GetDestination(destination), info, cancellationToken);
        var members = Insert(destination.Members, methodDeclaration, info, availableIndices, after: LastOperator);

        return AddMembersTo(destination, members, cancellationToken);
    }

    internal static OperatorDeclarationSyntax GenerateOperatorDeclaration(
        IMethodSymbol method,
        CodeGenerationDestination destination,
        CSharpCodeGenerationContextInfo info,
        CancellationToken cancellationToken)
    {
        var reusableSyntax = GetReuseableSyntaxNodeForSymbol<OperatorDeclarationSyntax>(method, info);
        if (reusableSyntax != null)
        {
            return reusableSyntax;
        }

        var declaration = GenerateOperatorDeclarationWorker(method, destination, info, cancellationToken);
        declaration = UseExpressionBodyIfDesired(info, declaration, cancellationToken);

        declaration = AddAnnotationsTo(method,
            ConditionallyAddDocumentationCommentTo(declaration, method, info, cancellationToken));
        return declaration.WithAdditionalAnnotations(Formatter.Annotation);
    }

    private static OperatorDeclarationSyntax UseExpressionBodyIfDesired(
        CSharpCodeGenerationContextInfo info, OperatorDeclarationSyntax declaration, CancellationToken cancellationToken)
    {
        if (declaration.ExpressionBody == null)
        {
            if (declaration.Body?.TryConvertToArrowExpressionBody(
                declaration.Kind(), info.LanguageVersion, info.Options.PreferExpressionBodiedOperators.Value, cancellationToken,
                out var expressionBody, out var semicolonToken) == true)
            {
                return declaration.WithBody(null)
                                  .WithExpressionBody(expressionBody)
                                  .WithSemicolonToken(semicolonToken);
            }
        }

        return declaration;
    }

    private static OperatorDeclarationSyntax GenerateOperatorDeclarationWorker(
        IMethodSymbol method,
        CodeGenerationDestination destination,
        CSharpCodeGenerationContextInfo info,
        CancellationToken cancellationToken)
    {
        var hasNoBody = !info.Context.GenerateMethodBodies || method.IsExtern || method.IsAbstract;

        var operatorSyntaxKind = SyntaxFacts.GetOperatorKind(method.MetadataName);
        if (operatorSyntaxKind == SyntaxKind.None)
        {
            throw new ArgumentException(string.Format(WorkspaceExtensionsResources.Cannot_generate_code_for_unsupported_operator_0, method.Name), nameof(method));
        }

        var operatorToken = Token(operatorSyntaxKind);
        var checkedToken = SyntaxFacts.IsCheckedOperator(method.MetadataName)
            ? CheckedKeyword
            : default;

        var isExplicit = method.ExplicitInterfaceImplementations.Length > 0;
        var operatorDecl = OperatorDeclaration(
            attributeLists: AttributeGenerator.GenerateAttributeLists(method.GetAttributes(), info),
            modifiers: GenerateModifiers(method, destination, info, hasNoBody),
            returnType: method.ReturnType.GenerateTypeSyntax(),
            explicitInterfaceSpecifier: GenerateExplicitInterfaceSpecifier(method.ExplicitInterfaceImplementations),
            operatorKeyword: OperatorKeyword,
            checkedKeyword: checkedToken,
            operatorToken: operatorToken,
            parameterList: ParameterGenerator.GenerateParameterList(method.Parameters, isExplicit: isExplicit, info: info),
            body: hasNoBody ? null : StatementGenerator.GenerateBlock(method),
            expressionBody: null,
            semicolonToken: hasNoBody ? SemicolonToken : new SyntaxToken());

        operatorDecl = UseExpressionBodyIfDesired(info, operatorDecl, cancellationToken);
        return operatorDecl;
    }

    private static SyntaxTokenList GenerateModifiers(
        IMethodSymbol method,
        CodeGenerationDestination destination,
        CSharpCodeGenerationContextInfo info,
        bool hasNoBody)
    {
        using var _ = ArrayBuilder<SyntaxToken>.GetInstance(out var tokens);

        if (method.ExplicitInterfaceImplementations.Length == 0 &&
            !(destination is CodeGenerationDestination.InterfaceType && hasNoBody))
        {
            tokens.Add(PublicKeyword);
        }

        tokens.AddRange(MethodGenerator.GenerateModifiers(method, destination, info, includeAccessibility: false));

        return [.. tokens.ToImmutableAndClear()];
    }
}
