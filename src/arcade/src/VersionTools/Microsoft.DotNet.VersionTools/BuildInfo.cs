// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.src.Util;

namespace Microsoft.DotNet.VersionTools
{
    public class BuildInfo
    {
        public const string LatestTxtFilename = "Latest.txt";
        public const string LatestPackagesTxtFilename = "Latest_Packages.txt";
        public const string LastBuildPackagesTxtFilename = "Last_Build_Packages.txt";

        public string Name { get; set; }

        public Dictionary<string, string> LatestPackages { get; set; }

        public string LatestReleaseVersion { get; set; }

        public static BuildInfo Get(
            string name,
            string rawBuildInfoBaseUrl,
            bool fetchLatestReleaseFile = true)
        {
            using (var client = X509Helper.GetHttpClientWithCertRevocation())
            {
                return GetAsync(
                    client,
                    name,
                    rawBuildInfoBaseUrl,
                    fetchLatestReleaseFile).Result;
            }
        }

        public static async Task<BuildInfo> GetAsync(
            HttpClient client,
            string name,
            string rawBuildInfoBaseUrl,
            bool fetchLatestReleaseFile = true)
        {
            Dictionary<string, string> packages;

            string rawLatestUrl = $"{rawBuildInfoBaseUrl}/{LatestTxtFilename}";
            string rawLatestPackagesUrl = $"{rawBuildInfoBaseUrl}/{LatestPackagesTxtFilename}";

            using (HttpResponseMessage response = await GetBuildInfoFileAsync(
                client,
                name,
                rawLatestPackagesUrl))
            using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                packages = await ReadPackageListAsync(reader);
            }

            string releaseVersion;

            if (fetchLatestReleaseFile)
            {
                using (HttpResponseMessage response = await GetBuildInfoFileAsync(
                    client,
                    name,
                    rawLatestUrl))
                {
                    releaseVersion = (await response.Content.ReadAsStringAsync()).Trim();
                }
            }
            else
            {
                releaseVersion = FindLatestReleaseFromPackages(packages);
            }

            return new BuildInfo
            {
                Name = name,
                LatestPackages = packages,
                LatestReleaseVersion = releaseVersion
            };
        }

        public static BuildInfo CachedGet(
            string name,
            string rawRepoUrl,
            string gitRef,
            string buildInfoPath,
            string cacheDir,
            bool fetchLatestReleaseFile = true)
        {
            // Check if the ref is a commit hash. If it's a branch name, it can't be cached.
            // A branch on GitHub can't have a name like this: GitHub refuses the push with
            // "GH002: Sorry, branch or tag names consisting of 40 hex characters are not allowed."
            bool useCache = !string.IsNullOrEmpty(cacheDir) &&
                gitRef.Length == 40 &&
                gitRef.All("0123456789abcdef".Contains);

            string cachedPath = useCache
                ? Path.Combine(cacheDir, gitRef, buildInfoPath, "buildinfo.json")
                : null;

            if (useCache && File.Exists(cachedPath))
            {
                try
                {
                    return JsonConvert.DeserializeObject<BuildInfo>(File.ReadAllText(cachedPath));
                }
                catch (Exception e)
                {
                    Trace.TraceWarning(
                        $"Couldn't read build info from cache '{cachedPath}'. Redownloading. " +
                        $"Exception caught: {e}");
                }
            }

            BuildInfo info = Get(
                name,
                RawBuildInfoBaseUrl(rawRepoUrl, gitRef, buildInfoPath),
                fetchLatestReleaseFile);

            if (useCache)
            {
                Directory.GetParent(cachedPath).Create();
                File.WriteAllText(cachedPath, JsonConvert.SerializeObject(info, Formatting.Indented));
            }
            return info;
        }

        public static async Task<BuildInfo> LocalFileGetAsync(
            string name,
            string dir,
            string relativePath,
            bool fetchLatestReleaseFile = true)
        {
            string latestPackagesPath = Path.Combine(dir, relativePath, LatestPackagesTxtFilename);
            using (var packageFileStream = File.OpenRead(latestPackagesPath))
            using (var packageReader = new StreamReader(packageFileStream))
            {
                Dictionary<string, string> packages = await ReadPackageListAsync(packageReader);

                string latestReleaseVersion;
                if (fetchLatestReleaseFile)
                {
                    string latestReleasePath = Path.Combine(dir, relativePath, LatestTxtFilename);
                    latestReleaseVersion = File.ReadLines(latestReleasePath).First().Trim();
                }
                else
                {
                    latestReleaseVersion = FindLatestReleaseFromPackages(packages);
                }

                return new BuildInfo
                {
                    Name = name,
                    LatestPackages = packages,
                    LatestReleaseVersion = latestReleaseVersion
                };
            }
        }

        public static string RawBuildInfoBaseUrl(string rawRepoUrl, string gitRef, string buildInfoPath)
        {
            return $"{rawRepoUrl}/{gitRef}/{buildInfoPath}";
        }

        public static async Task<Dictionary<string, string>> ReadPackageListAsync(TextReader reader)
        {
            var packages = new Dictionary<string, string>();
            string currentLine;
            while ((currentLine = await reader.ReadLineAsync()) != null)
            {
                int spaceIndex = currentLine.IndexOf(' ');

                string id = currentLine.Substring(0, spaceIndex);
                string version = currentLine.Substring(spaceIndex + 1);

                if (packages.ContainsKey(id))
                {
                    throw new Exception($"More than one package list entry with id '{id}'.");
                }
                packages[id] = version;
            }
            return packages;
        }

        private static async Task<HttpResponseMessage> GetBuildInfoFileAsync(
            HttpClient client,
            string buildInfoName,
            string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Trace.TraceError(
                    $"Failure response code while fetching BuildInfo with name: '{buildInfoName}', " +
                    $"url: '{url}'. Ensure the repository is correct and the file exists at " +
                    "the commit specified.");
            }
            response.EnsureSuccessStatusCode();
            return response;
        }

        private static string FindLatestReleaseFromPackages(IDictionary<string, string> packages)
        {
            IEnumerable<NuGetVersion> versions = packages.Values
                .Select(versionString => new NuGetVersion(versionString));

            return
                versions.FirstOrDefault(v => v.IsPrerelease)?.Release ??
                    // if there are no prerelease versions, just grab the first version
                    versions.FirstOrDefault()?.ToNormalizedString();
        }
    }
}
