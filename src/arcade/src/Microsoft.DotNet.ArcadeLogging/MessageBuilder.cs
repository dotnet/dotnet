using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.ArcadeLogging
{
    public sealed class MessageBuilder
    {
        internal enum State
        {
            NotStarted,
            Properties,
            Finished,
        }

        private readonly StringBuilder _builder = new StringBuilder();
        private State _state;

        public void Start(string kind)
        {
            Debug.Assert(_state == State.NotStarted || _state == State.Finished);
            _builder.Length = 0;
            _builder.Append($"##vso[task.{kind} ");
            _state = State.Properties;
        }

        public void AddProperty(string name, string value)
        {
            Debug.Assert(_state == State.Properties);

            _builder.Append($"{name}={Escape(value)};");
        }

        public void AddProperty(string name, DateTimeOffset value) => AddProperty(name, value.ToString("O"));
        public void AddProperty(string name, int value) => AddProperty(name, value.ToString());
        public void AddProperty(string name, Guid value) => AddProperty(name, value.ToString("D"));

        public void Finish(string message = null)
        {
            Debug.Assert(_state == State.Properties);
            _builder.Append("]");
            if (!string.IsNullOrEmpty(message))
            {
                _builder.Append(Escape(message));
            }

            _state = State.Finished;
        }

        public string GetMessage()
        {
            Debug.Assert(_state == State.Finished);
            return _builder.ToString();
        }

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
