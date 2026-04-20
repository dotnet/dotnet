namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
#if NET452
    using System;
    using System.Globalization;
    using System.Runtime.Remoting; 
    using System.Runtime.Remoting.Messaging;

    internal static class CallContextHelpers
    {
        /// <summary>
        /// Name of the operation context store item present in the context. Domain specific to avoid deserialization problems in other domain.
        /// </summary>
        private static readonly string FieldKey = string.Format(CultureInfo.InvariantCulture, "Microsoft.ApplicationInsights.Operation.OperationContextStore_{0}", AppDomain.CurrentDomain.Id); 

        /// <summary>
        /// Saves the context store to the call context.
        /// </summary>
        /// <param name="operationContext">Operation context store instance.</param>
        internal static void SaveOperationContext(OperationContextForCallContext operationContext)
        {
            CallContext.FreeNamedDataSlot(FieldKey);
            CallContext.LogicalSetData(FieldKey, new ObjectHandle(operationContext));
        }

        /// <summary>
        /// Returns the current operation context store present in the call context.
        /// </summary>
        internal static OperationContextForCallContext GetCurrentOperationContext()
        {
            var handle = CallContext.LogicalGetData(FieldKey) as ObjectHandle;
            if (handle != null)
            {
                return (OperationContextForCallContext)handle.Unwrap();
            }

            return null;
        }

        /// <summary>
        /// Clears the call context and restores the parent operation.
        /// </summary>
        /// <param name="parentContext">Parent operation context store to replace child operation context store.</param>
        internal static void RestoreOperationContext(OperationContextForCallContext parentContext)
        {
            CallContext.FreeNamedDataSlot(FieldKey);
            if (parentContext != null)
            {
                CallContext.LogicalSetData(FieldKey, new ObjectHandle(parentContext));
            }
        }
    }
#else // Use AsyncLocal<T> where available
    using System.Threading;

    internal static class CallContextHelpers
    {
        internal static AsyncLocal<OperationContextForCallContext> AsyncLocalContext = new AsyncLocal<OperationContextForCallContext>();

        /// <summary>
        /// Saves the context store to the call context.
        /// </summary>
        /// <param name="operationContext">Operation context store instance.</param>
        internal static void SaveOperationContext(OperationContextForCallContext operationContext)
        {
            AsyncLocalContext.Value = operationContext;
        }

        /// <summary>
        /// Returns the current operation context store present in the call context.
        /// </summary>
        internal static OperationContextForCallContext GetCurrentOperationContext()
        {
            return AsyncLocalContext.Value;
        }

        /// <summary>
        /// Clears the call context and restores the parent operation.
        /// </summary>
        /// <param name="parentContext">Parent operation context store to replace child operation context store.</param>
        internal static void RestoreOperationContext(OperationContextForCallContext parentContext)
        {
            AsyncLocalContext.Value = null;
            if (parentContext != null)
            {
                AsyncLocalContext.Value = parentContext;
            }
        }
    }
#endif
}
