// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Web.XmlTransform
{
    // This doesn't do anything, except mark an error as having come from
    // the transformation engine
    [Serializable]
    public class XmlTransformationException : Exception
    {
        public XmlTransformationException(string message)
            : base(message) {
        }

        public XmlTransformationException(string message, Exception innerException)
            : base(message, innerException) {
        }
    }
}
