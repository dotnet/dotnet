// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Diagnostics.Abstractions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsAotCompatible", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Diagnostic abstractions for Microsoft.Extensions.Diagnostics.\r\n\r\nCommonly Used Types:\r\nMicrosoft.Extensions.Diagnostics.Metrics.IMetricsBuilder\r\nMicrosoft.Extensions.Diagnostics.Metrics.IMetricsListener\r\nMicrosoft.Extensions.Diagnostics.Metrics.InstrumentRule\r\nMicrosoft.Extensions.Diagnostics.Metrics.MeterScope\r\nMicrosoft.Extensions.Diagnostics.Metrics.MetricsBuilderExtensions\r\nMicrosoft.Extensions.Diagnostics.Metrics.MetricsOptions")]
[assembly: System.Reflection.AssemblyFileVersion("10.0.25.52411")]
[assembly: System.Reflection.AssemblyInformationalVersion("10.0.0+b0f34d51fccc69fd334253924abd8d6853fad7aa")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Diagnostics.Abstractions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/dotnet")]
[assembly: System.Reflection.AssemblyVersionAttribute("10.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Diagnostics.Metrics
{
    public partial interface IMetricsBuilder
    {
        DependencyInjection.IServiceCollection Services { get; }
    }

    public partial interface IMetricsListener
    {
        string Name { get; }

        MeasurementHandlers GetMeasurementHandlers();
        void Initialize(IObservableInstrumentsSource source);
        bool InstrumentPublished(System.Diagnostics.Metrics.Instrument instrument, out object? userState);
        void MeasurementsCompleted(System.Diagnostics.Metrics.Instrument instrument, object? userState);
    }

    public partial class InstrumentRule
    {
        public InstrumentRule(string? meterName, string? instrumentName, string? listenerName, MeterScope scopes, bool enable) { }
        public bool Enable { get { throw null; } }
        public string? InstrumentName { get { throw null; } }
        public string? ListenerName { get { throw null; } }
        public string? MeterName { get { throw null; } }
        public MeterScope Scopes { get { throw null; } }
    }
    public partial interface IObservableInstrumentsSource
    {
        void RecordObservableInstruments();
    }

    public partial class MeasurementHandlers
    {
        public System.Diagnostics.Metrics.MeasurementCallback<byte>? ByteHandler { get { throw null; } set { } }
        public System.Diagnostics.Metrics.MeasurementCallback<decimal>? DecimalHandler { get { throw null; } set { } }
        public System.Diagnostics.Metrics.MeasurementCallback<double>? DoubleHandler { get { throw null; } set { } }
        public System.Diagnostics.Metrics.MeasurementCallback<float>? FloatHandler { get { throw null; } set { } }
        public System.Diagnostics.Metrics.MeasurementCallback<int>? IntHandler { get { throw null; } set { } }
        public System.Diagnostics.Metrics.MeasurementCallback<long>? LongHandler { get { throw null; } set { } }
        public System.Diagnostics.Metrics.MeasurementCallback<short>? ShortHandler { get { throw null; } set { } }
    }
    [System.Flags]
    public enum MeterScope
    {
        None = 0,
        Global = 1,
        Local = 2
    }

    public static partial class MetricsBuilderExtensions
    {
        public static IMetricsBuilder AddListener(this IMetricsBuilder builder, IMetricsListener listener) { throw null; }
        public static IMetricsBuilder AddListener<T>(this IMetricsBuilder builder) where T : class, IMetricsListener { throw null; }
        public static IMetricsBuilder ClearListeners(this IMetricsBuilder builder) { throw null; }
        public static IMetricsBuilder DisableMetrics(this IMetricsBuilder builder, string? meterName, string? instrumentName = null, string? listenerName = null, MeterScope scopes = MeterScope.Global | MeterScope.Local) { throw null; }
        public static IMetricsBuilder DisableMetrics(this IMetricsBuilder builder, string? meterName) { throw null; }
        public static MetricsOptions DisableMetrics(this MetricsOptions options, string? meterName, string? instrumentName = null, string? listenerName = null, MeterScope scopes = MeterScope.Global | MeterScope.Local) { throw null; }
        public static MetricsOptions DisableMetrics(this MetricsOptions options, string? meterName) { throw null; }
        public static IMetricsBuilder EnableMetrics(this IMetricsBuilder builder, string? meterName, string? instrumentName = null, string? listenerName = null, MeterScope scopes = MeterScope.Global | MeterScope.Local) { throw null; }
        public static IMetricsBuilder EnableMetrics(this IMetricsBuilder builder, string? meterName) { throw null; }
        public static MetricsOptions EnableMetrics(this MetricsOptions options, string? meterName, string? instrumentName = null, string? listenerName = null, MeterScope scopes = MeterScope.Global | MeterScope.Local) { throw null; }
        public static MetricsOptions EnableMetrics(this MetricsOptions options, string? meterName) { throw null; }
    }
    public partial class MetricsOptions
    {
        public System.Collections.Generic.IList<InstrumentRule> Rules { get { throw null; } }
    }
}