// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure;
using Azure.Data.Tables;

namespace Microsoft.DotNet.GitSync.CommitManager
{
    public class CommitEntity : ITableEntity
    {
        public CommitEntity(string sourceRepo, string targetRepo, string commitId, string branch)
        {
            this.SourceRepo = sourceRepo;
            this.PartitionKey = targetRepo;
            this.RowKey = commitId;
            this.Branch = branch;
            this.PR = string.Empty;
        }

        public CommitEntity() { }
        public string SourceRepo { get; set; }
        public string Branch { get; set; }
        public bool Mirrored { get; set; }
        public string PR { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
