// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace TestApp.AspNetCore;

internal class CallbackMiddleware
{
    private readonly TestCallbackMiddleware testCallbackMiddleware;
    private readonly RequestDelegate next;

    public CallbackMiddleware(RequestDelegate next, TestCallbackMiddleware testCallbackMiddleware)
    {
        this.next = next;
        this.testCallbackMiddleware = testCallbackMiddleware;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (this.testCallbackMiddleware == null || await this.testCallbackMiddleware.ProcessAsync(context))
        {
            await this.next(context);
        }
    }
}
