// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.GenFacades
{
    internal class TypeParser
    {
        public static HashSet<string> GetAllPublicTypes(IEnumerable<string> files, IEnumerable<string> constants, string langVersion)
        {
            HashSet<string> types = new HashSet<string>();

            LanguageVersion languageVersion = LanguageVersion.Default;
            if (!string.IsNullOrEmpty(langVersion) && !LanguageVersionFacts.TryParse(langVersion, out languageVersion))
            {
                throw new ArgumentException($"Invalid C# language version value '{langVersion}'", nameof(langVersion));
            }

            var syntaxTreeCollection = GetSourceTrees(files, constants, languageVersion);

            foreach (var tree in syntaxTreeCollection)
            {
                AddTypesFromTypeForwards(tree, types);
                AddBaseTypes(tree, types);
                AddTypesFromDelegates(tree, types);
            }
            return types;
        }

        private static void AddTypesFromTypeForwards(SyntaxTree tree, HashSet<string> types)
        {
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            foreach (var attribute in root.AttributeLists)
            {
                foreach (var item in attribute.Attributes)
                {
                    string attributeString = item.ToFullString();
                    if (attributeString.Contains("TypeForwardedTo"))
                    {
                        var typeNameExpression = (TypeOfExpressionSyntax)item.ArgumentList.Arguments[0].Expression;
                        string typeName = typeNameExpression.Type.ToFullString();
                        types.Add(typeName);
                    }
                }
            }
        }

        private static void AddBaseTypes(SyntaxTree tree, HashSet<string> types)
        {
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var allPublicTypes = root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>()
                .Where(t => HasPublicModifier(t));

            foreach (var item in allPublicTypes)
            {
                string fullyQualifiedName = GetFullyQualifiedName(item);
                if (!types.Contains(fullyQualifiedName))
                    types.Add(fullyQualifiedName);
            }
        }

        private static void AddTypesFromDelegates(SyntaxTree tree, HashSet<string> types)
        {
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var allPublicTypes = root.DescendantNodes().OfType<DelegateDeclarationSyntax>()
                .Where(t => HasPublicModifier(t));

            foreach (var item in allPublicTypes)
            {
                string fullyQualifiedName;
                if (item.Parent is NamespaceDeclarationSyntax parent)
                {
                    fullyQualifiedName = parent.Name.ToFullString().Trim() + "." + GetDelegateTypeName(item);
                }
                else
                {
                    fullyQualifiedName = GetFullyQualifiedName((BaseTypeDeclarationSyntax)item.Parent, item.Identifier.ValueText.Trim());
                }

                if (!types.Contains(fullyQualifiedName))
                    types.Add(fullyQualifiedName);
            }
        }

        private static string GetFullyQualifiedName(BaseTypeDeclarationSyntax node, string nested = "")
        {
            string typeName = GetBaseTypeName(node);
            if (node.Parent is NamespaceDeclarationSyntax parent)
            {
                string withoutNested = GetNamespaceName(parent) + "." + typeName;
                return string.IsNullOrEmpty(nested) ? withoutNested : withoutNested + "." + nested;
            }

            return GetFullyQualifiedName((BaseTypeDeclarationSyntax)node.Parent, string.IsNullOrEmpty(nested) ? typeName : typeName + "." + nested);
        }

        // BaseType here refers to classes, structs, interfaces and enums.
        private static string GetBaseTypeName(BaseTypeDeclarationSyntax type)
        {
            string typeName = type.Identifier.ValueText;

            if (type is TypeDeclarationSyntax actualType)
            {
                return GetTypeNameWithTypeParameter(actualType.TypeParameterList, typeName);
            }
            return typeName;
        }

        private static string GetDelegateTypeName(DelegateDeclarationSyntax type)
        {
            string typeName = type.Identifier.ValueText;
            return GetTypeNameWithTypeParameter(type.TypeParameterList, typeName);
        }

        private static string GetTypeNameWithTypeParameter(TypeParameterListSyntax typeParameterList, string identifier)
        {
            return typeParameterList != null ? identifier + "`" + typeParameterList.Parameters.Count : identifier;
        }

        private static string GetNamespaceName(NamespaceDeclarationSyntax namespaceSyntax)
        {
            return namespaceSyntax.Name.ToFullString().Trim();
        }

        private static IEnumerable<SyntaxTree> GetSourceTrees(IEnumerable<string> sourceFiles, IEnumerable<string> constants, LanguageVersion languageVersion)
        {
            CSharpParseOptions options = new CSharpParseOptions(languageVersion: languageVersion, preprocessorSymbols: constants);
            List<SyntaxTree> result = new List<SyntaxTree>();
            foreach (string sourceFile in sourceFiles)
            {
                if (string.IsNullOrEmpty(sourceFile))
                {
                    continue;
                }
                if (!File.Exists(sourceFile))
                {
                    throw new FileNotFoundException($"File {sourceFile} was not found.");
                }
                SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFile), options);
                result.Add(tree);
            }
            return result;
        }

        private static bool HasPublicModifier(BaseTypeDeclarationSyntax token)
        {
            if (token.Parent == null || token.Parent is NamespaceDeclarationSyntax)
                return HasPublicModifier(token.Modifiers);

            return HasPublicModifier(token.Modifiers) && HasPublicModifier((BaseTypeDeclarationSyntax)(token.Parent));
        }

        private static bool HasPublicModifier(DelegateDeclarationSyntax token)
        {
            if (token.Parent == null || token.Parent is NamespaceDeclarationSyntax)
                return HasPublicModifier(token.Modifiers);

            return HasPublicModifier(token.Modifiers) && HasPublicModifier((BaseTypeDeclarationSyntax)(token.Parent));
        }

        private static bool HasPublicModifier(SyntaxTokenList modifiers)
        {
            foreach (SyntaxToken modifier in modifiers)
            {
                if (modifier.Text == "public")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
