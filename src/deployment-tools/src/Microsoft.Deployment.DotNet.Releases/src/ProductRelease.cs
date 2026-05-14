// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Describes a single .NET <see cref="ProductRelease"/>. A release may include multiple SDKs and runtime releases.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ProductRelease
    {
        private string DebuggerDisplay => $"Release {Version} (SDKs: {Sdks.Count}, Runtimes: {Runtimes.Count})";


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
        /// The collection of CVEs addressed by this release. The collection may be empty if no CVEs are associated with
        /// it, typically when the release does not contain any security fixes.
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
        /// The .NET runtime included with this <see cref="ProductRelease"/>.
        /// </summary>
        public RuntimeReleaseComponent Runtime
        {
            get;
        }

        /// <summary>
        /// A collection of all the runtime components (.NET, ASP.NET Core, Windows Desktop, etc.) included in this <see cref="ProductRelease"/>.
        /// </summary>
        public ReadOnlyCollection<ReleaseComponent> Runtimes
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
        /// The Windows desktop runtime associated with this release or <see langword="null"/> if the component is absent.
        /// </summary>
        public WindowsDesktopReleaseComponent WindowsDesktopRuntime
        {
            get;
        }

        /// <summary>
        /// Creates a new <see cref="ProductRelease"/> instance.
        /// </summary>
        /// <param name="element">The JSON element of the release.</param>
        /// <param name="product">The product to which the release belongs.</param>
        internal ProductRelease(JsonElement element, Product product)
        {
            ReleaseDate = element.GetProperty("release-date").GetDateTime();
            Version = element.GetReleaseVersionOrDefault("release-version");
            IsSecurityUpdate = element.GetProperty("security").GetBoolean();
            ReleaseNotes = element.GetUriOrDefault("release-notes");
            Product = product;

            var cves = new List<Cve>();

            if (element.TryGetProperty("cve-list", out JsonElement cveListElement) && cveListElement.ValueKind == JsonValueKind.Array)
            {
                var enumerator = cveListElement.EnumerateArray();

                while (enumerator.MoveNext())
                {
                    cves.Add(new(enumerator.Current));
                }
            }

            Cves = new ReadOnlyCollection<Cve>(cves);

            var runtimes = new List<ReleaseComponent>();

            if (element.TryGetProperty("aspnetcore-runtime", out JsonElement aspNetCoreValue) && aspNetCoreValue.ValueKind != JsonValueKind.Null)
            {
                AspNetCoreRuntime = new AspNetCoreReleaseComponent(aspNetCoreValue, this);
                runtimes.Add(AspNetCoreRuntime);
            }

            if (element.TryGetProperty("runtime", out JsonElement runtimeValue) && runtimeValue.ValueKind != JsonValueKind.Null)
            {
                Runtime = new RuntimeReleaseComponent(runtimeValue, this);
                runtimes.Add(Runtime);
            }

            if (element.TryGetProperty("windowsdesktop", out JsonElement desktopValue) && desktopValue.ValueKind != JsonValueKind.Null)
            {
                WindowsDesktopRuntime = new WindowsDesktopReleaseComponent(desktopValue, this);
                runtimes.Add(WindowsDesktopRuntime);
            }

            Runtimes = new ReadOnlyCollection<ReleaseComponent>(runtimes);

            var sdks = new List<SdkReleaseComponent>();

            if (element.TryGetProperty("sdks", out JsonElement sdksValue) && sdksValue.ValueKind == JsonValueKind.Array)
            {
                var enumerator = sdksValue.EnumerateArray();

                while (enumerator.MoveNext())
                {
                    sdks.Add(new SdkReleaseComponent(enumerator.Current, this));
                }
            }
            else if (element.TryGetProperty("sdk", out JsonElement sdkValue) && sdkValue.ValueKind != JsonValueKind.Null)
            {
                sdks.Add(new SdkReleaseComponent(sdkValue, this));
            }

            Sdks = new ReadOnlyCollection<SdkReleaseComponent>(sdks);

            var components = new List<ReleaseComponent>();
            components.AddRange(runtimes);
            components.AddRange(sdks);
            Components = new ReadOnlyCollection<ReleaseComponent>(components);

            // Distinct is necessary because some releases have overlapping files.
            Files = new ReadOnlyCollection<ReleaseFile>(
                Components.SelectMany(c => c.Files).Distinct().ToList());
        }
    }
}
