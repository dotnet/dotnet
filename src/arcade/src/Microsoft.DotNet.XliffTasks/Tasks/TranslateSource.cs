// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.IO;
using XliffTasks.Model;

namespace XliffTasks.Tasks
{
    public sealed class TranslateSource : XlfTask
    {
        [Required]
        public ITaskItem XlfFile { get; set; }

        protected override void ExecuteCore()
        {
            string sourcePath = XlfFile.GetMetadataOrThrow(MetadataKey.XlfSource);
            string sourceFormat = XlfFile.GetMetadataOrThrow(MetadataKey.XlfSourceFormat);
            string language = XlfFile.GetMetadataOrThrow(MetadataKey.XlfLanguage);
            string translatedFullPath = XlfFile.GetMetadataOrThrow(MetadataKey.XlfTranslatedFullPath);

            TranslatableDocument sourceDocument = XlfTask.LoadSourceDocument(sourcePath, XlfFile.GetMetadata(MetadataKey.XlfSourceFormat));
            XlfDocument xlfDocument = XlfTask.LoadXlfDocument(XlfFile.ItemSpec);

            bool validationFailed = false;
            xlfDocument.Validate(validationError =>
            {
                validationFailed = true;
                Log.LogErrorInFile(XlfFile.ItemSpec, validationError.LineNumber, validationError.Message);
            });

            IReadOnlyDictionary<string, string> translations = validationFailed
                ? new Dictionary<string, string>()
                : xlfDocument.GetTranslations();

            sourceDocument.Translate(translations);

            Directory.CreateDirectory(Path.GetDirectoryName(translatedFullPath));

            sourceDocument.RewriteRelativePathsToAbsolute(Path.GetFullPath(sourcePath));
            sourceDocument.Save(translatedFullPath);
        }
    }
}