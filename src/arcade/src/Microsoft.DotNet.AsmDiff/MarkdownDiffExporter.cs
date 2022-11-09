// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci;
using Microsoft.Cci.Differs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.DotNet.AsmDiff
{
    public sealed class MarkdownDiffExporter
    {
        private readonly DiffDocument _diffDocument;
        private readonly string _path;
        private readonly bool _includeTableOfContents;
        private readonly bool _createFilePerNamespace;

        public MarkdownDiffExporter(DiffDocument diffDocument, string path, bool includeTableOfContents, bool createFilePerNamespace)
        {
            _diffDocument = diffDocument;
            _path = path;
            _includeTableOfContents = includeTableOfContents;
            _createFilePerNamespace = createFilePerNamespace;
        }

        public void Export()
        {
            if (_createFilePerNamespace && !_includeTableOfContents)
            {
                WriteDiffForNamespaces(null);
            }
            else
            {
                using (var writer = new StreamWriter(_path))
                {
                    WriteHeader(writer);

                    if (_includeTableOfContents)
                        WriteTableOfContents(writer);

                    WriteDiffForNamespaces(writer);
                }
            }
        }

        private void WriteHeader(StreamWriter writer)
        {
            if (_diffDocument.IsDiff)
            {
                writer.WriteLine(Resources.MarkdownTitle, _diffDocument.Left.Name, _diffDocument.Right.Name);
            }
            else
            {
                string singleSideName = _diffDocument.Left.IsEmpty ? _diffDocument.Right.Name : _diffDocument.Left.Name;
                writer.WriteLine(Resources.MarkdownAPIListTitle, singleSideName);
            }

            writer.WriteLine();

            if (_diffDocument.IsDiff)
            {
                writer.WriteLine(Resources.MarkdownDiffDescription);
                writer.WriteLine();
            }
        }

        private void WriteTableOfContents(StreamWriter writer)
        {
            foreach (var topLevelApi in _diffDocument.ApiDefinitions)
            {
                string linkTitle = topLevelApi.Name;
                string linkTarget = GetLinkTarget(topLevelApi.Name);

                writer.WriteLine("* [{0}]({1})", linkTitle, linkTarget);
            }

            writer.WriteLine();
        }

        private void WriteDiffForNamespaces(StreamWriter writer)
        {
            if (_createFilePerNamespace)
            {
                foreach (var topLevelApi in _diffDocument.ApiDefinitions)
                {
                    string fileName = GetFileNameForNamespace(topLevelApi.Name);
                    using (var nestedWriter = new StreamWriter(fileName))
                        WriteDiffForNamespace(nestedWriter, topLevelApi, isStandalone: true);
                }
            }
            else
            {
                foreach (DiffApiDefinition topLevelApi in _diffDocument.ApiDefinitions)
                {
                    WriteDiffForNamespace(writer, topLevelApi, isStandalone: false);
                }
            }
        }

        private void WriteDiffForNamespace(StreamWriter writer, DiffApiDefinition topLevelApi, bool isStandalone)
        {
            string heading = _createFilePerNamespace ? "#" : "##";
            writer.WriteLine(heading + " " + topLevelApi.Name);
            writer.WriteLine();

            WriteDiff(writer, topLevelApi);

            writer.WriteLine();
        }

        private static void WriteDiff(StreamWriter writer, DiffApiDefinition topLevelApi)
        {
            writer.WriteLine("``` diff");
            WriteDiff(writer, topLevelApi, 0);
            writer.WriteLine("```");
        }

        private static void WriteDiff(StreamWriter writer, DiffApiDefinition api, int level)
        {
            bool hasChildren = api.Children.Any();

            string indent = new string(' ', level * 4);
            string suffix = hasChildren ? " {" : string.Empty;
            DifferenceType diff = api.Difference;

            if (diff == DifferenceType.Changed)
            {
                // Let's see whether the syntax actually changed. For some cases the syntax might not
                // diff, for example, when attribute declarations have changed.

                string left = api.Left.GetCSharpDeclaration();
                string right = api.Right.GetCSharpDeclaration();

                if (string.Equals(left, right, StringComparison.OrdinalIgnoreCase))
                    diff = DifferenceType.Unchanged;
            }

            switch (diff)
            {
                case DifferenceType.Added:
                    WriteDiff(writer, "+", indent, suffix, api.Right);
                    break;
                case DifferenceType.Removed:
                    WriteDiff(writer, "-", indent, suffix, api.Left);
                    break;
                case DifferenceType.Changed:
                    WriteDiff(writer, "-", indent, suffix, api.Left);
                    WriteDiff(writer, "+", indent, suffix, api.Right);
                    break;
                default:
                    WriteDiff(writer, " ", indent, suffix, api.Definition);
                    break;
            }

            if (hasChildren)
            {
                foreach (DiffApiDefinition child in api.Children)
                {
                    WriteDiff(writer, child, level + 1);
                }

                var diffMarker = diff == DifferenceType.Added
                                    ? "+"
                                    : diff == DifferenceType.Removed
                                        ? "-"
                                        : " ";

                writer.Write(diffMarker);
                writer.Write(indent);
                writer.WriteLine("}");
            }
        }

        private static void WriteDiff(TextWriter writer, string marker, string indent, string suffix, IDefinition api)
        {
            IEnumerable<string> lines = GetCSharpDecalarationLines(api);
            bool isFirst = true;

            foreach (string line in lines)
            {
                if (isFirst)
                    isFirst = false;
                else
                    writer.WriteLine();

                writer.Write(marker);
                writer.Write(indent);
                writer.Write(line);
            }

            writer.WriteLine(suffix);
        }

        private static IEnumerable<string> GetCSharpDecalarationLines(IDefinition api)
        {
            string text = api.GetCSharpDeclaration();
            using (var reader = new StringReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return line;
            }
        }

        private string GetLinkTarget(string namespaceName)
        {
            return _createFilePerNamespace
                        ? Path.GetFileName(GetFileNameForNamespace(namespaceName))
                        : "#" + GetAnchorName(namespaceName);
        }

        private string GetFileNameForNamespace(string namespaceName)
        {
            string directory = Path.GetDirectoryName(_path);
            string fileName = Path.GetFileNameWithoutExtension(_path);
            string extension = Path.GetExtension(_path);
            return Path.Combine(directory, fileName + "_" + namespaceName + extension);
        }

        private static string GetAnchorName(string name)
        {
            return name.Replace(".", "").ToLower();
        }
    }
}
