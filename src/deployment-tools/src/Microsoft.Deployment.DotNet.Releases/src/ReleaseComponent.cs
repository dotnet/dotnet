// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Defines properties common to all types of release components.
    /// </summary>
    public abstract class ReleaseComponent
    {
        /// <summary>
        /// A display friendly version.
        /// </summary>
        public string DisplayVersion
        {
            get;
        }

        /// <summary>
        /// The files associated with the <see cref="ReleaseComponent"/>.
        /// </summary>
        public ReadOnlyCollection<ReleaseFile> Files
        {
            get;
        }

        /// <summary>
        /// The friendly display name for the component.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// The <see cref="ProductRelease"/> to which this component belongs.
        /// </summary>
        public ProductRelease Release
        {
            get;
        }

        /// <summary>
        /// The version of the <see cref="ReleaseComponent"/>.
        /// </summary>
        public ReleaseVersion Version
        {
            get;
        }

        /// <summary>
        /// Creates a new <see cref="ReleaseComponent"/> instance.
        /// </summary>
        /// <param name="element">The JSON element of the component.</param>
        /// <param name="release">The release to which the component belongs.</param>
        internal ReleaseComponent(JsonElement element, ProductRelease release)
        {
            Release = release;
            Version = element.GetReleaseVersionOrDefault("version");
            DisplayVersion = element.GetStringOrDefault("version-display");

            List<ReleaseFile> files = new List<ReleaseFile>();

            if (element.TryGetProperty("files", out JsonElement P) && P.ValueKind == JsonValueKind.Array)
            {
                var enumerator = P.EnumerateArray();

                while (enumerator.MoveNext())
                {
                    ReleaseFile file = new(enumerator.Current);

                    // Trim out marketing files.
                    if (!string.IsNullOrEmpty(file.Name) && !file.Name.Contains("-gs") && !file.Name.Contains("-nj"))
                    {
                        files.Add(file);
                    }
                }
            }

            Files = new ReadOnlyCollection<ReleaseFile>(files);
        }
    }
}
