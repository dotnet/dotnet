using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.DotNet.Helix.Sdk
{
    internal class CommandPayload : IDisposable
    {
        private static readonly Encoding s_utf8NoBom = new UTF8Encoding(false);

        private readonly SendHelixJob _task;

        private readonly Lazy<DirectoryInfo> _directoryInfo = new Lazy<DirectoryInfo>(CreateDirectory);

        public DirectoryInfo Directory => _directoryInfo.Value;

        private static DirectoryInfo CreateDirectory()
        {
            var dir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
            dir.Create();
            return dir;
        }

        public CommandPayload(SendHelixJob task)
        {
            _task = task;
        }

        public string AddCommandFile(IEnumerable<string> commands)
        {
            var contents = new StringBuilder();
            string name;
            if (_task.IsPosixShell)
            {
                name = $"scripts/{Guid.NewGuid():N}/execute.sh";
                contents.Append("#!/bin/sh\n");
                contents.Append("chmod +x $HELIX_WORKITEM_ROOT/*.sh\n");
                foreach (var command in commands)
                {
                    contents.Append(command + "\n");
                }
            }
            else
            {
                name = $"scripts\\{Guid.NewGuid():N}\\execute.cmd";
                foreach (var command in commands)
                {
                    contents.Append(command + "\r\n");
                }
            }

            var scriptFile = new FileInfo(Path.Combine(Directory.FullName, name));
            scriptFile.Directory.Create();
            File.WriteAllText(scriptFile.FullName, contents.ToString(), s_utf8NoBom);
            return name;
        }

        public bool TryGetPayloadDirectory(out string directory)
        {
            if (_directoryInfo.IsValueCreated)
            {
                directory = Directory.FullName;
                return true;
            }

            directory = null;
            return false;
        }

        public void Dispose()
        {
            if (_directoryInfo.IsValueCreated)
            {
                _directoryInfo.Value.Delete(true);
            }
        }
    }
}
