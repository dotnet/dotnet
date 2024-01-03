// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Linq;

namespace XliffTasks.Model
{
    internal static class XlfNames
    {
        public static readonly XNamespace XliffNS = "urn:oasis:names:tc:xliff:document:1.2";
        public static readonly XName Xliff = XliffNS + "xliff";
        public static readonly XName File = XliffNS + "file";
        public static readonly XName Body = XliffNS + "body";
        public static readonly XName Group = XliffNS + "group";
        public static readonly XName TransUnit = XliffNS + "trans-unit";
        public static readonly XName Source = XliffNS + "source";
        public static readonly XName Target = XliffNS + "target";
        public static readonly XName Note = XliffNS + "note";
    }
}
