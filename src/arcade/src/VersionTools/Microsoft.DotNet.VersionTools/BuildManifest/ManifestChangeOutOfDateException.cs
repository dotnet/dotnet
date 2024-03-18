// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.VersionTools.BuildManifest
{
    public class ManifestChangeOutOfDateException : Exception
    {
        public ManifestChangeOutOfDateException(
            string changeBuildId,
            string remoteBuildId)
            : base(
                $"Build manifest change is out of date. Edited version '{changeBuildId}', but " +
                $"remote version is '{remoteBuildId}'.")
        {
            ChangeBuildId = changeBuildId;
            RemoteBuildId = remoteBuildId;
        }

        public string ChangeBuildId { get; }
        public string RemoteBuildId { get; }
    }
}
