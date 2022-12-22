using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;

namespace Microsoft.DotNet.ArcadeLogging
{
    /// <summary>
    /// Logger for converting MSBuild error messages to the Azure Pipelines Tasks format
    /// 
    /// https://github.com/Microsoft/azure-pipelines-tasks/blob/601dd2f0a3e671b19b55bcf139f554a09f3414da/docs/authoring/commands.md
    /// </summary>
    public sealed class PipelinesLogger : ILogger
    {
        private readonly MessageBuilder _builder = new MessageBuilder();
        private readonly Dictionary<BuildEventContext, Guid> _buildEventContextMap = new Dictionary<BuildEventContext, Guid>(BuildEventContextComparer.Instance);
        private readonly Dictionary<Guid, ProjectInfo> _projectInfoMap = new Dictionary<Guid, ProjectInfo>();
        private readonly Dictionary<Guid, TelemetryTaskInfo> _taskTelemetryInfoMap = new Dictionary<Guid, TelemetryTaskInfo>();
        private readonly HashSet<Guid> _detailedLoggedSet = new HashSet<Guid>();
        private HashSet<string> _ignoredTargets;
        private string _solutionDirectory;
        public LoggerVerbosity Verbosity { get; set; }
        public string Parameters { get; set; }
        private static readonly string s_TelemetryMarker = "NETCORE_ENGINEERING_TELEMETRY";

        public void Initialize(IEventSource eventSource)
        {
            var parameters = LoggerParameters.Parse(this.Parameters);

            _solutionDirectory = parameters["SolutionDir"];

            var verbosityString = parameters["Verbosity"];
            Verbosity = !string.IsNullOrEmpty(verbosityString) && Enum.TryParse(verbosityString, out LoggerVerbosity verbosity)
                ? verbosity
                : LoggerVerbosity.Normal;

            var ignoredTargets = new string[]
            {
                "GetCopyToOutputDirectoryItems",
                "GetNativeManifest",
                "GetTargetPath",
                "GetTargetFrameworks",
            };
            _ignoredTargets = new HashSet<string>(ignoredTargets, StringComparer.OrdinalIgnoreCase);

            // TargetsNotLogged is an optional parameter.
            var targetsNotLogged = parameters["TargetsNotLogged"];
            if (!string.IsNullOrEmpty(targetsNotLogged))
            {
                _ignoredTargets.UnionWith(targetsNotLogged.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }

            eventSource.ErrorRaised += OnErrorRaised;
            eventSource.WarningRaised += OnWarningRaised;
            eventSource.ProjectStarted += OnProjectStarted;

            IEventSource2 eventSource2 = eventSource as IEventSource2;
            eventSource2.TelemetryLogged += OnTelemetryLogged;

            if (Verbosity == LoggerVerbosity.Diagnostic)
            {
                eventSource.ProjectFinished += OnProjectFinished;
            }
        }
        public void Shutdown()
        {

        }

        private void LogErrorOrWarning(
            bool isError,
            string sourceFilePath,
            int line,
            int column,
            string code,
            string message,
            BuildEventContext buildEventContext)
        {
            var parentId = _buildEventContextMap.TryGetValue(buildEventContext, out var guid)
                ? (Guid?)guid
                : null;
            string telemetryCategory = null;
            if (parentId.HasValue)
            {
                if(_taskTelemetryInfoMap.TryGetValue(parentId.Value, out TelemetryTaskInfo telemetryInfo))
                {
                    telemetryCategory = telemetryInfo.Category;
                }
                if (string.IsNullOrEmpty(telemetryCategory))
                {
                    if (_projectInfoMap.TryGetValue(parentId.Value, out ProjectInfo projectInfo))
                    {
                        telemetryCategory = projectInfo.PropertiesCategory;
                    }
                }
            }
            _builder.Start("logissue");
            _builder.AddProperty("type", isError ? "error" : "warning");
            _builder.AddProperty("sourcepath", sourceFilePath);
            _builder.AddProperty("linenumber", line);
            _builder.AddProperty("columnnumber", column);
            _builder.AddProperty("code", code);
            if (telemetryCategory != null)
            {
                message = $"({s_TelemetryMarker}={telemetryCategory}) {message}";
            }
            _builder.Finish(message);
            Console.WriteLine(_builder.GetMessage());
        }

        private void LogDetail(
            Guid id,
            string type,
            string name,
            State state,
            Result? result = null,
            DateTimeOffset? startTime = null,
            DateTimeOffset? endTime = null,
            string order = null,
            string progress = null,
            Guid? parentId = null)
        {
            _builder.Start("logdetail");
            _builder.AddProperty("id", id);

            if (parentId != null)
            {
                _builder.AddProperty("parentid", parentId.Value);
            }

            // Certain values on logdetail can only be set once by design of VSO
            if (_detailedLoggedSet.Add(id))
            {
                _builder.AddProperty("type", type);
                _builder.AddProperty("name", name);

                if (!string.IsNullOrEmpty(order))
                {
                    _builder.AddProperty("order", order);
                }
            }

            if (startTime.HasValue)
            {
                _builder.AddProperty("starttime", startTime.Value);
            }

            if (endTime.HasValue)
            {
                _builder.AddProperty("endtime", endTime.Value);
            }

            if (!string.IsNullOrEmpty(progress))
            {
                _builder.AddProperty("progress", progress);
            }

            _builder.AddProperty("state", state.ToString());
            if (result.HasValue)
            {
                _builder.AddProperty("result", result.Value.ToString());
            }

            _builder.Finish();

            Console.WriteLine(_builder.GetMessage());
        }

        private void LogBuildEvent(
            in ProjectInfo projectInfo,
            State state,
            Result? result = null,
            DateTimeOffset? startTime = null,
            DateTimeOffset? endTime = null,
            string order = null,
            string progress = null) =>
            LogDetail(
                id: projectInfo.Id,
                type: "Build",
                name: projectInfo.Name,
                state: state,
                result: result,
                startTime: startTime,
                endTime: endTime,
                progress: progress,
                order: order,
                parentId: projectInfo.ParentId);

        private void OnErrorRaised(object sender, BuildErrorEventArgs e) =>
            LogErrorOrWarning(isError: true, e.File, e.LineNumber, e.ColumnNumber, e.Code, e.Message, e.BuildEventContext);

        private void OnWarningRaised(object sender, BuildWarningEventArgs e) =>
            LogErrorOrWarning(isError: false, e.File, e.LineNumber, e.ColumnNumber, e.Code, e.Message, e.BuildEventContext);

        private void OnTelemetryLogged(object sender, TelemetryEventArgs e)
        {
            if (e.EventName.Equals(s_TelemetryMarker))
            {
                if (!e.Properties.TryGetValue("Category", out string telemetryCategory))
                    return;

                if (!_buildEventContextMap.TryGetValue(e.BuildEventContext, out var parentId))
                    return;

                if (string.IsNullOrEmpty(telemetryCategory))
                {
                    _taskTelemetryInfoMap.Remove(parentId);
                }
                else
                {
                    var telemetryInfo = new TelemetryTaskInfo(parentId, telemetryCategory);
                    _taskTelemetryInfoMap[parentId] = telemetryInfo;
                }
            }
        }
        private void OnProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            if (!_buildEventContextMap.TryGetValue(e.BuildEventContext, out Guid projectId) ||
                !_projectInfoMap.TryGetValue(projectId, out ProjectInfo projectInfo))
            {
                return;
            }

            LogBuildEvent(
                in projectInfo,
                State.Completed,
                result: e.Succeeded ? Result.Succeeded : Result.Failed,
                startTime: projectInfo.StartTime,
                endTime: DateTimeOffset.UtcNow,
                progress: "100");
        }

        private void OnProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            if (_ignoredTargets.Contains(e.TargetNames))
            {
                return;
            }

            string propertyCategory = e.Properties?.Cast<DictionaryEntry>().LastOrDefault(p => p.Key.ToString().Equals(s_TelemetryMarker)).Value?.ToString();
            if(string.IsNullOrWhiteSpace(propertyCategory))
            {
                propertyCategory = e.GlobalProperties?.LastOrDefault(p => p.Key.ToString().Equals($"_{s_TelemetryMarker}")).Value;
            }
            var parentId = _buildEventContextMap.TryGetValue(e.ParentProjectBuildEventContext, out var guid)
            ? (Guid?)guid
            : null;

            var projectInfo = new ProjectInfo(getName(), parentId, propertyCategory);

            _buildEventContextMap[e.BuildEventContext] = projectInfo.Id;

            _projectInfoMap[projectInfo.Id] = projectInfo;

            if (Verbosity == LoggerVerbosity.Diagnostic)
            {
                LogBuildEvent(
                    in projectInfo,
                    State.Initialized,
                    startTime: projectInfo.StartTime,
                    endTime: null,
                    progress: "0");
            }
            string getName()
            {
                if(Verbosity != LoggerVerbosity.Diagnostic)
                {
                    return string.Empty;
                }
                // Note, website projects (sln file only, no proj file) emit a started event with projectFile == $"{m_solutionDirectory}\\".
                // This causes issues when attempting to get the relative path (and also Path.GetFileName returns empty string).
                var projectFile = e.ProjectFile;
                projectFile = (projectFile ?? string.Empty).TrimEnd('\\');

                // Make the name relative.
                if (!string.IsNullOrEmpty(_solutionDirectory) &&
                    projectFile.StartsWith(_solutionDirectory + @"\", StringComparison.OrdinalIgnoreCase))
                {
                    projectFile = projectFile.Substring(_solutionDirectory.Length).TrimStart('\\');
                }
                else
                {
                    try
                    {
                        projectFile = Path.GetFileName(projectFile);
                    }
                    catch (Exception)
                    {
                    }
                }

                // Default the project file.
                if (string.IsNullOrEmpty(projectFile))
                {
                    projectFile = "Unknown";
                }

                string targetFrameworkQualifier = string.Empty;
                if (e.GlobalProperties.TryGetValue("TargetFramework", out string targetFramework))
                {
                    targetFrameworkQualifier = $" - {targetFramework}";
                }

                string targetNamesQualifier = string.IsNullOrEmpty(e.TargetNames) ? string.Empty : $" ({e.TargetNames})";

                return projectFile + targetFrameworkQualifier + targetNamesQualifier;
            }
        }

        internal sealed class LoggerParameters
        {
            internal const char NameValueDelimiter = '=';
            internal const char NameValuePairDelimiter = '|';
            internal static StringComparer Comparer => StringComparer.OrdinalIgnoreCase;

            private readonly Dictionary<string, string> _parameters;

            public string this[string name] => _parameters.TryGetValue(name, out var value) ? value : string.Empty;

            public LoggerParameters(Dictionary<string, string> parameters)
            {
                _parameters = parameters;
            }

            public static LoggerParameters Parse(string paramString)
            {
                if (string.IsNullOrEmpty(paramString))
                {
                    return new LoggerParameters(new Dictionary<string, string>(Comparer));
                }

                // split the given string into name1 = value1 | name2 = value2
                string[] nameValuePairs = paramString.Split(NameValuePairDelimiter);
                var parameters = new Dictionary<string, string>(Comparer);
                foreach (string str in nameValuePairs)
                {
                    // look for the = char. URI's are value and can have = in them.
                    int valueDelimiterIndex = str.IndexOf(NameValueDelimiter);
                    if (valueDelimiterIndex >= 0)
                    {
                        // get the 2 strings.
                        string name = str.Substring(0, valueDelimiterIndex);
                        string value = str.Substring(valueDelimiterIndex + 1);
                        parameters.Add(name.Trim(), value.Trim());
                    }
                }

                return new LoggerParameters(parameters);
            }
        }

        internal readonly struct TelemetryTaskInfo
        {
            internal Guid Id { get; }
            internal string Category { get; }

            internal TelemetryTaskInfo(Guid id, string category)
            {
                Id = id;
                Category = category;
            }
        }

        internal readonly struct ProjectInfo
        {
            internal string Name { get; }
            internal Guid Id { get; }
            internal Guid? ParentId { get; }
            internal DateTimeOffset StartTime { get; }
            internal string PropertiesCategory { get; }

            internal ProjectInfo(string name, Guid? parentId, string propertiesCategory)
            {
                Name = name;
                Id = Guid.NewGuid();
                ParentId = parentId;
                StartTime = DateTimeOffset.UtcNow;
                PropertiesCategory = propertiesCategory;
            }
        }

        internal enum State
        {
            Unknown,
            Initialized,
            InProgress,
            Completed,
        }

        internal enum Result
        {
            Succeeded,
            SucceededWithIssues,
            Failed,
            Canceled,
            Skipped,
        }

        /// <summary>
        /// Compares two event contexts on ProjectContextId and NodeId only.
        /// NOTE: Copied from MSBuild ParallelLoggerHelpers.cs.
        /// </summary>
        internal sealed class BuildEventContextComparer : IEqualityComparer<BuildEventContext>
        {
            public static BuildEventContextComparer Instance { get; } = new BuildEventContextComparer();

            public bool Equals(BuildEventContext x, BuildEventContext y) =>
                x.NodeId == y.NodeId &&
                x.ProjectContextId == y.ProjectContextId;

            // This gives the low 24 bits to ProjectContextId and the high 8 to NodeId.  
            public int GetHashCode(BuildEventContext x) => x.ProjectContextId + (x.NodeId << 24);
        }
    }
}
