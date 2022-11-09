// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "We know at this point that the Task is complete so waiting synchronously is fine", Scope = "member", Target = "~M:NuGet.StaFact.NuGetWpfTestCase.CopyTaskResultFrom``1(System.Threading.Tasks.TaskCompletionSource{``0},System.Threading.Tasks.Task{``0})")]
[assembly: SuppressMessage("Usage", "VSTHRD003:Avoid awaiting foreign Tasks", Justification = "The core purpose of this code is to call this task", Scope = "member", Target = "~M:NuGet.StaFact.NuGetWpfTestCase.RunAsync(Xunit.Abstractions.IMessageSink,Xunit.Sdk.IMessageBus,System.Object[],Xunit.Sdk.ExceptionAggregator,System.Threading.CancellationTokenSource)~System.Threading.Tasks.Task{Xunit.Sdk.RunSummary}")]
