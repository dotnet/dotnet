// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.ArcadeLogging
{
    /// <summary>
    /// Logger that translates MSBuild errors and warnings into the Azure Pipelines task
    /// logging format so that they surface as issues on the build timeline.
    ///
    /// https://github.com/Microsoft/azure-pipelines-tasks/blob/601dd2f0a3e671b19b55bcf139f554a09f3414da/docs/authoring/commands.md
    /// </summary>
    public sealed class PipelinesLogger : ILogger
    {
        public LoggerVerbosity Verbosity { get; set; }
        public string Parameters { get; set; }

        public void Initialize(IEventSource eventSource)
        {
            eventSource.ErrorRaised += OnErrorRaised;
            eventSource.WarningRaised += OnWarningRaised;
        }

        public void Shutdown()
        {
        }

        private static void LogIssue(
            bool isError,
            string sourceFilePath,
            int line,
            int column,
            string code,
            string message)
        {
            var builder = new StringBuilder();
            builder.Append("##vso[task.logissue ");
            builder.Append($"type={(isError ? "error" : "warning")};");
            builder.Append($"sourcepath={Escape(sourceFilePath)};");
            builder.Append($"linenumber={line};");
            builder.Append($"columnnumber={column};");
            builder.Append($"code={Escape(code)};");
            builder.Append("]");
            builder.Append(Escape(message));
            Console.WriteLine(builder.ToString());
        }

        private void OnErrorRaised(object sender, BuildErrorEventArgs e) =>
            LogIssue(isError: true, e.File, e.LineNumber, e.ColumnNumber, e.Code, e.Message);

        private void OnWarningRaised(object sender, BuildWarningEventArgs e) =>
            LogIssue(isError: false, e.File, e.LineNumber, e.ColumnNumber, e.Code, e.Message);

        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var result = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                switch (c)
                {
                    case '%':
                        result.Append("%25");
                        break;
                    case ';':
                        result.Append("%3B");
                        break;
                    case '\r':
                        result.Append("%0D");
                        break;
                    case '\n':
                        result.Append("%0A");
                        break;
                    case ']':
                        result.Append("%5D");
                        break;
                    default:
                        result.Append(c);
                        break;
                }
            }

            return result.ToString();
        }
    }
}
