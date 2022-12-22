// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("Microsoft.CodeAnalysis.CSharp.Workspaces")]
[assembly: AssemblyDescription("Microsoft.CodeAnalysis.CSharp.Workspaces")]
[assembly: AssemblyDefaultAlias("Microsoft.CodeAnalysis.CSharp.Workspaces")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.0.121.55815")]
[assembly: AssemblyInformationalVersion("4.0.121.55815 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace Microsoft.CodeAnalysis.CSharp.Formatting
{
    public enum BinaryOperatorSpacingOptions
    {
        Single = 0,
        Ignore = 1,
        Remove = 2,
    }
    public static partial class CSharpFormattingOptions
    {
        public static Microsoft.CodeAnalysis.Options.Option<bool> IndentBlock { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> IndentBraces { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> IndentSwitchCaseSection { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> IndentSwitchCaseSectionWhenBlock { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> IndentSwitchSection { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<Microsoft.CodeAnalysis.CSharp.Formatting.LabelPositionOptions> LabelPositioning { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLineForCatch { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLineForClausesInQuery { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLineForElse { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLineForFinally { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLineForMembersInAnonymousTypes { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLineForMembersInObjectInit { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInAccessors { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInAnonymousMethods { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInAnonymousTypes { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInControlBlocks { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInLambdaExpressionBody { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInMethods { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInObjectCollectionArrayInitializers { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInProperties { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> NewLinesForBracesInTypes { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceAfterCast { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceAfterColonInBaseTypeDeclaration { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceAfterComma { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceAfterControlFlowStatementKeyword { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceAfterDot { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceAfterMethodCallName { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceAfterSemicolonsInForStatement { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceBeforeColonInBaseTypeDeclaration { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceBeforeComma { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceBeforeDot { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceBeforeOpenSquareBracket { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceBeforeSemicolonsInForStatement { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceBetweenEmptyMethodCallParentheses { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceBetweenEmptyMethodDeclarationParentheses { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceBetweenEmptySquareBrackets { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpacesIgnoreAroundVariableDeclaration { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceWithinCastParentheses { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceWithinExpressionParentheses { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceWithinMethodCallParentheses { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceWithinMethodDeclarationParenthesis { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceWithinOtherParentheses { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpaceWithinSquareBrackets { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> SpacingAfterMethodDeclarationName { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<Microsoft.CodeAnalysis.CSharp.Formatting.BinaryOperatorSpacingOptions> SpacingAroundBinaryOperator { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> WrappingKeepStatementsOnSingleLine { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> WrappingPreserveSingleLine { get { throw null; } }
    }
    public enum LabelPositionOptions
    {
        LeftMost = 0,
        OneLess = 1,
        NoIndent = 2,
    }
}
