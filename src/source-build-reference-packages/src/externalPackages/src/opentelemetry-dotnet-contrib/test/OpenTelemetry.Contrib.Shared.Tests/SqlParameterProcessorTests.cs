// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.Tests;

public static class SqlParameterProcessorTests
{
    [Fact]
    public static void HandlesNullCommand()
    {
        // Arrange
        var activity = new Activity("TestActivity");

        // Act
        SqlParameterProcessor.AddQueryParameters(activity, null);

        // Assert
        Assert.Empty(activity.TagObjects);
        Assert.Empty(activity.Tags);
    }

    [Fact]
    public static void HandlesArbitraryObject()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        var command = new object();

        // Act
        SqlParameterProcessor.AddQueryParameters(activity, command);

        // Assert
        Assert.Empty(activity.TagObjects);
        Assert.Empty(activity.Tags);
    }

    [Fact]
    public static void HandlesUnnamedCommandParameters()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        var command = new MyDbCommand();

        command.Parameters.Add(new MyDbCommandParameter(1));
        command.Parameters.Add(new MyDbCommandParameter(2));

        // Act
        SqlParameterProcessor.AddQueryParameters(activity, command);

        // Assert
        Assert.Equal(1, activity.GetTagValue("db.query.parameter.0"));
        Assert.Equal(2, activity.GetTagValue("db.query.parameter.1"));
        Assert.Equal(2, activity.TagObjects.Count());
    }

    [Fact]
    public static void HandlesNamedCommandParameters()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        var command = new MyDbCommand();

        command.Parameters.Add(new MyDbCommandParameter("foo", 1));
        command.Parameters.Add(new MyDbCommandParameter("bar", 2));

        // Act
        SqlParameterProcessor.AddQueryParameters(activity, command);

        // Assert
        Assert.Equal(1, activity.GetTagValue("db.query.parameter.foo"));
        Assert.Equal(2, activity.GetTagValue("db.query.parameter.bar"));
        Assert.Equal(2, activity.TagObjects.Count());
    }

    [Fact]
    public static void HandlesMixedCommandParameters()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        var command = new MyDbCommand();

        command.Parameters.Add(new MyDbCommandParameter("foo", 1));
        command.Parameters.Add(new MyDbCommandParameter(2));
        command.Parameters.Add(new object());
        command.Parameters.Add(new MyDbCommandParameter("FOO", 3));

        // Act
        SqlParameterProcessor.AddQueryParameters(activity, command);

        // Assert
        Assert.Equal(1, activity.GetTagValue("db.query.parameter.foo"));
        Assert.Equal(2, activity.GetTagValue("db.query.parameter.1"));
        Assert.Equal(3, activity.GetTagValue("db.query.parameter.FOO"));
        Assert.Equal(3, activity.TagObjects.Count());
    }

#nullable disable
    private sealed class MyDbCommand : DbCommand
    {
        public override string CommandText { get; set; } = string.Empty;

        public override int CommandTimeout { get; set; }

        public override CommandType CommandType { get; set; }

        public override bool DesignTimeVisible { get; set; }

        public override UpdateRowSource UpdatedRowSource { get; set; }

        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection { get; } = new MyDbParameterCollection();

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel() => throw new NotImplementedException();

        public override int ExecuteNonQuery() => throw new NotImplementedException();

        public override object ExecuteScalar() => throw new NotImplementedException();

        public override void Prepare() => throw new NotImplementedException();

        protected override DbParameter CreateDbParameter() => new MyDbCommandParameter();

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => throw new NotImplementedException();
    }

    private sealed class MyDbCommandParameter() : DbParameter()
    {
        public MyDbCommandParameter(string name, object value)
            : this(value)
        {
            this.ParameterName = name;
        }

        public MyDbCommandParameter(object value)
            : this()
        {
            this.Value = value;
        }

        public override DbType DbType { get; set; }

        public override ParameterDirection Direction { get; set; }

        public override bool IsNullable { get; set; }

        public override string ParameterName { get; set; } = string.Empty;

        public override int Size { get; set; }

        public override string SourceColumn { get; set; } = string.Empty;

        public override bool SourceColumnNullMapping { get; set; }

        public override object Value { get; set; }

        public override void ResetDbType() => throw new NotImplementedException();
    }

    private sealed class MyDbParameterCollection : DbParameterCollection
    {
        public List<object> Parameters { get; } = [];

        public override int Count => this.Parameters.Count;

        public override object SyncRoot => this;

        public override int Add(object value)
        {
            this.Parameters.Add(value);
            return this.Count - 1;
        }

        public override void AddRange(Array values) => throw new NotImplementedException();

        public override void Clear() => throw new NotImplementedException();

        public override bool Contains(object value) => throw new NotImplementedException();

        public override bool Contains(string value) => throw new NotImplementedException();

        public override void CopyTo(Array array, int index) => throw new NotImplementedException();

        public override IEnumerator GetEnumerator() => this.Parameters.GetEnumerator();

        public override int IndexOf(object value) => throw new NotImplementedException();

        public override int IndexOf(string parameterName) => throw new NotImplementedException();

        public override void Insert(int index, object value) => throw new NotImplementedException();

        public override void Remove(object value) => throw new NotImplementedException();

        public override void RemoveAt(int index) => throw new NotImplementedException();

        public override void RemoveAt(string parameterName) => throw new NotImplementedException();

        protected override DbParameter GetParameter(int index) => throw new NotImplementedException();

        protected override DbParameter GetParameter(string parameterName) => throw new NotImplementedException();

        protected override void SetParameter(int index, DbParameter value) => throw new NotImplementedException();

        protected override void SetParameter(string parameterName, DbParameter value) => throw new NotImplementedException();
    }
#nullable restore
}
