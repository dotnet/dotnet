// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace TestApp.AspNetCore;

public class TestCallbackMiddleware
{
    public virtual async Task<bool> ProcessAsync(HttpContext context)
    {
        return await Task.FromResult(true);
    }
}
