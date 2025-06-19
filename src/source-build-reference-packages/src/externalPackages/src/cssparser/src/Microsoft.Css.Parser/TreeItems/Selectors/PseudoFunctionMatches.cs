// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.TreeItems.Functions;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    /// <summary>
    /// CSS :matches() pseudo class function
    /// </summary>
    internal sealed class PseudoFunctionMatches : Function
    {
        public PseudoFunctionMatches()
        {
        }

        protected override ParseItem CreateArgumentObject(ComplexItem parent, ItemFactory itemFactory, int argumentNumber)
        {
            return itemFactory.Create<PseudoSelectorArgument>(this);
        }
    }
}
