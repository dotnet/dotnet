// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation;

internal static class ActivityInstrumentationHelper
{
    internal static readonly Action<Activity, ActivityKind> SetKindProperty = CreateActivityKindSetter();
    internal static readonly Action<Activity, ActivitySource> SetActivitySourceProperty = CreateActivitySourceSetter();

    private static Action<Activity, ActivitySource> CreateActivitySourceSetter()
    {
#if NET
        return typeof(Activity).GetProperty("Source")!
            .SetMethod!.CreateDelegate<Action<Activity, ActivitySource>>();
#else
        return (Action<Activity, ActivitySource>)typeof(Activity).GetProperty("Source")!
            .SetMethod!.CreateDelegate(typeof(Action<Activity, ActivitySource>));
#endif
    }

    private static Action<Activity, ActivityKind> CreateActivityKindSetter()
    {
#if NET
        return typeof(Activity).GetProperty("Kind")!
            .SetMethod!.CreateDelegate<Action<Activity, ActivityKind>>();
#else
        return (Action<Activity, ActivityKind>)typeof(Activity).GetProperty("Kind")!
            .SetMethod!.CreateDelegate(typeof(Action<Activity, ActivityKind>));
#endif
    }
}
