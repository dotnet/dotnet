// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.DiagnosticSource")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.24705.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.24705.01. Commit Hash: 4d1af962ca0fede10beb01d197367c2f90e92c97")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics
{
    public partial class DiagnosticListener : DiagnosticSource, IObservable<Collections.Generic.KeyValuePair<string, object>>, IDisposable
    {
        public DiagnosticListener(string name) { }

        public static IObservable<DiagnosticListener> AllListeners { get { throw null; } }

        public string Name { get { throw null; } }

        public virtual void Dispose() { }

        public override bool IsEnabled(string name) { throw null; }

        public virtual IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object>> observer, Predicate<string> isEnabled) { throw null; }

        public IDisposable Subscribe(IObserver<Collections.Generic.KeyValuePair<string, object>> observer) { throw null; }

        public override string ToString() { throw null; }

        public override void Write(string name, object value) { }
    }

    public abstract partial class DiagnosticSource
    {
        public abstract bool IsEnabled(string name);
        public abstract void Write(string name, object value);
    }
}