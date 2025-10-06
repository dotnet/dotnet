// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NuGet.CommandLine.XPlat
{
    internal class Column
    {
        public string Header { get; set; }
        public int Width { get; set; }
        public bool Highlight { get; set; }
    }

    internal class Table
    {
        // This is the default window width if we cannot get the actual window width
        const int DefaultWindowWidth = 115;
        // This is the minimum number of characters in a column which includes "| c |" where c is a character
        const int MinimumCharactersInAColumn = 4;
        // This is the list of columns in the table
        internal readonly List<Column> _columns = new List<Column>();
        // This is the list of rows in the table
        internal List<string[]> _rows = new List<string[]>();
        // This is the list of columns to highlight
        private int[] _columnsToHighlight;
        // This is the highlighter color
        private ConsoleColor _highlighter = ConsoleColor.Red;
        // This is the maximum column width: The maximum number of characters in a column based on the window width
        private readonly int _maxColumnWidth;
        // This is the default console color
        private readonly ConsoleColor _consoleColor = Console.ForegroundColor;

        public Table(int[] columnsToHighlight, params string[] headers)
        {
            _columnsToHighlight = columnsToHighlight;
            int windowWidth = -1;

            // Get the window width if possible 
            try
            {
                windowWidth = Console.WindowWidth;
            }
            catch (Exception)
            {
                // Ignore any exception
            }

            // If the window width is not available, use the default window width
            if (windowWidth <= 0)
            {
                _maxColumnWidth = DefaultWindowWidth;
            }
            else
            {
                _maxColumnWidth = Math.Max(MinimumCharactersInAColumn, (windowWidth - MinimumCharactersInAColumn * headers.Length) / headers.Length);
            }

            // Add the headers
            foreach (var header in headers)
            {
                _columns.Add(new Column { Header = header, Width = header.Length });
            }
        }

        /* Add a row to the table
         * row: The list of values in the row
         */
        public void AddRow(params string[] row)
        {
            if (row.Length != _columns.Count)
            {
                throw new InvalidOperationException("Row column count does not match header column count.");
            }

            for (int i = 0; i < row.Length; i++)
            {
                _columns[i].Width = Math.Min(_maxColumnWidth, Math.Max(_columns[i].Width, row[i]?.Length ?? 0));
            }

            _rows.Add(row);
        }

        /* Print the table with highlighting
         * logger: The logger to use for printing
         * highlightTerm: The term to highlight in the table
         */
        public void PrintResult(string highlightTerm, ILoggerWithColor logger)
        {
            if (_rows.Count == 0)
            {
                logger.LogMinimal("No results found.");
                return;
            }
            // Print the header
            PrintRow(logger, _columns.Select(c => c.Header).ToList(), highlightTerm);
            // Print a separator line
            PrintRow(logger, _columns.Select(c => "".PadRight(c.Width, '-')).ToList(), "");

            foreach (string[] row in _rows)
            {
                // Sanitize the values to remove new lines and tabs
                List<string> sanitizedValues = row.Select(v => SanitizeString(v)).ToList();
                PrintRow(logger, sanitizedValues, highlightTerm);

                // Print a separator line
                PrintRow(logger, _columns.Select(c => "".PadRight(c.Width, '-')).ToList(), "");
            }
        }

        private string SanitizeString(string value)
        {
            return Regex.Replace(value ?? string.Empty, @"\r\n|\n\r|\n|\r|\t", " ");
        }

        /* Print a row in the table
         * logger: The logger to use for printing
         * values: The list of values in the row
         * highlightTerm: The term to highlight in the row
         */
        private void PrintRow(ILoggerWithColor logger, List<string> values, string highlightTerm)
        {
            ConsoleColor color = _consoleColor;

            // In one row there could be multiple rows if the value is too long. subRow is the index of the sub row
            int subRow = 0;
            // Keep track of the columns that have been printed
            List<int> renderedColumns = new List<int>();
            bool done = false;

            List<List<int>> highlight = new List<List<int>>();

            // Find the indices of the highlight term in each value
            foreach (string value in values)
            {
                highlight.Add(FindSubstringIndices(value, highlightTerm));
            }

            // Keep printing the row until all the columns have been printed
            while (!done)
            {
                // Print column by column
                for (int column = 0; column < _columns.Count; column++)
                {
                    logger.LogMinimal("| ", color);
                    string value = values[column];

                    // For each column, print character by character with the appropriate color
                    for (int i = 0; i < _columns[column].Width; i++)
                    {
                        int CharacterIndex = subRow * _columns[column].Width + i;

                        // Change to highlighter color if the character index is within the highlight term
                        if (_columnsToHighlight.Contains(column) && highlight[column].Contains(CharacterIndex))
                        {
                            color = _highlighter;
                        }

                        // All the characters have been printed
                        if (CharacterIndex >= value.Length)
                        {
                            if (!renderedColumns.Contains(column))
                            {
                                renderedColumns.Add(column);
                            }

                            logger.LogMinimal("".PadRight(_columns[column].Width - i), color);
                            break;
                        }

                        // If the character index is within the length of the value, print the character
                        if (CharacterIndex < value.Length)
                        {
                            logger.LogMinimal(value[CharacterIndex].ToString(), color);
                        }
                        else
                        {
                            logger.LogMinimal(" ", color);
                        }

                        // If the character index is the last character in the value, add the column to the list of rendered columns
                        if (CharacterIndex == value.Length - 1)
                        {
                            if (!renderedColumns.Contains(column))
                            {
                                renderedColumns.Add(column);
                            }
                        }

                        // Reset the color to the default color
                        color = _consoleColor;
                    }

                    logger.LogMinimal(" ", color);
                }

                // New line for new row
                logger.LogMinimal("|", color);
                logger.LogMinimal("");
                subRow++;

                // If all the columns have been printed, we are done
                if (renderedColumns.Count >= values.Count)
                {
                    done = true;
                }
            }
        }

        private static List<int> FindSubstringIndices(string str, string substring)
        {
            List<int> indices = new List<int>();

            if (string.IsNullOrEmpty(substring))
            {
                return indices;
            }

            int index = 0;
            while ((index = str.IndexOf(substring, index, StringComparison.CurrentCultureIgnoreCase)) != -1)
            {
                for (int i = 0; i < substring.Length; i++)
                {
                    indices.Add(index + i);
                }
                index += substring.Length;
            }

            return indices;
        }
    }
}
