// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.NamedPipes;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Nerdbank.Streams;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class RazorLanguageServerTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public async Task LocaleIsSetCorrectly()
    {
        var (_, serverStream) = FullDuplexStream.CreatePair();
        RazorLanguageServer server;
        using (var host = CreateLanguageServerHost(serverStream, serverStream))
        {
            server = host.GetTestAccessor().Server;

            server.Initialize();
            var queue = server.GetTestAccessor().GetRequestExecutionQueue();

            var initializeParams = JsonSerializer.SerializeToElement(new InitializeParams
            {
                Capabilities = new(),
                Locale = "de-DE"
            });

            await queue.ExecuteAsync(initializeParams, Methods.InitializeName, server.GetLspServices(), DisposalToken);

            // We have to send one more request, because culture is set before any request starts, but the first initialize request has to
            // be started in order to set the culture. The request must be valid because the culture is set in `BeforeRequest` but it doesn't
            // have to succeed.
            try
            {
                var namedPipeParams = new RazorNamedPipeConnectParams()
                {
                    PipeName = ""
                };

                await queue.ExecuteAsync(JsonSerializer.SerializeToElement(namedPipeParams), CustomMessageNames.RazorNamedPipeConnectEndpointName, server.GetLspServices(), DisposalToken);
            }
            catch { }

            var cultureInfo = queue.GetTestAccessor().GetCultureInfo();

            Assert.NotNull(cultureInfo);
            Assert.Equal("de-DE", cultureInfo.Name);
        }

        await server.WaitForExitAsync();
    }

    [Fact]
    public async Task AllHandlersRegisteredAsync()
    {
        var (_, serverStream) = FullDuplexStream.CreatePair();
        RazorLanguageServer server;
        using (var host = CreateLanguageServerHost(serverStream, serverStream))
        {
            server = host.GetTestAccessor().Server;
            var handlerProvider = server.GetTestAccessor().HandlerProvider;

            var registeredMethods = handlerProvider.GetRegisteredMethods();
            var handlerTypes = typeof(RazorLanguageServerHost).Assembly.GetTypes()
                .Where(t => CLaSPTypeHelpers.IMethodHandlerType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

            // We turn this into a Set to handle cases like Completion where we have two handlers, only one of which will be registered
            // CLaSP will throw if two handlers register for the same method, so if THAT doesn't hold it's a CLaSP bug, not a Razor bug.
            var typeMethods = handlerTypes.Select(t => GetMethodFromType(t)).ToHashSet();

            if (registeredMethods.Length != typeMethods.Count)
            {
                var unregisteredHandlers = typeMethods.Where(t => !registeredMethods.Any(m => m.MethodName == t));
                Assert.Fail($"Unregistered handlers: {string.Join(";", unregisteredHandlers.Select(t => t))}");
            }
        }

        await server.WaitForExitAsync();

        static string GetMethodFromType(Type t)
        {
            var attribute = CLaSPTypeHelpers.GetLanguageServerEnpointAttribute(t);
            if (attribute is null)
            {
                foreach (var inter in t.GetInterfaces())
                {
                    attribute = CLaSPTypeHelpers.GetLanguageServerEnpointAttribute(inter);

                    if (attribute is not null)
                    {
                        break;
                    }
                }
            }

            Assert.NotNull(attribute);

            return attribute.Method;
        }
    }

    private RazorLanguageServerHost CreateLanguageServerHost(Stream input, Stream output)
    {
        return RazorLanguageServerHost.Create(
            input,
            output,
            LoggerFactory,
            NoOpTelemetryReporter.Instance,
            configureServices: s =>
            {
                s.AddSingleton<IRazorProjectInfoDriver, TestProjectInfoDriver>();

                // VS Code only handlers are added by rzls, but add here for testing purposes
                s.AddHandler<RazorNamedPipeConnectEndpoint>();
                s.AddHandlerWithCapabilities<DocumentDiagnosticsEndpoint>();
                s.AddSingleton(new LogLevelProvider(CodeAnalysis.Razor.Logging.LogLevel.None));
                s.AddHandler<UpdateLogLevelEndpoint>();
            });
    }

    private class TestProjectInfoDriver : IRazorProjectInfoDriver
    {
        public void AddListener(IRazorProjectInfoListener listener)
        {
        }

        public ImmutableArray<RazorProjectInfo> GetLatestProjectInfo() => [];

        public Task WaitForInitializationAsync() => Task.CompletedTask;
    }
}
