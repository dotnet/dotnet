// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.IO;
using XliffTasks.Model;

namespace XliffTasks.Tasks;

[MSBuildMultiThreadableTask]
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

        AbsolutePath sourceAbsolutePath = TaskEnvironment.GetAbsolutePath(sourcePath);
        AbsolutePath translatedAbsolutePath = TaskEnvironment.GetAbsolutePath(translatedFullPath);
        TranslatableDocument sourceDocument = XlfTask.LoadSourceDocument(sourceAbsolutePath, XlfFile.GetMetadata(MetadataKey.XlfSourceFormat));
        XlfDocument xlfDocument = XlfTask.LoadXlfDocument(TaskEnvironment.GetAbsolutePath(XlfFile.ItemSpec));

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

        Directory.CreateDirectory(Path.GetDirectoryName(translatedAbsolutePath));

        // These paths are written into the translated document, and Path.GetFullPath canonicalized
        // them before they were embedded.
        sourceDocument.RewriteRelativePathsToAbsolute(sourceAbsolutePath.GetCanonicalForm());
        sourceDocument.Save(new FileInfo(translatedAbsolutePath));
    }
}
