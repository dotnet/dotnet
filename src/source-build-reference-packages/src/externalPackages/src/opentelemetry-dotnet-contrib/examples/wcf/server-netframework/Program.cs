// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Examples.Wcf.Server;

internal static class Program
{
    public static void Main()
    {
        using var openTelemetry = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Wcf-Server"))
            .AddWcfInstrumentation()
            .AddOtlpExporter()
            .Build();

        var serviceHost = new ServiceHost(typeof(StatusService));
        serviceHost.Open();

        Console.WriteLine("Service listening. Press enter to exit.");
        Console.ReadLine();

        serviceHost.Close();
    }
}
