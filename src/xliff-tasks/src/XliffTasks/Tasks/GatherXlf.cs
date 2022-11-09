// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using System;
using System.Collections.Generic;

namespace XliffTasks.Tasks
{
    public sealed class GatherXlf : XlfTask
    {
        [Required]
        public ITaskItem[] Sources { get; set; }

        [Required]
        public string[] Languages { get; set; }

        [Required]
        public string TranslatedOutputDirectory { get; set; }

        [Output]
        public ITaskItem[] Outputs { get; private set; }

        protected override void ExecuteCore()
        {
            int index = 0;
            ITaskItem[] outputs = new ITaskItem[Sources.Length * Languages.Length];
            HashSet<string> outputPaths = new(StringComparer.OrdinalIgnoreCase);

            foreach (ITaskItem source in Sources)
            {
                string sourceDocumentPath = source.GetMetadataOrDefault(MetadataKey.SourceDocumentPath, source.ItemSpec);

                foreach (string language in Languages)
                {
                    string xlfPath = XlfTask.GetXlfPath(sourceDocumentPath, language);
                    TaskItem xlf = new(source) { ItemSpec = xlfPath };
                    xlf.SetMetadata(MetadataKey.XlfSource, source.ItemSpec);
                    xlf.SetMetadata(MetadataKey.XlfTranslatedFullPath, GetTranslatedOutputPath(source, language, outputPaths));
                    xlf.SetMetadata(MetadataKey.XlfLanguage, language);
                    outputs[index++] = xlf;
                }
            }

            Release.Assert(index == outputs.Length);
            Outputs = outputs;
        }

        private string GetTranslatedOutputPath(ITaskItem source, string language, HashSet<string> outputPaths)
        {
            string translatedFilename = source.GetMetadata(MetadataKey.XlfTranslatedFilename);
            if (string.IsNullOrEmpty(translatedFilename))
            {
                translatedFilename = Path.GetFileNameWithoutExtension(source.ItemSpec);
                source.SetMetadata(MetadataKey.XlfTranslatedFilename, translatedFilename);
            }

            string sourceDocumentPath = source.GetMetadataOrDefault(MetadataKey.SourceDocumentPath, source.ItemSpec);
            string extension = Path.GetExtension(source.ItemSpec);
            string outputPath = Path.Combine(TranslatedOutputDirectory, $"{translatedFilename}.{language}{extension}");

            if (!outputPaths.Add(outputPath))
            {
                throw new BuildErrorException(
                    $"Two or more source files to be translated in the same project are named {Path.GetFileName(sourceDocumentPath)}. " +
                    $"Give them unique names or set unique {MetadataKey.XlfTranslatedFilename} metadata.");
            }

            return outputPath;
        }
    }
}