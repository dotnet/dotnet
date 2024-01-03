// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System.IO;
using XliffTasks.Model;

namespace XliffTasks.Tasks
{
    public sealed class UpdateXlf : XlfTask
    {
        [Required]
        public ITaskItem[] Sources { get; set; }

        [Required]
        public string[] Languages { get; set; }

        [Required]
        public bool AllowModification { get; set; }

        private const string HowToUpdate =
            "Run `msbuild /t:UpdateXlf` to update .xlf files or set UpdateXlfOnBuild=true"
            + " to update them on every build, but note that it is strongly discouraged to set"
            + " UpdateXlfOnBuild=true in official/CI build environments as they should not"
            + " modify source code during the build.";

        protected override void ExecuteCore()
        {
            foreach (ITaskItem item in Sources)
            {
                string sourcePath = item.ItemSpec;
                string sourceDocumentPath = item.GetMetadataOrDefault(MetadataKey.SourceDocumentPath, item.ItemSpec);
                string sourceFormat = item.GetMetadataOrThrow(MetadataKey.XlfSourceFormat);
                TranslatableDocument sourceDocument = XlfTask.LoadSourceDocument(sourcePath, sourceFormat);
                string sourceDocumentId = XlfTask.GetSourceDocumentId(sourcePath);

                foreach (string language in Languages)
                {
                    string xlfPath = XlfTask.GetXlfPath(sourceDocumentPath, language);
                    XlfDocument xlfDocument;

                    try
                    {
                        xlfDocument = XlfTask.LoadXlfDocument(xlfPath, language, createIfNonExistent: AllowModification);
                    }
                    catch (FileNotFoundException fileNotFoundEx) when (fileNotFoundEx.FileName == xlfPath)
                    {
                        Release.Assert(!AllowModification);
                        throw new BuildErrorException($"'{xlfPath}' for '{sourcePath}' does not exist. {HowToUpdate}");
                    }
                    catch (System.Xml.XmlException xmlEx)
                    {
                        throw new BuildErrorException($"Unable to load file: {xmlEx.Message}")
                        {
                            RelatedFile = xlfPath
                        };
                    }

                    bool updated = xlfDocument.Update(sourceDocument, sourceDocumentId);

                    if (!updated)
                    {
                        continue; // no changes
                    }

                    if (!AllowModification)
                    {
                        throw new BuildErrorException($"'{xlfPath}' is out-of-date with '{sourcePath}'. {HowToUpdate}");
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(xlfPath));
                    xlfDocument.Save(xlfPath);
                }
            }
        }
    }
}