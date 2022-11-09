// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Git.IssueManager.Clients
{
    static class AzureDevOpsClient
    {
        private static readonly Regex RepositoryUriPattern = new Regex(
            @"^https://dev\.azure\.com\/(?<account>[a-zA-Z0-9]+)/(?<project>[a-zA-Z0-9-]+)/_git/(?<repo>[a-zA-Z0-9-\\.]+)");

        private static readonly Regex LegacyRepositoryUriPattern = new Regex(
            @"^https://(?<account>[a-zA-Z0-9]+)\.visualstudio\.com/(?<project>[a-zA-Z0-9-]+)/_git/(?<repo>[a-zA-Z0-9-\.]+)");

        private const string DefaultApiVersion = "5.0";

        public static async Task<string> GetCommitAuthorAsync(
            string repositoryUrl,
            string commit,
            string personalAccessToken)
        {
            (string accountName, string projectName, string repoName) = ParseRepoUri(repositoryUrl);

            using (HttpClient httpClient = GetHttpClient(accountName, projectName, personalAccessToken))
            {
                HttpRequestMessage getMessage = new HttpRequestMessage(HttpMethod.Get, $"_apis/git/repositories/{repoName}/commits?searchCriteria.ids={commit}");
                HttpResponseMessage response = await httpClient.SendAsync(getMessage);

                response.EnsureSuccessStatusCode();

                AzureDevOpsCommit commitResponse = JsonConvert.DeserializeObject<AzureDevOpsCommit>(await response.Content.ReadAsStringAsync());

                if (commitResponse == null)
                {
                    throw new Exception($"No commit with id {commit} found in '{repositoryUrl}'");
                }

                return $"Azure DevOps user: {commitResponse.Value.First().Author.Name}";
            }
        }

        private static HttpClient GetHttpClient(string accountName, string projectName, string personalAccessToken)
        {
            HttpClient client = new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true })
            {
                BaseAddress = new Uri($"https://dev.azure.com/{accountName}/{projectName}/")
            };

            client.DefaultRequestHeaders.Add(
                "Accept",
                $"application/json;api-version={DefaultApiVersion}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken))));

            return client;
        }

        private static (string accountName, string projectName, string repoName) ParseRepoUri(string repositoryUri)
        {
            Match m = RepositoryUriPattern.Match(repositoryUri);
            if (!m.Success)
            {
                m = LegacyRepositoryUriPattern.Match(repositoryUri);
                if (!m.Success)
                {
                    throw new ArgumentException(
                        "Repository URI should be in the form https://dev.azure.com/:account/:project/_git/:repo or " +
                        "https://:account.visualstudio.com/:project/_git/:repo");
                }
            }

            return (m.Groups["account"].Value,
                    m.Groups["project"].Value,
                    m.Groups["repo"].Value);
        }
    }
}
