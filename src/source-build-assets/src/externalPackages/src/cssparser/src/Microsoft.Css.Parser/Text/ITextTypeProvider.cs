// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Css.Parser.Text
{
    /// <summary>
    /// This can optionally be implemented by classes that implement ITextProvider
    /// </summary>
    public interface ITextTypeProvider
    {
        TextTypes TextTypes { get; }
    }
}
