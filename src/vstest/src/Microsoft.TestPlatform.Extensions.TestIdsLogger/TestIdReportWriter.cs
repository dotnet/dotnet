// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger;

/// <summary>
/// Writes the test id report as CSV.
/// </summary>
/// <remarks>
/// <para>
/// CSV because the report exists to be joined against whatever the consumer already stored their
/// test ids in. A flat, fixed set of scalar columns with no nesting is exactly the shape a
/// relational load or a spreadsheet wants, and every database, BI tool and scripting language reads
/// it without a parser being written for the occasion.
/// </para>
/// <para>
/// The quoting follows RFC 4180: a field is quoted when it contains a comma, a quote, a carriage
/// return or a line feed, and an embedded quote is doubled. Test names routinely contain commas
/// - a data driven row is usually rendered as <c>Test (1,2)</c> - so unquoted output would be
/// silently wrong rather than obviously broken, which is the worst kind of wrong for a file whose
/// entire job is to be machine read.
/// </para>
/// </remarks>
internal static class TestIdReportWriter
{
    /// <summary>
    /// The header row, and by extension the column order.
    /// </summary>
    public const string Header = "Source,ExecutorUri,FullyQualifiedName,DisplayName,Id,Sha1Id,XxHash128Id,IdSource";

    /// <summary>
    /// Writes the header and one row per record, in the order given.
    /// </summary>
    public static void Write(TextWriter writer, IEnumerable<TestIdRecord> records)
    {
        ValidateArg.NotNull(writer, nameof(writer));
        ValidateArg.NotNull(records, nameof(records));

        // A fixed newline rather than Environment.NewLine: a report produced on one operating
        // system is routinely consumed on another, and RFC 4180 says CRLF.
        writer.Write(Header);
        writer.Write("\r\n");

        foreach (TestIdRecord record in records)
        {
            var row = new StringBuilder();
            AppendField(row, record.Source);
            row.Append(',');
            AppendField(row, record.ExecutorUri);
            row.Append(',');
            AppendField(row, record.FullyQualifiedName);
            row.Append(',');
            AppendField(row, record.DisplayName);
            row.Append(',');
            AppendField(row, Format(record.Id));
            row.Append(',');
            AppendField(row, Format(record.Sha1Id));
            row.Append(',');
            AppendField(row, Format(record.XxHash128Id));
            row.Append(',');
            AppendField(row, record.IdSource.ToString());

            writer.Write(row.ToString());
            writer.Write("\r\n");
        }
    }

    /// <summary>
    /// Formats a test id the way every other vstest surface prints one, so a value in this report
    /// compares byte for byte against a value taken from a TRX file or from the object model.
    /// </summary>
    private static string Format(Guid id) => id.ToString("d", CultureInfo.InvariantCulture);

    private static void AppendField(StringBuilder row, string value)
    {
        if (value.IndexOfAny(CharactersRequiringQuoting) < 0)
        {
            row.Append(value);
            return;
        }

        row.Append('"');
        foreach (char character in value)
        {
            if (character == '"')
            {
                row.Append('"');
            }

            row.Append(character);
        }

        row.Append('"');
    }

    private static readonly char[] CharactersRequiringQuoting = new[] { ',', '"', '\r', '\n' };
}
