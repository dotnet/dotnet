// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.AWSLambda.Implementation;

/// <summary>
/// This wrapper class is used to add a layer of protection around calling LambdaTraceProvider.CurrentTraceId
/// from Amazon.Lambda.Core. If the provided Lambda function code does not include a version of Amazon.Lambda.Core
/// that has Amazon.Lambda.Core.LambdaTraceProvider.CurrentTraceId a <see cref="TypeLoadException"/>
/// will be thrown when parent calling method is called. This gives the parent calling method a chance to catch the
/// <see cref="TypeLoadException"/> and handle it appropriately.
/// </summary>
/// <remarks>
/// This situation where an older version of Amazon.Lambda.Core is being used can happen when the Lambda function
/// is being auto-instrumented and the version of OpenTelemetry.Instrumentation.AWSLambda
/// is not added explicitly to the Lambda function. In this scenario normal NuGet dependency resolution is not done
/// forcing a version of Amazon.Lambda.Core that will have the LambdaTraceProvider.CurrentTraceId property.
/// </remarks>
internal static class TraceProviderIsolated
{
    /// <summary>
    /// Gets the current trace id from the LambdaTraceProvider in Amazon.Lambda.Core.
    /// </summary>
    /// <exception cref="TypeLoadException">If the version of Amazon.Lambda.Core used does not contain the LambdaTraceProvider method.</exception>
    internal static string CurrentTraceId
    {
        get
        {
            return Amazon.Lambda.Core.LambdaTraceProvider.CurrentTraceId;
        }
    }
}
