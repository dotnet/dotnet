// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

[DataContract]
public class ServiceRequest
{
    public ServiceRequest(string payload)
    {
        this.Payload = payload;
    }

    [DataMember]
    public string Payload { get; set; }
}
