// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using CommandLine;
using log4net;
using log4net.Config;

namespace Microsoft.DotNet.GitSync.CommitManager
{
    public class Program
    {
        private const string _cloudTableName = "CommitHistory";
        private const string _repoTableName = "MirrorBranchRepos";
        private static Table s_table { get; set; }
        private static Dictionary<(string, string), List<string>> s_repos { get; set; } = new Dictionary<(string, string), List<string>>();
        private static ILog s_logger = LogManager.GetLogger(typeof(Program));

        public static async Task Main(string[] args)
        {
            CommandLineOptions myOptions = null;
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(opts => myOptions = opts);

            if (myOptions != null)
            {
                await SetupAsync(myOptions.Username, myOptions.Key);
                await InsertCommitsAsync(myOptions.Repository, myOptions.Commit, myOptions.Branch);
            }
        }

        private static async Task SetupAsync(string username, string key)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            s_table = new Table(username, key, _cloudTableName, _repoTableName);
            await s_table.CommitTable.CreateIfNotExistsAsync();
            await s_table.RepoTable.CreateIfNotExistsAsync();


            string getAllMirrorPairs = TableClient.CreateQueryFilter<TableEntity>(ent => ent.PartitionKey != null);

            AsyncPageable<TableEntity> queryResultsMaxPerPage = s_table.RepoTable.QueryAsync<TableEntity>(getAllMirrorPairs);

            await foreach (Page<TableEntity> page in queryResultsMaxPerPage.AsPages())
            {
                foreach (TableEntity item in page.Values)
                {
                    s_repos.Add((item.PartitionKey, item.GetString("Branch")), item.GetString("ReposToMirrorInto").Split(';').ToList());
                    s_logger.Info($"The commits in  {item.PartitionKey} repo will be mirrored into {item.GetString("ReposToMirrorInto")} Repos");
                }
            }
        }

        private static async Task InsertCommitsAsync(string sourceRepoFullname, string commitList, string branch)
        {
            string sourceRepo = sourceRepoFullname.Split("/")[1];
            foreach (string repo in s_repos[(sourceRepo, branch)])
            {
                foreach (var commitId in commitList.Split(";"))
                {
                    CommitEntity entry = new CommitEntity(sourceRepo, repo, commitId, branch);

                    try
                    {
                        await s_table.CommitTable.AddEntityAsync<CommitEntity>(entry);
                        s_logger.Info($"Commit {commitId} added to table to get mirrored from {sourceRepo} to {repo}");
                    }
                    catch (WindowsAzure.Storage.StorageException)
                    {
                        s_logger.Warn($"The commit {commitId} already exists in {repo}");
                    }
                    catch (Exception ex)
                    {
                        s_logger.Warn($"Insert Operation for commit {commitId} for {repo}\n" + ex.Message);
                    }
                }
            }
        }
    }
}
