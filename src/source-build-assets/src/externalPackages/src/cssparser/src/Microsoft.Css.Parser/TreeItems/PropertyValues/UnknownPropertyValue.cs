// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.PropertyValues
{
    /// <summary>
    /// Unknown (generic) property value
    /// </summary>
    internal sealed class UnknownPropertyValue : ComplexItem
    {
        public UnknownPropertyValue()
        {
        }

        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            Debug.Fail("UnknownPropertyValue.Parse whould never be called as this class is a temporary object to pass over to external factory");
            return false;
        }
    }
}
