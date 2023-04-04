// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Xml.XPath.XDocument")]
[assembly: System.Reflection.AssemblyDescription("System.Xml.XPath.XDocument")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Xml.XPath.XDocument")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Xml.XPath
{
    public static partial class Extensions
    {
        public static XPathNavigator CreateNavigator(this Linq.XNode node, XmlNameTable nameTable) { throw null; }

        public static XPathNavigator CreateNavigator(this Linq.XNode node) { throw null; }

        public static object XPathEvaluate(this Linq.XNode node, string expression, IXmlNamespaceResolver resolver) { throw null; }

        public static object XPathEvaluate(this Linq.XNode node, string expression) { throw null; }

        public static Linq.XElement XPathSelectElement(this Linq.XNode node, string expression, IXmlNamespaceResolver resolver) { throw null; }

        public static Linq.XElement XPathSelectElement(this Linq.XNode node, string expression) { throw null; }

        public static Collections.Generic.IEnumerable<Linq.XElement> XPathSelectElements(this Linq.XNode node, string expression, IXmlNamespaceResolver resolver) { throw null; }

        public static Collections.Generic.IEnumerable<Linq.XElement> XPathSelectElements(this Linq.XNode node, string expression) { throw null; }
    }

    public static partial class XDocumentExtensions
    {
        public static IXPathNavigable ToXPathNavigable(this Linq.XNode node) { throw null; }
    }
}