// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;

namespace Examples.Wcf;

[DataContract]
public class StatusResponse
{
    [DataMember]
    public DateTimeOffset ServerTime { get; set; }
}
