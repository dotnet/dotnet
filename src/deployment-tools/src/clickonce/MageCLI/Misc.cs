// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Xml;
using Microsoft.Win32;
#if RUNTIME_TYPE_NETCORE
using System.Runtime.Versioning;
#endif

namespace Microsoft.Deployment.Utilities
{
    /// <summary>
    /// Constants
    /// </summary>
    /// 
    public struct Constants
    {
        public const int MAXTARGETPATH = 100;
    }

    /// <summary>
    /// Helper functions.
    /// </summary>
    /// 
    public sealed class Misc
    {
        /// <summary>
        /// Determines if exception is in the list of critical exceptions.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>true/false</returns>
        public static bool IsCriticalException(Exception ex)
        {
            return ex is NullReferenceException
                    || ex is StackOverflowException
                    || ex is OutOfMemoryException
                    || ex is System.Threading.ThreadAbortException;
        }

        /// <summary>
        /// Validates that codebase is a URL or path.
        /// </summary>
        /// <param name="codebase"></param>
        /// <returns></returns>
        private static void ValidateCodebase(string codebase)
        {
            try
            {
                ValidateUrl(codebase);
            }
            catch (UriFormatException)
            {
                // This will throw if codebase isn't a legal path
                Path.GetDirectoryName(codebase);
            }
        }

        /// <summary>
        /// Validates the URL.
        /// </summary>
        /// <param name="url"></param>
        private static void ValidateUrl(string url)
        {
            // This will throw if codebase isn't a legal URI
            Uri uri = new Uri(url);

            // This will throw if url is a URN
            if (uri.AbsoluteUri.IndexOf("://") != uri.Scheme.Length)
            {
                throw new UriFormatException();
            }
        }

        /// <summary>
        /// Validates the URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>true/false</returns>
        public static bool IsValidUrl(string url)
        {
            bool result = false;

            try
            {
                ValidateUrl(url);
                result = true;
            }
            catch (UriFormatException)
            {
                result = false;
            }

            return result;
        }


        /// <summary>
        /// As above, but returns boolean rather than throwing
        /// </summary>
        /// <param name="codebase"></param>
        /// <returns>true/false</returns>
        public static bool IsValidCodebase(string codebase)
        {
            bool result = true;
            try
            {
                ValidateCodebase(codebase);
            }
            catch (System.Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Copy an element into a document, setting the copy and its 
        /// descendants to the given namespace.  
        /// </summary>
        /// <param name="element">Element</param>
        /// <param name="document">Document</param>
        /// <param name="namespaceURI">Namespace URI</param>
        /// <returns>New element</returns>
        public static XmlElement CloneElementToDocument(XmlElement element, XmlDocument document, string namespaceURI)
        {
            XmlElement newElement = document.CreateElement(element.Name, namespaceURI);

            foreach (XmlAttribute attribute in element.Attributes)
            {
                XmlAttribute newAttribute = document.CreateAttribute(attribute.Name);
                newAttribute.Value = attribute.Value;
                newElement.Attributes.Append(newAttribute);
            }

            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlElement childElement = CloneElementToDocument((XmlElement)node, document, namespaceURI);
                    newElement.AppendChild(childElement);
                }
            }

            return newElement;
        }

        /// <summary>
        /// Get the default publisher name 
        /// </summary>
        /// <returns>Publisher name</returns>
#if RUNTIME_TYPE_NETCORE
        [SupportedOSPlatform("windows")]
#endif
        internal static string GetRegisteredOrganization()
        {
            string result = string.Empty;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false))
            {
                if (key != null)
                {
                    result = key.GetValue("RegisteredOrganization") as string;
                    if (result == null)
                    {
                        result = string.Empty;
                    }
                }
            }

            return result;
        }
    }
}

