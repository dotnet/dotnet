// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Microsoft.DotNet.VersionTools.Util;

namespace Microsoft.DotNet.VersionTools.Dependencies.Submodule
{
    public class SubmoduleDependencyInfo : IDependencyInfo
    {
        public static SubmoduleDependencyInfo Create(
            string repository,
            string @ref,
            string path,
            bool remote)
        {
            string commit;

            if (remote)
            {
                string remoteRefOutput = GitCommand.LsRemoteHeads(path, repository, @ref);

                string[] remoteRefLines = remoteRefOutput
                    .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToArray();

                if (remoteRefLines.Length != 1)
                {
                    string allRefs = "";
                    if (remoteRefLines.Length > 1)
                    {
                        allRefs = $" ({string.Join(", ", remoteRefLines)})";
                    }

                    throw new NotSupportedException(
                        $"The configured Ref '{@ref}' for '{path}' " +
                        $"must match exactly one ref on the remote, '{repository}'. " +
                        $"Matched {remoteRefLines.Length}{allRefs}. ");
                }

                commit = remoteRefLines.Single().Split('\t').First();
            }
            else
            {
                // Get the current commit of the submodule as tracked by the containing repo. This
                // ensures local changes don't interfere.
                // https://git-scm.com/docs/git-submodule/1.8.2#git-submodule---cached
                commit = GitCommand.SubmoduleStatusCached(path)
                    .Substring(1, 40);
            }

            return new SubmoduleDependencyInfo(repository, @ref, commit);
        }

        /// <summary>
        /// The target repository, in a format that works with commands like "git fetch".
        /// 
        /// For example: https://github.com/dotnet/buildtools
        /// </summary>
        public string Repository { get; }

        /// <summary>
        /// The Git reference/ref (branch or tag) this dependency info tracks.
        /// </summary>
        public string Ref { get; }

        /// <summary>
        /// The commit that Ref points to in Repository.
        /// </summary>
        public string Commit { get; }

        public SubmoduleDependencyInfo(string repository, string @ref, string commit)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (@ref == null)
            {
                throw new ArgumentNullException(nameof(@ref));
            }
            if (commit == null)
            {
                throw new ArgumentNullException(nameof(commit));
            }
            Repository = repository;
            Ref = @ref;
            Commit = commit;
        }

        public override string ToString() => $"{SimpleName}:{Ref} ({Commit})";

        public string SimpleName => Repository.Split('/').Last();

        public string SimpleVersion => Commit?.Substring(0, Math.Min(7, Commit.Length)) ?? "latest";
    }
}
