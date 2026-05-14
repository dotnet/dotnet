// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Microsoft.Web.XmlTransform
{
    internal class XmlNodeContext
    {
        #region private data members
        private XmlNode node;
        #endregion

        public XmlNodeContext(XmlNode node) {
            this.node = node;
        }

        #region data accessors
        public XmlNode Node {
            get {
                return node;
            }
        }

        public bool HasLineInfo {
            get {
                return node is IXmlLineInfo;
            }
        }

        public int LineNumber {
            get {
                IXmlLineInfo lineInfo = node as IXmlLineInfo;
                if (lineInfo != null) {
                    return lineInfo.LineNumber;
                }
                else {
                    return 0;
                }
            }
        }

        public int LinePosition {
            get {
                IXmlLineInfo lineInfo = node as IXmlLineInfo;
                if (lineInfo != null) {
                    return lineInfo.LinePosition;
                }
                else {
                    return 0;
                }
            }
        }
        #endregion
    }
}
