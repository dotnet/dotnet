// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using XliffTasks.Model;

namespace XliffTasks.Tasks
{
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
                    string xlfPath = XlfTask.GetXlfPath(sourceDocumentPath, language);
                    XlfDocument xlfDocument;

                    try
                    {
                        xlfDocument = XlfTask.LoadXlfDocument(xlfPath);
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
