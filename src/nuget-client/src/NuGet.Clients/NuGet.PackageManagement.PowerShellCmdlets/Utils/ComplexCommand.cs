// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NuGetConsole.Host
{
    /// <summary>
    /// Represents a complex (multi-line) command. This class builds a complex
    /// command and provides helpers to check command completeness.
    /// </summary>
    internal class ComplexCommand
    {
        private readonly StringBuilder _lines = new StringBuilder();
        private readonly Func<string, string, bool> _checkComplete;

        /// <summary>
        /// Creates a new complex command.
        /// </summary>
        /// <param name="checkComplete">
        /// A delegate to check complex command completeness.
        /// Expected signature: bool (allLines, lastLine)
        /// </param>
        public ComplexCommand(Func<string, string, bool> checkComplete)
        {
            UtilityMethods.ThrowIfArgumentNull(checkComplete);
            _checkComplete = checkComplete;
        }

        /// <summary>
        /// Get if currently this command is already completed.
        /// </summary>
        public bool IsComplete
        {
            get { return _lines.Length == 0; }
        }

        /// <summary>
        /// Add a new line to this complex command.
        /// </summary>
        /// <param name="line">The new line content.</param>
        /// <param name="fullCommand">
        /// The completed full command if this complex command
        /// becomes complete after appending line.
        /// </param>
        /// <returns>true if the complex command is completed with appending line.</returns>
        public bool AddLine(string line, out string fullCommand)
        {
            UtilityMethods.ThrowIfArgumentNull(line);

            // Some languages expect ending with \n.
            //
            // IronPython 2.6:
            //  "#comment"           --> IncompleteToken
            //  "#comment\ndef f():" --> Invalid

            _lines.Append(line);
            _lines.Append("\n");
            string allLines = _lines.ToString();

            if (CheckComplete(allLines, line))
            {
                Clear();
                fullCommand = allLines;
                return true;
            }
            fullCommand = null;
            return false;
        }

        /// <summary>
        /// Clear this command, discarding any previous incompleted lines if any.
        /// </summary>
        public void Clear()
        {
            _lines.Clear();
        }

        /// <summary>
        /// Check if given command lines represent a completed command. This calls
        /// the delegate given in constructor.
        /// Note: Empty string is considered a completed command. This method returns
        /// true if Empty string without calling user delegate.
        /// </summary>
        /// <param name="allLines">All lines of the command.</param>
        /// <param name="lastLine">The last line of the command.</param>
        /// <returns>true if the command is completed.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool CheckComplete(string allLines, string lastLine)
        {
            if (!string.IsNullOrEmpty(allLines))
            {
                // Do not throw from this method.
                try
                {
                    return _checkComplete(allLines, lastLine);
                }
                catch (Exception)
                {
                    // Ignore and fall through. Treat as invalid commands.
                }
            }

            return true;
        }
    }
}
