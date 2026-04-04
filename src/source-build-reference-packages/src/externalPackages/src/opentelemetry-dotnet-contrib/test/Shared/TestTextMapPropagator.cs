// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Context.Propagation;

namespace OpenTelemetry.Tests;

internal class TestTextMapPropagator : TextMapPropagator
{
    public Action<PropagationContext, object, Action<object, string, string>>? OnInject { get; set; }

    public Action? Extracted { get; set; }

    public override ISet<string> Fields => throw new NotImplementedException();

    public override PropagationContext Extract<T>(PropagationContext context, T carrier, Func<T, string, IEnumerable<string>?> getter)
    {
        this.Extracted?.Invoke();
        return context;
    }

    public override void Inject<T>(PropagationContext context, T carrier, Action<T, string, string> setter)
    {
        this.OnInject?.Invoke(
            context,
            carrier!,
            new Action<object, string, string>((c, k, v) => setter((T)c, k, v)));
    }
}
