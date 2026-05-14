// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Helix.Sdk
{
    public static class LoggerExtensions
    {
        private const string EventName = "NETCORE_ENGINEERING_TELEMETRY";
        private const string CategoryKey = "Category";

        private static readonly AsyncLocal<ImmutableStack<string>> s_localCategoryStack = new AsyncLocal<ImmutableStack<string>>();

        private static ImmutableStack<string> CategoryStack
        {
            get => s_localCategoryStack.Value ?? ImmutableStack<string>.Empty;
            set => s_localCategoryStack.Value = value;
        }

        public static FailureCategoryScope EnterFailureCategoryScope(this TaskLoggingHelper log, FailureCategory category)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            CategoryStack = CategoryStack.Push(category.Value);
            UpdateCategory(log);
            return new FailureCategoryScope(log);
        }

        private static void UpdateCategory(TaskLoggingHelper log)
        {
            string currentCategory = CategoryStack.IsEmpty ? "" : CategoryStack.Peek();
            log.LogTelemetry(EventName, new Dictionary<string, string> {{CategoryKey, currentCategory}});
        }

        public static void LogError(this TaskLoggingHelper log, FailureCategory category, string message, params object[] messageArgs)
        {
            using (EnterFailureCategoryScope(log, category))
            {
                log.LogError(message, messageArgs);
            }
        }

        public static void LogErrorFromException(
            this TaskLoggingHelper log,
            FailureCategory category,
            Exception exception,
            bool showStackTrace = false,
            bool showDetail = false,
            string file = null)
        {
            using (EnterFailureCategoryScope(log, category))
            {
                log.LogErrorFromException(exception, showStackTrace, showDetail, file);
            }
        }

        public struct FailureCategoryScope : IDisposable
        {
            private TaskLoggingHelper _log;

            public FailureCategoryScope(TaskLoggingHelper log)
            {
                _log = log;
            }

            public void Dispose()
            {
                if (_log == null)
                    return;
                CategoryStack = CategoryStack.Pop();
                UpdateCategory(_log);
                _log = null;
            }
        }
    }
}
