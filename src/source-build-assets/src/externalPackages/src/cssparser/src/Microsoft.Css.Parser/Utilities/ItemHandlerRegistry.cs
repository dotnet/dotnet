// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Css.Parser.Utilities
{
    /// <summary>
    /// This class maps item types to a list of "handlers", which could be anything.
    /// When asking for handlers for a type, all parent type handlers are also returned
    /// (sorted with parents first)
    /// </summary>
    internal sealed class ItemHandlerRegistry<TTypeHandler> where TTypeHandler : class
    {
        private readonly List<(Type _type, List<TTypeHandler> _handlers)> _typeHandlers;
        private readonly List<(Type _type, List<TTypeHandler> _handlers)> _combinedHandlersCache;

        internal ItemHandlerRegistry()
        {
            _typeHandlers = new List<(Type _type, List<TTypeHandler> _handlers)>();

            _combinedHandlersCache = new List<(Type _type, List<TTypeHandler> _handlers)>();
        }

        internal bool RegisterHandler(Type type, TTypeHandler handler)
        {
            List<TTypeHandler> existingHandlers = null;

            for (int i = 0; i < _typeHandlers.Count; i++)
            {
                if (_typeHandlers[i]._type == type)
                {
                    existingHandlers = _typeHandlers[i]._handlers;
                    break;
                }
            }

            if (existingHandlers == null)
            {
                existingHandlers = new List<TTypeHandler>();

                _typeHandlers.Add((type, existingHandlers));
            }

            existingHandlers.Add(handler);
            _combinedHandlersCache.Clear();

            return true;
        }

        /// <summary>
        /// The returned handlers are sorted by base type. Parent types are before child types.
        /// </summary>
        internal IList<TTypeHandler> GetHandlers(Type type)
        {
            List<TTypeHandler> combinedHandlers = null;

            for (int i = 0; i < _combinedHandlersCache.Count; i++)
            {
                if (_combinedHandlersCache[i]._type == type)
                {
                    combinedHandlers = _combinedHandlersCache[i]._handlers;
                    break;
                }
            }

            if (combinedHandlers == null)
            {
                combinedHandlers = new List<TTypeHandler>();

                for (Type curType = type; curType != null; curType = curType.BaseType)
                {
                    for (int i = 0; i < _typeHandlers.Count; i++)
                    {
                        if (_typeHandlers[i]._type == curType)
                        {
                            combinedHandlers.InsertRange(0, _typeHandlers[i]._handlers);
                        }
                    }
                }

                if (combinedHandlers.Count == 0)
                {
                    combinedHandlers = null;
                }

                _combinedHandlersCache.Add((type, combinedHandlers));
            }

            return combinedHandlers;
        }

        internal IList<TTypeHandler> GetHandlers()
        {
            List<TTypeHandler> combinedHandlers = new List<TTypeHandler>();

            for (int i = 0; i < _typeHandlers.Count; i++)
            {
                combinedHandlers.AddRange(_typeHandlers[i]._handlers);
            }

            return combinedHandlers;
        }
    }
}
