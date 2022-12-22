// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace NuGet.CommandLine.XPlat.Utility
{
    internal static class TableParser
    {
        internal static IEnumerable<FormattedCell> ToStringTable<T>(
          this IEnumerable<T> values,
          string[] columnHeaders,
          params Func<T, object>[] valueSelectors)
        {
            return ToFormattedStringTable(values.ToArray(), columnHeaders, valueSelectors);
        }

        internal static IEnumerable<FormattedCell> ToFormattedStringTable<T>(
          this T[] values,
          string[] columnHeaders,
          params Func<T, object>[] valueSelectors)
        {
            var stringTable = new List<List<FormattedCell>>();

            // Fill headers
            if (columnHeaders != null)
            {
                Debug.Assert(columnHeaders.Length == valueSelectors.Length);

                var headers = new List<FormattedCell>();
                headers.AddRange(columnHeaders.Select(h => new FormattedCell(h)));
                stringTable.Add(headers);
            }

            // Fill table rows - we need a queue for multi-line values
            var columnQueues = new Dictionary<int, Queue<FormattedCell>>();
            for (var rowIndex = 0; rowIndex < values.Length; rowIndex++)
            {
                // process row
                var row = new List<FormattedCell>();
                for (var colIndex = 0; colIndex < valueSelectors.Length; colIndex++)
                {
                    var data = valueSelectors[colIndex](values[rowIndex]);
                    if (data is IEnumerable<object> dataEnum)
                    {
                        // we have a potential multi-line value--we need to add the first line and store remainder
                        var firstLine = true;
                        var queue = new Queue<FormattedCell>();
                        foreach (var dataCell in dataEnum)
                        {
                            if (dataCell is FormattedCell formattedDataCell)
                            {
                                formattedDataCell.Value = (colIndex == 0 ? "> " : "") + formattedDataCell.Value?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
                                if (firstLine)
                                {
                                    // print it
                                    row.Add(formattedDataCell);
                                    firstLine = false;
                                }
                                else
                                {
                                    // store the rest
                                    queue.Enqueue(formattedDataCell);
                                }
                            }
                        }

                        if (queue.Count > 0) // only add a queue when there's something to store
                        {
                            columnQueues[colIndex] = queue;
                        }
                    }
                    else
                    {
                        // the normal case
                        if (data is FormattedCell formattedDataCell)
                        {
                            formattedDataCell.Value = (colIndex == 0 ? "> " : "") + formattedDataCell.Value?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
                            row.Add(formattedDataCell);
                        }
                    }
                }

                stringTable.Add(row);

                // clear column queues (strings for subsequent rows for this value) before proceeding with next row
                while (columnQueues.Count > 0)
                {
                    var subsequentRow = new List<FormattedCell>();
                    for (var colIndex = 0; colIndex < valueSelectors.Length; colIndex++)
                    {
                        var formattedDataCell = (FormattedCell)null;
                        if (columnQueues.TryGetValue(colIndex, out var thisColumnQueue)) // we have at least one remaining value for this column
                        {
                            formattedDataCell = thisColumnQueue.Dequeue();
                            if (thisColumnQueue.Count == 0)
                            {
                                columnQueues.Remove(colIndex); // once these are all cleared the outer loop will break
                            }
                        }
                        else
                        {
                            formattedDataCell = new FormattedCell();
                        }

                        subsequentRow.Add(formattedDataCell);
                    }

                    stringTable.Add(subsequentRow);
                }
            }

            return ToPaddedStringTable(stringTable);
        }

        internal static IEnumerable<FormattedCell> ToPaddedStringTable(IEnumerable<ICollection<FormattedCell>> values)
        {
            var maxColumnsWidth = GetMaxColumnsWidth(values);
            var stringTable = new List<FormattedCell>();

            foreach (var row in values)
            {
                int colIndex = 0;
                foreach (var dataCell in row)
                {
                    dataCell.Value = "   " + dataCell.Value?.PadRight(maxColumnsWidth[colIndex]) ?? string.Empty;
                    stringTable.Add(dataCell);
                    colIndex++;
                }

                stringTable.Add(new FormattedCell(Environment.NewLine));
            }

            return stringTable;
        }

        private static int[] GetMaxColumnsWidth(IEnumerable<ICollection<FormattedCell>> values)
        {
            int[] maxColumnsWidth = null;
            foreach (var row in values)
            {
                // use the first row to get dimension
                if (maxColumnsWidth == null)
                {
                    maxColumnsWidth = new int[row.Count];
                }

                var colIndex = 0;
                foreach (var formattedDataCell in row)
                {
                    if ((formattedDataCell.Value?.Length ?? 0) > maxColumnsWidth[colIndex])
                    {
                        maxColumnsWidth[colIndex] = formattedDataCell.Value.Length;
                    }

                    colIndex++;
                }
            }

            return maxColumnsWidth;
        }
    }
}
