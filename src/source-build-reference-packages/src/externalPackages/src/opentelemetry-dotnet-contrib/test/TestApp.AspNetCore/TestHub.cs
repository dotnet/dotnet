// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.AspNetCore.SignalR;

namespace TestApp.AspNetCore;

public class TestHub : Hub
{
    public override Task OnConnectedAsync() => base.OnConnectedAsync();

    public void Send(string message)
    {
    }
}
