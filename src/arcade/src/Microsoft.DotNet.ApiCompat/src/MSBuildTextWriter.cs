// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.DotNet.Build.Tasks;
using System;
using System.IO;
using System.Text;

namespace Microsoft.DotNet.ApiCompat
{
    /// <summary>
    /// A text writer that uses MSBuild's logging infrastructure to log errors.
    /// </summary>
    internal class MSBuildTextWriter : TextWriter
    {
        private readonly Log _log;

        public MSBuildTextWriter(Log log) : base()
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public override Encoding Encoding => Encoding.Default;

        public override void WriteLine(string value)
        {
            _log.LogError(value);
        }

        public override void WriteLine()
        {
            _log.LogError(Environment.NewLine);
        }

        public override void Write(string value)
        {
            _log.LogError(value);
        }
    }
}
