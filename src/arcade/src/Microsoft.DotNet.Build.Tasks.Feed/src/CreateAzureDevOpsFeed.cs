// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Core;
using Azure.Identity;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MSBuild = Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class CreateAzureDevOpsFeed : MSBuild.Task
    {
        [Output]
        public string TargetFeedURL { get; set; }

        [Output]
        public string TargetFeedName { get; set; }

        /// <summary>
        /// Organization that the feed should be created in
        /// </summary>
        [Required]
        public string AzureDevOpsOrg { get; set; }

        /// <summary>
        /// Project that that feed should be created in. The public/internal visibility
        /// of this project will determine whether the feed is public.
        /// </summary>
        [Required]
        public string AzureDevOpsProject { get; set; }

        /// <summary>
        /// Personal access token used to authorize to the API and create the feed
        /// </summary>
        public string AzureDevOpsPersonalAccessToken { get; set; }

        public string RepositoryName { get; set; }

        public string CommitSha { get; set; }

        /// <summary>
        /// In case we want to use a defined feed name instead of calculate one dynamically
        /// </summary>
        public string FeedName { get; set; }

        /// <summary>
        /// Additional info to include in the feed name (for example "sym")
        /// </summary>
        public string ContentIdentifier { get; set; }

        public string AzureDevOpsFeedsApiVersion { get; set; } = "5.1-preview.1";

        public string LocalViewVisibility { get; set; } = "collection";

        /// <summary>
        /// Number of characters from the commit SHA prefix that should be included in the feed name.
        /// </summary>
        private readonly int ShaUsableLength = 8;

        /// <summary>
        /// Maximum allowed length for AzDO feed names.
        /// </summary>
        private readonly int MaxLengthForAzDoFeedNames = 64;

        public override bool Execute()
        {
            return ExecuteAsync().GetAwaiter().GetResult();
        }

        private async Task<bool> ExecuteAsync()
        {
            try
            {
                if (CommitSha?.Length < ShaUsableLength)
                {
                    Log.LogError($"The CommitSHA should be at least {ShaUsableLength} characters long: CommitSha is '{CommitSha}'. Aborting feed creation.");
                    return false;
                }

                JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };

                // GitHub repos may appear in the repository name with an 'org/repo' form.
                // When creating a repo, Github aslready replaces all of the characters invalid in AzDO feed names (see below)
                // with '-' in the repo name. We just need to replace '/' with '-' to deal with the org/repo input.
                // From the AzDO docs:
                // The feed name can't contain spaces, start with a '.' or '_', end with a '.',
                // or contain any of these: @ ~ ; { } ' + = , < > | / \ ? : & $ * " # [ ] %
                string feedCompatibleRepositoryName = RepositoryName?.Replace('/', '-');

                // For clarity, and compatibility with existing infrastructure, we include the feed visibility tag.
                // This serves two purposes:
                // 1. In nuget.config files (and elsewhere), the name at a glance can identify its visibility
                // 2. Existing automation has knowledge of "darc-int" and "darc-pub" for purposes of injecting authentication for internal builds
                //    and managing the isolated feeds within the NuGet.config files.
                string extraContentInfo = !string.IsNullOrEmpty(ContentIdentifier) ? $"-{ContentIdentifier}" : "";
                string baseFeedName = FeedName ?? $"darc-{GetFeedVisibilityTag(AzureDevOpsOrg, AzureDevOpsProject)}{extraContentInfo}-{feedCompatibleRepositoryName}-{CommitSha.Substring(0, ShaUsableLength)}";
                string versionedFeedName = baseFeedName;
                bool needsUniqueName = false;
                int subVersion = 0;

                Log.LogMessage(MessageImportance.High, $"Creating the new Azure DevOps artifacts feed '{baseFeedName}'...");

                if (baseFeedName.Length > MaxLengthForAzDoFeedNames)
                {
                    Log.LogError($"The name of the new feed ({baseFeedName}) exceeds the maximum feed name size of 64 chars. Aborting feed creation.");
                    return false;
                }

                string azureDevOpsFeedsBaseUrl = $"https://feeds.dev.azure.com/{AzureDevOpsOrg}/";

                if (string.IsNullOrEmpty(AzureDevOpsPersonalAccessToken))
                {
                    const string AzureDevOpsScope = "499b84ac-1321-427f-aa17-267ca6975798/.default";
                    AzureDevOpsPersonalAccessToken = new AzureCliCredential().GetToken(new TokenRequestContext(new[] { AzureDevOpsScope })).Token;
                }

                do
                {
                    using (HttpClient client = new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true })
                    {
                        BaseAddress = new Uri(azureDevOpsFeedsBaseUrl)
                    })
                    {
                        client.DefaultRequestHeaders.Add(
                            "Accept",
                            $"application/json;api-version={AzureDevOpsFeedsApiVersion}");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", AzureDevOpsPersonalAccessToken))));

                        AzureDevOpsArtifactFeed newFeed = new AzureDevOpsArtifactFeed(versionedFeedName, AzureDevOpsOrg, AzureDevOpsProject);

                        string createBody = JsonConvert.SerializeObject(newFeed, _serializerSettings);

                        using HttpRequestMessage createFeedMessage = new HttpRequestMessage(HttpMethod.Post, $"{AzureDevOpsProject}/_apis/packaging/feeds");
                        createFeedMessage.Content = new StringContent(createBody, Encoding.UTF8, "application/json");
                        using HttpResponseMessage createFeedResponse = await client.SendAsync(createFeedMessage);

                        if (createFeedResponse.StatusCode == HttpStatusCode.Created)
                        {
                            needsUniqueName = false;
                            baseFeedName = versionedFeedName;

                            /// This is where we would potentially update the Local feed view with permissions to the organization's
                            /// valid users. But, see <seealso cref="AzureDevOpsArtifactFeed"/> for more info on why this is not
                            /// done this way.
                        }
                        else if (createFeedResponse.StatusCode == HttpStatusCode.Conflict)
                        {
                            versionedFeedName = $"{baseFeedName}-{++subVersion}";
                            needsUniqueName = true;

                            if (versionedFeedName.Length > MaxLengthForAzDoFeedNames)
                            {
                                Log.LogError($"The name of the new feed ({baseFeedName}) exceeds the maximum feed name size of 64 chars. Aborting feed creation.");
                                return false;
                            }
                        }
                        else
                        {
                            throw new Exception($"Feed '{baseFeedName}' was not created. Request failed with status code {createFeedResponse.StatusCode}. Exception: {await createFeedResponse.Content.ReadAsStringAsync()}");
                        }
                    }
                } while (needsUniqueName);

                TargetFeedURL = $"https://pkgs.dev.azure.com/{AzureDevOpsOrg}/{AzureDevOpsProject}/_packaging/{baseFeedName}/nuget/v3/index.json";
                TargetFeedName = baseFeedName;

                Log.LogMessage(MessageImportance.High, $"Feed '{TargetFeedURL}' created successfully!");
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// Returns a tag for feed visibility that will be added to the feed name
        /// </summary>
        /// <param name="organization">Organization containing the feed</param>
        /// <param name="project">Project within <paramref name="organization"/> containing the feed</param>
        /// <returns>Feed tag</returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GetFeedVisibilityTag(string organization, string project)
        {
            switch (organization)
            {
                case "dnceng":
                    switch (project)
                    {
                        case "internal":
                            return "int";
                        case "public":
                            return "pub";
                        default:
                            throw new NotImplementedException($"Project '{project}' within organization '{organization}' has no visibility mapping.");
                    }
                default:
                    return project.Substring(0, Math.Min(3, project.Length));
            }
        }
    }
}
