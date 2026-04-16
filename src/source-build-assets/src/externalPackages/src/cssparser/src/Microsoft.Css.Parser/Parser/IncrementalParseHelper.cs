// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.Parser
{
    internal static class IncrementalParseHelper
    {
        /// <summary>
        /// All items that can incrementally parse their children should run their
        /// full parse in the same way. This function does that work.
        /// </summary>
        public static bool FullParseIncrementalItem(
            IIncrementalParseItem item,
            ItemFactory itemFactory,
            ITextProvider text,
            TokenStream tokens)
        {
            ComplexItem complexItem = (ComplexItem)item;
            ParseItem prevChild = null;

            while (true)
            {
                ParseItem newChild = item.CreateNextChild(prevChild, itemFactory, text, tokens);

                if (newChild != null)
                {
                    complexItem.Children.Add(newChild);
                    prevChild = newChild;
                }
                else
                {
                    break;
                }
            }

            item.UpdateCachedChildren();
            item.UpdateParseErrors();

            return complexItem.Children.Count > 0;
        }
    }
}
