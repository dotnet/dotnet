// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Runtime;

/// <summary>
/// Options to define the runtime metrics.
/// </summary>
public class RuntimeInstrumentationOptions
{
    /*
            /// <summary>
            /// Gets or sets a value indicating whether garbage collection metrics should be collected.
            /// </summary>
            public bool? GcEnabled { get; set; }

    #if NET
            /// <summary>
            /// Gets or sets a value indicating whether jitter metrics should be collected.
            /// </summary>
            public bool? JitEnabled { get; set; }
    #endif

    #if NET
            /// <summary>
            /// Gets or sets a value indicating whether threading metrics should be collected.
            /// </summary>
            public bool? ThreadingEnabled { get; set; }
    #endif

            /// <summary>
            /// Gets or sets a value indicating whether assembly metrics should be collected.
            /// </summary>
            public bool? AssembliesEnabled { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether exception count metrics should be collected.
            /// </summary>
            public bool? ExceptionCountEnabled { get; set; }

            /// <summary>
            /// Gets a value indicating whether all metrics are enabled.
            /// </summary>
            internal bool IsAllEnabled => this.GcEnabled == null
    #if NET
            && this.JitEnabled == null
    #endif
    #if NET
            && this.ThreadingEnabled == null
    #endif
            && this.AssembliesEnabled == null
            && this.ExceptionCountEnabled == null;

            /// <summary>
            /// Gets a value indicating whether garbage collection metrics is enabled.
            /// </summary>
            internal bool IsGcEnabled => this.GcEnabled == true || this.IsAllEnabled;

    #if NET
            /// <summary>
            /// Gets a value indicating whether jitter metrics is enabled.
            /// </summary>
            internal bool IsJitEnabled => this.JitEnabled == true || this.IsAllEnabled;
    #endif

    #if NET
            /// <summary>
            /// Gets a value indicating whether threading metrics is enabled.
            /// </summary>
            internal bool IsThreadingEnabled => this.ThreadingEnabled == true || this.IsAllEnabled;
    #endif

            /// <summary>
            /// Gets a value indicating whether assembly metrics is enabled.
            /// </summary>
            internal bool IsAssembliesEnabled => this.AssembliesEnabled == true || this.IsAllEnabled;

            /// <summary>
            /// Gets a value indicating whether exception count metrics is enabled.
            /// </summary>
            internal bool IsExceptionCountEnabled => this.ExceptionCountEnabled == true || this.IsAllEnabled;
    */
}
