// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Data.Tables;

namespace Microsoft.DotNet.GitSync.CommitManager
{
    internal class Table
    {
        public Table(string accountName, string accountKey, string tableName, string repoTableName)
        {
            var tableServiceClient = new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=" + accountName + ";AccountKey=" + accountKey + ";TableEndpoint=https://" + accountName + ".table.cosmosdb.azure.com:443/;");
            CommitTable = tableServiceClient.GetTableClient(tableName);
            RepoTable = tableServiceClient.GetTableClient(repoTableName);
        }

        public TableClient CommitTable { get; set; }

        public TableClient RepoTable { get; set; }
    }
}
