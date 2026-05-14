// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Xml.Linq;

namespace Xunit.ConsoleClient
{
    public class Transform
    {
        public string CommandLine;
        public string Description;
        public Action<XElement, string> OutputHandler;
    }
}
