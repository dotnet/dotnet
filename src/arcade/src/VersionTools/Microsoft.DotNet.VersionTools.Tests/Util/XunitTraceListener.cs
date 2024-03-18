// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Microsoft.DotNet.VersionTools.Tests.Util
{
    public class XunitTraceListener : TraceListener
    {
        private readonly StringBuilder _partialLine = new StringBuilder();

        public XunitTraceListener(ITestOutputHelper output)
        {
            Output = output;
        }

        public ITestOutputHelper Output { get; }

        public override void Flush()
        {
            Output.WriteLine(_partialLine.ToString());
            _partialLine.Clear();
        }

        public override void Write(string message)
        {
            _partialLine.Append(message);
        }

        public override void WriteLine(string message)
        {
            _partialLine.Append(message);
            Flush();
        }
    }
}
