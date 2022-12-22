// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Describes a single .NET <see cref="ProductRelease"/>. A release may include multiple SDKs and runtime releases.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ProductRelease
    {
        private string DebuggerDisplay => $"Release {Version} (SDKs: {Sdks.Count}, Runtimes: {AllRuntimes.Count})";

        /// <summary>
        /// A collection of all the runtime components (.NET Core, ASP.NET Core, Windows Desktop, etc.) included in this <see cref="ProductRelease"/>.
        /// </summary>
        [JsonIgnore]
        public ReadOnlyCollection<ReleaseComponent> AllRuntimes
        {
            get;
        }

        /// <summary>
        /// The ASP.NET Core runtime included in this release, or <see langword="null"/> if the component is absent."/>
        /// </summary>
        public AspNetCoreReleaseComponent AspNetCoreRuntime
        {
            get;
        }

        /// <summary>
        /// Gets a collection of all the release components (SDKs and runtimes) in the release.
        /// </summary>
        public ReadOnlyCollection<ReleaseComponent> Components
        {
            get;
        }

        /// <summary>
        /// The collection CVEs addressed by this release. The collection may be empty if no CVEs are associated with
        /// the release.
        /// </summary>
        public ReadOnlyCollection<Cve> Cves
        {
            get;
        }

        /// <summary>
        /// Gets all files from all the release components (SDKs and runtimes) associated with this <see cref="ProductRelease"/>.
        /// </summary>
        public ReadOnlyCollection<ReleaseFile> Files
        {
            get;
        }

        /// <summary>
        /// <see langword="true"/> if the release version describes a prerelease; <see langword="false"/> otherwise.
        /// </summary>
        [JsonIgnore]
        public bool IsPreview => !string.IsNullOrWhiteSpace(Version.Prerelease);

        /// <summary>
        /// <see langword="true"/> if the release includes security fixes; otherwise <see langword="false"/>.
        /// </summary>
        public bool IsSecurityUpdate
        {
            get;
        }

        /// <summary>
        /// The <see cref="Product"/> to which this <see cref="ProductRelease"/> belongs.
        /// </summary>

        public Product Product
        {
            get;
        }

        /// <summary>
        /// The date this <see cref="ProductRelease"/> was published.
        /// </summary>
        public DateTime ReleaseDate
        {
            get;
        }

        /// <summary>
        /// A <see cref="Uri"/> pointing to the release notes of this <see cref="ProductRelease"/>.
        /// </summary>
        public Uri ReleaseNotes
        {
            get;
        }

        /// <summary>
        /// The .NET Core Runtime included with this <see cref="ProductRelease"/>.
        /// </summary>
        public RuntimeReleaseComponent Runtime
        {
            get;
        }

        /// <summary>
        /// All SDKs included in this release. 
        /// </summary>
        public ReadOnlyCollection<SdkReleaseComponent> Sdks
        {
            get;
        }

        /// <summary>
        /// The version of this <see cref="ProductRelease"/>.
        /// </summary>
        public ReleaseVersion Version
        {
            get;
        }

        /// <summary>
        /// The Windows Desktop runtime associated with this release or <see langword="null"/> if the component is absent.
        /// </summary>
        public WindowsDesktopReleaseComponent WindowsDesktopRuntime
        {
            get;
        }

        internal ProductRelease(JToken jtoken, Product product)
        {
            var js = JsonSerializer.CreateDefault(Utils.DefaultSerializerSettings);

            List<SdkReleaseComponent> sdkList = new List<SdkReleaseComponent>();
            var componentList = new List<ReleaseComponent>();
            var runtimeList = new List<ReleaseComponent>();

            ReleaseDate = jtoken["release-date"].ToObject<DateTime>(js);
            Version = jtoken["release-version"].ToObject<ReleaseVersion>(js);

            var cveListToken = jtoken["cve-list"];
            var cveList = cveListToken.IsNullOrEmpty()
                ? new List<Cve>()
                : JsonConvert.DeserializeObject<List<Cve>>(cveListToken.ToString(), Utils.DefaultSerializerSettings);
            Cves = new ReadOnlyCollection<Cve>(cveList);
            IsSecurityUpdate = jtoken["security"].ToObject<bool>(js);
            ReleaseNotes = jtoken["release-notes"]?.ToObject<Uri>(js);

            var aspNetCoreRuntimeToken = jtoken["aspnetcore-runtime"];
            var runtimeToken = jtoken["runtime"];
            var winDesktopToken = jtoken["windowsdesktop"];

            if (!aspNetCoreRuntimeToken.IsNullOrEmpty())
            {
                AspNetCoreRuntime = new AspNetCoreReleaseComponent(aspNetCoreRuntimeToken, this);
                runtimeList.Add(AspNetCoreRuntime);
            }

            if (!runtimeToken.IsNullOrEmpty())
            {
                Runtime = new RuntimeReleaseComponent(runtimeToken, this);
                runtimeList.Add(Runtime);
            }

            if (!winDesktopToken.IsNullOrEmpty())
            {
                WindowsDesktopRuntime = new WindowsDesktopReleaseComponent(winDesktopToken, this);
                runtimeList.Add(WindowsDesktopRuntime);
            }

            var sdkToken = jtoken["sdk"];
            var sdksToken = jtoken["sdks"];

            if (!sdksToken.IsNullOrEmpty())
            {
                foreach (var token in sdksToken)
                {
                    sdkList.Add(new SdkReleaseComponent(token, this));
                }
            }
            else if (!sdkToken.IsNullOrEmpty())
            {
                sdkList.Add(new SdkReleaseComponent(sdkToken, this));
            }

            componentList.AddRange(runtimeList);
            componentList.AddRange(sdkList);

            Sdks = new ReadOnlyCollection<SdkReleaseComponent>(sdkList);
            Components = new ReadOnlyCollection<ReleaseComponent>(componentList);
            AllRuntimes = new ReadOnlyCollection<ReleaseComponent>(runtimeList);

            Files = new ReadOnlyCollection<ReleaseFile>(
                Components.SelectMany(c => c.Files).Distinct().ToList());

            Product = product;
        }
    }
}
