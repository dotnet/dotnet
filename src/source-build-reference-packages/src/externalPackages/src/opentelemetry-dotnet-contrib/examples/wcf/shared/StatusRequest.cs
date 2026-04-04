// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.Serialization;

namespace Examples.Wcf;

[DataContract]
public class StatusRequest
{
    public StatusRequest(string status)
    {
        this.Status = status;
    }

    [DataMember]
    public string Status { get; set; }
}
