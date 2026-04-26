// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace OpenTelemetry.Instrumentation;

internal static class SqlProcessor
{
    private const int MaxSummaryLength = 255;
    private const int CacheCapacity = 1000;

    private const char SanitizationPlaceholder = '?';
    private const char SpaceChar = ' ';
    private const char CommaChar = ',';
    private const char OpenSquareBracketChar = '[';
    private const char CloseSquareBracketChar = ']';
    private const char OpenParenChar = '(';
    private const char CloseParenChar = ')';
    private const char DashChar = '-';
    private const char ForwardSlashChar = '/';
    private const char SingleQuoteChar = '\'';
    private const char AsteriskChar = '*';
    private const char UnderscoreChar = '_';
    private const char DotChar = '.';
    private const char NewLineChar = '\n';
    private const char CarriageReturnChar = '\r';
    private const char TabChar = '\t';
    private const char UnicodePrefixChar = 'N';

    private static readonly ConcurrentDictionary<string, SqlStatementInfo> Cache = new();

    private static readonly char[] WhitespaceChars = [SpaceChar, TabChar, CarriageReturnChar, NewLineChar];

#if NET
    private static readonly SearchValues<char> WhitespaceSearchValues = SearchValues.Create(WhitespaceChars);
#endif

    // This is not an exhaustive list but covers the majority of common reserved SQL keywords that may follow a FROM clause.
    // This is used when determining if the previous token is a keyword in order to identify the end of a comma separated FROM clause.
    // NOTE: These are ordered so that more likely keywords appear first to shorten the comparison loop.
    private static readonly string[] FromClauseReservedKeywords = [
        "WHERE", "BY", "AS", "JOIN", "WITH", "CROSS", "HAVING", "WINDOW", "LIMIT", "OFFSET", "TABLESAMPLE", "PIVOT", "UNPIVOT"
    ];

    private static readonly int MaxFromClauseReservedKeywordLength = FromClauseReservedKeywords.Max(k => k.Length);
    private static readonly int MinFromClauseReservedKeywordLength = FromClauseReservedKeywords.Min(k => k.Length);

    // We can extend this in the future to include more keywords if needed.
    // The keywords should be ordered by frequency of use to optimize performance.
    // This only includes keywords that may be the first keyword in a statement.
    private static readonly SqlKeywordInfo[] SqlKeywords =
    [
        SqlKeywordInfo.SelectKeyword,
        SqlKeywordInfo.InsertKeyword,
        SqlKeywordInfo.UpdateKeyword,
        SqlKeywordInfo.DeleteKeyword,
        SqlKeywordInfo.CreateKeyword,
        SqlKeywordInfo.AlterKeyword,
        SqlKeywordInfo.DropKeyword,
        SqlKeywordInfo.ExecKeyword,
        SqlKeywordInfo.ExecuteKeyword,
        SqlKeywordInfo.GrantKeyword,
        SqlKeywordInfo.DenyKeyword,
        SqlKeywordInfo.TruncateKeyword,
        SqlKeywordInfo.RevokeKeyword,
        SqlKeywordInfo.BulkKeyword,
        SqlKeywordInfo.DisableKeyword,
        SqlKeywordInfo.EnableKeyword,
        SqlKeywordInfo.BackupKeyword,
        SqlKeywordInfo.RestoreKeyword,
    ];

    // This is a special case used when handling sub-queries in parentheses.
    private static readonly SqlKeywordInfo[] SelectOnlyKeywordArray =
    [
        SqlKeywordInfo.SelectKeyword,
    ];

    // Maintain our own approximate count to avoid ConcurrentDictionary.Count on hot path.
    // We only increment on successful TryAdd. This may result in a slightly oversized cache
    // under high concurrency but this is acceptable for this scenario.
    private static int approxCacheCount;

    private enum SqlKeyword
    {
        Unknown,
        Backup,
        Bulk,
        Alter,
        Clustered,
        Connect,
        Create,
        Database,
        Delete,
        Deny,
        Disable,
        Distinct,
        Drop,
        Enable,
        Exec,
        Execute,
        Exists,
        From,
        Function,
        Grant,
        If,
        Index,
        Insert,
        Into,
        Join,
        Login,
        NonClustered,
        Not,
        On,
        Procedure,
        Restore,
        Revoke,
        Role,
        Schema,
        Select,
        Sequence,
        Statistics,
        Table,
        Trigger,
        Truncate,
        Unique,
        Union,
        Update,
        User,
        View,
    }

    public static SqlStatementInfo GetSanitizedSql(string? sql)
    {
        if (sql == null)
        {
            return default;
        }

        if (Cache.TryGetValue(sql, out var sqlStatementInfo))
        {
            return sqlStatementInfo;
        }

        sqlStatementInfo = SanitizeSql(sql);

        // Fast-path capacity check using our own approximate count to avoid ConcurrentDictionary.Count cost.
        if (Volatile.Read(ref approxCacheCount) >= CacheCapacity)
        {
            return sqlStatementInfo;
        }

        // Attempt to add when under capacity. Increment our count only on successful add.
        if (Cache.TryAdd(sql, sqlStatementInfo))
        {
            Interlocked.Increment(ref approxCacheCount);
            return sqlStatementInfo;
        }

        // If another thread added meanwhile, return the cached value if available.
        return Cache.TryGetValue(sql, out var existing) ? existing : sqlStatementInfo;
    }

#if !NET
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsAsciiLetter(char c)
    {
        var lower = (char)(c | 0x20);
        return lower >= 'a' && lower <= 'z';
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsAsciiDigit(char c) =>
#if NET
        char.IsAsciiDigit(c);
#else
        c >= '0' && c <= '9';
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsUnescapedIdentifierChar(char c) =>
#if NET
        char.IsLetter(c) || char.IsAsciiDigit(c) || c == UnderscoreChar || c == DotChar;
#else
        char.IsLetter(c) || IsAsciiDigit(c) || c == UnderscoreChar || c == DotChar;
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsValidTokenCharacter(ReadOnlySpan<char> sql, int currentPosition, int indexInToken, in ParseState state)
    {
        var currentChar = sql[currentPosition];

        // If we are not capturing the next token as an identifier, we only accept unescaped identifier characters.
        if (!state.CaptureNextNonKeywordTokenAsIdentifier)
        {
            return IsUnescapedIdentifierChar(currentChar);
        }

        if (state.InEscapedIdentifier)
        {
            // In escaped identifiers, all characters except null are valid.
            // Double closing brackets (]]) are treated as an escaped bracket within the identifier.
            // A single closing bracket ends the identifier.
            if (currentChar == '\0')
            {
                return false;
            }

            if (currentChar == CloseSquareBracketChar)
            {
                var nextPosition = currentPosition + 1;
                return nextPosition < sql.Length && sql[nextPosition] == CloseSquareBracketChar;
            }

            return true;
        }

        // In unescaped identifiers, periods are invalid at the start but valid in the middle (for schema-qualified names).
        return (currentChar != DotChar || indexInToken != 0) && IsUnescapedIdentifierChar(currentChar);
    }

    private static SqlStatementInfo SanitizeSql(string sql)
    {
        var sqlSpan = sql.AsSpan();

        // We use a single buffer for both sanitized SQL and DB query summary.
        // We rent a buffer twice the size of the input SQL to ensure
        // we have enough space for the sanitized SQL and summary. The summary starts
        // from the middle position of the rented buffer.
        var rentedBuffer = ArrayPool<char>.Shared.Rent(sqlSpan.Length * 2);

        var buffer = rentedBuffer.AsSpan();

        ParseState state = default;

        // Precompute the summary buffer slice once and carry it via state to avoid repeated Span.Slice calls.
        state.SummaryBuffer = buffer.Slice(rentedBuffer.Length / 2);

        while (state.ParsePosition < sqlSpan.Length)
        {
            if (SkipComment(sqlSpan, ref state))
            {
                continue;
            }

            if (SanitizeStringLiteral(sqlSpan, buffer, ref state) ||
                SanitizeHexLiteral(sqlSpan, buffer, ref state) ||
                SanitizeNumericLiteral(sqlSpan, buffer, ref state))
            {
                continue;
            }

            if (ParseWhitespace(sqlSpan, buffer, ref state))
            {
                continue;
            }

            if (state.SummaryPosition >= MaxSummaryLength)
            {
                ParseNextTokenFast(sqlSpan, buffer, ref state);
            }
            else
            {
                ParseNextToken(sqlSpan, buffer, ref state);
            }
        }

        var summary = state.SummaryBuffer.Slice(0, state.SummaryPosition);

        // If we have exceeded the max length for the summary, find the index of the last whitespace
        // and trim the summary to that position. This avoids truncating within an operation name or target.
        if (state.SummaryPosition > MaxSummaryLength)
        {
#if NET
            var indexOfLastWhitespace = summary.Slice(0, MaxSummaryLength).LastIndexOfAny(WhitespaceSearchValues);
#else
            var indexOfLastWhitespace = summary.Slice(0, MaxSummaryLength).LastIndexOfAny(WhitespaceChars);
#endif

            summary = summary.Slice(0, indexOfLastWhitespace);
        }

        var summaryLength = summary.Length;

        // Trim trailing whitespace
        if (summaryLength > 0)
        {
            var lastChar = summary[summaryLength - 1];

            if (lastChar == SpaceChar || lastChar == TabChar || lastChar == NewLineChar || lastChar == CarriageReturnChar)
            {
                summaryLength -= 1;
            }
        }

        var sanitizedSqlSpan = buffer.Slice(0, state.SanitizedPosition);

        // If the sanitized SQL is identical to the input SQL, we can reuse the original string instance.
        var sanitizedSql = sanitizedSqlSpan.SequenceEqual(sqlSpan) ? sql : sanitizedSqlSpan.ToString();

        var sqlStatementInfo = new SqlStatementInfo(
            sanitizedSql,
            summary.Slice(0, summaryLength).ToString());

        // We don't clear the buffer as we know the content has been sanitized
        ArrayPool<char>.Shared.Return(rentedBuffer);

        return sqlStatementInfo;
    }

    private static void ParseNextTokenFast(
        ReadOnlySpan<char> sql,
        Span<char> buffer,
        ref ParseState state)
    {
        var start = state.ParsePosition;
        var remaining = sql.Length - start;

#if NET
        var indexOfNextWhitespace = sql.Slice(start).IndexOfAny(WhitespaceSearchValues);
#else
        var indexOfNextWhitespace = sql.Slice(start).IndexOfAny(WhitespaceChars);
#endif

        var length = indexOfNextWhitespace >= 0 ? indexOfNextWhitespace : remaining;

        sql.Slice(start, length).CopyTo(buffer.Slice(state.SanitizedPosition));
        state.SanitizedPosition += length;
        state.ParsePosition += length;

        // Note, for efficiency, we do not attempt to update the previous token start/end positions
        // We no longer use these when in fast path mode.
    }

    private static void ParseNextToken(
        ReadOnlySpan<char> sql,
        Span<char> buffer,
        ref ParseState state)
    {
        var start = state.ParsePosition;
        var currentChar = sql[start];

        // Quick first-character filter: only attempt keyword matching if the current char is an ASCII letter.
        // NOTE: We don't check CaptureNextNonKeywordTokenAsIdentifier here because we want to capture and handle keywords
        // first, before considering identifiers.
#if NET
        var mayBeKeyword = !state.InEscapedIdentifier && char.IsAsciiLetter(currentChar);
#else
        var mayBeKeyword = !state.InEscapedIdentifier && IsAsciiLetter(currentChar);
#endif

        if (mayBeKeyword)
        {
            var sqlLength = sql.Length;
            var remaining = sqlLength - start;

            // Determine the length of the next contiguous ascii-letter run.
            // This allows some fast paths in the comparisons below.
            var asciiLetterLength = 1;
            while (asciiLetterLength < remaining)
            {
                var ch = sql[start + asciiLetterLength];
#if NET
                if (!char.IsAsciiLetter(ch))
#else
                if (!IsAsciiLetter(ch))
#endif
                {
                    break;
                }

                asciiLetterLength++;
            }

            // IMPLEMENTATION NOTE: At one stage we tried checking if the length was between 2 and 12 (inclusive)
            // the range of shortest and longest keywords. This ended up being slower in practice
            // as many tokens fall into this range and it was faster to skip the length check.

            ReadOnlySpan<SqlKeywordInfo> keywordsToCheck;

            // Check if the previous character is '(', in which case, we only check against the SELECT keyword.
            // Otherwise, check if the previous keyword may be the start of a keyword chain so we can limit the
            // number of keyword comparisons we need to do by only comparing for tokens we expect to appear next.
            if (state.ParsePosition > 0 && sql[state.ParsePosition - 1] == OpenParenChar)
            {
                keywordsToCheck = SelectOnlyKeywordArray;
            }
            else
            {
                var previousKeywordInfo = state.PreviousParsedKeyword;

                keywordsToCheck = previousKeywordInfo != null && previousKeywordInfo.FollowedByKeywords.Length > 0
                    ? (ReadOnlySpan<SqlKeywordInfo>)previousKeywordInfo.FollowedByKeywords
                    : (ReadOnlySpan<SqlKeywordInfo>)SqlKeywords;
            }

            for (int i = 0; i < keywordsToCheck.Length; i++)
            {
                var potentialKeywordInfo = keywordsToCheck[i];
                var keywordSpan = potentialKeywordInfo.KeywordText.AsSpan();
                var keywordLength = keywordSpan.Length;

                // If the next token length doesn't match the keyword length, it can't be a match.
                if (asciiLetterLength != keywordLength)
                {
                    continue;
                }

                var matchedKeyword = true;

                // Compare the potential keyword in a case-insensitive manner using indices instead of slicing.
                for (var charPos = 1; charPos < keywordLength; charPos++)
                {
                    // We know that sql[start..] is all ascii letters so this comparison is safe.
                    if ((sql[start + charPos] | 0x20) != keywordSpan[charPos])
                    {
                        matchedKeyword = false;
                        break;
                    }
                }

                if (matchedKeyword)
                {
                    sql.Slice(start, keywordLength).CopyTo(buffer.Slice(state.SanitizedPosition));
                    state.SanitizedPosition += keywordLength;

                    // Potentially copy the keyword to the summary buffer.
                    if (SqlKeywordInfo.CaptureInSummary(in state, potentialKeywordInfo))
                    {
                        if (state.SummaryPosition == 0)
                        {
                            state.FirstSummaryKeyword = potentialKeywordInfo.SqlKeyword;
                        }

                        sql.Slice(start, keywordLength).CopyTo(state.SummaryBuffer.Slice(state.SummaryPosition));
                        state.SummaryPosition += keywordLength;

                        // Add a space after the keyword. The trailing space will be trimmed later.
                        state.SummaryBuffer[state.SummaryPosition++] = ' ';

                        state.PreviousSummaryKeyword = potentialKeywordInfo.SqlKeyword;
                    }

                    state.CaptureNextNonKeywordTokenAsIdentifier = SqlKeywordInfo.CaptureNextTokenInSummary(in state, potentialKeywordInfo.SqlKeyword);
                    state.SanitizeNextNonKeywordToken = SqlKeywordInfo.SanitizeNextToken(in state, potentialKeywordInfo.SqlKeyword);
                    state.InFromClause = potentialKeywordInfo.SqlKeyword == SqlKeyword.From || (state.PreviousParsedKeyword?.SqlKeyword == SqlKeyword.From && state.CaptureNextNonKeywordTokenAsIdentifier);
                    state.PreviousParsedKeyword = potentialKeywordInfo;
                    state.ParsePosition += keywordLength;
                    state.PreviousTokenStartPosition = start;
                    state.PreviousTokenEndPosition = start + keywordLength;

                    // No further parsing needed for this token
                    return;
                }
            }
        }

        // If we get this far, we have not matched a keyword, so we copy the token as-is.
        if (IsValidTokenCharacter(sql, start, 0, state))
        {
            // This first block handles identifiers (which start with a letter or underscore).

            // Scan the token once using indices, then bulk-copy to minimize per-char branching.
            var i = start;
            var position = -1;
            while (i < sql.Length)
            {
                position++;

                if (!IsValidTokenCharacter(sql, i, position, state))
                {
                    break;
                }

                i++;
            }

            var length = i - start;
            if (length > 0)
            {
                // Special handling: if we are in a FROM clause, check if this identifier is a reserved keyword
                // that indicates the end of the FROM clause.
                if (state.InFromClause)
                {
                    var isReservedKeyword = false;

                    // Fast check to ensure the length is within the range of known reserved keywords.
                    if (length >= MinFromClauseReservedKeywordLength && length <= MaxFromClauseReservedKeywordLength)
                    {
                        for (int k = 0; k < FromClauseReservedKeywords.Length; k++)
                        {
                            var keyword = FromClauseReservedKeywords[k];
                            if (length == keyword.Length && IsCaseInsensitiveMatch(sql, start, length, keyword))
                            {
                                isReservedKeyword = true;
                                break;
                            }
                        }
                    }

                    if (isReservedKeyword)
                    {
                        state.InFromClause = false;
                    }
                }

                if (state.SanitizeNextNonKeywordToken)
                {
                    buffer[state.SanitizedPosition++] = SanitizationPlaceholder;
                }
                else
                {
                    sql.Slice(start, length).CopyTo(buffer.Slice(state.SanitizedPosition));
                    state.SanitizedPosition += length;
                }

                // Optionally copy to summary buffer.
                if (state.CaptureNextNonKeywordTokenAsIdentifier)
                {
                    sql.Slice(start, length).CopyTo(state.SummaryBuffer.Slice(state.SummaryPosition));
                    state.SummaryPosition += length;

                    // Add a space after the identifier. The trailing space will be trimmed later.
                    state.SummaryBuffer[state.SummaryPosition++] = SpaceChar;
                }
            }

            state.ParsePosition = i;
            state.CaptureNextNonKeywordTokenAsIdentifier = false;
            state.SanitizeNextNonKeywordToken = false;
            state.PreviousTokenStartPosition = start;
            state.PreviousTokenEndPosition = i;
        }
        else
        {
            // If we end up here, we copy a single-character token to the sanitized buffer.
            // We also handle some special cases for tracking state.

            // If we are currently in an escaped identifier, check for the closing bracket.
            if (state.InEscapedIdentifier && currentChar is CloseSquareBracketChar)
            {
                state.InEscapedIdentifier = false;

                // Remove the space we added after the identifier in the summary buffer before we write the closing bracket.
                state.SummaryPosition--;

                state.SummaryBuffer[state.SummaryPosition++] = CloseSquareBracketChar;
                state.SummaryBuffer[state.SummaryPosition++] = SpaceChar;
            }

            // If we are in a FROM clause, we want to capture the next identifier following a comma or open square bracket.
            // Commas may occur when listing multiple tables in a FROM clause.
            // Brackets may occur when using schema-qualified or delimited identifiers.
            state.CaptureNextNonKeywordTokenAsIdentifier = state.InFromClause && (currentChar is CommaChar or OpenSquareBracketChar);

            if (state.CaptureNextNonKeywordTokenAsIdentifier && currentChar is OpenSquareBracketChar)
            {
                state.InEscapedIdentifier = true;
                state.SummaryBuffer[state.SummaryPosition++] = OpenSquareBracketChar;
            }

            buffer[state.SanitizedPosition++] = currentChar;
            state.ParsePosition++;

            // NOTE: We don't update previous token start/end positions for single-char tokens.
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsCaseInsensitiveMatch(ReadOnlySpan<char> sql, int tokenStart, int tokenLength, string reservedKeyword)
        {
            if (tokenLength != reservedKeyword.Length)
            {
                return false;
            }

            for (var charPos = 0; charPos < tokenLength; charPos++)
            {
                if ((sql[tokenStart + charPos] | 0x20) != (reservedKeyword[charPos] | 0x20))
                {
                    return false;
                }
            }

            return true;
        }
    }

    private static bool ParseWhitespace(ReadOnlySpan<char> sql, Span<char> buffer, ref ParseState state)
    {
        var start = state.ParsePosition;
        var foundWhitespace = false;

        // Find the end of whitespace run first
        var i = start;
        while (i < sql.Length)
        {
            var currentChar = sql[i];

#if NET
            if (WhitespaceSearchValues.Contains(currentChar))
#else
            if (currentChar == SpaceChar || currentChar == TabChar || currentChar == CarriageReturnChar || currentChar == NewLineChar)
#endif
            {
                foundWhitespace = true;
                i++;
                continue;
            }

            break;
        }

        // Bulk copy whitespace if found
        if (foundWhitespace)
        {
            var length = i - start;
            sql.Slice(start, length).CopyTo(buffer.Slice(state.SanitizedPosition));
            state.SanitizedPosition += length;
            state.ParsePosition = i;
        }

        return foundWhitespace;
    }

    private static bool SkipComment(ReadOnlySpan<char> sql, ref ParseState state)
    {
        var i = state.ParsePosition;
        var ch = sql[i];
        var length = sql.Length;

        var iPlusOne = i + 1;
        var iPlusTwo = i + 2;

        // Scan past multi-line comment
        if (ch == '/' && iPlusOne < length && sql[iPlusOne] == AsteriskChar)
        {
            // Use index arithmetic instead of slicing
            var searchPos = iPlusTwo;
            while (searchPos < length)
            {
                if (sql[searchPos] == AsteriskChar)
                {
                    if (searchPos + 1 < length && sql[searchPos + 1] == ForwardSlashChar)
                    {
                        state.ParsePosition = searchPos + 2;
                        return true;
                    }
                }

                searchPos++;
            }

            // Unterminated comment, consume to end
            state.ParsePosition = length;
            return true;
        }

        // Scan past single-line comment
        if (ch == DashChar && iPlusOne < length && sql[iPlusOne] == DashChar)
        {
            // Find next line break efficiently using index arithmetic
            var searchPosition = iPlusTwo;
            while (searchPosition < length)
            {
                var currentChar = sql[searchPosition];
                if (currentChar == CarriageReturnChar || currentChar == NewLineChar)
                {
                    // Position at the newline so ParseWhitespace can copy it
                    state.ParsePosition = searchPosition;
                    return true;
                }

                searchPosition++;
            }

            state.ParsePosition = length;
            return true;
        }

        return false;
    }

    private static bool SanitizeStringLiteral(ReadOnlySpan<char> sql, Span<char> buffer, ref ParseState state)
    {
        var currentChar = sql[state.ParsePosition];
        if (currentChar == SingleQuoteChar)
        {
            if (TrySanitizeLiteralsForInClause(sql, buffer, ref state, state.ParsePosition))
            {
                return true;
            }

            // Is the string literal of the form `N'foo'` (i.e. a Unicode literal)?
            // If so, we want to skip the Unicode prefix when sanitizing.
            bool isUnicode = state.ParsePosition >= 1 && sql[state.ParsePosition - 1] is UnicodePrefixChar;

            // Use index arithmetic instead of slicing
            var searchPos = state.ParsePosition + 1;
            while (searchPos < sql.Length)
            {
                if (sql[searchPos] == SingleQuoteChar)
                {
                    if (searchPos + 1 < sql.Length && sql[searchPos + 1] == SingleQuoteChar)
                    {
                        // Skip escaped quote ('')
                        searchPos += 2;
                        continue;
                    }

                    // Found terminating quote
                    if (isUnicode)
                    {
                        // Skip the Unicode prefix by overwriting the previous position instead
                        state.SanitizedPosition--;
                    }

                    state.ParsePosition = searchPos + 1;
                    buffer[state.SanitizedPosition++] = SanitizationPlaceholder;
                    return true;
                }

                searchPos++;
            }

            state.ParsePosition = sql.Length;
            buffer[state.SanitizedPosition++] = SanitizationPlaceholder;
            return true;
        }

        return false;
    }

    private static bool SanitizeHexLiteral(ReadOnlySpan<char> sql, Span<char> buffer, ref ParseState state)
    {
        var i = state.ParsePosition;
        var ch = sql[i];
        var length = sql.Length;
        var iPlusOne = i + 1;

        if (ch == '0' && iPlusOne < length && (sql[iPlusOne] == 'x' || sql[iPlusOne] == 'X'))
        {
            if (TrySanitizeLiteralsForInClause(sql, buffer, ref state, i))
            {
                return true;
            }

            for (i += 2; i < length; ++i)
            {
                ch = sql[i];
#if NET
                if (char.IsAsciiHexDigit(ch))
                {
                    continue;
                }
#else
                if (IsAsciiDigit(ch) ||
                    ch == 'A' || ch == 'a' ||
                    ch == 'B' || ch == 'b' ||
                    ch == 'C' || ch == 'c' ||
                    ch == 'D' || ch == 'd' ||
                    ch == 'E' || ch == 'e' ||
                    ch == 'F' || ch == 'f')
                {
                    continue;
                }
#endif

                i -= 1;
                break;
            }

            state.ParsePosition = ++i;

            buffer[state.SanitizedPosition++] = SanitizationPlaceholder;
            return true;
        }

        return false;
    }

    private static bool SanitizeNumericLiteral(ReadOnlySpan<char> sql, Span<char> buffer, ref ParseState state)
    {
        var i = state.ParsePosition;
        var currentChar = sql[i];
        var length = sql.Length;
        var iPlusOne = i + 1;

        // Scan past leading sign
        if ((currentChar == '-' || currentChar == '+') && iPlusOne < length && (IsAsciiDigit(sql[iPlusOne]) || sql[iPlusOne] == DotChar))
        {
            i += 1;
            iPlusOne = i + 1;
            currentChar = sql[i];
        }

        // Scan past leading decimal point
        var periodMatched = false;
        if (currentChar == '.' && iPlusOne < length && IsAsciiDigit(sql[iPlusOne]))
        {
            periodMatched = true;
            i += 1;
            currentChar = sql[i];
        }

        if (IsAsciiDigit(currentChar))
        {
            if (TrySanitizeLiteralsForInClause(sql, buffer, ref state, i))
            {
                return true;
            }

            var exponentMatched = false;
            for (i += 1; i < length; ++i)
            {
                currentChar = sql[i];
                if (IsAsciiDigit(currentChar))
                {
                    continue;
                }

                if (!periodMatched && currentChar == '.')
                {
                    periodMatched = true;
                    continue;
                }

                if (!exponentMatched && (currentChar == 'e' || currentChar == 'E'))
                {
                    // Scan past sign in exponent
                    if (i + 1 < length && (sql[i + 1] == '-' || sql[i + 1] == '+'))
                    {
                        i += 1;
                    }

                    exponentMatched = true;
                    continue;
                }

                i -= 1;
                break;
            }

            state.ParsePosition = ++i;

            buffer[state.SanitizedPosition++] = SanitizationPlaceholder;
            return true;
        }

        return false;
    }

    private static bool TrySanitizeLiteralsForInClause(ReadOnlySpan<char> sql, Span<char> buffer, ref ParseState state, int parsePosition)
    {
        // Special case: We may be in an IN clause with a list of literals.
        // If the previously sanitized character was '(' and the previous token was "IN", we can simplify the sanitization.
        // In this case, we fast-path to the closing parenthesis and replace the entire contents with a single '?'.

        if (state.SanitizedPosition > 0 && buffer[state.SanitizedPosition - 1] == OpenParenChar
            && state.PreviousTokenEndPosition - state.PreviousTokenStartPosition == 2)
        {
            // Check the token is actually "IN" (case-insensitive) to avoid false positives.
            var firstChar = sql[state.PreviousTokenStartPosition];
            var secondChar = sql[state.PreviousTokenStartPosition + 1];

            if (!((firstChar == 'i' || firstChar == 'I') && (secondChar == 'n' || secondChar == 'N')))
            {
                return false;
            }

            // Use index arithmetic instead of slicing
            var searchPosition = parsePosition;
            while (searchPosition < sql.Length)
            {
                if (sql[searchPosition] == CloseParenChar)
                {
                    state.ParsePosition = searchPosition;
                    buffer[state.SanitizedPosition++] = SanitizationPlaceholder;
                    return true;
                }

                searchPosition++;
            }
        }

        return false;
    }

    private ref struct ParseState
    {
        // ParseState intentionally uses public fields (not properties):
        // - This is a ref struct that lives on the stack and is passed by ref through hot paths.
        // - Fields avoid property accessor calls in tight loops and yield smaller/faster code after inlining.
        // - Grouping Span<> and larger struct fields first helps layout and may reduce padding.
        // - Keeping the struct simple and flat minimizes stack pressure and lets the JIT keep values in registers.

        // Stored in state to avoid slicing repeatedly.
        public Span<char> SummaryBuffer;

        /// <summary>
        /// Will be set if a keyword has been matched by the parser.
        /// Not all keywords are necessarily matched.
        /// </summary>
        public SqlKeywordInfo? PreviousParsedKeyword; // 8 bytes (reference type)

        public SqlKeyword FirstSummaryKeyword; // 4 bytes (enum, underlying int)
        public SqlKeyword PreviousSummaryKeyword; // 4 bytes (enum, underlying int)

        // These track the current parse position in the input SQL and the current write position
        // for the sanitized SQL and summary buffers.
        public int ParsePosition; // 4 bytes
        public int SanitizedPosition; // 4 bytes
        public int SummaryPosition; // 4 bytes

        // These track the start and end position of the previous (non-literal) token parsed.
        public int PreviousTokenStartPosition; // 4 bytes
        public int PreviousTokenEndPosition; // 4 bytes

        // NOTE: If the number of bool fields increases significantly, consider combining into a bitfield.

        public bool CaptureNextNonKeywordTokenAsIdentifier; // 1 byte

        public bool SanitizeNextNonKeywordToken; // 1 byte

        /// <summary>
        /// Used to track if we are in an escaped identifier (e.g., "[table]").
        /// </summary>
        public bool InEscapedIdentifier; // 1 byte

        /// <summary>
        /// Used to track if we are in a FROM clause for special handling of comma-separated table lists.
        /// When set to <c>true</c>, subsequent unmatched tokens will be compared against reserved keywords.
        /// As soon as we match a reserved keyword, we exit the FROM clause state.
        /// </summary>
        public bool InFromClause; // 1 byte
    }

    private sealed class SqlKeywordInfo
    {
        // Used on keywords that are only included in the summary if they are the first keyword in the statement.
        private static readonly SqlKeyword[] Unknown = [SqlKeyword.Unknown];

        private static readonly SqlKeyword[] DdlKeywords =
        [
            SqlKeyword.Create,
            SqlKeyword.Drop,
            SqlKeyword.Alter,
        ];

        private readonly SqlKeyword[]? captureInSummaryWhenPrevious;

        static SqlKeywordInfo()
        {
            // Phase 1: Create all static instances.
            // We will compare the SQL we are parsing in lowercase, so we store these in lowercase also.
            AlterKeyword = new("alter", SqlKeyword.Alter, Unknown);
            BackupKeyword = new("backup", SqlKeyword.Backup, Unknown);
            BulkKeyword = new("bulk", SqlKeyword.Bulk, Unknown);
            ConnectKeyword = new("connect", SqlKeyword.Connect, Unknown);
            CreateKeyword = new("create", SqlKeyword.Create, Unknown);
            DatabaseKeyword = new("database", SqlKeyword.Database, [.. DdlKeywords, SqlKeyword.Backup, SqlKeyword.Restore]);
            DeleteKeyword = new("delete", SqlKeyword.Delete, Unknown);
            DenyKeyword = new("deny", SqlKeyword.Deny, Unknown);
            DisableKeyword = new("disable", SqlKeyword.Disable, Unknown);
            DropKeyword = new("drop", SqlKeyword.Drop, Unknown);
            EnableKeyword = new("enable", SqlKeyword.Enable, Unknown);
            ExecKeyword = new("exec", SqlKeyword.Exec, Unknown);
            ExecuteKeyword = new("execute", SqlKeyword.Execute, Unknown);
            ExistsKeyword = new("exists", SqlKeyword.Exists);
            FromKeyword = new("from", SqlKeyword.From);
            FunctionKeyword = new("function", SqlKeyword.Function, DdlKeywords);
            GrantKeyword = new("grant", SqlKeyword.Grant, Unknown);
            IfKeyword = new("if", SqlKeyword.If);
            IndexKeyword = new("index", SqlKeyword.Index, [.. DdlKeywords, SqlKeyword.Unique, SqlKeyword.Clustered, SqlKeyword.NonClustered]);
            InsertKeyword = new("insert", SqlKeyword.Insert, [SqlKeyword.Unknown, SqlKeyword.Bulk]);
            IntoKeyword = new("into", SqlKeyword.Into);
            JoinKeyword = new("join", SqlKeyword.Join);
            LoginKeyword = new("login", SqlKeyword.Login, DdlKeywords);
            NotKeyword = new("not", SqlKeyword.Not);
            OnKeyword = new("on", SqlKeyword.On);
            ProcedureKeyword = new("procedure", SqlKeyword.Procedure, DdlKeywords);
            RestoreKeyword = new("restore", SqlKeyword.Restore, Unknown);
            RevokeKeyword = new("revoke", SqlKeyword.Revoke, Unknown);
            RoleKeyword = new("role", SqlKeyword.Role, DdlKeywords);
            SchemaKeyword = new("schema", SqlKeyword.Schema, DdlKeywords);
            SelectKeyword = new("select", SqlKeyword.Select, [SqlKeyword.Select, SqlKeyword.Unknown]);
            SequenceKeyword = new("sequence", SqlKeyword.Sequence, DdlKeywords);
            StatisticsKeyword = new("statistics", SqlKeyword.Statistics, [SqlKeyword.Update]);
            TableKeyword = new("table", SqlKeyword.Table, [.. DdlKeywords, SqlKeyword.Truncate]);
            TriggerKeyword = new("trigger", SqlKeyword.Trigger, [.. DdlKeywords, SqlKeyword.Enable, SqlKeyword.Disable]);
            TruncateKeyword = new("truncate", SqlKeyword.Truncate, Unknown);
            UnionKeyword = new("union", SqlKeyword.Union);
            UnknownKeyword = new(string.Empty, SqlKeyword.Unknown);
            UpdateKeyword = new("update", SqlKeyword.Update, Unknown);
            UserKeyword = new("user", SqlKeyword.User, DdlKeywords);
            ViewKeyword = new("view", SqlKeyword.View, DdlKeywords);

            // Phase 2: Build arrays that depend on instances
            // NOTE: This array is sorted by an estimation of the most likely
            // keywords first to optimise the comparison loop.
            DdlSubKeywords =
            [
                TableKeyword,
                IndexKeyword,
                ViewKeyword,
                ProcedureKeyword,
                TriggerKeyword,
                DatabaseKeyword,
                LoginKeyword,
                UserKeyword,
                RoleKeyword,
                SequenceKeyword,
                SchemaKeyword,
                FunctionKeyword,
            ];

            // Phase 3: Wire follow relationships
            AlterKeyword.FollowedByKeywords = DdlSubKeywords;
            BackupKeyword.FollowedByKeywords = [DatabaseKeyword];
            BulkKeyword.FollowedByKeywords = [InsertKeyword];
            CreateKeyword.FollowedByKeywords = DdlSubKeywords;
            DatabaseKeyword.FollowedByKeywords = [IfKeyword];
            DenyKeyword.FollowedByKeywords = [ConnectKeyword];
            DisableKeyword.FollowedByKeywords = [TriggerKeyword];
            DropKeyword.FollowedByKeywords = DdlSubKeywords;
            EnableKeyword.FollowedByKeywords = [TriggerKeyword];
            FromKeyword.FollowedByKeywords = [JoinKeyword, UnionKeyword];
            FunctionKeyword.FollowedByKeywords = [IfKeyword];
            GrantKeyword.FollowedByKeywords = [ConnectKeyword];
            IfKeyword.FollowedByKeywords = [NotKeyword, ExistsKeyword];
            IndexKeyword.FollowedByKeywords = [OnKeyword, IfKeyword];
            InsertKeyword.FollowedByKeywords = [IntoKeyword];
            JoinKeyword.FollowedByKeywords = [OnKeyword];
            LoginKeyword.FollowedByKeywords = [IfKeyword];
            NotKeyword.FollowedByKeywords = [ExistsKeyword];
            OnKeyword.FollowedByKeywords = [JoinKeyword];
            ProcedureKeyword.FollowedByKeywords = [IfKeyword];
            RestoreKeyword.FollowedByKeywords = [DatabaseKeyword];
            RevokeKeyword.FollowedByKeywords = [ConnectKeyword];
            RoleKeyword.FollowedByKeywords = [IfKeyword];
            SchemaKeyword.FollowedByKeywords = [IfKeyword, UnionKeyword];
            SelectKeyword.FollowedByKeywords = [FromKeyword];
            SequenceKeyword.FollowedByKeywords = [IfKeyword];
            TableKeyword.FollowedByKeywords = [IfKeyword];
            TriggerKeyword.FollowedByKeywords = [IfKeyword];
            TruncateKeyword.FollowedByKeywords = [TableKeyword];
            UnionKeyword.FollowedByKeywords = [SelectKeyword];
            UpdateKeyword.FollowedByKeywords = [StatisticsKeyword];
            UserKeyword.FollowedByKeywords = [IfKeyword];
            ViewKeyword.FollowedByKeywords = [IfKeyword];
        }

        private SqlKeywordInfo(
            string keyword,
            SqlKeyword sqlKeyword,
            SqlKeyword[]? captureInSummaryWhenPrevious = null)
        {
            this.KeywordText = keyword;
            this.SqlKeyword = sqlKeyword;
            this.captureInSummaryWhenPrevious = captureInSummaryWhenPrevious;
            this.FollowedByKeywords = [];
        }

        public static SqlKeywordInfo AlterKeyword { get; }

        public static SqlKeywordInfo BackupKeyword { get; }

        public static SqlKeywordInfo BulkKeyword { get; }

        public static SqlKeywordInfo ConnectKeyword { get; }

        public static SqlKeywordInfo CreateKeyword { get; }

        public static SqlKeywordInfo DatabaseKeyword { get; }

        public static SqlKeywordInfo DeleteKeyword { get; }

        public static SqlKeywordInfo DenyKeyword { get; }

        public static SqlKeywordInfo DisableKeyword { get; }

        public static SqlKeywordInfo DropKeyword { get; }

        public static SqlKeywordInfo EnableKeyword { get; }

        public static SqlKeywordInfo ExecKeyword { get; }

        public static SqlKeywordInfo ExecuteKeyword { get; }

        public static SqlKeywordInfo ExistsKeyword { get; }

        public static SqlKeywordInfo FromKeyword { get; }

        public static SqlKeywordInfo FunctionKeyword { get; }

        public static SqlKeywordInfo GrantKeyword { get; }

        public static SqlKeywordInfo IfKeyword { get; }

        public static SqlKeywordInfo IndexKeyword { get; }

        public static SqlKeywordInfo InsertKeyword { get; }

        public static SqlKeywordInfo IntoKeyword { get; }

        public static SqlKeywordInfo JoinKeyword { get; }

        public static SqlKeywordInfo LoginKeyword { get; }

        public static SqlKeywordInfo NotKeyword { get; }

        public static SqlKeywordInfo OnKeyword { get; }

        public static SqlKeywordInfo ProcedureKeyword { get; }

        public static SqlKeywordInfo RestoreKeyword { get; }

        public static SqlKeywordInfo RevokeKeyword { get; }

        public static SqlKeywordInfo RoleKeyword { get; }

        public static SqlKeywordInfo SchemaKeyword { get; }

        public static SqlKeywordInfo SelectKeyword { get; }

        public static SqlKeywordInfo SequenceKeyword { get; }

        public static SqlKeywordInfo StatisticsKeyword { get; }

        public static SqlKeywordInfo TableKeyword { get; }

        public static SqlKeywordInfo TriggerKeyword { get; }

        public static SqlKeywordInfo TruncateKeyword { get; }

        public static SqlKeywordInfo UnionKeyword { get; }

        public static SqlKeywordInfo UnknownKeyword { get; }

        public static SqlKeywordInfo UpdateKeyword { get; }

        public static SqlKeywordInfo UserKeyword { get; }

        public static SqlKeywordInfo ViewKeyword { get; }

        public static SqlKeywordInfo[] DdlSubKeywords { get; }

        public string KeywordText { get; }

        public SqlKeyword SqlKeyword { get; }

        public SqlKeywordInfo[] FollowedByKeywords { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CaptureNextTokenInSummary(in ParseState state, SqlKeyword currentKeyword) => currentKeyword switch
        {
            SqlKeyword.Exec => true,
            SqlKeyword.Exists => state.FirstSummaryKeyword is SqlKeyword.Create or SqlKeyword.Alter or SqlKeyword.Drop && state.PreviousSummaryKeyword is not (SqlKeyword.Login or SqlKeyword.User),
            SqlKeyword.From => state.PreviousSummaryKeyword is SqlKeyword.Select or SqlKeyword.Distinct,
            SqlKeyword.Into => state.FirstSummaryKeyword is SqlKeyword.Insert,
            SqlKeyword.Join => state.FirstSummaryKeyword is SqlKeyword.Select or SqlKeyword.Join,
            SqlKeyword.Login or SqlKeyword.User => false,
            SqlKeyword.Statistics => state.FirstSummaryKeyword is SqlKeyword.Update,
            SqlKeyword.Trigger => state.FirstSummaryKeyword is SqlKeyword.Create or SqlKeyword.Alter or SqlKeyword.Drop or SqlKeyword.Disable or SqlKeyword.Enable,
            SqlKeyword.Truncate => state.FirstSummaryKeyword is SqlKeyword.Table,
            SqlKeyword.Database or
            SqlKeyword.Function or
            SqlKeyword.Index or
            SqlKeyword.Procedure or
            SqlKeyword.Role or
            SqlKeyword.Schema or
            SqlKeyword.Sequence or
            SqlKeyword.Table or
            SqlKeyword.View => state.FirstSummaryKeyword is SqlKeyword.Create or SqlKeyword.Alter or SqlKeyword.Drop,
            _ => false,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SanitizeNextToken(in ParseState state, SqlKeyword currentKeyword) => currentKeyword switch
        {
            SqlKeyword.Login or SqlKeyword.User => state.FirstSummaryKeyword is SqlKeyword.Create or SqlKeyword.Alter or SqlKeyword.Drop,
            SqlKeyword.Exists => state.PreviousSummaryKeyword is SqlKeyword.Login or SqlKeyword.User,
            _ => false,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CaptureInSummary(in ParseState state, SqlKeywordInfo currentKeyword)
        {
            if (currentKeyword.captureInSummaryWhenPrevious == null || currentKeyword.captureInSummaryWhenPrevious.Length == 0)
            {
                return false;
            }

            var prev = state.PreviousParsedKeyword?.SqlKeyword ?? SqlKeyword.Unknown;
            for (int i = 0; i < currentKeyword.captureInSummaryWhenPrevious.Length; i++)
            {
                if (currentKeyword.captureInSummaryWhenPrevious[i] == prev)
                {
                    return true;
                }
            }

            return currentKeyword.SqlKeyword == SqlKeyword.Select
                && state.FirstSummaryKeyword is not SqlKeyword.Create
                && state.PreviousParsedKeyword?.SqlKeyword is not SqlKeyword.Union;
        }
    }
}
