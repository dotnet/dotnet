// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GenerateLicenseList
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("This tool expects 3 arguments: licenses.json, exceptions.json, targetFileLocation.");
                Console.WriteLine(@"This tool should be run only through .\scripts\utils\UpdateNuGetLicenseSPDXList.ps1");
                return -1;
            }

            var licenseJson = Path.GetFullPath(args[0]);
            var exceptionJson = Path.GetFullPath(args[1]);
            var targetFile = Path.GetFullPath(args[2]);

            Console.WriteLine($"License json: {licenseJson}");
            Console.WriteLine($"Exception json: {exceptionJson}");
            Console.WriteLine($"Target file: {targetFile}");

            var licenseDataCodeGenerator = new LicenseDataCodeGenerator(licenseJson, exceptionJson);

            var node = licenseDataCodeGenerator.GenerateLicenseDataFile();
            if (node != null)
            {
                var codeIssues = node.GetDiagnostics();
                if (!codeIssues.Any())
                {
                    if (File.Exists(targetFile))
                    {
                        SyntaxNode existingNode = CSharpSyntaxTree.ParseText(File.ReadAllText(targetFile)).GetRoot();
                        string? existingContent = GetContentWithoutVersion(existingNode);
                        if (existingContent != null &&
                            string.Equals(existingContent, GetContentWithoutVersion(node), StringComparison.Ordinal))
                        {
                            Console.WriteLine($"License and exception data are unchanged. Leaving {targetFile} unchanged.");
                            return 0;
                        }
                    }

                    using (var writer = File.CreateText(targetFile))
                    {
                        node.WriteTo(writer);
                        writer.Flush();
                    }
                    Console.WriteLine($"Completed the update of {targetFile}");
                    return 0;
                }

                foreach (var codeIssue in codeIssues)
                {
                    var issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}, Location: {codeIssue.Location.GetLineSpan()}, Severity: {codeIssue.Severity}";
                    Console.WriteLine(issue);
                }
                Console.WriteLine("License List file generation failed.");
            }
            return -1;
        }

        private static string? GetContentWithoutVersion(SyntaxNode node)
        {
            if (node.ContainsDiagnostics)
            {
                return null;
            }

            PropertyDeclarationSyntax[] versionProperties = node.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(declaration => declaration.Identifier.ValueText == "NuGetLicenseData")
                .SelectMany(declaration => declaration.Members.OfType<PropertyDeclarationSyntax>())
                .Where(property => property.Identifier.ValueText == "LicenseListVersion")
                .ToArray();

            if (versionProperties.Length != 1 ||
                versionProperties[0].Initializer?.Value is not LiteralExpressionSyntax version ||
                !version.IsKind(SyntaxKind.StringLiteralExpression))
            {
                return null;
            }

            LiteralExpressionSyntax placeholder = SyntaxFactory.LiteralExpression(
                SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(string.Empty)).WithTriviaFrom(version);
            node = node.ReplaceNode(version, placeholder);
            node = node.ReplaceTrivia(
                node.DescendantTrivia()
                    .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia)),
                (original, _) => SyntaxFactory.Comment(original.ToString().TrimEnd(' ', '\t')));

            return node.NormalizeWhitespace().ToFullString();
        }
    }
}
