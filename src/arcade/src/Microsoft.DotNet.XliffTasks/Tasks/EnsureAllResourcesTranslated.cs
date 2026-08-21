// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using XliffTasks.Model;

namespace XliffTasks.Tasks
{
    [MSBuildMultiThreadableTask]
    public sealed class EnsureAllResourcesTranslated : XlfTask
    {
        [Required]
        public ITaskItem[] Sources { get; set; }

        [Required]
        public string[] Languages { get; set; }

        protected override void ExecuteCore()
        {
            foreach (ITaskItem item in Sources)
            {
                string sourceDocumentPath = item.GetMetadataOrDefault(MetadataKey.SourceDocumentPath, item.ItemSpec);

                SortedSet<string> untranslatedResourceSet = new(StringComparer.Ordinal);

                foreach (string language in Languages)
                {
                    // The neutral .xlf file is an input to the localization system, not a
                    // translation, so its untranslated resources are expected.
                    if (XlfTask.IsNeutralLanguage(language))
                    {
                        continue;
                    }

                    string xlfPath = XlfTask.GetXlfPath(sourceDocumentPath, language);
                    XlfDocument xlfDocument;

                    try
                    {
                        xlfDocument = XlfTask.LoadXlfDocument(TaskEnvironment.GetAbsolutePath(xlfPath));
                    }
                    catch (FileNotFoundException)
                    {
                        // If the file doesn't exist, we don't need to worry about it having
                        // untranslated resources.
                        continue;
                    }

                    untranslatedResourceSet.UnionWith(xlfDocument.GetUntranslatedResourceIDs());
                }

                if (untranslatedResourceSet.Count > 0)
                {
                    string untranslatedResourceNames = string.Join(", ", untranslatedResourceSet);
                    Log.LogErrorInFile(sourceDocumentPath, $"Found {untranslatedResourceSet.Count} untranslated resource(s): {untranslatedResourceNames}");
                }
            }
        }
    }
}
