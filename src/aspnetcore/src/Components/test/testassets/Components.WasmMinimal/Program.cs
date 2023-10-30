// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Components.TestServer.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

Assembly.Load(nameof(TestContentPackage));

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSingleton<AsyncOperationService>();

await builder.Build().RunAsync();
