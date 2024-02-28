// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class SyntaxNavigator
{
    private static Func<SyntaxToken, bool> GetPredicateFunction(bool includeZeroWidth)
    {
        return includeZeroWidth ? SyntaxToken.Any : SyntaxToken.NonZeroWidth;
    }

    private static bool Matches(Func<SyntaxToken, bool>? predicate, SyntaxToken token)
    {
        return predicate == null || ReferenceEquals(predicate, SyntaxToken.Any) || predicate(token);
    }

    internal static SyntaxToken? GetFirstToken(SyntaxNode current, bool includeZeroWidth)
    {
        return GetFirstToken(current, GetPredicateFunction(includeZeroWidth));
    }

    internal static SyntaxToken? GetLastToken(SyntaxNode current, bool includeZeroWidth)
    {
        return GetLastToken(current, GetPredicateFunction(includeZeroWidth));
    }

    internal static SyntaxToken? GetPreviousToken(SyntaxToken current, bool includeZeroWidth)
    {
        return GetPreviousToken(current, GetPredicateFunction(includeZeroWidth));
    }

    internal static SyntaxToken? GetNextToken(SyntaxToken current, bool includeZeroWidth)
    {
        return GetNextToken(current, GetPredicateFunction(includeZeroWidth));
    }

    internal static SyntaxToken? GetFirstToken(SyntaxNode current, Func<SyntaxToken, bool>? predicate)
    {
        using var stack = new PooledArrayBuilder<ChildSyntaxList.Enumerator>();
        stack.Push(current.ChildNodes().GetEnumerator());

        while (stack.Count > 0)
        {
            var en = stack.Pop();
            if (en.MoveNext())
            {
                var child = en.Current;

                if (child.IsToken)
                {
                    var token = GetFirstToken((SyntaxToken)child, predicate);
                    if (token != null)
                    {
                        return token;
                    }
                }

                // push this enumerator back, not done yet
                stack.Push(en);

                if (!child.IsToken)
                {
                    stack.Push(child.ChildNodes().GetEnumerator());
                }
            }
        }

        return null;
    }

    internal static SyntaxToken? GetLastToken(SyntaxNode current, Func<SyntaxToken, bool> predicate)
    {
        using var stack = new PooledArrayBuilder<ChildSyntaxList.Reversed.Enumerator>();
        stack.Push(current.ChildNodes().Reverse().GetEnumerator());

        while (stack.Count > 0)
        {
            var en = stack.Pop();

            if (en.MoveNext())
            {
                var child = en.Current;

                if (child.IsToken)
                {
                    var token = GetLastToken((SyntaxToken)child, predicate);
                    if (token != null)
                    {
                        return token;
                    }
                }

                // push this enumerator back, not done yet
                stack.Push(en);

                if (!child.IsToken)
                {
                    stack.Push(child.ChildNodes().Reverse().GetEnumerator());
                }
            }
        }

        return null;
    }

    private static SyntaxToken? GetFirstToken(SyntaxToken token, Func<SyntaxToken, bool>? predicate)
    {
        if (Matches(predicate, token))
        {
            return token;
        }

        return null;
    }

    private static SyntaxToken? GetLastToken(SyntaxToken token, Func<SyntaxToken, bool> predicate)
    {
        if (Matches(predicate, token))
        {
            return token;
        }

        return null;
    }

    internal static SyntaxToken? GetNextToken(SyntaxNode node, Func<SyntaxToken, bool>? predicate)
    {
        while (node.Parent != null)
        {
            // walk forward in parent's child list until we find ourselves and then return the
            // next token
            var returnNext = false;
            foreach (var child in node.Parent.ChildNodes())
            {
                if (returnNext)
                {
                    if (child.IsToken)
                    {
                        var token = GetFirstToken((SyntaxToken)child, predicate);
                        if (token != null)
                        {
                            return token;
                        }
                    }
                    else
                    {
                        var token = GetFirstToken(child, predicate);
                        if (token != null)
                        {
                            return token;
                        }
                    }
                }
                else if (child == node)
                {
                    returnNext = true;
                }
            }

            // didn't find the next token in my parent's children, look up the tree
            node = node.Parent;
        }

        return null;
    }

    internal static SyntaxToken? GetPreviousToken(
        SyntaxNode node,
        Func<SyntaxToken, bool> predicate)
    {
        while (node.Parent != null)
        {
            // walk forward in parent's child list until we find ourselves and then return the
            // previous token
            var returnPrevious = false;
            foreach (var child in node.Parent.ChildNodes().Reverse())
            {
                if (returnPrevious)
                {
                    if (child.IsToken)
                    {
                        var token = GetLastToken((SyntaxToken)child, predicate);
                        if (token != null)
                        {
                            return token;
                        }
                    }
                    else
                    {
                        var token = GetLastToken(child, predicate);
                        if (token != null)
                        {
                            return token;
                        }
                    }
                }
                else if (child == node)
                {
                    returnPrevious = true;
                }
            }

            // didn't find the previous token in my parent's children, look up the tree
            node = node.Parent;
        }

        return null;
    }

    internal static SyntaxToken? GetNextToken(SyntaxToken current, Func<SyntaxToken, bool>? predicate)
    {
        if (current.Parent != null)
        {
            // walk forward in parent's child list until we find ourself
            // and then return the next token
            var returnNext = false;
            foreach (var child in current.Parent.ChildNodes())
            {
                if (returnNext)
                {
                    if (child.IsToken)
                    {
                        var token = GetFirstToken((SyntaxToken)child, predicate);
                        if (token != null)
                        {
                            return token;
                        }
                    }
                    else
                    {
                        var token = GetFirstToken(child, predicate);
                        if (token != null)
                        {
                            return token;
                        }
                    }
                }
                else if (child == current)
                {
                    returnNext = true;
                }
            }

            // otherwise get next token from the parent's parent, and so on
            return GetNextToken(current.Parent, predicate);
        }

        return null;
    }

    internal static SyntaxToken? GetPreviousToken(SyntaxToken current, Func<SyntaxToken, bool> predicate)
    {
        if (current.Parent != null)
        {
            // walk forward in parent's child list until we find ourself
            // and then return the next token
            var returnPrevious = false;
            foreach (var child in current.Parent.ChildNodes().Reverse())
            {
                if (returnPrevious)
                {
                    if (child.IsToken)
                    {
                        var token = GetLastToken((SyntaxToken)child, predicate);
                        if (token != null)
                        {
                            return token;
                        }
                    }
                    else
                    {
                        var token = GetLastToken(child, predicate);
                        if (token != null)
                        {
                            return token;
                        }
                    }
                }
                else if (child == current)
                {
                    returnPrevious = true;
                }
            }

            // otherwise get next token from the parent's parent, and so on
            return GetPreviousToken(current.Parent, predicate);
        }

        return null;
    }
}
