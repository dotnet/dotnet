// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
