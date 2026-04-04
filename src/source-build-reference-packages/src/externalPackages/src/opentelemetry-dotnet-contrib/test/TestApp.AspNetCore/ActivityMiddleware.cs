// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace TestApp.AspNetCore;

internal class ActivityMiddleware
{
    private readonly TestActivityMiddleware testActivityMiddleware;
    private readonly RequestDelegate next;

    public ActivityMiddleware(RequestDelegate next, TestActivityMiddleware testActivityMiddleware)
    {
        this.next = next;
        this.testActivityMiddleware = testActivityMiddleware;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (this.testActivityMiddleware != null)
        {
            this.testActivityMiddleware.PreProcess(context);
        }

        await this.next(context);

        if (this.testActivityMiddleware != null)
        {
            this.testActivityMiddleware.PostProcess(context);
        }
    }
}
