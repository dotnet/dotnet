// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Configuration;
using System.ServiceModel;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Examples.Wcf.Client;

internal static class Program
{
    public static async Task Main()
    {
        using var openTelemetry = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Wcf-Client"))
            .AddWcfInstrumentation()
            .AddOtlpExporter()
            .Build();

        switch (ConfigurationManager.AppSettings["Server"].ToUpperInvariant())
        {
            case "ASPNET":
                await CallService("StatusService_AspNet").ConfigureAwait(false);
                break;
            default:
                await CallService("StatusService_Http").ConfigureAwait(false);
                await CallService("StatusService_Tcp").ConfigureAwait(false);
                await CallService("StatusService_Rest").ConfigureAwait(false);
                break;
        }

        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
    }

    private static async Task CallService(string name)
    {
        // Note: Best practice is to re-use your client/channel instances.
        // This code is not meant to illustrate best practices, only the
        // instrumentation.
        var client = new StatusServiceClient(name);
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
