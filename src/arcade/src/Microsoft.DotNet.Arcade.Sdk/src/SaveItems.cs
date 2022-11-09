// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Construction;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Microsoft.DotNet.Arcade.Sdk
{
    /// <summary>
    /// This task writes msbuild Items with their metadata to a props file.
    /// Useful to statically save a status of an Item that will be used later on by just importing the generated file.
    /// </summary>
    public class SaveItems : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ItemName { get; set; }

        [Required]
        public ITaskItem[] Items { get; set; }

        [Output]
        [Required]
        public string File { get; set; }

        public override bool Execute()
        {
            var project = ProjectRootElement.Create();

            foreach (var item in Items)
            {
                var metadata = ((ITaskItem2)item).CloneCustomMetadataEscaped();

                if (!(metadata is IEnumerable<KeyValuePair<string, string>> metadataPairs))
                {
                    metadataPairs = metadata.Keys.OfType<string>().Select(key => new KeyValuePair<string, string>(key, metadata[key] as string));
                }

                project.AddItem(ItemName, item.ItemSpec, metadataPairs);
            }

            string path = Path.GetDirectoryName(File);

            if (!string.IsNullOrEmpty(path))
            {
                Directory.CreateDirectory(path);
            }

            project.Save(File);

            return !Log.HasLoggedErrors;
        }
    }
}
