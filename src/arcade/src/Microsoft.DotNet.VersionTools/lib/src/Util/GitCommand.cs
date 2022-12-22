// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Linq;
using Microsoft.Arcade.Common;

namespace Microsoft.DotNet.VersionTools.Util
{
    internal static class GitCommand
    {
        private static readonly ICommandFactory s_commandFactory = new CommandFactory();

        internal static ICommand Create(params string[] args) => s_commandFactory.Create("git", args);

        internal static string PorcelainStatus()
        {
            CommandResult result = Create("status", "--porcelain")
                .CaptureStdOut()
                .Execute();
            result.EnsureSuccessful();
            return result.StdOut;
        }

        internal static void Commit(string message, string authorName, string authorEmail, bool all)
        {
            string allFlag = all ? "--all" : "";

            Create("commit", allFlag, "-m", message, "--author", $"{authorName} <{authorEmail}>")
                .EnvironmentVariable("GIT_COMMITTER_NAME", authorName)
                .EnvironmentVariable("GIT_COMMITTER_EMAIL", authorEmail)
                .Execute()
                .EnsureSuccessful();
        }

        internal static void Push(string repository, string redactedRepository, string refSpec, bool force)
        {
            string forceFlag = force ? "--force" : "";

            string[] args =
            {
                "push", forceFlag, repository, refSpec
            };
            var logArgs = args.Select(arg => arg == repository ? redactedRepository : arg);

            string logMessage = ArgumentEscaper.EscapeAndConcatenateArgArrayForProcessStart(logArgs);

            Trace.TraceInformation($"EXEC {logMessage}");

            CommandResult pushResult =
                Create(args)
                    .QuietBuildReporter()  // we don't want secrets showing up in our logs
                    .CaptureStdErr() // git push will write to StdErr upon success, disable that
                    .CaptureStdOut()
                    .Execute();

            var message = $"{logMessage} exited with exit code {pushResult.ExitCode}";
            if (pushResult.ExitCode == 0)
            {
                Trace.TraceInformation($"EXEC success: {message}");
            }
            else
            {
                Trace.TraceError($"EXEC failure: {message}");
            }

            pushResult.EnsureSuccessful(suppressOutput: true);
        }

        internal static void Checkout(string path, string hash)
        {
            Trace.TraceInformation($"In '{path}', checking out '{hash}'.");
            Create("-C", path, "checkout", hash)
                .Execute()
                .EnsureSuccessful();
        }

        internal static void Fetch(
            string path,
            string repository,
            string refspec)
        {
            Create("-C", path, "fetch", repository, refspec)
                .Execute()
                .EnsureSuccessful();
        }

        internal static void FetchAll(string path)
        {
            Create("-C", path, "fetch", "--all")
                .Execute()
                .EnsureSuccessful();
        }

        internal static string LsRemoteHeads(string path, string repository, string @ref)
        {
            CommandResult result = Create("-C", path, "ls-remote", "--heads", repository, @ref)
                .CaptureStdOut()
                .Execute();
            result.EnsureSuccessful();
            return result.StdOut;
        }

        internal static string RevParse(string path, params string[] args)
        {
            CommandResult result = Create(new[] { "-C", path, "rev-parse" }.Concat(args).ToArray())
                .CaptureStdOut()
                .Execute();
            result.EnsureSuccessful();
            return result.StdOut;
        }

        internal static string SubmoduleStatusCached(string submodulePath)
        {
            CommandResult result = Create("submodule", "status", "--cached", submodulePath)
                .CaptureStdOut()
                .Execute();
            result.EnsureSuccessful();
            return result.StdOut;
        }
    }
}
