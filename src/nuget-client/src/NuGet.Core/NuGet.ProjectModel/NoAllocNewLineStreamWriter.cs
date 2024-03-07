// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable
#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// Represents a <see cref="StreamWriter"/> that does not allocate a new string when accessing <see cref="TextWriter.NewLine" />.
    /// </summary>
    internal class NoAllocNewLineStreamWriter : StreamWriter
    {
        private string _newLine;

        public NoAllocNewLineStreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen) :
            base(stream, encoding, bufferSize, leaveOpen)
        {
            _newLine = new string(CoreNewLine);
        }

        public NoAllocNewLineStreamWriter(Stream stream) :
            base(stream)
        {
            _newLine = new string(CoreNewLine);
        }


        /// <summary>
        /// Gets or sets the line terminator string used by the current TextWriter.
        /// </summary>
        /// <remarks>
        /// The base implementation of <see cref="TextWriter.NewLine" />in .NET Framework allocates a new string every time it is accessed and looks like this:
        /// <code>
        /// public virtual string NewLine
        /// {
        ///     get
        ///     {
        ///         return new string (CoreNewLine);
        ///     }
        ///     set
        ///     {
        ///         if (value == null)
        ///         {
        ///             value = "\r\n";
        ///         }
        /// 
        ///         CoreNewLine = value.ToCharArray();
        ///     }
        /// }
        /// </code>
        ///
        /// This implementation instead returns a cached string to be used by the caller.
        /// </remarks>
        [AllowNull]
        public override string NewLine
        {
            get
            {
                return _newLine;
            }
            set
            {
                _newLine = value ?? Environment.NewLine;

                CoreNewLine = _newLine.ToCharArray();
            }
        }
    }
}
#endif
