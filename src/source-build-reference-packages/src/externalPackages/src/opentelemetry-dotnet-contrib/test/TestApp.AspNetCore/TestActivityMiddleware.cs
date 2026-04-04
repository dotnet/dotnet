// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace TestApp.AspNetCore;

public class TestActivityMiddleware
{
    public virtual void PreProcess(HttpContext context)
    {
        // Do nothing
    }

    public virtual void PostProcess(HttpContext context)
    {
        // Do nothing
    }
}
