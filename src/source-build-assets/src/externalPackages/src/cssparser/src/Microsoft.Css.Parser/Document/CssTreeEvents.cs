// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Css.Parser.Parser;

namespace Microsoft.Css.Parser.Document
{
    public class CssItemsChangedEventArgs : EventArgs
    {
        internal CssItemsChangedEventArgs(
            ParseItemList deletedItems,
            ParseItemList insertedItems,
            ParseItemList errorsChangedItems)
        {
            DeletedItems = deletedItems ?? new ParseItemList();
            InsertedItems = insertedItems ?? new ParseItemList();
            ErrorsChangedItems = errorsChangedItems ?? new ParseItemList();
        }

        public ParseItemList DeletedItems { get; private set; }
        public ParseItemList InsertedItems { get; private set; }
        public ParseItemList ErrorsChangedItems { get; private set; }
    }

    public class CssTreeUpdateEventArgs : EventArgs
    {
        internal CssTreeUpdateEventArgs(CssTree tree)
        {
            Tree = tree;
        }

        public CssTree Tree { get; private set; }
    }
}
