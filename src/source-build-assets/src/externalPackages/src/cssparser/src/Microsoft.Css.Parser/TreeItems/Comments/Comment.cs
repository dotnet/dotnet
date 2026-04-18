// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;

namespace Microsoft.Css.Parser.TreeItems.Comments
{
    /// <summary>
    /// Base class for any type of comment
    /// </summary>
    public abstract class Comment : ComplexItem
    {
        protected Comment()
        {
        }
    }
}
