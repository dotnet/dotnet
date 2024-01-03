// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System.IO;
using XliffTasks.Model;

namespace XliffTasks.Tasks
{
    public sealed class SortXlf : XlfTask
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
                        // If the file doesn't exist, we don't need to worry about sorting it.
                        continue;
                    }

                    bool modified = xlfDocument.Sort();
                    if (!modified)
                    {
                        continue; // no changes
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(xlfPath));
                    xlfDocument.Save(xlfPath);
                }
            }
        }
    }
}
