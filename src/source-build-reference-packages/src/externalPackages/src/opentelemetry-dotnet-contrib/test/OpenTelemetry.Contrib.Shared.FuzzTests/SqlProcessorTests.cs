// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace OpenTelemetry.Instrumentation;

public static class SqlProcessorTests
{
    private const int MaxValue = 1_000;

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Null_Returns_Default()
    {
        // Act
        var actual = SqlProcessor.GetSanitizedSql(null);

        // Assert
        Assert.Null(actual.SanitizedSql);
        Assert.Null(actual.DbQuerySummary);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Empty_Returns_Empty_Strings()
    {
        // Act
        var actual = SqlProcessor.GetSanitizedSql(string.Empty);

        // Assert
        Assert.Empty(actual.DbQuerySummary);
        Assert.Empty(actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Arbitrary_Input_Does_Not_Throw(NonEmptyString input)
    {
        // Act
        var actual = Record.Exception(() => SqlProcessor.GetSanitizedSql(input.Get));

        // Assert
        Assert.Null(actual);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Numeric_Literals_Are_Sanitized(PositiveInt number)
    {
        // Arrange
        var sql = $"SELECT * FROM table WHERE id = {number.Get}";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("?", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_String_Literals_Are_Sanitized(NonEmptyString input)
    {
        // Arrange
        var sql = $"SELECT * FROM table WHERE name = '{input.Get.Replace("'", "''")}'";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("?", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Hex_Literals_Are_Sanitized(NonNegativeInt number)
    {
        // Arrange
        var sql = $"SELECT * FROM table WHERE bin = 0x{number.Get:X}";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("?", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Whitespace_Is_Preserved(PositiveInt spaces)
    {
        // Arrange
        var count = Math.Min(spaces.Get, 100);
        var sql = $"SELECT{new string(' ', count)}*{new string(' ', count)}FROM table";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains(" ", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Comments_Are_Removed(NonEmptyString input)
    {
        // Arrange
        var comment = input.Get.Replace("*/", string.Empty);
        var sql = $"SELECT * /* {comment} */ FROM table";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.DoesNotContain("/*", actual.SanitizedSql);
        Assert.DoesNotContain("*/", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Single_Line_Comments_Are_Removed(NonEmptyString input)
    {
        // Arrange
        var comment = input.Get.Replace("\n", " ").Replace("\r", " ");
        var sql = $"SELECT * FROM table -- {comment}\nWHERE id = 1";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.DoesNotContain("--", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Keywords_Preserved_Case_Insensitive(bool uppercase)
    {
        // Arrange
        var keyword = uppercase ? "SELECT" : "select";
        var sql = $"{keyword} * FROM table";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("SELECT", actual.SanitizedSql.ToUpperInvariant());
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Summary_Length_Limited(NonEmptyString input)
    {
        // Arrange
        var sql = $"SELECT {input.Get} FROM table WHERE {input.Get} = {input.Get}";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.True(actual.DbQuerySummary.Length <= 255);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_In_Clause_Optimizes_Sanitization(PositiveInt input)
    {
        // Arrange
        var count = Math.Min(input.Get, 100);
        var items = string.Join(", ", Enumerable.Range(1, count).Select((p) => p.ToString()));
        var sql = $"SELECT * FROM table WHERE id IN ({items})";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        var questionMarkCount = actual.SanitizedSql.Count((p) => p == '?');
        Assert.Equal(1, questionMarkCount);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Multiple_Invocations_Returns_Same_Result(NonEmptyString input)
    {
        // Act
        var actual1 = SqlProcessor.GetSanitizedSql(input.Get);
        var actual2 = SqlProcessor.GetSanitizedSql(input.Get);

        // Assert
        Assert.Equal(actual1.SanitizedSql, actual2.SanitizedSql);
        Assert.Equal(actual1.DbQuerySummary, actual2.DbQuerySummary);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Escaped_Quotes_Handled_Correctly()
    {
        // Arrange
        var sql = $"SELECT * FROM table WHERE name = 'O''Reilly'";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("?", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Floating_Point_Is_Sanitized(NormalFloat value)
    {
        // Arrange
        if (double.IsNaN(value.Get) || double.IsInfinity(value.Get))
        {
            return;
        }

        var sql = $"SELECT * FROM table WHERE rate = {value.Get}";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("?", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Scientific_Notation_Is_Sanitized(int exponent)
    {
        // Arrange
        if (exponent < -10 || exponent > 10)
        {
            return;
        }

        var sql = $"SELECT * FROM table WHERE rate = 1.23e{exponent}";

        // Act
        var result = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("?", result.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Mixed_Case_Keywords(NonEmptyString input)
    {
        // Arrange
        var tableName = new string([.. input.Get.Where(char.IsLetterOrDigit).Take(50)]);

        if (string.IsNullOrEmpty(tableName))
        {
            tableName = "table";
        }

        var sql = $"SeLeCt * FrOm {tableName}";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.True(actual.SanitizedSql.Length > 0);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Special_Characters_Do_Not_Throw(char special)
    {
        // Arrange
        var sql = $"SELECT * FROM table WHERE col = '{special}'";

        // Act
        var actual = Record.Exception(() => SqlProcessor.GetSanitizedSql(sql));

        // Assert
        Assert.Null(actual);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Very_Long_Input_Handled_Gracefully(PositiveInt length)
    {
        // Arrange
        var count = Math.Min(length.Get, 10000);
        var sql = "SELECT " + new string('a', count) + " FROM table";

        // Act
        var actual = Record.Exception(() => SqlProcessor.GetSanitizedSql(sql));

        // Assert
        Assert.Null(actual);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Unterminated_String_Does_Not_Throw(NonEmptyString content)
    {
        // Arrange
        var sql = $"SELECT * FROM table WHERE name = '{content.Get}";

        // Act
        var actual = Record.Exception(() => SqlProcessor.GetSanitizedSql(sql));

        // Assert
        Assert.Null(actual);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Unterminated_Comment_Does_Not_Throw(NonEmptyString content)
    {
        // Arrange
        var sql = $"SELECT * /* {content.Get}";

        // Act
        var actual = Record.Exception(() => SqlProcessor.GetSanitizedSql(sql));

        // Assert
        Assert.Null(actual);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Nested_Parentheses_Handled(PositiveInt depth)
    {
        // Arrange
        var count = Math.Min(depth.Get, 10);
        var openParenthesis = new string('(', count);
        var closePatenthesis = new string(')', count);

        var sql = $"SELECT * FROM table WHERE id IN {openParenthesis}1, 2, 3{closePatenthesis}";

        // Act
        var actual = Record.Exception(() => SqlProcessor.GetSanitizedSql(sql));

        // Assert
        Assert.Null(actual);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Negative_Numbers_Are_Sanitized(int number)
    {
        // Arrange
        var sql = $"SELECT * FROM table WHERE id = {number}";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("?", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Decimal_Numbers_Are_Sanitized(decimal number)
    {
        // Arrange
        var sql = $"SELECT * FROM table WHERE amount = {number}";

        // Act
        var actual = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        Assert.Contains("?", actual.SanitizedSql);
    }

    [Property(MaxTest = MaxValue)]
    public static void GetSanitizedSql_Multiple_Values_In_In_Clause(PositiveInt value)
    {
        // Arrange
        var count = Math.Min(value.Get, 50);
        var values = string.Join(", ", Enumerable.Range(1, count).Select((p) => $"'{p}'"));

        var sql = $"SELECT * FROM table WHERE name IN ({values})";

        // Act
        var result = SqlProcessor.GetSanitizedSql(sql);

        // Assert
        // IN clause optimization should replace all values with single ?
        var questionMarkCount = result.SanitizedSql.Count((p) => p == '?');
        Assert.Equal(1, questionMarkCount);
    }
}
