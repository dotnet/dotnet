// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace OpenTelemetry.Instrumentation.Benchmarks;

[MemoryDiagnoser(displayGenColumns: false)]
public class SqlProcessorBenchmarks
{
    [Params(
         "SELECT * FROM Orders o, OrderDetails od",
         "SELECT order_date\nFROM   (SELECT *\nFROM   orders o\nJOIN customers c\nON o.customer_id = c.customer_id)",
         "INSERT INTO Orders(Id, Name, Bin, Rate) VALUES(1, 'abc''def', 0xFF, 1.23e-5)",
         "UPDATE Orders SET Name = 'foo' WHERE Id = 42",
         "DELETE FROM Orders WHERE Id = 42",
         "CREATE UNIQUE CLUSTERED INDEX IX_Orders_Id ON Orders(Id)",
         "SELECT DISTINCT o.Id FROM Orders o JOIN Customers c ON o.CustomerId = c.Id",
         "SELECT column -- end of line comment\nFROM /* block \n comment */ table",
         "SELECT Col1, Col2, Col3, Col4, Col5 FROM VeryLongTableName_Sales2024_Q4, Another_Very_Long_Table_Name_Inventory")]
    public string Sql { get; set; } = string.Empty;

    [Benchmark]
    public void GetSanitizedSql() => SqlProcessor.GetSanitizedSql(this.Sql);
}
