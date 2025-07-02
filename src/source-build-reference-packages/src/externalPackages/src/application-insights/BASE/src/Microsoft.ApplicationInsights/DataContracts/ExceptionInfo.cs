namespace Microsoft.ApplicationInsights.DataContracts
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Wrapper class for <see cref="ExceptionData"/> that lets user provide exception data without having the actual Exception object.
    /// </summary>
    internal sealed class ExceptionInfo
    {
        private readonly ExceptionData data;
        
        /// <summary>
        /// Constructs the instance of <see cref="ExceptionInfo"/>.
        /// </summary>
        public ExceptionInfo(IEnumerable<ExceptionDetailsInfo> exceptionDetailsInfoList, SeverityLevel? severityLevel, string problemId,
            IDictionary<string, string> properties, IDictionary<string, double> measurements)
        {
            this.data = new ExceptionData
            {
                exceptions = exceptionDetailsInfoList.Select(edi => edi.ExceptionDetails).ToList(),
                severityLevel = severityLevel.TranslateSeverityLevel(),
                problemId = problemId,
                properties = new ConcurrentDictionary<string, string>(properties),
                measurements = new ConcurrentDictionary<string, double>(measurements),
            };
        }

        internal ExceptionInfo(ExceptionData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Gets a list of <see cref="ExceptionDetailsInfo"/> to modify as needed.
        /// </summary>
        public IReadOnlyList<ExceptionDetailsInfo> ExceptionDetailsInfoList => this.data.exceptions.Select(ed => new ExceptionDetailsInfo(ed)).ToList().AsReadOnly();

        /// <summary>
        /// Gets or sets Exception severity level.
        /// </summary>
        public SeverityLevel? SeverityLevel
        {
            get => this.data.severityLevel.TranslateSeverityLevel();
            set => this.data.severityLevel = value.TranslateSeverityLevel();
        }

        /// <summary>
        /// Gets or sets problem id.
        /// </summary>
        public string ProblemId
        {
            get => this.data.problemId;
            set => this.data.problemId = value;
        }

        /// <summary>
        /// Gets or sets properties collection.
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get => this.data.properties;
            set => this.data.properties = value;
        }

        /// <summary>
        /// Gets or sets measurements collection.
        /// </summary>
        public IDictionary<string, double> Measurements
        {
            get => this.data.measurements;
            set => this.data.measurements = value;
        }

        internal ExceptionData Data => this.data;

        internal ExceptionInfo DeepClone()
        {
            return new ExceptionInfo(this.data.DeepClone());
        }
    }
}