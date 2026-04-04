// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry;
using OpenTelemetry.Trace;
using StackExchange.Redis;

// Prerequisite:
/*
 * Setup redis service inside local docker.
 * docker run --name opentelemetry-redis-test -d -p 6379:6379 redis
 */

// connect to the redis server. The default port 6379 will be used.
var connection = ConnectionMultiplexer.Connect("localhost");

// Configure exporter to export traces to Console
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddConsoleExporter()
    .AddRedisInstrumentation(connection, options =>
    {
        // changing flush interval from 10s to 5s
        options.FlushInterval = TimeSpan.FromSeconds(5);
    })
    .Build();

// select a database (by default, DB = 0)
var db = connection.GetDatabase();
db.StringSet("key", "value " + DateTime.Now.ToLongDateString());
Thread.Sleep(1000);

// run a command, in this case a GET
var myVal = db.StringGet("key");
