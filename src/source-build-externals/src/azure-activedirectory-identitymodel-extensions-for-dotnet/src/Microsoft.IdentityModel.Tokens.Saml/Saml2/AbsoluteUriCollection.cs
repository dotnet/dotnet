//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// A collection of absolute URIs.
    /// </summary>
    internal class AbsoluteUriCollection : Collection<Uri>
    {
        public AbsoluteUriCollection() { }

        protected override void InsertItem(int index, Uri item)
        {
            if (item == null)
                throw LogArgumentNullException(nameof(item));

            if (!item.IsAbsoluteUri)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13139, item)));

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Uri item)
        {
            if (item == null)
                throw LogArgumentNullException(nameof(item));

            if (!item.IsAbsoluteUri)
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13139, item)));

            base.SetItem(index, item);
        }
    }
}
