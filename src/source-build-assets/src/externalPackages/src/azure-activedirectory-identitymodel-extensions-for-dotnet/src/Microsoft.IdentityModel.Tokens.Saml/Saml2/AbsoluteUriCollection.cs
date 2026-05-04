// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
