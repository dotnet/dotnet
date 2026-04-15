// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v1.1", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.DiagnosticSource")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyFileVersion("5.0.20.51904")]
[assembly: System.Reflection.AssemblyInformationalVersion("5.0.0+cf258a14b70ad9069470a108f13765e0e5988f51")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("5.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics
{
    public partial class DiagnosticListener : DiagnosticSource, IObservable<Collections.Generic.KeyValuePair<string, object?>>, IDisposable
    {
        public DiagnosticListener(string name) { }

        public static IObservable<DiagnosticListener> AllListeners { get { throw null; } }

        public string Name { get { throw null; } }

        public virtual void Dispose() { }

        public bool IsEnabled() { throw null; }

        public override bool IsEnabled(string name, object? arg1, object? arg2 = null) { throw null; }

        public override bool IsEnabled(string name) { throw null; }

        public virtual IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object?>> observer, Func<string, object?, object?, bool>? isEnabled) { throw null; }

        public virtual IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object?>> observer, Predicate<string>? isEnabled) { throw null; }

        public virtual IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object?>> observer) { throw null; }

        public override string ToString() { throw null; }

        public override void Write(string name, object? value) { }
    }

    public abstract partial class DiagnosticSource
    {
        public virtual bool IsEnabled(string name, object? arg1, object? arg2 = null) { throw null; }

        public abstract bool IsEnabled(string name);
        public abstract void Write(string name, object? value);
    }
}