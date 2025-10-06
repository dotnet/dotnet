namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class ActivityExtensions
    {
        private const string OperationNameTag = "OperationName";
        private static bool isInitialized;
        private static bool isAvailable;

        /// <summary>
        /// Executes action if Activity is available (DiagnosticSource DLL is available).
        /// Decorate all code that works with Activity with this method.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <returns>True if Activity is available, false otherwise.</returns>
        public static bool TryRun(Action action)
        {
            Debug.Assert(action != null, "Action must not be null");
            if (!isInitialized)
            {
                isAvailable = Initialize();
            }

            if (isAvailable)
            {
                action.Invoke();
            }

            return isAvailable;
        }

        internal static string GetOperationName(this Activity activity)
        {
            Debug.Assert(activity != null, "Activity must not be null");
            return activity.Tags.FirstOrDefault(tag => tag.Key == OperationNameTag).Value;
        }

        internal static void SetOperationName(this Activity activity, string operationName)
        {
            Debug.Assert(!string.IsNullOrEmpty(operationName), "OperationName must not be null or empty");
            Debug.Assert(activity != null, "Activity must not be null");
            activity.AddTag(OperationNameTag, operationName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool Initialize()
        {
            try
            {
#if REDFIELD
                Assembly.Load(new AssemblyName("System.Diagnostics.DiagnosticSource, Version=4.0.5.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"));
#else
                Assembly.Load(new AssemblyName("System.Diagnostics.DiagnosticSource, Version=5.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"));
#endif
                return true;
            }
            catch (System.IO.FileNotFoundException)
            {
                // This is a workaround that allows ApplicationInsights Core SDK run without DiagnosticSource.dll
                // so the ApplicationInsights.dll could be used alone to track telemetry, and will fall back to CallContext/AsyncLocal instead of Activity
                return false;
            }
            catch (System.IO.FileLoadException)
            {
                // Dll version, public token or culture is different
                return false;
            }
            finally
            {
                isInitialized = true;
            }
        }
    }
}
