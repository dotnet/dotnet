// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.Parser
{
    /// <summary>
    /// CSS parse item factory. Parser calls this interface when it is about to create an item.
    /// This gives a chance to external factory to create a different item instead.
    /// For example, @var: value; in LESS CSS is not a direcive but a variable declaration.
    /// LESS CSS parser can override default behavior and create variable declaration item
    /// that will be included in the tree of elements.
    /// </summary>
    public interface ICssItemFactory
    {
        /// <summary>
        /// Parser calls this interface when it is about to create an item.
        /// This gives a chance to external factory to override default behavior and
        /// create a different item instead.
        /// </summary>
        /// <param name="itemFactory">This factory</param>
        /// <param name="text">Text provider</param>
        /// <param name="tokens">Token stream that is being processed. Current stream position corresponds
        /// to the item being created</param>
        /// <param name="parent">Parent element</param>
        /// <param name="type">Suggested item type</param>
        /// <returns></returns>
        ParseItem CreateItem(ItemFactory itemFactory, ITextProvider text, TokenStream tokens, ComplexItem parent, Type type);
    }

    /// <summary>
    /// Default item factory. Parser calls this interface when it is about to create an item.
    /// Default factory will delegate call to external factory if it is present.
    /// </summary>
    public class ItemFactory
    {
        /// <summary>
        /// External item factory, if present.
        /// </summary>
        internal ICssItemFactory ExternalFactory { get; private set; }
        /// <summary>
        /// Text provider
        /// </summary>
        internal ITextProvider TextProvider { get; private set; }
        /// <summary>
        /// Token stream
        /// </summary>
        internal TokenStream TokenStream { get; private set; }

        /// <summary>
        /// The ITextProvider and TokenStream are only cached in here to reduce
        /// the parameter count to the Create* functions.
        /// </summary>
        internal ItemFactory(ITextProvider textProvider, TokenStream tokenStream)
            : this(null, textProvider, tokenStream)
        {
        }

        internal ItemFactory(ICssItemFactory externalFactory, ITextProvider textProvider, TokenStream tokenStream)
        {
            ExternalFactory = externalFactory;
            TextProvider = textProvider;
            TokenStream = tokenStream;
        }

        /// <summary>
        /// Create parse item of any type as long as it is derived from ParseItem
        /// </summary>
        /// <typeparam name="T">Type of item to create</typeparam>
        /// <param name="parent">Parent element</param>
        /// <param name="type">Suggested type to create</param>
        /// <returns></returns>
        internal ParseItem Create<T>(ComplexItem parent, Type type) where T : ParseItem, new()
        {
            ParseItem item = null;

            if (ExternalFactory != null)
            {
                item = ExternalFactory.CreateItem(this, TextProvider, TokenStream, parent, type);
            }

            if (item == null)
            {
                item = new T();
            }

            return item;
        }

        /// <summary>
        /// Create specific item. Called when parser cannot accept random type and rather
        /// needs a specific type. Returned item can be derived from type T.
        /// </summary>
        /// <typeparam name="T">Type of item to create</typeparam>
        /// <param name="parent">Parent element</param>
        /// <param name="type">Suggested item type</param>
        /// <returns></returns>
        internal T CreateSpecific<T>(ComplexItem parent, Type type) where T : ParseItem, new()
        {
            T item = default;

            if (ExternalFactory != null)
            {
                ParseItem pi = ExternalFactory.CreateItem(this, TextProvider, TokenStream, parent, type);

                if (pi != null)
                {
                    item = pi as T;
                    Debug.Assert(item != null);
                }
            }

            if (item == null)
            {
                item = new T();
            }

            return item;
        }

        internal ParseItem Create<T>(ComplexItem parent) where T : ParseItem, new()
        {
            return Create<T>(parent, typeof(T));
        }

        internal T CreateSpecific<T>(ComplexItem parent) where T : ParseItem, new()
        {
            return CreateSpecific<T>(parent, typeof(T));
        }

        internal ParseItem Create(Type type)
        {
            ParseItem item = Activator.CreateInstance(type) as ParseItem;

            return item;
        }
    }
}
