// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.ServiceFabric.Actors;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

public interface IMyTestActor : IActor
{
    Task<ServiceResponse> TestContextPropagation(string valueToReturn);
}
