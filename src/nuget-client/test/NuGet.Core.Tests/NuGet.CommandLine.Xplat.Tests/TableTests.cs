// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NuGet.CommandLine.XPlat;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class TableTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(50)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(800)]
        [InlineData(9)]
        public void AddRow_AddNColumnedRowInAnNColumnedTable_AddsRow(int columns)
        {
            // Arrange
            Table myTable = new Table(Array.Empty<int>(), Enumerable.Repeat("Header", columns).ToArray());
            List<string[]> expectedTable = new();
            expectedTable.Add(Enumerable.Repeat("Header", columns).ToArray());
            expectedTable.Add(Enumerable.Repeat("row 1", columns).ToArray());

            // Act
            myTable.AddRow(Enumerable.Repeat("row 1", columns).ToArray());

            // Assert
            Assert.Equal(expectedTable, myTable._rows);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(400)]
        [InlineData(5)]
        [InlineData(60)]
        [InlineData(7)]
        public void AddRow_AddTwoColumnedRowInANotTwoColumnedTable_ThrowsAnException(int columns)
        {
            // Arrange
            Table myTable = new Table(Array.Empty<int>(), Enumerable.Repeat("Header", columns).ToArray());

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => myTable.AddRow(new string[] { "row", "row column2" }));

            // Assert
            Assert.Equal("Row column count does not match header column count.", exception.Message);
        }

        [Fact]
        public void PrintResult_ZeroRowsAdded_PrintsNoResultFound()
        {
            // Arrange
            string searchTerm = "TestPackage";
            Mock<ILoggerWithColor> mockLoggerWithColor = new Mock<ILoggerWithColor>();
            Table table = new Table(Array.Empty<int>(), Enumerable.Repeat("header", 4).ToArray());

            // Act
            table.PrintResult(searchTerm, mockLoggerWithColor.Object);

            // Assert
            mockLoggerWithColor.Verify(x => x.LogMinimal("No results found."), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(130)]
        [InlineData(14)]
        [InlineData(100)]
        public void PrintResult_NRowsAdded_PrintsNRows(int rows)
        {
            string searchTerm = "term";
            Mock<ILoggerWithColor> mockLoggerWithColor = new Mock<ILoggerWithColor>();
            Table table = new Table(new int[] { 0, 1, 2, 3 }, new string[] { "column1", "column2", "column3", "column4" });

            for (int i = 0; i < rows; i++)
            {
                table.AddRow(new string[] { "column1", searchTerm, "column3", "column4" });
            }

            // Act
            table.PrintResult(searchTerm, mockLoggerWithColor.Object);

            // Assert
            mockLoggerWithColor.Verify(x => x.LogMinimal("| column1 ", System.Console.ForegroundColor), Times.Exactly(rows + 1));
            mockLoggerWithColor.Verify(x => x.LogMinimal("| column3 ", System.Console.ForegroundColor), Times.Exactly(rows + 1));
            mockLoggerWithColor.Verify(x => x.LogMinimal("| column4 ", System.Console.ForegroundColor), Times.Exactly(rows + 1));
            mockLoggerWithColor.Verify(x => x.LogMinimal("| column2 ", System.Console.ForegroundColor), Times.Exactly(1));
            mockLoggerWithColor.Verify(x => x.LogMinimal(searchTerm, ConsoleColor.Red), Times.Exactly(rows));
        }
    }
}
