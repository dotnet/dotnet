// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DotNet.VersionTools.src.Util;

namespace Microsoft.DotNet.VersionTools.Automation.VstsApi
{
    /// <summary>
    /// Interact with VSTS by pretending it's GitHub. This class implements a basic set of
    /// functionality that enables a certain set of VersionTools functionality: auto-PR submission.
    ///
    /// Not supported: a VSTS-hosted dotnet/versions repo.
    /// </summary>
    public class VstsAdapterClient : IGitHubClient
    {
        private const string DefaultVstsApiVersion = "5.0";

        private static JsonSerializerSettings s_jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private HttpClient _httpClient;

        public GitHubAuth Auth { get; }

        /// <summary>
        /// For example, "dotnet" for the "dotnet.visualstudio.com" instance
        /// </summary>
        public string VstsInstanceName { get; }

        public VstsAdapterClient(
            GitHubAuth auth,
            string vstsInstanceName,
            string apiVersionOverride = null)
        {
            Auth = auth;
            VstsInstanceName = vstsInstanceName;

            _httpClient = X509Helper.GetHttpClientWithCertRevocation();

            _httpClient.DefaultRequestHeaders.Add(
                "Accept",
                $"application/json;api-version={apiVersionOverride ?? DefaultVstsApiVersion}");

            if (auth?.AuthToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    ClientHelpers.ToBase64($":{auth.AuthToken}"));
            }
        }

        public Task<GitHubContents> GetGitHubFileAsync(
            string path,
            GitHubProject project,
            string @ref)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetGitHubFileContentsAsync(
            string path,
            GitHubBranch branch)
        {
            try
            {
                GitHubContents file = await GetGitHubFileAsync(path, branch.Project, $"heads/{branch.Name}");
                return ClientHelpers.FromBase64(file.Content);
            }
            catch (HttpFailureResponseException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<string> GetGitHubFileContentsAsync(
            string path,
            GitHubProject project,
            string @ref)
        {
            try
            {
                GitHubContents file = await GetGitHubFileAsync(path, project, @ref);
                return ClientHelpers.FromBase64(file.Content);
            }
            catch (HttpFailureResponseException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public Task PutGitHubFileAsync(
            string fileUrl,
            string commitMessage,
            string newFileContents)
        {
            throw new NotImplementedException();
        }

        public async Task PostGitHubPullRequestAsync(
            string title,
            string description,
            GitHubBranch headBranch,
            GitHubBranch baseBranch,
            // Ignored: GitHub-only feature.
            bool maintainersCanModify)
        {
            EnsureAuthenticated();

            string createPrBody = JsonConvert.SerializeObject(new
            {
                title = title,
                description = description,
                sourceRefName = $"refs/heads/{headBranch.Name}",
                targetRefName = $"refs/heads/{baseBranch.Name}"
            }, Formatting.Indented);

            string pullUrl = $"{GitApiBaseUrl(baseBranch.Project)}pullrequests";

            var bodyContent = new StringContent(createPrBody, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await _httpClient.PostAsync(pullUrl, bodyContent))
            {
                await EnsureSuccessfulAsync(response);

                Trace.TraceInformation("Created pull request.");
                Trace.TraceInformation($"Pull request page: {await GetPullRequestUrlAsync(response)}");
            }
        }

        public async Task UpdateGitHubPullRequestAsync(
            GitHubProject project,
            int number,
            string title = null,
            string body = null,
            // Ignored: no callers try to close or reopen PRs.
            string state = null,
            // Ignored: GitHub-only feature.
            bool? maintainersCanModify = null)
        {
            EnsureAuthenticated();

            var updatePrBody = new JObject();

            if (title != null)
            {
                updatePrBody.Add(new JProperty("title", title));
            }
            if (body != null)
            {
                updatePrBody.Add(new JProperty("description", body));
            }

            string url = $"{GitApiBaseUrl(project)}pullrequests/{number}";

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(updatePrBody),
                    Encoding.UTF8,
                    "application/json")
            };

            using (HttpResponseMessage response = await _httpClient.SendAsync(request))
            {
                await EnsureSuccessfulAsync(response);

                Trace.TraceInformation($"Updated pull request #{number}.");
                Trace.TraceInformation($"Pull request page: {await GetPullRequestUrlAsync(response)}");
            }
        }

        public async Task<GitHubPullRequest> SearchPullRequestsAsync(
            GitHubProject project,
            string headPrefix,
            string author,
            // Parameter ignored: hasn't been important so far and harder in VSTS.
            string sortType = "created")
        {
            string url = $"{GitApiBaseUrl(project)}pullrequests" +
                $"?searchCriteria.sourceRefName=refs/heads/{headPrefix}" +
                $"&searchCriteria.status=active" +
                $"&searchCriteria.creatorId={author}";

            using (HttpResponseMessage response = await _httpClient.GetAsync(url))
            {
                await EnsureSuccessfulAsync(response);

                JObject queryResponse = JObject.Parse(await response.Content.ReadAsStringAsync());

                int count = queryResponse["count"].Value<int>();

                GitHubPullRequest[] prs = queryResponse["value"]
                    .Values<JObject>()
                    .Select(o => new GitHubPullRequest
                    {
                        Number = o["pullRequestId"].Value<int>(),
                        Title = o["title"].Value<string>(),
                        // Description seems optional and may not be returned.
                        Body = o["description"]?.Value<string>() ?? "",
                        Head = new GitHubHead
                        {
                            Sha = o["lastMergeSourceCommit"]["commitId"].Value<string>(),
                            Ref = o["sourceRefName"].Value<string>().Substring("refs/heads/".Length),
                            Label = o["sourceRefName"].Value<string>().Substring("refs/heads/".Length),
                            User = new GitHubUser
                            {
                                Login = o["createdBy"]["id"].Value<string>()
                            }
                        }
                    })
                    .ToArray();

                if (count == 0)
                {
                    Trace.TraceInformation($"Could not find any pull request with head {headPrefix}");
                    return null;
                }

                if (count > 1)
                {
                    IEnumerable<int> allIds = prs.Select(pr => pr.Number);
                    Trace.TraceInformation(
                        $"Found multiple pull requests with head {headPrefix}. " +
                        $"On this page, found {string.Join(", ", allIds)}");
                }

                // Get the PR with the highest ID if there are multiple, but this is not expected
                // in current VersionTools scenarios.
                return prs.OrderBy(pr => pr.Number).Last();
            }
        }

        public Task<GitHubCombinedStatus> GetStatusAsync(GitHubProject project, string @ref)
        {
            throw new NotImplementedException();
        }

        public async Task PostCommentAsync(GitHubProject project, int issueNumber, string message)
        {
            EnsureAuthenticated();

            string body = JsonConvert.SerializeObject(new
            {
                comments = new[]
                {
                    new
                    {
                        content = message
                    },
                },
                status = "closed"
            }, Formatting.Indented);

            string pullUrl = $"{GitApiBaseUrl(project)}pullrequests/{issueNumber}/threads";

            var bodyContent = new StringContent(body, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await _httpClient.PostAsync(pullUrl, bodyContent))
            {
                await EnsureSuccessfulAsync(response);
            }
        }

        public async Task<GitCommit> GetCommitAsync(GitHubProject project, string sha)
        {
            string url = $"{GitApiBaseUrl(project)}commits/{sha}";

            using (HttpResponseMessage response = await _httpClient.GetAsync(url))
            {
                Trace.TraceInformation($"Getting info about commit {sha} in {project.Segments}");

                await EnsureSuccessfulAsync(response);
                JObject o = JObject.Parse(await response.Content.ReadAsStringAsync());

                return new GitCommit
                {
                    Sha = o["commitId"].Value<string>(),
                    Message = o["comment"]?.Value<string>(),
                    HtmlUrl = o["_links"]?["web"]?["href"]?.Value<string>(),
                    Author = new GitCommitUser
                    {
                        Name = o["author"]?["name"]?.Value<string>(),
                        Email = o["author"]?["email"]?.Value<string>(),
                    },
                    Committer = new GitCommitUser
                    {
                        Name = o["committer"]?["name"]?.Value<string>(),
                        Email = o["committer"]?["email"]?.Value<string>(),
                    }
                };
            }
        }

        public async Task<GitReference> GetReferenceAsync(GitHubProject project, string @ref)
        {
            // A specific common reason to escape is '/' => '%2F'.
            string escapedRef = Uri.EscapeDataString(@ref);
            string url = $"{GitApiBaseUrl(project)}refs?filter={escapedRef}";

            using (HttpResponseMessage response = await _httpClient.GetAsync(url))
            {
                Trace.TraceInformation($"Getting info about ref {@ref} in {project.Segments}");

                await EnsureSuccessfulAsync(response);
                JObject responseRefs = JObject.Parse(await response.Content.ReadAsStringAsync());

                if (responseRefs["count"].Value<int>() == 0)
                {
                    Trace.TraceInformation($"Could not find ref '{@ref}'");
                    return null;
                }

                JObject o = responseRefs["value"].Values<JObject>().First();

                return new GitReference
                {
                    Ref = o["name"].Value<string>(),
                    Object = new GitReferenceObject
                    {
                        Sha = o["objectId"].Value<string>()
                    },
                };
            }
        }

        public Task<GitTree> PostTreeAsync(GitHubProject project, string baseTree, GitObject[] tree)
        {
            throw new NotImplementedException();
        }

        public Task<GitCommit> PostCommitAsync(
            GitHubProject project,
            string message,
            string tree,
            string[] parents)
        {
            throw new NotImplementedException();
        }

        public Task<GitReference> PostReferenceAsync(
            GitHubProject project,
            string @ref,
            string sha)
        {
            throw new NotImplementedException();
        }

        public Task<GitReference> PatchReferenceAsync(
            GitHubProject project,
            string @ref,
            string sha,
            bool force)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetMyAuthorIdAsync()
        {
            VstsProfile profile = await GetMyProfileAsync();
            return profile.Id;
        }

        public string CreateGitRemoteUrl(GitHubProject project) =>
            $"{VstsInstanceName}.visualstudio.com/{project.Owner}/_git/{project.Name}";

        public void AdjustOptionsToCapability(PullRequestOptions options)
        {
            // VSTS doesn't use GitHub-like fork owner system. See property's doc for more info.
            options.AllowBranchOnAnyRepoOwner = true;
            // VSTS adapter client doesn't support comments or reading CI status yet.
            options.TrackDiscardedCommits = false;
        }

        public async Task<VstsProfile> GetMyProfileAsync()
        {
            string url = $"https://{VstsInstanceName}.vssps.visualstudio.com/_apis/profile/profiles/me";

            using (HttpResponseMessage response = await _httpClient.GetAsync(url))
            {
                return await DeserializeSuccessfulAsync<VstsProfile>(response);
            }
        }

        private string GitApiBaseUrl(GitHubProject project) =>
            $"https://{VstsInstanceName}.visualstudio.com/" +
            $"{project.Owner}/_apis/git/" +
            $"repositories/{project.Name}/";

        private void EnsureAuthenticated()
        {
            if (Auth == null)
            {
                throw new NotSupportedException($"Authentication is required, but {nameof(Auth)} is null, indicating anonymous mode.");
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private static async Task<T> DeserializeSuccessfulAsync<T>(HttpResponseMessage response)
        {
            await EnsureSuccessfulAsync(response);

            return JsonConvert.DeserializeObject<T>(
                await response.Content.ReadAsStringAsync(),
                s_jsonSettings);
        }

        private static async Task EnsureSuccessfulAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string failureContent = await response.Content.ReadAsStringAsync();
                string message = $"Response code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})";
                if (!string.IsNullOrWhiteSpace(failureContent))
                {
                    message += $" with content: {failureContent}";
                }
                throw new HttpFailureResponseException(response.StatusCode, message, failureContent);
            }
        }

        private static async Task<string> GetPullRequestUrlAsync(HttpResponseMessage response)
        {
            JObject responseContent = JObject.Parse(await response.Content.ReadAsStringAsync());
            return responseContent["url"].Value<string>()
                .Replace("_apis/git/repositories", "_git")
                .Replace("pullRequests", "pullrequest");
        }
    }
}
