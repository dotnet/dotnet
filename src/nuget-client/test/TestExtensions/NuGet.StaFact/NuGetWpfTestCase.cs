// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NuGet.StaFact
{
    [DebuggerDisplay(@"\{ class = {TestMethod.TestClass.Class.Name}, method = {TestMethod.Method.Name}, display = {DisplayName}, skip = {SkipReason} \}")]
    public sealed class NuGetWpfTestCase : LongLivedMarshalByRefObject, IXunitTestCase
    {
        private IXunitTestCase _testCase;

        public string DisplayName => _testCase.DisplayName;
        public IMethodInfo Method => _testCase.Method;
        public string SkipReason => _testCase.SkipReason;
        public ITestMethod TestMethod => _testCase.TestMethod;
        public object[] TestMethodArguments => _testCase.TestMethodArguments;
        public Dictionary<string, List<string>> Traits => _testCase.Traits;
        public string UniqueID => _testCase.UniqueID;

        public ISourceInformation SourceInformation
        {
            get => _testCase.SourceInformation;
            set => _testCase.SourceInformation = value;
        }

        public Exception InitializationException => _testCase.InitializationException;
        public int Timeout => _testCase.Timeout;

        public NuGetWpfTestCase(IXunitTestCase testCase)
        {
            if (testCase == null)
            {
                throw new ArgumentNullException(nameof(testCase));
            }

            _testCase = testCase;
        }

        [Obsolete("Called by the deserializer", error: true)]
        public NuGetWpfTestCase() { }

        public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            var taskCompletionSource = new TaskCompletionSource<RunSummary>();
            var thread = new Thread(() =>
                {
                    try
                    {
                        // Set up the SynchronizationContext so that any awaits
                        // resume on the STA thread as they would in a GUI app.
                        SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

                        // Start off the test method.
                        var testCaseTask = _testCase.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);

                        // Arrange to pump messages to execute any async work associated with the test.
                        var frame = new DispatcherFrame();
                        Task forget = Task.Run(async delegate
                        {
                            try
                            {
                                await testCaseTask;
                            }
                            finally
                            {
                                // The test case's execution is done. Terminate the message pump.
                                frame.Continue = false;
                            }
                        });
                        Dispatcher.PushFrame(frame);

                        // Report the result back to the Task we returned earlier.
                        CopyTaskResultFrom(taskCompletionSource, testCaseTask);
                    }
                    catch (Exception e)
                    {
                        taskCompletionSource.SetException(e);
                    }
                    finally
                    {
                        Dispatcher.CurrentDispatcher.InvokeShutdown();
                    }
                });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return taskCompletionSource.Task;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            _testCase = info.GetValue<IXunitTestCase>("InnerTestCase");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("InnerTestCase", _testCase);
        }

        private static void CopyTaskResultFrom<T>(TaskCompletionSource<T> taskCompletionSource, Task<T> template)
        {
            if (taskCompletionSource == null)
            {
                throw new ArgumentNullException(nameof(taskCompletionSource));
            }

            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (!template.IsCompleted)
            {
                throw new ArgumentException("Task must be completed first.", nameof(template));
            }

            if (template.IsFaulted)
            {
                taskCompletionSource.SetException(template.Exception);
            }
            else if (template.IsCanceled)
            {
                taskCompletionSource.SetCanceled();
            }
            else
            {
                taskCompletionSource.SetResult(template.Result);
            }
        }
    }
}
