// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;

namespace XliffTasks.Tasks
{
    public sealed class GatherTranslatedSource : XlfTask
    {
        [Required]
        public ITaskItem[] XlfFiles { get; set; }

        [Output]
        public ITaskItem[] Outputs { get; private set; }

        protected override void ExecuteCore()
        {
            int index = 0;
            ITaskItem[] outputs = new ITaskItem[XlfFiles.Length];

            foreach (ITaskItem xlf in XlfFiles)
            {
                string translatedFullPath = xlf.GetMetadataOrThrow(MetadataKey.XlfTranslatedFullPath);
                string language = xlf.GetMetadataOrThrow(MetadataKey.XlfLanguage);

                TaskItem output = new(xlf) { ItemSpec = translatedFullPath };

                // Set up metadata required to give the correct resource names to translated source.
                SetLink(xlf, output, translatedFullPath);
                AdjustManifestResourceName(xlf, output, language);
                AdjustLogicalName(xlf, output, language);
                AdjustDependentUpon(xlf, output);

                outputs[index++] = output;
            }

            Release.Assert(index == XlfFiles.Length);
            Outputs = outputs;
        }

        private static readonly char[] s_directorySeparatorChars = new[]
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        };

        private static void SetLink(ITaskItem xlf, ITaskItem output, string translatedFullPath)
        {
            // Set link metadata to logically locate translated source next to untranslated source
            // so that the correct resource names are generated.
            string link = xlf.GetMetadata(MetadataKey.Link);
            if (string.IsNullOrEmpty(link))
            {
                link = xlf.GetMetadataOrThrow(MetadataKey.XlfSource);
            }

            string linkFileName = Path.GetFileName(translatedFullPath);
            if (link.IndexOfAny(s_directorySeparatorChars) < 0)
            {
                link = linkFileName;
            }
            else
            {
                string linkDirectory = Path.GetDirectoryName(link);
                link = Path.Combine(linkDirectory, linkFileName);
            }

            output.SetMetadata(MetadataKey.Link, link);
        }

        private static void AdjustManifestResourceName(ITaskItem xlf, ITaskItem output, string language)
        {
            string manifestResourceName = xlf.GetMetadata(MetadataKey.ManifestResourceName);
            if (!string.IsNullOrEmpty(manifestResourceName))
            {
                manifestResourceName = $"{manifestResourceName}.{language}";
                output.SetMetadata(MetadataKey.ManifestResourceName, manifestResourceName);
            }
        }

        private static void AdjustLogicalName(ITaskItem xlf, ITaskItem output, string language)
        {
            string logicalName = xlf.GetMetadata(MetadataKey.LogicalName);
            if (!string.IsNullOrEmpty(logicalName))
            {
                string logicalExtension = Path.GetExtension(logicalName);
                logicalName = Path.ChangeExtension(logicalName, $".{language}{logicalExtension}");
                output.SetMetadata(MetadataKey.LogicalName, logicalName);
            }
        }

        private static void AdjustDependentUpon(ITaskItem xlf, ITaskItem output)
        {
            string dependentUpon = xlf.GetMetadata(MetadataKey.DependentUpon);
            if (!string.IsNullOrEmpty(dependentUpon))
            {
                string sourceDirectory = Path.GetDirectoryName(xlf.GetMetadataOrThrow(MetadataKey.XlfSource));
                dependentUpon = Path.GetFullPath(Path.Combine(sourceDirectory, dependentUpon));
                output.SetMetadata(MetadataKey.DependentUpon, dependentUpon);
            }
        }
    }
}