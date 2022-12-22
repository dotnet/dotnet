// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        internal ReleaseComponent(JToken token, ProductRelease release)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            Version = token["version"]?.ToObject<ReleaseVersion>(Utils.DefaultSerializer);
            DisplayVersion = token["version-display"]?.ToString();
            JToken filesToken = token["files"];
            List<ReleaseFile> fileList = filesToken.IsNullOrEmpty()
                ? new List<ReleaseFile>()
                : JsonConvert.DeserializeObject<List<ReleaseFile>>(
                    filesToken.ToString(), Utils.DefaultSerializerSettings);
            Release = release;

            // Trim out marketing files. Users should never interact with these
            Files = new ReadOnlyCollection<ReleaseFile>(
                fileList.Where(f => !(f.Name.Contains("-gs") || f.Name.Contains("-nj"))).ToList());
        }
    }
}
