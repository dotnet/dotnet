// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Utilities.Internal;

/// <summary>
/// Extension methods for <see cref="ReadOnlySpan{T}"/>.
/// </summary>
internal static class SpanExtensions
{
    /// <summary>
    /// Breaks the provided <paramref name="span"/> into sections based on the provided <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The input span.</param>
    /// <param name="separator">The delimiter to use.</param>
    /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
    /// <returns>A <see cref="CharSpanSplitEnumerator"/> that can be enumerated to evaluate the segments.</returns>
    internal static CharSpanSplitEnumerator Split(this ReadOnlySpan<char> span, char separator, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        return new CharSpanSplitEnumerator(span, separator, int.MaxValue, splitOptions);
    }

    /// <summary>
    /// Breaks the provided <paramref name="span"/> into sections based on the provided <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The input span.</param>
    /// <param name="separator">The delimiter to use.</param>
    /// <param name="count">The maximum number of elements to return.</param>
    /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
    /// <returns>A <see cref="CharSpanSplitEnumerator"/> that can be enumerated to evaluate the segments.</returns>
    internal static CharSpanSplitEnumerator Split(this ReadOnlySpan<char> span, char separator, int count, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return new CharSpanSplitEnumerator(span, separator, count, splitOptions);
    }

    /// <summary>
    /// Breaks the provided <paramref name="span"/> into sections based on the provided <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The input span.</param>
    /// <param name="separator">The separator to use.</param>
    /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
    /// <returns>A <see cref="CharSpanSplitEnumerator"/> that can be enumerated to evaluate the segments.</returns>
    internal static CharSpanSplitEnumerator Split(this ReadOnlySpan<char> span, ReadOnlySpan<char> separator, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        return new CharSpanSplitEnumerator(span, separator, int.MaxValue, splitOptions);
    }

    /// <summary>
    /// Breaks the provided <paramref name="span"/> into sections based on the provided <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The input span.</param>
    /// <param name="separator">The separator to use.</param>
    /// <param name="count">The maximum number of elements to return.</param>
    /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
    /// <returns>A <see cref="CharSpanSplitEnumerator"/> that can be enumerated to evaluate the segments.</returns>
    internal static CharSpanSplitEnumerator Split(this ReadOnlySpan<char> span, ReadOnlySpan<char> separator, int count, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return new CharSpanSplitEnumerator(span, separator, count, splitOptions);
    }

    /// <summary>
    /// Breaks the provided <paramref name="span"/> into sections based on the provided <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The input span.</param>
    /// <param name="separator">The separator to use.</param>
    /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
    /// <returns>A <see cref="CharSpanSplitEnumerator"/> that can be enumerated to evaluate the segments.</returns>
    internal static StringSplitEnumerator Split(this ReadOnlySpan<char> span, string separator, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        return new StringSplitEnumerator(span, separator, int.MaxValue, splitOptions);
    }

    /// <summary>
    /// Breaks the provided <paramref name="span"/> into sections based on the provided <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The input span.</param>
    /// <param name="separator">The separator to use.</param>
    /// <param name="count">The maximum number of elements to return.</param>
    /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
    /// <returns>A <see cref="CharSpanSplitEnumerator"/> that can be enumerated to evaluate the segments.</returns>
    internal static StringSplitEnumerator Split(this ReadOnlySpan<char> span, string separator, int count, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        return new StringSplitEnumerator(span, separator, count, splitOptions);
    }

    /// <summary>
    /// Breaks the provided <paramref name="span"/> into sections based on the provided <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The input span.</param>
    /// <param name="separator">The separator to use.</param>
    /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
    /// <returns>A <see cref="CharSpanSplitEnumerator"/> that can be enumerated to evaluate the segments.</returns>
    internal static StringSplitEnumerator Split(this ReadOnlySpan<char> span, ReadOnlySpan<string> separator, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        return new StringSplitEnumerator(span, separator, int.MaxValue, splitOptions);
    }

    /// <summary>
    /// Breaks the provided <paramref name="span"/> into sections based on the provided <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The input span.</param>
    /// <param name="separator">The separator to use.</param>
    /// <param name="count">The maximum number of elements to return.</param>
    /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
    /// <returns>A <see cref="CharSpanSplitEnumerator"/> that can be enumerated to evaluate the segments.</returns>
    internal static StringSplitEnumerator Split(this ReadOnlySpan<char> span, string[] separator, int count, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        return new StringSplitEnumerator(span, separator, count, splitOptions);
    }

    /// <summary>
    /// Finds the index of the first whitespace character in <paramref name="span"/>.
    /// </summary>
    /// <param name="span">The <see cref="ReadOnlySpan{Char}"/>.</param>
    /// <returns>The zero-based index of the first whitespace character or -1.</returns>
    internal static int IndexOfFirstWhitespaceCharacter(this ReadOnlySpan<char> span)
    {
        for (int i = 0; i < span.Length; ++i)
        {
            if (char.IsWhiteSpace(span[i]))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// A struct enumerator for a split span.
    /// </summary>
    internal ref struct CharSpanSplitEnumerator
    {
        private readonly StringSplitOptions splitOptions;
        private readonly ReadOnlySpan<char> separators;
        private readonly char separator;
        private readonly bool multiCharSeparator;
        private readonly bool removeEmptyEntries;
#if NET5_0_OR_GREATER
        private readonly bool trimEntries;
#endif
        private readonly ReadOnlySpan<char> originalSpan;
        private readonly int originalCount;
        private ReadOnlySpan<char> internalSpan;
        private int count;
        private bool endReached;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharSpanSplitEnumerator"/> struct.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="separator">The separator to use.</param>
        /// <param name="count">The maximum number of elements to return.</param>
        /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
        internal CharSpanSplitEnumerator(ReadOnlySpan<char> span, char separator, int count, StringSplitOptions splitOptions)
            : this(span, separator, [], multiCharSeparator: false, count, splitOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharSpanSplitEnumerator"/> struct.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="separator">The separator to use.</param>
        /// <param name="count">The maximum number of elements to return.</param>
        /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
        internal CharSpanSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> separator, int count, StringSplitOptions splitOptions)
            : this(span, default, separator, multiCharSeparator: true, count, splitOptions)
        {
        }

        private CharSpanSplitEnumerator(ReadOnlySpan<char> span, char separator, ReadOnlySpan<char> separators, bool multiCharSeparator, int count, StringSplitOptions splitOptions)
        {
            this.splitOptions = splitOptions;
            this.separator = separator;
            this.separators = separators;
            this.multiCharSeparator = multiCharSeparator;

            if (multiCharSeparator)
            {
                this.removeEmptyEntries = (splitOptions & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;
#if NET5_0_OR_GREATER
                this.trimEntries = (splitOptions & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries && !separators.IsEmpty;
#endif
            }
            else
            {
                this.removeEmptyEntries = (splitOptions & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;
#if NET5_0_OR_GREATER
                this.trimEntries = (splitOptions & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries;
#endif
            }

            this.originalSpan = span;
            this.internalSpan = span;
            this.originalCount = count;
            this.count = count;
            this.Current = default;
            this.endReached = false;
        }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        public ReadOnlySpan<char> Current { get; private set; }

        /// <summary>
        /// Gets the Enumerator.
        /// </summary>
        /// <returns><see cref="CharSpanSplitEnumerator"/>.</returns>
        public readonly CharSpanSplitEnumerator GetEnumerator() => this;

        /// <summary>
        /// Advances to the next item.
        /// </summary>
        /// <returns><see langword="bool"/> indicating if there was another item.</returns>
        public bool MoveNext()
        {
            if (this.endReached || this.count == 0)
            {
                return false;
            }

            if (this.count == 1)
            {
                return this.CalculateFinalItem();
            }

            while (true)
            {
                int separatorIndex = this.GetSeparatorIndex();

                if (separatorIndex < 0)
                {
                    this.Current = this.internalSpan;
                    this.internalSpan = [];
                    this.endReached = true;

                    return this.NextSectionFound();
                }
                else
                {
                    this.Current = this.internalSpan.Slice(0, separatorIndex);
                    this.internalSpan = this.internalSpan.Slice(separatorIndex + 1);

                    if (this.NextSectionFound())
                    {
                        --this.count;

                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Resets the <see cref="CharSpanSplitEnumerator"/> to its initial state.
        /// </summary>
        internal void Reset()
        {
            this.internalSpan = this.originalSpan;
            this.count = this.originalCount;
            this.Current = default;
            this.endReached = false;
        }

        /// <summary>
        /// Converts a the current <see cref="CharSpanSplitEnumerator"/> to an array of <see cref="string"/>.
        /// This method doesn't modify the current <see cref="CharSpanSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>The array of <see cref="string"/>.</returns>
        internal readonly string[] ToArray()
        {
            int count = this.Count();

            if (count == 0)
            {
                return [];
            }

            CharSpanSplitEnumerator toArrayEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiCharSeparator, this.originalCount, this.splitOptions);

            string[] result = new string[count];
            for (int i = 0; i < result.Length && toArrayEnumerator.MoveNext(); ++i)
            {
                result[i] = toArrayEnumerator.Current.ToString();
            }

            return result;
        }

        /// <summary>
        /// Converts a the current <see cref="CharSpanSplitEnumerator"/> to a <see cref="List{T}"/> of <see cref="string"/>.
        /// This method doesn't modify the current <see cref="CharSpanSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>.</returns>
        internal readonly List<string> ToList()
        {
            int count = this.Count();
            List<string> result = new(count);

            if (count == 0)
            {
                return result;
            }

            CharSpanSplitEnumerator toArrayEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiCharSeparator, this.originalCount, this.splitOptions);
            foreach (ReadOnlySpan<char> item in toArrayEnumerator)
            {
                result.Add(item.ToString());
            }

            return result;
        }

        /// <summary>
        /// Gets the count of elements returned by the current <see cref="CharSpanSplitEnumerator"/>.
        /// This method doesn't modify the current <see cref="CharSpanSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>A count of the results.</returns>
        internal readonly int Count()
        {
            int count = 0;
            CharSpanSplitEnumerator countEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiCharSeparator, this.originalCount, this.splitOptions);
            while (countEnumerator.MoveNext())
            {
                ++count;
            }

            return count;
        }

        /// <summary>
        /// Gets the first element returned by the current <see cref="CharSpanSplitEnumerator"/>.
        /// This method doesn't modify the current <see cref="CharSpanSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>The first result or throws if there are none.</returns>
        internal readonly ReadOnlySpan<char> First()
        {
            CharSpanSplitEnumerator firstEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiCharSeparator, this.originalCount, this.splitOptions);
            if (!firstEnumerator.MoveNext())
            {
                throw new InvalidOperationException();
            }

            return firstEnumerator.Current;
        }

        /// <summary>
        /// Gets the last element returned by the current <see cref="CharSpanSplitEnumerator"/>.
        /// This method doesn't modify the current <see cref="CharSpanSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>The last result or throws if there are none.</returns>
        internal readonly ReadOnlySpan<char> Last()
        {
            CharSpanSplitEnumerator lastEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiCharSeparator, this.originalCount, this.splitOptions);
            ReadOnlySpan<char> result = [];
            bool anyFound = false;

            foreach (ReadOnlySpan<char> section in lastEnumerator)
            {
                anyFound = true;
                result = section;
            }

            if (!anyFound)
            {
                throw new InvalidOperationException();
            }

            return result;
        }

        private readonly int GetSeparatorIndex()
        {
            if (!this.multiCharSeparator)
            {
                return this.internalSpan.IndexOf(this.separator);
            }

            if (this.separators.Length != 0)
            {
                return this.internalSpan.IndexOfAny(this.separators);
            }

            return this.internalSpan.IndexOfFirstWhitespaceCharacter();
        }

        private bool CalculateFinalItem()
        {
            if (this.removeEmptyEntries)
            {
                int i = 0;
                for (; i < this.internalSpan.Length; ++i)
                {
#if NET5_0_OR_GREATER
                    if (this.trimEntries)
                    {
                        for (; i < this.internalSpan.Length; ++i)
                        {
                            if (!char.IsWhiteSpace(this.internalSpan[i]))
                            {
                                break;
                            }
                        }

                        if (i >= this.internalSpan.Length)
                        {
                            break;
                        }
                    }
#endif
                    char currentChar = this.internalSpan[i];

                    if (this.multiCharSeparator)
                    {
                        if (!this.AnyMultiCharSeparatorMatches(currentChar))
                        {
                            break;
                        }
                    }
                    else if (currentChar != this.separator)
                    {
                        break;
                    }
                }

                if (i < this.internalSpan.Length)
                {
                    this.internalSpan = this.internalSpan.Slice(i);
                }
                else
                {
                    this.internalSpan = [];
                }
            }

            this.count = 0;
            this.endReached = true;
            this.Current = this.internalSpan;
            this.internalSpan = [];

            return this.NextSectionFound();
        }

        private bool NextSectionFound()
        {
#if NET5_0_OR_GREATER
            if (this.trimEntries)
            {
                this.Current = this.Current.Trim();
            }
#endif
            return !this.removeEmptyEntries || !this.Current.IsEmpty;
        }

        private readonly bool AnyMultiCharSeparatorMatches(char currentChar)
        {
            if (this.UseWhitespaceAsSeparator())
            {
                if (char.IsWhiteSpace(currentChar))
                {
                    return true;
                }
            }
            else
            {
                for (int i = 0; i < this.separators.Length; ++i)
                {
                    if (currentChar == this.separators[i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private readonly bool UseWhitespaceAsSeparator()
        {
            return this.separators.Length == 0;
        }
    }

    /// <summary>
    /// A struct enumerator for a split span.
    /// </summary>
    internal ref struct StringSplitEnumerator
    {
        private readonly StringSplitOptions splitOptions;
        private readonly ReadOnlySpan<string> separators;
        private readonly ReadOnlySpan<char> separator;
        private readonly bool multiStringSeparator;
        private readonly bool removeEmptyEntries;
#if NET5_0_OR_GREATER
        private readonly bool trimEntries;
#endif
        private readonly ReadOnlySpan<char> originalSpan;
        private readonly int originalCount;
        private ReadOnlySpan<char> internalSpan;
        private int count;
        private bool endReached;

#if NET5_0_OR_GREATER
        private bool firstIteration;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSplitEnumerator"/> struct.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="separator">The separator to use.</param>
        /// <param name="count">The maximum number of elements to return.</param>
        /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
        internal StringSplitEnumerator(ReadOnlySpan<char> span, string separator, int count, StringSplitOptions splitOptions)
            : this(span, separator.AsSpan(), [], multiStringSeparator: false, count, splitOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSplitEnumerator"/> struct.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="separator">The separator to use.</param>
        /// <param name="count">The maximum number of elements to return.</param>
        /// <param name="splitOptions"><see cref="StringSplitOptions"/> enum indicating how split should function.</param>
        internal StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<string> separator, int count, StringSplitOptions splitOptions)
            : this(span, [], separator, multiStringSeparator: true, count, splitOptions)
        {
        }

        private StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> separator, ReadOnlySpan<string> separators, bool multiStringSeparator, int count, StringSplitOptions splitOptions)
        {
            this.splitOptions = splitOptions;
            this.separator = separator;
            this.separators = separators;
            this.multiStringSeparator = multiStringSeparator;

            this.removeEmptyEntries = (splitOptions & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;
            this.originalSpan = span;
            this.internalSpan = span;
            this.originalCount = count;
            this.count = count;
            this.Current = default;
            this.endReached = false;

            if (multiStringSeparator)
            {
#if NET5_0_OR_GREATER
                this.trimEntries = (splitOptions & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries && this.separators.Length > 0;
                this.firstIteration = true;
#endif
            }
            else
            {
#if NET5_0_OR_GREATER
                this.trimEntries = (splitOptions & StringSplitOptions.TrimEntries) == StringSplitOptions.TrimEntries;
                this.firstIteration = true;
#endif
            }
        }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        public ReadOnlySpan<char> Current { get; private set; }

        /// <summary>
        /// Gets the Enumerator.
        /// </summary>
        /// <returns><see cref="StringSplitEnumerator"/>.</returns>
        public readonly StringSplitEnumerator GetEnumerator() => this;

        /// <summary>
        /// Advances to the next item.
        /// </summary>
        /// <returns><see langword="bool"/> indicating if there was another item.</returns>
        public bool MoveNext()
        {
            // we were passed a count of 0 and should return an empty enumerator.
            if (this.endReached || this.count == 0)
            {
                return false;
            }

            if (this.count == 1)
            {
                return this.CalculateFinalItem();
            }

            if (!this.multiStringSeparator && this.separator.IsEmpty)
            {
                this.Current = this.internalSpan;
                this.internalSpan = [];
                this.endReached = true;

                return this.NextSectionFound();
            }

            while (true)
            {
                (int separatorIndex, int separatorLength) = this.GetNextSeparatorAndLength();

                if (separatorIndex < 0 || separatorLength < 0)
                {
                    this.Current = this.internalSpan;
                    this.internalSpan = [];
                    this.endReached = true;

#if NET5_0_OR_GREATER
                    if (this.trimEntries && (!this.firstIteration || !this.multiStringSeparator))
                    {
                        this.Current = this.Current.Trim();
                    }

                    this.firstIteration = false;
#endif

                    return !this.removeEmptyEntries || !this.Current.IsEmpty;
                }
                else
                {
                    this.Current = this.internalSpan.Slice(0, separatorIndex);
                    this.internalSpan = this.internalSpan.Slice(separatorIndex + separatorLength);
#if NET5_0_OR_GREATER
                    this.firstIteration = false;
#endif
                    if (this.NextSectionFound())
                    {
                        --this.count;

                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the count of elements returned by the current <see cref="StringSplitEnumerator"/>.
        /// This method doesn't modify the current <see cref="StringSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>A count of the results.</returns>
        internal readonly int Count()
        {
            int count = 0;
            StringSplitEnumerator countEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiStringSeparator, this.originalCount, this.splitOptions);
            while (countEnumerator.MoveNext())
            {
                ++count;
            }

            return count;
        }

        /// <summary>
        /// Gets the first element returned by the current <see cref="StringSplitEnumerator"/>.
        /// This method doesn't modify the current <see cref="StringSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>The first result or throws if there are none.</returns>
        internal readonly ReadOnlySpan<char> First()
        {
            StringSplitEnumerator firstEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiStringSeparator, this.originalCount, this.splitOptions);
            if (!firstEnumerator.MoveNext())
            {
                throw new InvalidOperationException();
            }

            return firstEnumerator.Current;
        }

        /// <summary>
        /// Gets the last element returned by the current <see cref="StringSplitEnumerator"/>.
        /// This method doesn't modify the current <see cref="StringSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>The last result or throws if there are none.</returns>
        internal readonly ReadOnlySpan<char> Last()
        {
            StringSplitEnumerator lastEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiStringSeparator, this.originalCount, this.splitOptions);
            ReadOnlySpan<char> result = [];
            bool anyFound = false;
            while (lastEnumerator.MoveNext())
            {
                anyFound = true;
                result = lastEnumerator.Current;
            }

            if (!anyFound)
            {
                throw new InvalidOperationException();
            }

            return result;
        }

        /// <summary>
        /// Converts the current <see cref="StringSplitEnumerator"/> to an array of <see cref="string"/>.
        /// This method doesn't modify the current <see cref="StringSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>The array of <see cref="string"/>.</returns>
        internal readonly string[] ToArray()
        {
            int count = this.Count();

            if (count == 0)
            {
                return [];
            }

            StringSplitEnumerator toArrayEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiStringSeparator, this.originalCount, this.splitOptions);

            string[] result = new string[count];
            for (int i = 0; i < result.Length && toArrayEnumerator.MoveNext(); ++i)
            {
                result[i] = toArrayEnumerator.Current.ToString();
            }

            return result;
        }

        /// <summary>
        /// Converts the current <see cref="StringSplitEnumerator"/> to a <see cref="List{T}"/> of <see cref="string"/>
        /// This method doesn't modify the current <see cref="StringSplitEnumerator"/> and starts at the beginning.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>.</returns>
        internal readonly List<string> ToList()
        {
            int count = this.Count();

            List<string> result = new(count);
            if (count == 0)
            {
                return result;
            }

            StringSplitEnumerator toArrayEnumerator = new(this.originalSpan, this.separator, this.separators, this.multiStringSeparator, this.originalCount, this.splitOptions);
            foreach (ReadOnlySpan<char> item in toArrayEnumerator)
            {
                result.Add(item.ToString());
            }

            return result;
        }

        /// <summary>
        /// Resets the <see cref="StringSplitEnumerator"/> to its initial state.
        /// </summary>
        internal void Reset()
        {
            this.internalSpan = this.originalSpan;
            this.count = this.originalCount;
            this.Current = default;
            this.endReached = false;
        }

        private readonly (int Index, int SeparatorLength) GetNextSeparatorAndLength()
        {
            if (this.multiStringSeparator)
            {
                return this.FindFirstSeparator();
            }
            else
            {
                return (this.internalSpan.IndexOf(this.separator), this.separator.Length);
            }
        }

        private bool CalculateFinalItem()
        {
            if (this.removeEmptyEntries)
            {
                while (!this.internalSpan.IsEmpty)
                {
#if NET5_0_OR_GREATER
                    if (this.trimEntries)
                    {
                        this.internalSpan = this.internalSpan.TrimStart();
                    }
#endif

                    if (this.multiStringSeparator)
                    {
                        if (!this.AnyMultiStringSeparatorMatches())
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (!this.internalSpan.StartsWith(this.separator, StringComparison.Ordinal))
                        {
                            break;
                        }

                        this.internalSpan = this.internalSpan.Slice(this.separator.Length);
                    }
                }
            }

            this.count = 0;
            this.endReached = true;
            this.Current = this.internalSpan;
            this.internalSpan = [];

            return this.NextSectionFound();
        }

        private bool AnyMultiStringSeparatorMatches()
        {
            if (this.UseWhitespaceAsSeparator())
            {
                if (char.IsWhiteSpace(this.internalSpan[0]))
                {
                    this.internalSpan = this.internalSpan.Slice(1);
                    return true;
                }
            }
            else
            {
                for (int i = 0; i < this.separators.Length; ++i)
                {
                    ReadOnlySpan<char> separatorSpan = this.separators[i].AsSpan();
                    if (!separatorSpan.IsEmpty && this.internalSpan.StartsWith(separatorSpan, StringComparison.Ordinal))
                    {
                        this.internalSpan = this.internalSpan.Slice(separatorSpan.Length);
                        return true;
                    }
                }
            }

            return false;
        }

        private readonly bool UseWhitespaceAsSeparator()
        {
            return this.separators.Length == 0;
        }

        private bool NextSectionFound()
        {
#if NET5_0_OR_GREATER
            if (this.trimEntries)
            {
                this.Current = this.Current.Trim();
            }
#endif

            return !this.removeEmptyEntries || !this.Current.IsEmpty;
        }

        private readonly (int Index, int SeparatorLength) FindFirstSeparator()
        {
            // string.Split treats an empty array as split on whitespace.
            if (this.UseWhitespaceAsSeparator())
            {
                return (this.internalSpan.IndexOfFirstWhitespaceCharacter(), 1);
            }
            else
            {
                int index = -1;
                int separatorLength = -1;

                for (int i = 0; i < this.separators.Length; ++i)
                {
                    string currentSeparator = this.separators[i];
                    if (!string.IsNullOrEmpty(currentSeparator))
                    {
                        int currentIndex = this.internalSpan.IndexOf(this.separators[i].AsSpan());
                        if (currentIndex >= 0 && (index < 0 || currentIndex < index))
                        {
                            separatorLength = currentSeparator.Length;
                            index = currentIndex;
                        }
                    }
                }

                return (index, separatorLength);
            }
        }
    }
}
