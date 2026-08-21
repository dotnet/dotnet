// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;
using System;
using System.IO;
using XliffTasks.Model;
using Microsoft.Build.Framework;

namespace XliffTasks.Tasks
{
    public abstract class XlfTask : Task, IMultiThreadableTask
    {
        /// <summary>
        /// The language of the neutral (language-agnostic) .xlf file, which is handed to the
        /// localization system as the source of truth for the strings to translate.
        ///
        /// English is the source language and therefore never a translation target, so it is not a
        /// legal <c>XlfLanguages</c> entry (the targets validate this) and unambiguously identifies
        /// the neutral file.
        /// </summary>
        internal const string NeutralLanguage = "en";

        internal XlfTask()
        {
        }

        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

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

        internal static TranslatableDocument LoadSourceDocument(Microsoft.Build.Framework.AbsolutePath path, string format)
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
            else if (format.Equals("Json", StringComparison.OrdinalIgnoreCase))
            {
                document = new JsonDocument();
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

        internal static XlfDocument LoadXlfDocument(Microsoft.Build.Framework.AbsolutePath path, string language = null, bool createIfNonExistent = false)
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

        internal static bool IsNeutralLanguage(string language)
        {
            return string.Equals(language, NeutralLanguage, StringComparison.OrdinalIgnoreCase);
        }

        internal static string GetXlfPath(string sourcePath, string language)
        {
            string directory = Path.GetDirectoryName(sourcePath);
            string filename = Path.GetFileNameWithoutExtension(sourcePath);
            string extension = Path.GetExtension(sourcePath);

            string languageSuffix = IsNeutralLanguage(language) ? string.Empty : $".{language}";

            string xlfExtension;
            if (extension.Equals(".resx", StringComparison.OrdinalIgnoreCase))
            {
                xlfExtension = $"{languageSuffix}.xlf";
            }
            else
            {
                xlfExtension = $"{extension}{languageSuffix}.xlf";
            }

            return Path.Combine(directory, "xlf", filename + xlfExtension);
        }

        internal static string GetSourceDocumentId(string sourcePath)
        {
            return $"../{Path.GetFileName(sourcePath)}";
        }
    }
}

