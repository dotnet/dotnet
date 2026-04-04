// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Instrumentation.Wcf;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Examples.Wcf.Client;

internal static class Program
{
    public static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        using var openTelemetry = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Wcf-Client-Core"))
            .AddWcfInstrumentation()
            .AddOtlpExporter()
            .Build();

        switch (config.GetValue<string>("Server")!.ToUpperInvariant())
        {
            case "ASPNET":
                await CallService(
                    new BasicHttpBinding(BasicHttpSecurityMode.None),
                    new EndpointAddress(config.GetSection("Service").GetValue<string>("AspNetAddress"))).ConfigureAwait(false);
                break;
            default:
                await CallService(
                    new BasicHttpBinding(BasicHttpSecurityMode.None),
                    new EndpointAddress(config.GetSection("Service").GetValue<string>("HttpAddress"))).ConfigureAwait(false);
                await CallService(
                    new NetTcpBinding(SecurityMode.None),
                    new EndpointAddress(config.GetSection("Service").GetValue<string>("TcpAddress"))).ConfigureAwait(false);
                break;
        }

        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
    }

    private static async Task CallService(Binding binding, EndpointAddress remoteAddress)
    {
        // Note: Best practice is to re-use your client/channel instances.
        // This code is not meant to illustrate best practices, only the
        // instrumentation.
        var client = new StatusServiceClient(binding, remoteAddress);
        client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
        try
        {
            await client.OpenAsync().ConfigureAwait(false);

            var response = await client.PingAsync(new StatusRequest(Guid.NewGuid().ToString("N"))).ConfigureAwait(false);

            Console.WriteLine($"Server returned: {response?.ServerTime}");
        }
        finally
        {
            try
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync().ConfigureAwait(false);
                }
            }
            catch
            {
            }
        }
    }
}
