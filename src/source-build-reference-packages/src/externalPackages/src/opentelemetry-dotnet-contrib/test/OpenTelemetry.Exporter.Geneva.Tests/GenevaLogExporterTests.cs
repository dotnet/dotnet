// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#nullable disable

using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public class GenevaLogExporterTests
{
    [Fact]
    public void BadArgs()
    {
        string connectionString;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            connectionString = "EtwSession=OpenTelemetry";
        }
        else
        {
            var path = GenerateTempFilePath();
            connectionString = "Endpoint=unix:" + path;
        }

        // should reject null exporter options
        Assert.Throws<ArgumentNullException>(() =>
        {
            using var exporter = new GenevaLogExporter(null);
        });

        // reserved field in ResourceFieldNames
        Assert.Throws<ArgumentException>(() =>
        {
            using var exporter = new GenevaTraceExporter(new GenevaExporterOptions
            {
                ConnectionString = connectionString,
                ResourceFieldNames = ["env_cloud_role"],
            });
        });

        // should reject mutually exclusive ResourceFieldNames and PrepopulatedFields
        Assert.Throws<ArgumentException>(() =>
        {
            using var exporter = new GenevaTraceExporter(new GenevaExporterOptions
            {
                ConnectionString = connectionString,
                ResourceFieldNames = ["resource"],
                PrepopulatedFields = new Dictionary<string, object>
                {
                    ["prepopulated"] = "hello",
                },
            });
        });
    }

    [Fact]
    public void ExportExceptionStackDefaultIsDrop()
    {
        var exporterOptions = new GenevaExporterOptions();
        Assert.Equal(ExceptionStackExportMode.Drop, exporterOptions.ExceptionStackExportMode);
    }

    [Fact]
    public void ExportEventNameDefaultIsNone()
    {
        var exporterOptions = new GenevaExporterOptions();
        Assert.Equal(EventNameExportMode.None, exporterOptions.EventNameExportMode);
    }

    [Fact]
    public void SpecialCharactersInTableNameMappings()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            using var exporter = new GenevaLogExporter(new GenevaExporterOptions
            {
                TableNameMappings = new Dictionary<string, string> { ["TestCategory"] = "\u0418" },
            });
        });

        Assert.Throws<ArgumentException>(() =>
        {
            using var exporter = new GenevaLogExporter(new GenevaExporterOptions
            {
                TableNameMappings = new Dictionary<string, string> { ["*"] = "\u0418" },
            });
        });

        // Throw on null value - include key in exception message
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            new GenevaExporterOptions
            {
                TableNameMappings = new Dictionary<string, string> { ["TestCategory"] = null },
            };
        });
        Assert.Contains("The table name mapping value provided for key 'TestCategory' was null, empty, or consisted only of white-space characters.", ex.Message);

        // Throw when TableNameMappings is null
        Assert.Throws<ArgumentNullException>(() =>
        {
            new GenevaExporterOptions
            {
                TableNameMappings = null,
            };
        });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void InvalidConnectionString(string connectionString)
    {
        var exporterOptions = new GenevaExporterOptions() { ConnectionString = connectionString };
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            using var exporter = new GenevaLogExporter(exporterOptions);
        });
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Windows)]
    public void IncompatibleConnectionString_Windows()
    {
        var exporterOptions = new GenevaExporterOptions() { ConnectionString = "Endpoint=unix:" + @"C:\Users\user\AppData\Local\Temp\14tj4ac4.v2q" };
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            using var exporter = new GenevaLogExporter(exporterOptions);
        });
        Assert.Equal("Unix domain socket should not be used on Windows.", exception.Message);
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Windows)]
    public void IncompatibleConnectionString_Windows_UserEvents()
    {
        var exporterOptions = new GenevaExporterOptions() { ConnectionString = "PrivatePreviewEnableUserEvents=true" };
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            using var exporter = new GenevaLogExporter(exporterOptions);
        });
        Assert.Equal("Exporting data in user_events is only supported for .NET 8 or later on Linux.", exception.Message);
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux)]
    public void IncompatibleConnectionString_Linux()
    {
        var exporterOptions = new GenevaExporterOptions() { ConnectionString = "EtwSession=OpenTelemetry" };
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            using var exporter = new GenevaLogExporter(exporterOptions);
        });
        Assert.Equal("ETW cannot be used on non-Windows operating systems.", exception.Message);
    }

    [Theory]
    [InlineData("categoryA", "TableA")]
    [InlineData("categoryB", "TableB")]
    [InlineData("categoryA", "TableA", "categoryB", "TableB")]
    [InlineData("categoryA", "TableA", "*", "CatchAll")]
    [InlineData("Example.DefaultService", "myTableName")]
    [InlineData(null)]
    public void TableNameMappingTest(params string[] category)
    {
        // ARRANGE
        var path = string.Empty;
        Dictionary<string, string> mappingsDict = null;
        try
        {
            var exporterOptions = new GenevaExporterOptions();
            if (category?.Length > 0)
            {
                mappingsDict = [];
                for (var i = 0; i < category.Length; i += 2)
                {
                    mappingsDict.Add(category[i], category[i + 1]);
                }

                exporterOptions.TableNameMappings = mappingsDict;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
            }

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                })
                .AddFilter("*", LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            ILogger logger;
            object fluentdData;
            string actualTableName;
            var defaultLogTable = "Log";
            if (mappingsDict != null)
            {
                foreach (var mapping in mappingsDict)
                {
                    if (!mapping.Key.Equals("*"))
                    {
                        logger = loggerFactory.CreateLogger(mapping.Key);
                        logger.LogError("this does not matter");

                        Assert.Single(exportedData);
                        fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
                        actualTableName = (fluentdData as object[])[0] as string;
                        Assert.Equal(mapping.Value, actualTableName);
                        exportedData.Clear();
                    }
                    else
                    {
                        defaultLogTable = mapping.Value;
                    }
                }

                // test default table
                logger = loggerFactory.CreateLogger("random category");
                logger.LogError("this does not matter");

                Assert.Single(exportedData);
                fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
                actualTableName = (fluentdData as object[])[0] as string;
                Assert.Equal(defaultLogTable, actualTableName);
                exportedData.Clear();
            }
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Fact]
    public void PassThruTableMappingsWhenTheRuleIsEnabled()
    {
        var path = string.Empty;
        Socket server = null;
        try
        {
            var userInitializedCategoryToTableNameMappings = new Dictionary<string, string>
            {
                ["Company.Store"] = "Store",
                ["Company.Orders"] = "Orders",
                ["*"] = "*",
            };

            var expectedCategoryToTableNameList = new List<KeyValuePair<string, string>>
            {
                // The category name must match "^[A-Z][a-zA-Z0-9]*$"; any character that is not allowed will be removed.
                new("Company.Customer", "CompanyCustomer"),

                new("Company-%-Customer*Region$##", "CompanyCustomerRegion"),

                // If the first character in the resulting string is lower-case ALPHA,
                // it will be converted to the corresponding upper-case.
                new("company.Calendar", "CompanyCalendar"),

                // After removing not allowed characters,
                // if the resulting string is still an illegal event name, the data will get dropped on the floor.
                new("$&-.$~!!", null),

                new("dlmwl3bvd84bxsx8wf700nx9rydrrhfewbxf82ceoo0h8rpla4", "Dlmwl3bvd84bxsx8wf700nx9rydrrhfewbxf82ceoo0h8rpla4"),

                // If the resulting string is longer than 50 characters, only the first 50 characters will be taken.
                new("Company.Customer.rsLiheLClHJasBOvM.XI4uW7iop6ghvwBzahfs", "CompanyCustomerrsLiheLClHJasBOvMXI4uW7iop6ghvwBzah"),

                // The data will be dropped on the floor as the exporter cannot deduce a valid table name.
                new("1.2", null),
            };

            var exporterOptions = new GenevaExporterOptions
            {
                TableNameMappings = userInitializedCategoryToTableNameMappings,
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
                server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
                server.Bind(endpoint);
                server.Listen(1);
            }

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                })
                .AddFilter("*", LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            ILogger passThruTableMappingsLogger, userInitializedTableMappingsLogger;
            object fluentdData;
            string actualTableName;

            // Verify that the category table mappings specified by the users in the Geneva Configuration are mapped correctly.
            foreach (var mapping in userInitializedCategoryToTableNameMappings)
            {
                if (mapping.Key != "*")
                {
                    userInitializedTableMappingsLogger = loggerFactory.CreateLogger(mapping.Key);
                    userInitializedTableMappingsLogger.LogInformation("This information does not matter.");

                    Assert.Single(exportedData);

                    fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
                    actualTableName = (fluentdData as object[])[0] as string;
                    userInitializedCategoryToTableNameMappings.TryGetValue(mapping.Key, out var expectedTableNme);
                    Assert.Equal(expectedTableNme, actualTableName);
                }

                exportedData.Clear();
            }

            // Verify that when the "*" = "*" were enabled, the correct table names were being deduced following the set of rules.
            foreach (var mapping in expectedCategoryToTableNameList)
            {
                passThruTableMappingsLogger = loggerFactory.CreateLogger(mapping.Key);
                passThruTableMappingsLogger.LogInformation("This information does not matter.");

                Assert.Single(exportedData);
                fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
                actualTableName = (fluentdData as object[])[0] as string;
                var expectedTableName = string.Empty;
                expectedTableName = mapping.Value;
                Assert.Equal(expectedTableName, actualTableName);

                exportedData.Clear();
            }
        }
        finally
        {
            server?.Dispose();
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SerializeILoggerScopes(bool hasCustomFields)
    {
        var path = string.Empty;
        try
        {
            var exporterOptions = new GenevaExporterOptions();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
            }

            if (hasCustomFields)
            {
                exporterOptions.CustomFields = ["Food", "Name", "Key1"];
            }

            var exportedLogs = new List<LogRecord>();

            var resourceBuilder = ResourceBuilder.CreateEmpty().AddAttributes([
                new KeyValuePair<string, object>("resourceAttr", "from resource"),
                new KeyValuePair<string, object>("service.name", "BusyWorker"),
                new KeyValuePair<string, object>("service.instanceId", "CY1SCH030021417")
            ]);

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.IncludeScopes = true;
                    options.SetResourceBuilder(resourceBuilder);
                    options.AddInMemoryExporter(exportedLogs);
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                }));

            List<ArraySegment<byte>> exportedData = [];

            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of internal buffer for validation.
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            using (logger.BeginScope("MyOuterScope"))
            using (logger.BeginScope("MyInnerScope"))
            using (logger.BeginScope("MyInnerInnerScope with {Name} and {Age} of custom", "John Doe", 25))
            using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("Key1", "Value1"), new("Key2", "Value2") }))
            {
                logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);
            }

            Assert.Single(exportedLogs);
            Assert.Single(exportedData);
            var logRecord = exportedLogs[0];

            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            var signal = (fluentdData as object[])[0] as string;
            var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
            var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;

            if (hasCustomFields)
            {
                var envProperties = mapping["env_properties"] as Dictionary<object, object>;

                // Custom Fields
                Assert.Equal("artichoke", mapping["Food"]);
                Assert.Equal("John Doe", mapping["Name"]);
                Assert.Equal("Value1", mapping["Key1"]);

                Assert.False(mapping.ContainsKey("MyOuterScope"));
                Assert.False(mapping.ContainsKey("MyInnerScope"));

                // env_properties
                Assert.True(Equals(envProperties["Price"], 3.99));
                Assert.Equal((byte)25, envProperties["Age"]);
                Assert.Equal("Value2", envProperties["Key2"]);

                Assert.False(envProperties.ContainsKey("MyOuterScope"));
                Assert.False(envProperties.ContainsKey("MyInnerScope"));
            }
            else
            {
                Assert.Equal("artichoke", mapping["Food"]);
                Assert.True(Equals(mapping["Price"], 3.99));
                Assert.Equal("John Doe", mapping["Name"]);
                Assert.Equal((byte)25, mapping["Age"]);
                Assert.Equal("Value1", mapping["Key1"]);
                Assert.Equal("Value2", mapping["Key2"]);

                Assert.False(mapping.ContainsKey("MyOuterScope"));
                Assert.False(mapping.ContainsKey("MyInnerScope"));
            }

            // Check other fields
            this.AssertFluentdForwardModeForLogRecord(exporterOptions, Resource.Empty, fluentdData, logRecord);
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void SerializationTestWithILoggerLogMethod(bool includeFormattedMessage, bool includeResourceAttributes)
    {
        // Dedicated test for the raw ILogger.Log method
        // https://docs.microsoft.com/dotnet/api/microsoft.extensions.logging.ilogger.log

        // ARRANGE
        var path = string.Empty;
        try
        {
            var exporterOptions = new GenevaExporterOptions();

            if (includeResourceAttributes)
            {
                exporterOptions.ResourceFieldNames = ["resourceAttr"];
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
            }

            var resourceBuilder = ResourceBuilder.CreateEmpty().AddAttributes([
                new KeyValuePair<string, object>("resourceAttr", "from resource"),
                new KeyValuePair<string, object>("service.name", "BusyWorker"),
                new KeyValuePair<string, object>("service.instanceId", "CY1SCH030021417")
            ]);

            using var exporter = new GenevaLogExporter(exporterOptions);
            List<LogRecord> exportedLogs = [];

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(resourceBuilder);
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                    options.AddInMemoryExporter(exportedLogs);
                    options.IncludeFormattedMessage = includeFormattedMessage;
                })
                .AddFilter(typeof(GenevaLogExporterTests).FullName, LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // ACT
            // This is treated as structured logging as the state can be converted to IReadOnlyList<KeyValuePair<string, object>>
            logger.Log(
                LogLevel.Information,
                default,
                new List<KeyValuePair<string, object>>()
                {
                    new("Key1", "Value1"),
                    new("Key2", "Value2"),
                },
                null,
                (state, ex) => "Formatted Message");

            // VALIDATE
            Assert.Single(exportedLogs);
            Assert.Single(exportedData);
            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            this.AssertFluentdForwardModeForLogRecord(exporterOptions, resourceBuilder.Build(), fluentdData, exportedLogs[0]);

            // ARRANGE
            exportedLogs.Clear();
            exportedData.Clear();

            // ACT
            // This is treated as Un-structured logging as the state cannot be converted to IReadOnlyList<KeyValuePair<string, object>>
            logger.Log(
                LogLevel.Information,
                default,
                state: "somestringasdata",
                exception: null,
                formatter: (state, ex) => "Formatted Message");

            // VALIDATE
            Assert.Single(exportedLogs);
            Assert.Single(exportedData);
            fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            this.AssertFluentdForwardModeForLogRecord(exporterOptions, resourceBuilder.Build(), fluentdData, exportedLogs[0]);

            // ARRANGE
            exportedLogs.Clear();
            exportedData.Clear();

            // ACT
            // This is treated as Un-structured logging as the state cannot be converted to IReadOnlyList<KeyValuePair<string, object>>
            logger.Log(
                LogLevel.Information,
                default,
                state: "somestringasdata",
                exception: null,
                formatter: null);

            // VALIDATE
            Assert.Single(exportedLogs);
            Assert.Single(exportedData);
            fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            this.AssertFluentdForwardModeForLogRecord(exporterOptions, resourceBuilder.Build(), fluentdData, exportedLogs[0]);

            // ARRANGE
            exportedLogs.Clear();
            exportedData.Clear();

            // ACT
            // This is treated as Structured logging as the state can be converted to IReadOnlyList<KeyValuePair<string, object>>
            logger.Log(
                logLevel: LogLevel.Information,
                eventId: default,
                new List<KeyValuePair<string, object>>()
                {
                    new("Key1", "Value1"),
                },
                exception: null,
                formatter: (state, ex) => "Example formatted message.");

            // VALIDATE
            Assert.Single(exportedLogs);
            Assert.Single(exportedData);
            fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            this.AssertFluentdForwardModeForLogRecord(exporterOptions, resourceBuilder.Build(), fluentdData, exportedLogs[0]);
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    [InlineData(true, true, false)]
    [InlineData(false, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, false, true)]
    [InlineData(true, true, true)]
    public void SerializationTestWithILoggerLogWithTemplates(bool hasTableNameMapping, bool hasCustomFields, bool parseStateValues)
    {
        var path = string.Empty;
        Socket server = null;
        var logRecordList = new List<LogRecord>();
        try
        {
            var exporterOptions = new GenevaExporterOptions
            {
                PrepopulatedFields = new Dictionary<string, object>
                {
                    ["cloud.roleVer"] = "9.0.15289.2",
                    ["prepopulated"] = "prepopulated field",
                },
            };

            var resource = ResourceBuilder.CreateEmpty().AddAttributes([
                new KeyValuePair<string, object>("resourceAttr", "from resource"),
                new KeyValuePair<string, object>("service.name", "BusyWorker"),
                new KeyValuePair<string, object>("service.instanceId", "CY1SCH030021417")])
                .Build();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
                server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
                server.Bind(endpoint);
                server.Listen(1);
            }

            if (hasTableNameMapping)
            {
                exporterOptions.TableNameMappings = new Dictionary<string, string>
                {
                    { typeof(GenevaLogExporterTests).FullName, "CustomLogRecord" },
                    { "*", "DefaultLogRecord" },
                };
            }

            if (hasCustomFields)
            {
                // The field "customField" of LogRecord.State should be present in the mapping as a separate key. Other fields of LogRecord.State which are not present
                // in CustomFields should be added in the mapping under "env_properties"
                exporterOptions.CustomFields = ["customField"];
            }

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddInMemoryExporter(logRecordList);
                    options.ParseStateValues = parseStateValues;
                })
                .AddFilter(typeof(GenevaLogExporterTests).FullName, LogLevel.Trace)); // Enable all LogLevels

            // Create a test exporter to get MessagePack byte data to validate if the data was serialized correctly.
            using var exporter = new MsgPackLogExporter(exporterOptions, () => resource);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // Set the ActivitySourceName to the unique value of the test method name to avoid interference with
            // the ActivitySource used by other unit tests.
            var sourceName = GetTestMethodName();

            using var listener = new ActivityListener();
            listener.ShouldListenTo = (activitySource) => activitySource.Name == sourceName;
            listener.Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded;
            ActivitySource.AddActivityListener(listener);

            using var source = new ActivitySource(sourceName);

            using (var activity = source.StartActivity("Activity"))
            {
                // Log inside an activity to set LogRecord.TraceId and LogRecord.SpanId
                logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99); // structured logging
            }

            // When the exporter options are configured with TableMappings only "customField" will be logged as a separate key in the mapping
            // "property" will be logged under "env_properties" in the mapping
            logger.Log(LogLevel.Trace, 101, "Log a {CustomField} and {Property}", "CustomFieldValue", "PropertyValue");
            logger.Log(LogLevel.Trace, 101, "Log a {CustomField} and {Property}", "CustomFieldValue", null);
            logger.Log(LogLevel.Trace, 101, "Log a {CustomField} and {Property}", null, "PropertyValue");
            logger.Log(LogLevel.Debug, 101, "Log a {CustomField} and {Property}", "CustomFieldValue", "PropertyValue");
            logger.Log(LogLevel.Information, 101, "Log a {CustomField} and {Property}", "CustomFieldValue", "PropertyValue");
            logger.Log(LogLevel.Warning, 101, "Log a {CustomField} and {Property}", "CustomFieldValue", "PropertyValue");
            logger.Log(LogLevel.Error, 101, "Log a {CustomField} and {Property}", "CustomFieldValue", "PropertyValue");
            logger.Log(LogLevel.Critical, 101, "Log a {CustomField} and {Property}", "CustomFieldValue", "PropertyValue");
            logger.LogInformation("Hello World!"); // unstructured logging
            logger.LogError(new InvalidOperationException("Oops! Food is spoiled!"), "Hello from {Food} {Price}.", "artichoke", 3.99);

            // Exception with a non-ASCII character in its type name
            logger.LogError(new CustomException\u0418(), "Hello from {Food} {Price}.", "artichoke", 3.99);

            var loggerWithDefaultCategory = loggerFactory.CreateLogger("DefaultCategory");
            loggerWithDefaultCategory.LogInformation("Basic test");
            loggerWithDefaultCategory.LogInformation("\u0418"); // Include non-ASCII characters in the message

            // logRecordList should have 14 logRecord entries as there were 14 Log calls
            Assert.Equal(14, logRecordList.Count);

            foreach (var logRecord in logRecordList)
            {
                var serializedLog = exporter.SerializeLogRecord(logRecord);
                var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(serializedLog, MessagePack.Resolvers.ContractlessStandardResolver.Options);
                this.AssertFluentdForwardModeForLogRecord(exporterOptions, resource, fluentdData, logRecord);
            }
        }
        finally
        {
            server?.Dispose();
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Windows)]
    public void SuccessfulExport_Windows()
    {
        var exporterOptions = new GenevaExporterOptions()
        {
            PrepopulatedFields = new Dictionary<string, object>
            {
                ["cloud.role"] = "BusyWorker",
                ["cloud.roleInstance"] = "CY1SCH030021417",
                ["cloud.roleVer"] = "9.0.15289.2",
            },
        };

        using var loggerFactory = LoggerFactory.Create(builder => builder
            .AddOpenTelemetry(options =>
            {
                options.AddGenevaLogExporter(options =>
                {
                    options.ConnectionString = "EtwSession=OpenTelemetry";
                    options.PrepopulatedFields = new Dictionary<string, object>
                    {
                        ["cloud.role"] = "BusyWorker",
                        ["cloud.roleInstance"] = "CY1SCH030021417",
                        ["cloud.roleVer"] = "9.0.15289.2",
                    };
                });
            }));

        var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

        logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);
    }

    [Fact]
    public void AutoMappedResourceAttrDoesNotOverridePrepopulated()
    {
        var path = GenerateTempFilePath();
        try
        {
            var endpoint = new UnixDomainSocketEndPoint(path);

            var exporterOptions = new GenevaExporterOptions
            {
                PrepopulatedFields = new Dictionary<string, object>
                {
                    ["cloud.role"] = "cloud.role from prepopulated",
                },
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
            }

            using var exporter = new GenevaLogExporter(exporterOptions);

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            var resourceBuilder = ResourceBuilder.CreateDefault().AddService("cloud.role from resource");

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(resourceBuilder);
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                }));

            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            logger.LogInformation("Hello");

            Assert.Single(exportedData);
            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);

            Assert.Equal("cloud.role from prepopulated", GetField(fluentdData, "env_cloud_role"));
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Fact]
    public void AutoMappedResourceAttr()
    {
        var path = GenerateTempFilePath();
        try
        {
            var endpoint = new UnixDomainSocketEndPoint(path);

            var exporterOptions = new GenevaExporterOptions
            {
                PrepopulatedFields = new Dictionary<string, object>
                {
                    // no prepopulated fields
                },
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
            }

            using var exporter = new GenevaLogExporter(exporterOptions);

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            var resourceBuilder = ResourceBuilder.CreateDefault().AddService("cloud.role from resource");

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(resourceBuilder);
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                }));

            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            logger.LogInformation("Hello");

            Assert.Single(exportedData);
            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);

            Assert.Equal("cloud.role from resource", GetField(fluentdData, "env_cloud_role"));
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux)]
    public void SuccessfulExport_Linux()
    {
        var path = GenerateTempFilePath();
        var exportedLogs = new List<LogRecord>();
        try
        {
            var endpoint = new UnixDomainSocketEndPoint(path);
            using var server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
            server.Bind(endpoint);
            server.Listen(1);

            // Create a test exporter to get MessagePack byte data for validation of the data received via Socket.
            var exporterOptions = new GenevaExporterOptions
            {
                ConnectionString = "Endpoint=unix:" + path,
            };

            using var exporter = new GenevaLogExporter(exporterOptions);

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            var resourceBuilder = ResourceBuilder.CreateDefault();

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(resourceBuilder);
                    options.AddInMemoryExporter(exportedLogs);
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                }));

            using var serverSocket = server.Accept();
            serverSocket.ReceiveTimeout = 10000;

            // Emit a LogRecord and grab a copy of internal buffer for validation.
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);

            // logRecordList should have a singleLogRecord entry after the logger.LogInformation call
            Assert.Single(exportedLogs);
            Assert.Single(exportedData);

            // Read the data sent via socket.
            var receivedData = new byte[1024];
            var receivedDataSize = serverSocket.Receive(receivedData);

            // the number of bytes received over the socket should match the number of bytes of data exported
            Assert.Equal(exportedData[0].Count, receivedDataSize);

            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            this.AssertFluentdForwardModeForLogRecord(exporterOptions, resourceBuilder.Build(), fluentdData, exportedLogs[0]);

            exportedLogs.Clear();
            exportedData.Clear();

            // Emit log on a different thread to test for multithreading scenarios
            var thread = new Thread(() =>
            {
                logger.LogInformation("Hello from another thread {Food} {Price}.", "artichoke", 3.99);
            });
            thread.Start();
            thread.Join();

            // logRecordList should have a singleLogRecord entry after the logger.LogInformation call
            Assert.Single(exportedLogs);
            Assert.Single(exportedData);

            // Read the data sent via socket.
            receivedDataSize = serverSocket.Receive(receivedData);

            // the number of bytes received over the socket should match the number of bytes of data exported
            Assert.Equal(exportedData[0].Count, receivedDataSize);

            fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            this.AssertFluentdForwardModeForLogRecord(exporterOptions, resourceBuilder.Build(), fluentdData, exportedLogs[0]);
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Fact]
    public void SerializationTestForException()
    {
        // ARRANGE
        var path = string.Empty;
        Socket server = null;
        try
        {
            var exporterOptions = new GenevaExporterOptions();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
                server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
                server.Bind(endpoint);
                server.Listen(1);
            }

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddProcessor(sp => new ReentrantExportProcessor<LogRecord>(exporter));
                })
                .AddFilter(typeof(GenevaLogExporterTests).FullName, LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // ACT
            // This is treated as structured logging as the state can be converted to IReadOnlyList<KeyValuePair<string, object>>
            logger.Log<object>(
                logLevel: LogLevel.Information,
                eventId: default,
                state: null,
                exception: new Exception("Exception Message"),
                formatter: null);

            // VALIDATE
            Assert.Single(exportedData);
            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            var exceptionType = GetField(fluentdData, "env_ex_type");
            var exceptionMessage = GetField(fluentdData, "env_ex_msg");
            Assert.Equal("System.Exception", exceptionType);
            Assert.Equal("Exception Message", exceptionMessage);
        }
        finally
        {
            server?.Dispose();
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Theory]
    [InlineData(EventNameExportMode.None, false)]
    [InlineData(EventNameExportMode.None, true)]
    [InlineData(EventNameExportMode.ExportAsPartAName, false)]
    [InlineData(EventNameExportMode.ExportAsPartAName, true)]
    public void SerializationTestForEventName(EventNameExportMode eventNameExportMode, bool hasTableNameMapping)
    {
        // ARRANGE
        var path = string.Empty;
        Socket server = null;
        try
        {
            var exporterOptions = new GenevaExporterOptions
            {
                EventNameExportMode = eventNameExportMode,
            };

            if (hasTableNameMapping)
            {
                exporterOptions.TableNameMappings = new Dictionary<string, string>()
                {
                    ["*"] = "CustomTableName",
                };
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
                server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
                server.Bind(endpoint);
                server.Listen(1);
            }

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddProcessor(sp => new ReentrantExportProcessor<LogRecord>(exporter));
                }));

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // ACT
            // This is treated as structured logging as the state can be converted to IReadOnlyList<KeyValuePair<string, object>>

            #region Test for `ILogger.Log`
            logger.Log<object>(
                logLevel: LogLevel.Information,
                eventId: new EventId(1, "TestEventNameWithLogMethod"),
                state: null,
                exception: new Exception("Exception Message"),
                formatter: null);

            // VALIDATE
            Assert.Single(exportedData);
            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            var eventName = GetField(fluentdData, "env_name");

            if (eventNameExportMode.HasFlag(EventNameExportMode.ExportAsPartAName))
            {
                Assert.Equal("TestEventNameWithLogMethod", eventName);
            }
            else
            {
                Assert.Equal(hasTableNameMapping ? "CustomTableName" : "Log", eventName);
            }

            #endregion

            exportedData.Clear();

            #region Test for extension method
            logger.LogInformation(eventId: new EventId(1, "TestEventNameWithLogExtensionMethod"), "Hello from {Name} {Price}.", "tomato", 2.99);

            Assert.Single(exportedData);
            fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            eventName = GetField(fluentdData, "env_name");

            if (eventNameExportMode.HasFlag(EventNameExportMode.ExportAsPartAName))
            {
                Assert.Equal("TestEventNameWithLogExtensionMethod", eventName);
            }
            else
            {
                Assert.Equal(hasTableNameMapping ? "CustomTableName" : "Log", eventName);
            }
            #endregion

            exportedData.Clear();

            #region Test with eventName as null
            logger.LogInformation(eventId: 1, "Hello from {Name} {Price}.", "tomato", 2.99);

            Assert.Single(exportedData);
            fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            eventName = GetField(fluentdData, "env_name");
            Assert.Equal(hasTableNameMapping ? "CustomTableName" : "Log", eventName);
            #endregion

        }
        finally
        {
            server?.Dispose();
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Theory]
    [InlineData(false, false, "Custom name")]
    [InlineData(false, false, "")]
    [InlineData(false, false, null)]
    [InlineData(false, false, 12345)]
    [InlineData(true, false, "Custom name")]
    [InlineData(true, true, "Custom name")]
    [InlineData(true, true, 12345)]
    [InlineData(true, false, 12345)]
    public void SerializationTestForPartBName(bool hasCustomFields, bool hasNameInCustomFields, object customNameValue)
    {
        // ARRANGE
        var path = string.Empty;
        try
        {
            var exporterOptions = new GenevaExporterOptions();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
            }

            if (hasCustomFields)
            {
                exporterOptions.CustomFields = hasNameInCustomFields ? ["name", "Key1"] : ["Key1"];
            }

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                }));

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // ACT
            // This is treated as structured logging as the state can be converted to IReadOnlyList<KeyValuePair<string, object>>

            var state = new List<KeyValuePair<string, object>>()
            {
                new("Key1", "Value1"),
                new("Key2", "Value2"),
            };

            if (customNameValue != null)
            {
                state.Add(new KeyValuePair<string, object>("name", customNameValue));
            }

            logger.Log(
                LogLevel.Information,
                default,
                state,
                null,
                null);

            // VALIDATE
            Assert.Single(exportedData);
            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            var signal = (fluentdData as object[])[0] as string;
            var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
            var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;
            var actualNameValue = mapping["name"];

            if (!hasCustomFields || hasNameInCustomFields)
            {
                if (customNameValue is string stringNameValue)
                {
                    Assert.Equal(stringNameValue, actualNameValue);
                }
                else
                {
                    Assert.Equal(typeof(GenevaLogExporterTests).FullName, actualNameValue);
                }
            }
            else
            {
                Assert.Equal(typeof(GenevaLogExporterTests).FullName, actualNameValue);
                if (customNameValue != null)
                {
                    var envProperties = mapping["env_properties"] as Dictionary<object, object>;
                    if (customNameValue is int customNameNumber)
                    {
                        Assert.Equal(Convert.ToInt32(envProperties["name"]), customNameNumber);
                    }
                    else
                    {
                        Assert.Equal((string)envProperties["name"], (string)customNameValue);
                    }
                }
            }
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Fact]
    public void SerializationTestForEventId()
    {
        // ARRANGE
        var path = string.Empty;
        Socket server = null;
        try
        {
            var exporterOptions = new GenevaExporterOptions();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
                server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
                server.Bind(endpoint);
                server.Listen(1);
            }

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddProcessor(sp => new ReentrantExportProcessor<LogRecord>(exporter));
                })
                .AddFilter(typeof(GenevaLogExporterTests).FullName, LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // ACT
            // This is treated as structured logging as the state can be converted to IReadOnlyList<KeyValuePair<string, object>>
            logger.Log<object>(
                logLevel: LogLevel.Information,
                eventId: new EventId(1, "logger-event-name"),
                state: null,
                exception: null,
                formatter: null);

            // VALIDATE
            Assert.Single(exportedData);
            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);

            var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
            var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;
            var eventId = GetField(fluentdData, "eventId");
            Assert.Equal((byte)1, eventId);
        }
        finally
        {
            server?.Dispose();
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Windows)]
    public void TLDLogExporter_Success_Windows()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder
            .AddOpenTelemetry(loggerOptions =>
            {
                loggerOptions.AddGenevaLogExporter(exporterOptions =>
                {
                    exporterOptions.ConnectionString = "EtwSession=OpenTelemetry;PrivatePreviewEnableTraceLoggingDynamic=true";
                    exporterOptions.PrepopulatedFields = new Dictionary<string, object>
                    {
                        ["cloud.role"] = "BusyWorker",
                        ["cloud.roleInstance"] = "CY1SCH030021417",
                        ["cloud.roleVer"] = "9.0.15289.2",
                    };
                });
            }));

        var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

        logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);
    }

    [Fact]
    public void AddGenevaExporterWithNamedOptions()
    {
        var connectionString = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "EtwSession=OpenTelemetry"
            : "Endpoint=unix:" + @"C:\Users\user\AppData\Local\Temp\14tj4ac4.v2q";

        var defaultConfigureExporterOptionsInvocations = 0;
        var namedConfigureExporterOptionsInvocations = 0;

        var sp = new ServiceCollection();
        sp.AddOpenTelemetry().WithLogging(builder => builder
            .ConfigureServices(services =>
            {
                services.Configure<GenevaExporterOptions>(o =>
                {
                    o.ConnectionString = connectionString;
                    defaultConfigureExporterOptionsInvocations++;
                });
                services.Configure<BatchExportLogRecordProcessorOptions>(o => defaultConfigureExporterOptionsInvocations++);

                services.Configure<GenevaExporterOptions>("Exporter2", o =>
                {
                    o.ConnectionString = connectionString;
                    namedConfigureExporterOptionsInvocations++;
                });
                services.Configure<BatchExportLogRecordProcessorOptions>("Exporter2", o => namedConfigureExporterOptionsInvocations++);

                services.Configure<GenevaExporterOptions>("Exporter3", o =>
                {
                    o.ConnectionString = connectionString;
                    namedConfigureExporterOptionsInvocations++;
                });
                services.Configure<BatchExportLogRecordProcessorOptions>("Exporter3", o => namedConfigureExporterOptionsInvocations++);
            })
            .AddGenevaLogExporter()
            .AddGenevaLogExporter("Exporter2", o => { })
            .AddGenevaLogExporter("Exporter3", o => { }));

        var s = sp.BuildServiceProvider();

        _ = s.GetRequiredService<LoggerProvider>();
        Assert.Equal(2, defaultConfigureExporterOptionsInvocations);
        Assert.Equal(4, namedConfigureExporterOptionsInvocations);
    }

    [Fact]
    public void AddGenevaBatchExportProcessorOptions()
    {
        var connectionString = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "EtwSession=OpenTelemetry"
            : "Endpoint=unix:" + GenerateTempFilePath();

        var sp = new ServiceCollection();
        sp.AddOpenTelemetry().WithLogging(builder => builder
            .ConfigureServices(services =>
            {
                services.Configure<GenevaExporterOptions>(o =>
                {
                    o.ConnectionString = connectionString;
                });
                services.Configure<BatchExportLogRecordProcessorOptions>(o => o.ScheduledDelayMilliseconds = 100);
            })
            .AddGenevaLogExporter());

        var s = sp.BuildServiceProvider();

        var loggerProvider = s.GetRequiredService<LoggerProvider>();

        var bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var processor = typeof(BaseProcessor<LogRecord>)
                    .Assembly
                    .GetType("OpenTelemetry.Logs.LoggerProviderSdk")
                    .GetProperty("Processor", bindingFlags)
                    .GetValue(loggerProvider) as ReentrantExportProcessor<LogRecord>;

            Assert.NotNull(processor);
        }
        else
        {
            var processor = typeof(BaseProcessor<LogRecord>)
                    .Assembly
                    .GetType("OpenTelemetry.Logs.LoggerProviderSdk")
                    .GetProperty("Processor", bindingFlags)
                    .GetValue(loggerProvider) as BatchLogRecordExportProcessor;

            Assert.NotNull(processor);

            var scheduledDelayMilliseconds = typeof(BatchLogRecordExportProcessor)
                .GetField("ScheduledDelayMilliseconds", bindingFlags)
                .GetValue(processor);

            Assert.Equal(100, scheduledDelayMilliseconds);
        }
    }

    [Fact]
    public void InvalidResourceAttrType_PlaceholderMessage()
    {
        var path = string.Empty;
        try
        {
            var exporterOptions = new GenevaExporterOptions
            {
                ResourceFieldNames = ["badresource"],
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
            }

            var resourceAttributes = new Dictionary<string, object>
            {
                { "badresource", new int[1] }, // the exporter does not accept complex types like an array
            };
            var resourceBuilder = ResourceBuilder.CreateEmpty().AddAttributes(resourceAttributes);
            var resource = resourceBuilder.Build();

            using var exporter = new GenevaLogExporter(exporterOptions);
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                    options.SetResourceBuilder(resourceBuilder);
                })
                .AddFilter(typeof(GenevaLogExporterTests).FullName, LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // Set the ActivitySourceName to the unique value of the test method name to avoid interference with
            // the ActivitySource used by other unit tests.
            var sourceName = GetTestMethodName();

            using var listener = new ActivityListener();
            listener.ShouldListenTo = (activitySource) => activitySource.Name == sourceName;
            listener.Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded;
            ActivitySource.AddActivityListener(listener);

            using var source = new ActivitySource(sourceName);

            using (var activity = source.StartActivity("Activity"))
            {
                // Log inside an activity to set LogRecord.TraceId and LogRecord.SpanId
                logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99); // structured logging
            }

            Assert.Single(exportedData);

            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
            var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;

            Assert.Contains("badresource", mapping.Keys);
            Assert.Equal("<Unsupported type: System.Int64[]>", mapping["badresource"]);
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [Fact]
    public void WithoutResourceAttributes()
    {
        var path = string.Empty;
        try
        {
            var exporterOptions = new GenevaExporterOptions
            {
                PrepopulatedFields = new Dictionary<string, object>
                {
                    ["unaffected prepopulated"] = "should be present",
                },

                // ResourceFieldNames not set
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
            }

            var resourceAttributes = new Dictionary<string, object>
            {
                { "resourceAttributes", "should not be present" },
            };
            var resourceBuilder = ResourceBuilder.CreateEmpty().AddAttributes(resourceAttributes);
            var resource = resourceBuilder.Build();

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(resourceBuilder);
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                })
                .AddFilter(typeof(GenevaLogExporterTests).FullName, LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // Set the ActivitySourceName to the unique value of the test method name to avoid interference with
            // the ActivitySource used by other unit tests.
            var sourceName = GetTestMethodName();

            using var listener = new ActivityListener();
            listener.ShouldListenTo = (activitySource) => activitySource.Name == sourceName;
            listener.Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded;
            ActivitySource.AddActivityListener(listener);

            using var source = new ActivitySource(sourceName);

            using (var activity = source.StartActivity("Activity"))
            {
                // Log inside an activity to set LogRecord.TraceId and LogRecord.SpanId
                logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99); // structured logging
            }

            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
            var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;

            Assert.DoesNotContain("resourceAttributes", mapping.Keys);

            if (mapping.ContainsKey("env_properties"))
            {
                var env_properties = mapping["env_properties"] as Dictionary<object, object> ?? [];
                Assert.DoesNotContain("resourceAttributes", env_properties);
            }

            Assert.Contains("unaffected prepopulated", mapping.Keys);
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// The purpose of this test is to make sure that when ResourceFieldNames is set to empty,
    /// that no resource attributes make it to Geneva.
    /// </summary>
    [Fact]
    public void WithEmptyResourceAttributes()
    {
        var path = string.Empty;
        try
        {
            var exporterOptions = new GenevaExporterOptions
            {
                ResourceFieldNames = [], // ResourceFieldNames empty
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
                var endpoint = new UnixDomainSocketEndPoint(path);
            }

            var resourceAttributes = new Dictionary<string, object>
            {
                { "resourceAttributes", "should not be present" },
            };
            var resource = new Resource(resourceAttributes);

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                })
                .AddFilter(typeof(GenevaLogExporterTests).FullName, LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            // Emit a LogRecord and grab a copy of the LogRecord from the collection passed to InMemoryExporter
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            // Set the ActivitySourceName to the unique value of the test method name to avoid interference with
            // the ActivitySource used by other unit tests.
            var sourceName = GetTestMethodName();

            using var listener = new ActivityListener();
            listener.ShouldListenTo = (activitySource) => activitySource.Name == sourceName;
            listener.Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded;
            ActivitySource.AddActivityListener(listener);

            using var source = new ActivitySource(sourceName);

            using (var activity = source.StartActivity("Activity"))
            {
                // Log inside an activity to set LogRecord.TraceId and LogRecord.SpanId
                logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99); // structured logging
            }

            Assert.Single(exportedData);

            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);
            var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
            var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;

            Assert.DoesNotContain("resourceAttributes", mapping.Keys);

            if (mapping.ContainsKey("env_properties"))
            {
                var env_properties = mapping["env_properties"] as Dictionary<object, object> ?? [];
                Assert.DoesNotContain("resourceAttributes", env_properties);
            }
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux)]
    public void SuccessfulUserEventsExport_Linux()
    {
#if NET
        var logRecordList = new List<LogRecord>();
        try
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(options =>
                {
                    options.AddGenevaLogExporter(options =>
                    {
                        options.ConnectionString = "PrivatePreviewEnableUserEvents=true";
                        options.PrepopulatedFields = new Dictionary<string, object>
                        {
                            ["cloud.role"] = "BusyWorker",
                            ["cloud.roleInstance"] = "CY1SCH030021417",
                            ["cloud.roleVer"] = "9.0.15289.2",
                        };
                    });
                    options.AddInMemoryExporter(logRecordList);
                }));

            // Uncomment this if testing manually with the perf tool.
            // Console.WriteLine("------------- ready to write events -------------");
            // Thread.Sleep(5000);

            // Emit a LogRecord and grab a copy of internal buffer for validation.
            var logger = loggerFactory.CreateLogger<GenevaLogExporterTests>();

            logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);

            // logRecordList should have a singleLogRecord entry after the logger.LogInformation call
            Assert.Single(logRecordList);
        }
        finally
        {
        }
#endif
    }

    private static string GenerateTempFilePath()
    {
        while (true)
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (!File.Exists(path))
            {
                return path;
            }
        }
    }

    private static string GetTestMethodName([CallerMemberName] string callingMethodName = "")
    {
        return callingMethodName;
    }

    private static object GetField(object fluentdData, string key)
    {
        /* Fluentd Forward Mode:
        [
            "Log",
            [
                [ <timestamp>, { "env_ver": "4.0", ... } ]
            ],
            { "TimeFormat": "DateTime" }
        ]
        */

        var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
        var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;

        return mapping.TryGetValue(key, out var value) ? value : null;
    }

    private void AssertFluentdForwardModeForLogRecord(GenevaExporterOptions exporterOptions, Resource resource, object fluentdData, LogRecord logRecord)
    {
        /* Fluentd Forward Mode:
        [
            "Log",
            [
                [ <timestamp>, { "env_ver": "4.0", ... } ]
            ],
            { "TimeFormat": "DateTime" }
        ]
        */

        var signal = (fluentdData as object[])[0] as string;
        var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
        var timeStamp = (DateTime)(TimeStampAndMappings as object[])[0];
        var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;
        var timeFormat = (fluentdData as object[])[2] as Dictionary<object, object>;

        var partAName = "Log";
        if (exporterOptions.TableNameMappings != null)
        {
            if (exporterOptions.TableNameMappings.ContainsKey(logRecord.CategoryName))
            {
                partAName = exporterOptions.TableNameMappings[logRecord.CategoryName];
            }
            else if (exporterOptions.TableNameMappings.ContainsKey("*"))
            {
                partAName = exporterOptions.TableNameMappings["*"];
            }
        }

        Assert.Equal(partAName, signal);

        // Timestamp check
        Assert.Equal(logRecord.Timestamp.Ticks, timeStamp.Ticks);

        // Part A core envelope fields

        var nameKey = MsgPackExporter.V40_PART_A_MAPPING[Schema.V40.PartA.Name];

        // Check if the user has configured a custom table mapping
        Assert.Equal(partAName, mapping[nameKey]);

        // TODO: Update this when we support multiple Schema formats
        var partAVer = "4.0";
        var verKey = MsgPackExporter.V40_PART_A_MAPPING[Schema.V40.PartA.Ver];
        Assert.Equal(partAVer, mapping[verKey]);

        foreach (var item in exporterOptions.PrepopulatedFields)
        {
            var partAValue = item.Value as string;
            var partAKey = item.Key;
            if (MsgPackExporter.V40_PART_A_MAPPING.ContainsKey(item.Key))
            {
                partAKey = MsgPackExporter.V40_PART_A_MAPPING[item.Key];
            }

            Assert.Equal(partAValue, mapping[partAKey]);
        }

        var timeKey = MsgPackExporter.V40_PART_A_MAPPING[Schema.V40.PartA.Time];
        Assert.Equal(logRecord.Timestamp.Ticks, ((DateTime)mapping[timeKey]).Ticks);

        // Part A dt extensions

        if (logRecord.TraceId != default)
        {
            Assert.Equal(logRecord.TraceId.ToHexString(), mapping["env_dt_traceId"]);
        }

        if (logRecord.SpanId != default)
        {
            Assert.Equal(logRecord.SpanId.ToHexString(), mapping["env_dt_spanId"]);
        }

        if (logRecord.Exception != null)
        {
            Assert.Equal(logRecord.Exception.GetType().FullName, mapping["env_ex_type"]);
            Assert.Equal(logRecord.Exception.Message, mapping["env_ex_msg"]);
        }

        // Part A cloud extensions
        if (exporterOptions.PrepopulatedFields != null && exporterOptions.PrepopulatedFields.Count == 0)
        {
            // this mapping only happens when there are no prepopulated fields, to avoid conflicts

            var serviceNameField = resource.Attributes.FirstOrDefault(attr => attr.Key == "service.name");
            if (serviceNameField.Key == "service.name")
            {
                Assert.Contains("env_cloud_role", mapping);
                Assert.Equal(serviceNameField.Value, mapping["env_cloud_role"]);
            }

            var serviceInstanceField = resource.Attributes.FirstOrDefault(attr => attr.Key == "service.instanceId");
            if (serviceInstanceField.Key == "service.instanceId")
            {
                Assert.Contains("env_cloud_roleInstance", mapping);
                Assert.Equal(serviceInstanceField.Value, mapping["env_cloud_roleInstance"]);
            }
        }

        // Part B fields

        // `LogRecord.LogLevel` was marked Obsolete in https://github.com/open-telemetry/opentelemetry-dotnet/pull/4568
#pragma warning disable 0618
        Assert.Equal(logRecord.LogLevel.ToString(), mapping["severityText"]);
        Assert.Equal((byte)(((int)logRecord.LogLevel * 4) + 1), mapping["severityNumber"]);
#pragma warning restore 0618

        Assert.Equal(logRecord.CategoryName, mapping["name"]);

        var isUnstructuredLog = true;
        IReadOnlyList<KeyValuePair<string, object>> stateKeyValuePairList;

        // `LogRecord.State` and `LogRecord.StateValues` were marked Obsolete in https://github.com/open-telemetry/opentelemetry-dotnet/pull/4334
#pragma warning disable 0618
        stateKeyValuePairList = logRecord.State == null
            ? logRecord.StateValues
            : logRecord.State as IReadOnlyList<KeyValuePair<string, object>>;
#pragma warning restore 0618

        if (stateKeyValuePairList != null)
        {
            isUnstructuredLog = stateKeyValuePairList.Count == 1;
        }

        if (isUnstructuredLog)
        {
            // `LogRecord.State` and `LogRecord.StateValues` were marked Obsolete in https://github.com/open-telemetry/opentelemetry-dotnet/pull/4334
#pragma warning disable 0618
            if (logRecord.FormattedMessage != null)
            {
                Assert.Equal(logRecord.FormattedMessage!, mapping["body"]);
            }
            else if (logRecord.State != null)
            {
                Assert.Equal(logRecord.State.ToString(), mapping["body"]);
            }
#pragma warning restore 0618
            else
            {
                Assert.Equal(stateKeyValuePairList[0].Value, mapping["body"]);
            }
        }
        else
        {
            _ = mapping.TryGetValue("env_properties", out var envProperties);
            var envPropertiesMapping = envProperties as IDictionary<object, object>;

            foreach (var item in stateKeyValuePairList)
            {
                if (item.Key == "{OriginalFormat}")
                {
                    Assert.Equal(item.Value.ToString(), mapping["body"]);
                }
                else if (exporterOptions.CustomFields == null || exporterOptions.CustomFields.Contains(item.Key))
                {
                    // It should be found as a custom field
                    if (item.Value != null)
                    {
                        Assert.Equal(item.Value, mapping[item.Key]);
                    }

                    if (envPropertiesMapping != null)
                    {
                        Assert.DoesNotContain(item.Key, envPropertiesMapping.Keys);
                    }
                }
                else
                {
                    // It should be found in env_properties
                    Assert.Equal(item.Value, envPropertiesMapping[item.Key]);
                    Assert.DoesNotContain(item.Key, mapping);
                }
            }

            foreach (var item in resource.Attributes)
            {
                if (item.Key == "service.name" || item.Key == "service.instanceId")
                {
                    // these ones are already checked.
                    continue;
                }

                if (exporterOptions.ResourceFieldNames != null && exporterOptions.ResourceFieldNames.Contains(item.Key))
                {
                    // It should always be found as a custom field
                    if (item.Value != null)
                    {
                        Assert.Equal(item.Value, mapping[item.Key]);
                    }

                    if (envPropertiesMapping != null)
                    {
                        Assert.DoesNotContain(item.Key, envPropertiesMapping.Keys);
                    }
                }
            }
        }

        if (logRecord.EventId != default)
        {
            Assert.Equal(logRecord.EventId.Id, int.Parse(mapping["eventId"].ToString(), CultureInfo.InvariantCulture));
        }

        // Epilogue
        Assert.Equal("DateTime", timeFormat["TimeFormat"]);
    }

    // A custom exception class with non-ASCII character in the type name
    private class CustomException\u0418 : Exception
    {
    }
}
