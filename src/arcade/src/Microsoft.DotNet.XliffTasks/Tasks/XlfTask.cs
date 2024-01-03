// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;
using System;
using System.IO;
using XliffTasks.Model;

namespace XliffTasks.Tasks
{
    public abstract class XlfTask : Task
    {
        internal XlfTask()
        {
        }

        public sealed override bool Execute()
        {
            try
            {
                ExecuteCore();
            }
            catch (BuildErrorException ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: false, showDetail: false, file: ex.RelatedFile);
            }

            return !Log.HasLoggedErrors;
        }

        protected abstract void ExecuteCore();

        internal static TranslatableDocument LoadSourceDocument(string path, string format)
        {
            TranslatableDocument document;

            if (format.Equals("Resx", StringComparison.OrdinalIgnoreCase))
            {
                document = new ResxDocument();
            }
            else if (format.Equals("Unstructured", StringComparison.OrdinalIgnoreCase))
            {
                document = new UnstructuredDocument();
            }
            else if (format.Equals("Vsct", StringComparison.OrdinalIgnoreCase))
            {
                document = new VsctDocument();
            }
            else if (format.Equals("XamlRule", StringComparison.OrdinalIgnoreCase))
            {
                document = new XamlRuleDocument();
            }
            else
            {
                throw new BuildErrorException($"Unknown source file format '{format}'.")
                {
                    RelatedFile = path
                };
            }

            document.Load(path);
            return document;
        }

        internal static XlfDocument LoadXlfDocument(string path, string language = null, bool createIfNonExistent = false)
        {
            XlfDocument document = new();

            if (File.Exists(path))
            {
                document.Load(path);
            }
            else if (createIfNonExistent)
            {
                Release.Assert(!string.IsNullOrEmpty(language));
                document.LoadNew(language);
            }
            else
            {
                throw new FileNotFoundException($"File not found: {path}", path);
            }

            return document;
        }

        internal static string GetXlfPath(string sourcePath, string language)
        {
            string directory = Path.GetDirectoryName(sourcePath);
            string filename = Path.GetFileNameWithoutExtension(sourcePath);
            string extension = Path.GetExtension(sourcePath);

            string xlfExtension;
            if (extension.Equals(".resx", StringComparison.OrdinalIgnoreCase))
            {
                xlfExtension = $".{language}.xlf";
            }
            else
            {
                xlfExtension = $"{extension}.{language}.xlf";
            }

            return Path.Combine(directory, "xlf", filename + xlfExtension);
        }

        internal static string GetSourceDocumentId(string sourcePath)
        {
            return $"../{Path.GetFileName(sourcePath)}";
        }
    }
}

