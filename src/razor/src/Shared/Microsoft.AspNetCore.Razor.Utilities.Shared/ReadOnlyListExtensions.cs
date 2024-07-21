// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace System.Collections.Generic;

internal static class ReadOnlyListExtensions
{
    public static ImmutableArray<TResult> SelectAsArray<T, TResult>(this IReadOnlyList<T> source, Func<T, TResult> selector)
    {
        return source switch
        {
            [] => [],
            [var item] => [selector(item)],
            [var item1, var item2] => [selector(item1), selector(item2)],
            [var item1, var item2, var item3] => [selector(item1), selector(item2), selector(item3)],
            [var item1, var item2, var item3, var item4] => [selector(item1), selector(item2), selector(item3), selector(item4)],
            var items => BuildResult(items, selector)
        };

        static ImmutableArray<TResult> BuildResult(IReadOnlyList<T> items, Func<T, TResult> selector)
        {
            using var results = new PooledArrayBuilder<TResult>(capacity: items.Count);

            for (var i = 0; i < items.Count; i++)
            {
                results.Add(selector(items[i]));
            }

            return results.DrainToImmutable();
        }
    }

    /// <summary>
    ///  Determines whether a list contains any elements.
    /// </summary>
    /// <param name="list">
    ///  The <see cref="IReadOnlyList{T}"/> to check for emptiness.
    /// </param>
    /// <returns>
    ///  <see langword="true"/> if the list contains any elements; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Any<T>(this IReadOnlyList<T> list)
        => list.Count > 0;

    /// <summary>
    ///  Determines whether any element of a list satisfies a condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> whose elements to apply the predicate to.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  <see langword="true"/> if the list is not empty and at least one of its elements passes
    ///  the test in the specified predicate; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Any<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
    {
        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///  Determines whether any element of a list satisfies a condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> whose elements to apply the predicate to.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  <see langword="true"/> if the list is not empty and at least one of its elements passes
    ///  the test in the specified predicate; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Any<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate)
    {
        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item, arg))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///  Determines whether all elements of a list satisfy a condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> whose elements to apply the predicate to.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  <see langword="true"/> if every element of the list passes the test
    ///  in the specified predicate, or if the list is empty; otherwise,
    ///  <see langword="false"/>.</returns>
    public static bool All<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
    {
        foreach (var item in list.AsEnumerable())
        {
            if (!predicate(item))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///  Determines whether all elements of a list satisfy a condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> whose elements to apply the predicate to.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  <see langword="true"/> if every element of the list passes the test
    ///  in the specified predicate, or if the list is empty; otherwise,
    ///  <see langword="false"/>.</returns>
    public static bool All<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate)
    {
        foreach (var item in list.AsEnumerable())
        {
            if (!predicate(item, arg))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///  Returns the first element of a list.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the first element of.
    /// </param>
    /// <returns>
    ///  The first element in the specified list.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  The list is empty.
    /// </exception>
    public static T First<T>(this IReadOnlyList<T> list)
        => list.Count > 0 ? list[0] : ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_elements);

    /// <summary>
    ///  Returns the first element in a list that satisfies a specified condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  The first element in the list that passes the test in the specified predicate function.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  No element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T First<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
    {
        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item))
            {
                return item;
            }
        }

        return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_matching_elements);
    }

    /// <summary>
    ///  Returns the first element in a list that satisfies a specified condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  The first element in the list that passes the test in the specified predicate function.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  No element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T First<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate)
    {
        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item, arg))
            {
                return item;
            }
        }

        return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_matching_elements);
    }

    /// <summary>
    ///  Returns the first element of a list, or a default value if no element is found.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the first element of.
    /// </param>
    /// <returns>
    ///  <see langword="default"/>(<typeparamref name="T"/>) if <paramref name="list"/> is empty; otherwise,
    ///  the first element in <paramref name="list"/>.
    /// </returns>
    public static T? FirstOrDefault<T>(this IReadOnlyList<T> list)
        => list.Count > 0 ? list[0] : default;

    /// <summary>
    ///  Returns the first element of a list, or a specified default value if the list contains no elements.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the first element of.
    /// </param>
    /// <param name="defaultValue">
    ///  The default value to return if the list is empty
    /// </param>
    /// <returns>
    ///  <paramref name="defaultValue"/> if <paramref name="list"/> is empty; otherwise,
    ///  the first element in <paramref name="list"/>.
    /// </returns>
    public static T FirstOrDefault<T>(this IReadOnlyList<T> list, T defaultValue)
        => list.Count > 0 ? list[0] : defaultValue;

    /// <summary>
    ///  Returns the first element of the list that satisfies a condition or a default value if no such element is found.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  <see langword="default"/>(<typeparamref name="T"/>) if <paramref name="list"/> is empty or if no element
    ///  passes the test specified by <paramref name="predicate"/>; otherwise, the first element in <paramref name="list"/>
    ///  that passes the test specified by <paramref name="predicate"/>.
    /// </returns>
    public static T? FirstOrDefault<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
    {
        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item))
            {
                return item;
            }
        }

        return default;
    }

    /// <summary>
    ///  Returns the first element of the list that satisfies a condition or a default value if no such element is found.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  <see langword="default"/>(<typeparamref name="T"/>) if <paramref name="list"/> is empty or if no element
    ///  passes the test specified by <paramref name="predicate"/>; otherwise, the first element in <paramref name="list"/>
    ///  that passes the test specified by <paramref name="predicate"/>.
    /// </returns>
    public static T? FirstOrDefault<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate)
    {
        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item, arg))
            {
                return item;
            }
        }

        return default;
    }

    /// <summary>
    ///  Returns the last element of a list.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the last element of.
    /// </param>
    /// <returns>
    ///  The value at the last position in the list.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  The list is empty.
    /// </exception>
    public static T Last<T>(this IReadOnlyList<T> list)
        => list.Count > 0 ? list[^1] : ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_elements);

    /// <summary>
    ///  Returns the last element of a list that satisfies a specified condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  The last element in the list that passes the test in the specified predicate function.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  No element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T Last<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
    {
        foreach (var item in list.AsEnumerable().Reverse())
        {
            if (predicate(item))
            {
                return item;
            }
        }

        return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_matching_elements);
    }

    /// <summary>
    ///  Returns the last element of a list that satisfies a specified condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  The last element in the list that passes the test in the specified predicate function.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  No element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T Last<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate)
    {
        foreach (var item in list.AsEnumerable().Reverse())
        {
            if (predicate(item, arg))
            {
                return item;
            }
        }

        return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_matching_elements);
    }

    /// <summary>
    ///  Returns the last element of a list, or a default value if the list contains no elements.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the last element of.
    /// </param>
    /// <returns>
    ///  <see langword="default"/>(<typeparamref name="T"/>) if <paramref name="list"/> is empty; otherwise,
    ///  the last element in <paramref name="list"/>.
    /// </returns>
    public static T? LastOrDefault<T>(this IReadOnlyList<T> list)
        => list.Count > 0 ? list[^1] : default;

    /// <summary>
    ///  Returns the last element of a list, or a specified default value if the list contains no elements.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the last element of.
    /// </param>
    /// <param name="defaultValue">
    ///  The default value to return if the list is empty
    /// </param>
    /// <returns>
    ///  <paramref name="defaultValue"/> if <paramref name="list"/> is empty; otherwise,
    ///  the last element in <paramref name="list"/>.
    /// </returns>
    public static T LastOrDefault<T>(this IReadOnlyList<T> list, T defaultValue)
        => list.Count > 0 ? list[^1] : defaultValue;

    /// <summary>
    ///  Returns the last element of a list that satisfies a condition or a default value if no such element is found.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  <see langword="default"/>(<typeparamref name="T"/>) if <paramref name="list"/> is empty or if no element
    ///  passes the test specified by <paramref name="predicate"/>; otherwise, the last element in <paramref name="list"/>
    ///  that passes the test specified by <paramref name="predicate"/>.
    /// </returns>
    public static T? LastOrDefault<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
    {
        foreach (var item in list.AsEnumerable().Reverse())
        {
            if (predicate(item))
            {
                return item;
            }
        }

        return default;
    }

    /// <summary>
    ///  Returns the last element of a list that satisfies a condition or a default value if no such element is found.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <returns>
    ///  <see langword="default"/>(<typeparamref name="T"/>) if <paramref name="list"/> is empty or if no element
    ///  passes the test specified by <paramref name="predicate"/>; otherwise, the last element in <paramref name="list"/>
    ///  that passes the test specified by <paramref name="predicate"/>.
    /// </returns>
    public static T? LastOrDefault<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate)
    {
        foreach (var item in list.AsEnumerable().Reverse())
        {
            if (predicate(item, arg))
            {
                return item;
            }
        }

        return default;
    }

    /// <summary>
    ///  Returns the last element of a list that satisfies a condition, or a specified default value if no such element is found.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <param name="defaultValue">
    ///  The default value to return if the list is empty.
    /// </param>
    /// <returns>
    ///  <paramref name="defaultValue"/> if <paramref name="list"/> is empty or if no element
    ///  passes the test specified by <paramref name="predicate"/>; otherwise, the last element in <paramref name="list"/>
    ///  that passes the test specified by <paramref name="predicate"/>.
    /// </returns>
    public static T LastOrDefault<T>(this IReadOnlyList<T> list, Func<T, bool> predicate, T defaultValue)
    {
        foreach (var item in list.AsEnumerable().Reverse())
        {
            if (predicate(item))
            {
                return item;
            }
        }

        return defaultValue;
    }

    /// <summary>
    ///  Returns the last element of a list that satisfies a condition, or a specified default value if no such element is found.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return an element from.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test each element for a condition.
    /// </param>
    /// <param name="defaultValue">
    ///  The default value to return if the list is empty.
    /// </param>
    /// <returns>
    ///  <paramref name="defaultValue"/> if <paramref name="list"/> is empty or if no element
    ///  passes the test specified by <paramref name="predicate"/>; otherwise, the last element in <paramref name="list"/>
    ///  that passes the test specified by <paramref name="predicate"/>.
    /// </returns>
    public static T LastOrDefault<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate, T defaultValue)
    {
        foreach (var item in list.AsEnumerable().Reverse())
        {
            if (predicate(item, arg))
            {
                return item;
            }
        }

        return defaultValue;
    }

    /// <summary>
    ///  Returns the only element of a list, and throws an exception if there is not exactly one element in the list.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the single element of.
    /// </param>
    /// <returns>
    ///  The single element of the list.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  The list is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///  The list contains more than one element.
    /// </exception>
    public static T Single<T>(this IReadOnlyList<T> list)
    {
        return list.Count switch
        {
            1 => list[0],
            0 => ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_elements),
            _ => ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_element)
        };
    }

    /// <summary>
    ///  Returns the only element of a list that satisfies a specified condition,
    ///  and throws an exception if more than one such element exists.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return a single element from.
    /// </param>
    /// <param name="predicate">
    ///  A function to test an element for a condition.
    /// </param>
    /// <returns>
    ///  The single element of the list that satisfies a condition.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  No element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///  More than one element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T Single<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
    {
        var firstSeen = false;
        T? result = default;

        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item))
            {
                if (firstSeen)
                {
                    return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_matching_element);
                }

                firstSeen = true;
                result = item;
            }
        }

        if (!firstSeen)
        {
            return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_matching_elements);
        }

        return result!;
    }

    /// <summary>
    ///  Returns the only element of a list that satisfies a specified condition,
    ///  and throws an exception if more than one such element exists.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return a single element from.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test an element for a condition.
    /// </param>
    /// <returns>
    ///  The single element of the list that satisfies a condition.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  No element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///  More than one element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T Single<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate)
    {
        var firstSeen = false;
        T? result = default;

        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item, arg))
            {
                if (firstSeen)
                {
                    return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_matching_element);
                }

                firstSeen = true;
                result = item;
            }
        }

        if (!firstSeen)
        {
            return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_no_matching_elements);
        }

        return result!;
    }

    /// <summary>
    ///  Returns the only element of a list, or a default value if the list is empty;
    ///  this method throws an exception if there is more than one element in the list.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the single element of.
    /// </param>
    /// <returns>
    ///  The single element in the list, or <see langword="default"/>(<typeparamref name="T"/>)
    ///  if the list contains no elements.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  The list contains more than one element.
    /// </exception>
    public static T? SingleOrDefault<T>(this IReadOnlyList<T> list)
    {
        return list.Count switch
        {
            1 => list[0],
            0 => default,
            _ => ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_element)
        };
    }

    /// <summary>
    ///  Returns the only element of a list, or a specified default value if the list is empty;
    ///  this method throws an exception if there is more than one element in the list.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return the single element of.
    /// </param>
    /// <param name="defaultValue">
    ///  The default value to return if the list is empty
    /// </param>
    /// <returns>
    ///  The single element in the list, or <paramref name="defaultValue"/>
    ///  if the list contains no elements.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  The list contains more than one element.
    /// </exception>
    public static T SingleOrDefault<T>(this IReadOnlyList<T> list, T defaultValue)
    {
        return list.Count switch
        {
            1 => list[0],
            0 => defaultValue,
            _ => ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_element)
        };
    }

    /// <summary>
    ///  Returns the only element of a list that satisfies a specified condition or a default value
    ///  if no such element exists; this method throws an exception if more than one element satisfies the condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return a single element from.
    /// </param>
    /// <param name="predicate">
    ///  A function to test an element for a condition.
    /// </param>
    /// <returns>
    ///  The single element of the list that satisfies the condition, or
    ///  <see langword="default"/>(<typeparamref name="T"/>) if no such element is found.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  More than one element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T? SingleOrDefault<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
    {
        var firstSeen = false;
        T? result = default;

        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item))
            {
                if (firstSeen)
                {
                    return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_matching_element);
                }

                firstSeen = true;
                result = item;
            }
        }

        return result;
    }

    /// <summary>
    ///  Returns the only element of a list that satisfies a specified condition or a default value
    ///  if no such element exists; this method throws an exception if more than one element satisfies the condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return a single element from.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test an element for a condition.
    /// </param>
    /// <returns>
    ///  The single element of the list that satisfies the condition, or
    ///  <see langword="default"/>(<typeparamref name="T"/>) if no such element is found.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  More than one element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T? SingleOrDefault<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate)
    {
        var firstSeen = false;
        T? result = default;

        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item, arg))
            {
                if (firstSeen)
                {
                    return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_matching_element);
                }

                firstSeen = true;
                result = item;
            }
        }

        return result;
    }

    /// <summary>
    ///  Returns the only element of a list that satisfies a specified condition, or a specified default value
    ///  if no such element exists; this method throws an exception if more than one element satisfies the condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return a single element from.
    /// </param>
    /// <param name="predicate">
    ///  A function to test an element for a condition.
    /// </param>
    /// <param name="defaultValue">
    ///  The default value to return if the list is empty.
    /// </param>
    /// <returns>
    ///  The single element of the list that satisfies the condition, or
    ///  <paramref name="defaultValue"/> if no such element is found.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  More than one element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T SingleOrDefault<T>(this IReadOnlyList<T> list, Func<T, bool> predicate, T defaultValue)
    {
        var firstSeen = false;
        var result = defaultValue;

        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item))
            {
                if (firstSeen)
                {
                    return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_matching_element);
                }

                firstSeen = true;
                result = item;
            }
        }

        return result;
    }

    /// <summary>
    ///  Returns the only element of a list that satisfies a specified condition, or a specified default value
    ///  if no such element exists; this method throws an exception if more than one element satisfies the condition.
    /// </summary>
    /// <param name="list">
    ///  An <see cref="IReadOnlyList{T}"/> to return a single element from.
    /// </param>
    /// <param name="arg">
    ///  An argument to pass to <paramref name="predicate"/>.
    /// </param>
    /// <param name="predicate">
    ///  A function to test an element for a condition.
    /// </param>
    /// <param name="defaultValue">
    ///  The default value to return if the list is empty.
    /// </param>
    /// <returns>
    ///  The single element of the list that satisfies the condition, or
    ///  <paramref name="defaultValue"/> if no such element is found.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///  More than one element satisfies the condition in <paramref name="predicate"/>.
    /// </exception>
    public static T SingleOrDefault<T, TArg>(this IReadOnlyList<T> list, TArg arg, Func<T, TArg, bool> predicate, T defaultValue)
    {
        var firstSeen = false;
        var result = defaultValue;

        foreach (var item in list.AsEnumerable())
        {
            if (predicate(item, arg))
            {
                if (firstSeen)
                {
                    return ThrowHelper.ThrowInvalidOperationException<T>(SR.Contains_more_than_one_matching_element);
                }

                firstSeen = true;
                result = item;
            }
        }

        return result;
    }

    public static Enumerable<T> AsEnumerable<T>(this IReadOnlyList<T> list) => new(list);

    public readonly ref struct Enumerable<T>(IReadOnlyList<T> list)
    {
        public Enumerator<T> GetEnumerator() => new(list);

        public ReverseEnumerable<T> Reverse() => new(list);
    }

    public ref struct Enumerator<T>(IReadOnlyList<T> list)
    {
        private readonly IReadOnlyList<T> _list = list;
        private int _index = 0;
        private T _current = default!;

        public readonly T Current => _current;

        public bool MoveNext()
        {
            if (_index < _list.Count)
            {
                _current = _list[_index];
                _index++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _index = 0;
            _current = default!;
        }
    }

    public readonly ref struct ReverseEnumerable<T>(IReadOnlyList<T> list)
    {
        public ReverseEnumerator<T> GetEnumerator() => new(list);
    }

    public ref struct ReverseEnumerator<T>
    {
        private readonly IReadOnlyList<T> _list;
        private int _index;
        private T _current;

        public readonly T Current => _current;

        public ReverseEnumerator(IReadOnlyList<T> list)
        {
            _list = list;
            _index = _list.Count - 1;
            _current = default!;
        }

        public bool MoveNext()
        {
            if (_index >= 0)
            {
                _current = _list[_index];
                _index--;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _index = _list.Count - 1;
            _current = default!;
        }
    }
}
