// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

public class ServiceResponse
{
    public string? ParameterValue { get; set; }

    public ActivityContext ActivityContext { get; set; }

    public Baggage Baggage { get; set; }
}
