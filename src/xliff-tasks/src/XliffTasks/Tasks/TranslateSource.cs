// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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