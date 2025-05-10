using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Abstractions;

namespace Microsoft.IdentityModel.Logging.Tests
{
    public class TestLogger : IIdentityLogger
    {
        readonly List<Tuple<string, EventLogLevel>> _logs = new List<Tuple<string, EventLogLevel>>();

        public bool IsLoggerEnabled { get; set; } = true;

        public bool IsEnabled(EventLogLevel logLevel)
        {
            return IsLoggerEnabled;
        }

        public void Log(LogEntry entry)
        {
            _logs.Add(new Tuple<string, EventLogLevel>(entry.Message, entry.EventLogLevel));
        }

        public bool LogStartsWith(string prefix, EventLogLevel logLevel)
        {
            if (string.IsNullOrEmpty(prefix))
                return true;

            return _logs.Any(x => x.Item1.StartsWith(prefix) && x.Item2 == logLevel);
        }

        public bool ContainsLog(string substring)
        {
            if (string.IsNullOrEmpty(substring))
                return true;

            return _logs.Any(x => x.Item1.Contains(substring));
        }

        public bool ContainsLogOfSpecificLevel(string substring, EventLogLevel logLevel)
        {
            if (string.IsNullOrEmpty(substring))
                throw new ArgumentException("Provided value is null or empty.", nameof(substring));

            return _logs.Any(x => x.Item1.Contains(substring) && x.Item2 == logLevel);
        }
    }
}
