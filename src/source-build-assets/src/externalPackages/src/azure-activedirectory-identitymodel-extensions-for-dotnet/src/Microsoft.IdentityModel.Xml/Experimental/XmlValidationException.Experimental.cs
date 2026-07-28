// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens.Experimental;

#if !NET8_0_OR_GREATER
using System.Text;
#endif

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// This exception is thrown when a problem occurs when validating the XML &lt;Signature>.
    /// </summary>
    public partial class XmlValidationException : XmlException
    {
        [NonSerialized]
        private string _stackTrace;

        [NonSerialized]
        private ValidationError _validationError;

        /// <summary>
        /// Sets the <see cref="ValidationError"/> that caused the exception.
        /// </summary>
        /// <param name="validationError"></param>
        internal void SetValidationError(ValidationError validationError)
        {
            _validationError = validationError;
        }

        /// <summary>
        /// Gets the stack trace that is captured when the exception is created.
        /// </summary>
        public override string StackTrace
        {
            get
            {
                if (_stackTrace == null)
                {
                    if (_validationError == null)
                        return base.StackTrace;
#if NET8_0_OR_GREATER
                    _stackTrace = new StackTrace(_validationError.StackFrames).ToString();
#else
                    StringBuilder sb = new();
                    foreach (StackFrame frame in _validationError.StackFrames)
                    {
                        sb.Append(frame.ToString());
                        sb.Append(Environment.NewLine);
                    }

                    _stackTrace = sb.ToString();
#endif
                }

                return _stackTrace;
            }
        }
    }
}
