// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Build.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.GenFacades
{
    /// <summary>
    /// The class generates an NotSupportedAssembly from the reference sources.
    /// </summary>
    public class NotSupportedAssemblyGenerator : RoslynBuildTask
    {
        [Required]
        public ITaskItem[] SourceFiles { get; set; }

        [Required]
        public string Message { get; set; }

        public string LangVersion { get; set; }

        public string ApiExclusionListPath { get; set; }

        public override bool ExecuteCore()
        {
            if (SourceFiles == null || SourceFiles.Length == 0)
            {
                Log.LogError("There are no ref source files.");
                return false;
            }

            GenerateNotSupportedAssemblyFiles(SourceFiles);

            return !Log.HasLoggedErrors;
        }

        private void GenerateNotSupportedAssemblyFiles(IEnumerable<ITaskItem> sourceFiles)
        {
            string[] apiExclusions = null;
            if (!string.IsNullOrEmpty(ApiExclusionListPath) && File.Exists(ApiExclusionListPath))
            {
                apiExclusions = File.ReadAllLines(ApiExclusionListPath);
            }

            foreach (ITaskItem item in sourceFiles)
            {
                string sourceFile = item.ItemSpec;
                string outputPath = item.GetMetadata("OutputPath");

                if (!File.Exists(sourceFile))
                {
                    Log.LogError($"File {sourceFile} was not found.");
                    continue;
                }

                GenerateNotSupportedAssemblyForSourceFile(sourceFile, outputPath, apiExclusions);
            }
        }

        private void GenerateNotSupportedAssemblyForSourceFile(string sourceFile, string outputPath, string[] apiExclusions)
        {
            SyntaxTree syntaxTree;

            try
            {
                LanguageVersion languageVersion = LanguageVersion.Default;
                if (!String.IsNullOrEmpty(LangVersion) && !LanguageVersionFacts.TryParse(LangVersion, out languageVersion))
                {
                    Log.LogError($"Invalid LangVersion value '{LangVersion}'");
                    return;
                }
                syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFile), new CSharpParseOptions(languageVersion));
            }
            catch(Exception ex)
            {
                Log.LogErrorFromException(ex, false);
                return;
            }

            var rewriter = new NotSupportedAssemblyRewriter(Message, apiExclusions);
            SyntaxNode root = rewriter.Visit(syntaxTree.GetRoot());
            string text = root.GetText().ToString();
            File.WriteAllText(outputPath, text);
        }
    }

    internal class NotSupportedAssemblyRewriter : CSharpSyntaxRewriter
    {
        private const string emptyBody = "{ }\n";
        private string _message;
        private IEnumerable<string> _exclusionApis;

        public NotSupportedAssemblyRewriter(string message, string[] exclusionApis)
        {
            if (message != null && message.StartsWith("SR."))
            {
                _message = "System." + message;
            }
            else
            {
                _message = message;
            }
            _exclusionApis = exclusionApis?.Select(t => t.Substring(t.IndexOf(':') + 1));
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.Body == null)
                return node;

            if (_exclusionApis != null && _exclusionApis.Contains(GetMethodDefinition(node)))
                return null;

            BlockSyntax block;
            if (node.Identifier.ValueText == "Dispose" || node.Identifier.ValueText == "Finalize")
            {
                block = (BlockSyntax)SyntaxFactory.ParseStatement(emptyBody);
            }
            else
            {
                block = (BlockSyntax)SyntaxFactory.ParseStatement(GetDefaultMessage());
            }
            return node.WithBody(block);
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (_exclusionApis != null && _exclusionApis.Contains(GetPropertyDefinition(node)))
                return null;

            return base.VisitPropertyDeclaration(node);
        }

        public override SyntaxNode VisitEventDeclaration(EventDeclarationSyntax node)
        {
            if (_exclusionApis != null && _exclusionApis.Contains(GetEventDefinition(node)))
                return null;

            return base.VisitEventDeclaration(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (_exclusionApis != null && _exclusionApis.Contains(GetFullyQualifiedName(node)))
                return null;

            return base.VisitClassDeclaration(node);
        }

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            BlockSyntax block = (BlockSyntax)SyntaxFactory.ParseStatement(GetDefaultMessage());
            return node.WithBody(block);
        }

        public override SyntaxNode VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            BlockSyntax block = (BlockSyntax)SyntaxFactory.ParseStatement(emptyBody);
            return node.WithBody(block);
        }

        public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            if (node.Body == null)
                return node;

            string message = "{ throw new System.PlatformNotSupportedException(" + $"{ _message }); "+ " } ";       
            BlockSyntax block = (BlockSyntax)SyntaxFactory.ParseStatement(message);

            return node.WithBody(block);
        }

        public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            if (node.Body == null)
                return node;

            BlockSyntax block = (BlockSyntax)SyntaxFactory.ParseStatement(GetDefaultMessage());
            return node.WithBody(block);
        }

        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            if (node.Body == null)
                return node;

            BlockSyntax block = (BlockSyntax)SyntaxFactory.ParseStatement(GetDefaultMessage());
            return node.WithBody(block);
        }

        private string GetFullyQualifiedName(TypeDeclarationSyntax node)
        {
            string parent;
            if (node.Parent is NamespaceDeclarationSyntax parentNamespace)
            {
                parent = GetFullyQualifiedName(parentNamespace);
            }
            else
            {
                parent = GetFullyQualifiedName((TypeDeclarationSyntax)node.Parent);
            }

            return parent + "." + node.Identifier.ValueText.Trim();
        }

        private string GetFullyQualifiedName(NamespaceDeclarationSyntax node) => node.Name.ToFullString().Trim();

        private string GetMethodDefinition(MethodDeclarationSyntax node) => GetFullyQualifiedName((TypeDeclarationSyntax)node.Parent) + "." + node.Identifier.ValueText;

        private string GetPropertyDefinition(PropertyDeclarationSyntax node) => GetFullyQualifiedName((TypeDeclarationSyntax)node.Parent) + "." + node.Identifier.ValueText;

        private string GetEventDefinition(EventDeclarationSyntax node) => GetFullyQualifiedName((TypeDeclarationSyntax)node.Parent) + "." + node.Identifier.ValueText;

        private string GetDefaultMessage() => "{ throw new System.PlatformNotSupportedException(" + $"{ _message }); " + " }\n";
    }
}
