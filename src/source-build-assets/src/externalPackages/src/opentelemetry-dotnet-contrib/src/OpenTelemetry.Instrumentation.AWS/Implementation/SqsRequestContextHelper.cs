// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime;
using Amazon.SQS.Model;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

internal class SqsRequestContextHelper
{
    // SQS/SNS message attributes collection size limit according to
    // https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-message-metadata.html and
    // https://docs.aws.amazon.com/sns/latest/dg/sns-message-attributes.html
    private const int MaxMessageAttributes = 10;

    internal static void AddAttributes(IRequestContext context, IReadOnlyDictionary<string, string> attributes)
    {
        var originalRequest = context.OriginalRequest as SendMessageRequest;
        if (originalRequest == null)
        {
            return;
        }

        if (originalRequest.MessageAttributes == null)
        {
            originalRequest.MessageAttributes = new Dictionary<string, MessageAttributeValue>(attributes.Count);
        }
        else
        {
            if (attributes.Keys.Any(originalRequest.MessageAttributes.ContainsKey))
            {
                // If at least one attribute is already present in the request then we skip the injection.
                return;
            }

            var attributesCount = originalRequest.MessageAttributes.Count;
            if (attributes.Count + attributesCount > MaxMessageAttributes)
            {
                // TODO: add logging (event source).
                return;
            }
        }

        foreach (var param in attributes)
        {
            originalRequest.MessageAttributes[param.Key] = new MessageAttributeValue { DataType = "String", StringValue = param.Value };
        }
    }
}
