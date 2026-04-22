namespace Microsoft.ApplicationInsights.Tests
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    using System;
    using System.Collections.Generic;
    using DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Shared DependencyTrackingTelemetryModuleTest class.
    /// </summary>
    [TestClass]
    public class HttpDependenciesParsingTelemetryInitializerTest
    {
        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerDoesNotFailOnNull()
        {
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            initializer.Initialize(null);
        }

        [TestMethod]

        public void HttpDependenciesParsingTelemetryInitializerDoesNotFailOnRequestTelemetry()
        {
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            initializer.Initialize(new RequestTelemetry());
        }

        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerDoesNotFailOnNonHttpDependencyTelemetry()
        {
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            initializer.Initialize(new DependencyTelemetry("nonHttp", "blob.core.windows.net", "GET test", "http://blob.core.windows.net/t/t"));
        }

        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerConvertsBlobs()
        {
            // check if Blob parsing is enabled
            // detailed parsing validation is in AzureBlobHttpParserTests
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            var d = new DependencyTelemetry(
                RemoteDependencyConstants.HTTP,
                "myaccount.blob.core.windows.net",
                "GET /my/container/myBlob",
                "https://myaccount.blob.core.windows.net/my/container/myBlob?comp=page&timeout=3");

            initializer.Initialize(d);

            Assert.AreEqual(RemoteDependencyConstants.AzureBlob, d.Type);
        }

        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerConvertsTables()
        {
            var testCases = new List<string[]>()
            {
                ////
                //// copied from https://msdn.microsoft.com/en-us/library/azure/dd179423.aspx 10/18/2016
                ////

                new string[5] { "Set Table Service Properties", "PUT",      "https://myaccount.table.core.windows.net/?restype=service&comp=properties",                            "myaccount", string.Empty },
                new string[5] { "Get Table Service Properties", "GET",      "https://myaccount.table.core.windows.net/?restype=service&comp=properties",                            "myaccount", string.Empty },
                new string[5] { "Preflight Table Request",      "OPTIONS",  "http://myaccount.table.core.windows.net/mytable",                                                      "myaccount", "mytable" },
                new string[5] { "Get Table Service Stats",      "GET",      "https://myaccount-secondary.table.core.windows.net/?restype=service&comp=stats",                       "myaccount-secondary", string.Empty },
                new string[5] { "Query Tables",                 "GET",      "https://myaccount.table.core.windows.net/Tables",                                                      "myaccount", "Tables" },
                new string[5] { "Create Table",                 "POST",     "https://myaccount.table.core.windows.net/Tables",                                                      "myaccount", "Tables" },
                new string[5] { "Delete Table",                 "DELETE",   "https://myaccount.table.core.windows.net/Tables('mytable')",                                           "myaccount", "Tables" },
                new string[5] { "Get Table ACL",                "GET",      "https://myaccount.table.core.windows.net/mytable?comp=acl",                                            "myaccount", "mytable" },
                new string[5] { "Get Table ACL",                "HEAD",     "https://myaccount.table.core.windows.net/mytable?comp=acl",                                            "myaccount", "mytable" },
                new string[5] { "Set Table ACL",                "PUT",      "https://myaccount.table.core.windows.net/mytable?comp=acl",                                            "myaccount", "mytable" },
                new string[5] { "Query Entities",               "GET",      "https://myaccount.table.core.windows.net/mytable(PartitionKey='<partition-key>',RowKey='<row-key>')?$select=<comma-separated-property-names>", "myaccount", "mytable" },
                new string[5] { "Query Entities",               "GET",      "https://myaccount.table.core.windows.net/mytable()?$filter=<query-expression>&$select=<comma-separated-property-names>", "myaccount", "mytable" },
                new string[5] { "Query Entities",               "GET",      "https://myaccount.table.core.windows.net/Customers()?$filter=(Rating%20ge%203)%20and%20(Rating%20le%206)&$select=PartitionKey,RowKey,Address,CustomerSince", "myaccount", "Customers" },
                new string[5] { "Insert Entity",                "POST",     "https://myaccount.table.core.windows.net/mytable",                                     "myaccount", "mytable" },
                new string[5] { "Insert Or Merge Entity",       "MERGE",    "https://myaccount.table.core.windows.net/mytable(PartitionKey='myPartitionKey', RowKey='myRowKey')",   "myaccount", "mytable" },
                new string[5] { "Insert Or Replace Entity",     "PUT",      "https://myaccount.table.core.windows.net/mytable(PartitionKey='myPartitionKey', RowKey='myRowKey')",   "myaccount", "mytable" },
                new string[5] { "Update Entity",                "PUT",      "https://myaccount.table.core.windows.net/mytable(PartitionKey='myPartitionKey', RowKey='myRowKey')",   "myaccount", "mytable" },
                new string[5] { "Merge Entity",                 "MERGE",    "https://myaccount.table.core.windows.net/mytable(PartitionKey='myPartitionKey', RowKey='myRowKey')",   "myaccount", "mytable" },
                new string[5] { "Delete Entity",                "DELETE",   "https://myaccount.table.core.windows.net/mytable(PartitionKey='myPartitionKey', RowKey='myRowKey')",   "myaccount", "mytable" },
            };

            foreach (var testCase in testCases)
            {
                this.HttpDependenciesParsingTelemetryInitializerConvertsTables(testCase[0], testCase[1], testCase[2], testCase[3], testCase[4]);
            }
        }

        public void HttpDependenciesParsingTelemetryInitializerConvertsTables(string operation, string verb, string url, string accountName, string tableName)
        {
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            Uri parsedUrl = new Uri(url);

            // Parse with verb
            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: verb + " " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(RemoteDependencyConstants.AzureTable, d.Type, operation);
            Assert.AreEqual(parsedUrl.Host, d.Target, operation);
            Assert.AreEqual(verb + " " + accountName + "/" + tableName, d.Name, operation);

            // Parse without verb
            d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(RemoteDependencyConstants.AzureTable, d.Type, operation);
            Assert.AreEqual(parsedUrl.Host, d.Target, operation);
            Assert.AreEqual(accountName + "/" + tableName, d.Name, operation);
        }

        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerConvertsQueues()
        {
            var testCases = new List<string[]>()
            {
                ////
                //// copied from https://msdn.microsoft.com/en-us/library/azure/dd179423.aspx 10/19/2016
                ////

                new string[5] { "Set Queue Service Properties",     "PUT",      "https://myaccount.queue.core.windows.net/?restype=service&comp=properties",        "myaccount", string.Empty },
                new string[5] { "Get Queue Service Properties",     "GET",      "https://myaccount.queue.core.windows.net/?restype=service&comp=properties",        "myaccount", string.Empty },
                new string[5] { "List Queues",                      "GET",      "https://myaccount.queue.core.windows.net?comp=list",                               "myaccount", string.Empty },
                new string[5] { "Preflight Queue Request",          "OPTIONS",  "http://myaccount.queue.core.windows.net/myqueue",                                  "myaccount", "myqueue" },
                new string[5] { "Get Queue Service Stats",          "GET",      "https://myaccount-secondary.queue.core.windows.net/?restype=service&comp=stats",   "myaccount-secondary", string.Empty },
                new string[5] { "Create Queue",                     "PUT",      "https://myaccount.queue.core.windows.net/myqueue",                                 "myaccount", "myqueue" },
                new string[5] { "Get Queue Metadata",               "GET",      "https://myaccount.queue.core.windows.net/myqueue?comp=metadata",                   "myaccount", "myqueue" },
                new string[5] { "Get Queue Metadata",               "HEAD",     "https://myaccount.queue.core.windows.net/myqueue?comp=metadata",                   "myaccount", "myqueue" },
                new string[5] { "Get Queue ACL",                    "GET",      "https://myaccount.queue.core.windows.net/myqueue?comp=acl",                        "myaccount", "myqueue" },
                new string[5] { "Get Queue ACL",                    "HEAD",     "https://myaccount.queue.core.windows.net/myqueue?comp=acl",                        "myaccount", "myqueue" },
                new string[5] { "Set Queue ACL",                    "PUT",      "https://myaccount.queue.core.windows.net/myqueue?comp=acl",                        "myaccount", "myqueue" },
                new string[5] { "Put Message",                      "POST",     "https://myaccount.queue.core.windows.net/myqueue/messages?visibilitytimeout=<int-seconds>&messagettl=<int-seconds>",   "myaccount", "myqueue" },
                new string[5] { "Delete Message",                   "DELETE",   "https://myaccount.queue.core.windows.net/myqueue/messages/messageid?popreceipt=string-value",                          "myaccount", "myqueue" },
                new string[5] { "Clear Messages",                   "DELETE",   "https://myaccount.queue.core.windows.net/myqueue/messages",                        "myaccount", "myqueue" },
            };

            foreach (var testCase in testCases)
            {
                this.HttpDependenciesParsingTelemetryInitializerConvertsQueues(testCase[0], testCase[1], testCase[2], testCase[3], testCase[4]);
            }
        }

        public void HttpDependenciesParsingTelemetryInitializerConvertsQueues(string operation, string verb, string url, string accountName, string queueName)
        {
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            Uri parsedUrl = new Uri(url);

            // Parse with verb
            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: verb + " " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(RemoteDependencyConstants.AzureQueue, d.Type, operation);
            Assert.AreEqual(parsedUrl.Host, d.Target, operation);
            Assert.AreEqual(verb + " " + accountName + "/" + queueName, d.Name, operation);

            // Parse without verb
            d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(RemoteDependencyConstants.AzureQueue, d.Type, operation);
            Assert.AreEqual(parsedUrl.Host, d.Target, operation);
            Assert.AreEqual(accountName + "/" + queueName, d.Name, operation);
        }

        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerConvertsServices()
        {
            var testCases = new List<string[]>()
            {
                ////
                //// copied from https://msdn.microsoft.com/en-us/library/azure/dd179423.aspx 10/19/2016
                ////

                new string[4] { "PUT", "https://microsoft.com/test.asmx", RemoteDependencyConstants.WebService, "/test.asmx" },
                new string[4] { "GET", "https://microsoft.com/test.svc", RemoteDependencyConstants.WcfService, "/test.svc" },
                new string[4] { "POST", "https://microsoft.com/test.asmx/myargument", RemoteDependencyConstants.WebService, "/test.asmx" },
                new string[4] { "HEAD", "https://microsoft.com/test.svc/myarguments", RemoteDependencyConstants.WcfService, "/test.svc" },
            };

            foreach (var testCase in testCases)
            {
                this.HttpDependenciesParsingTelemetryInitializerConvertsServices(testCase[0], testCase[1], testCase[2], testCase[3]);
            }
        }

        public void HttpDependenciesParsingTelemetryInitializerConvertsServices(string verb, string url, string expectedType, string expectedName)
        {
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            Uri parsedUrl = new Uri(url);

            // Parse with verb
            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: verb + " " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(expectedType, d.Type);
            Assert.AreEqual(parsedUrl.Host, d.Target);
            Assert.AreEqual(verb + " " + expectedName, d.Name);

            // Parse without verb
            d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(expectedType, d.Type);
            Assert.AreEqual(parsedUrl.Host, d.Target);
            Assert.AreEqual(expectedName, d.Name);
        }

        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerConvertsDocumentDb()
        {
            // check if DocumentDB parsing is enabled
            // detailed parsing validation is in DocumentDbHttpParserTests
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            Uri parsedUrl = new Uri("https://myaccount.documents.azure.com/dbs/myDatabase");

            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: "GET " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(RemoteDependencyConstants.AzureDocumentDb, d.Type);
        }

        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerConvertsServiceBus()
        {
            // check if Service Bus parsing is enabled
            // detailed parsing validation is in AzureServiceBusHttpParserTests
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            Uri parsedUrl = new Uri("https://myaccount.servicebus.windows.net/myQueue/messages");

            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: "POST " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, d.Type);
        }

        [TestMethod]
        public void HttpDependenciesParsingTelemetryInitializerConvertsIotHub()
        {
            // check if IoT Hub parsing is enabled
            HttpDependenciesParsingTelemetryInitializer initializer = new HttpDependenciesParsingTelemetryInitializer();
            Uri parsedUrl = new Uri("https://myaccount.azure-devices.net/devices");

            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: "GET " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            initializer.Initialize(d);

            Assert.AreEqual(RemoteDependencyConstants.AzureIotHub, d.Type);
        }
    }
}