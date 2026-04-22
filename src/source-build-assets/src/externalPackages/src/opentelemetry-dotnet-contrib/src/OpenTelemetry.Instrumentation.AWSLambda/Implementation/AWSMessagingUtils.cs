// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Text.Json;
using Amazon.Lambda.SNSEvents;
using Amazon.Lambda.SQSEvents;
using OpenTelemetry.Context.Propagation;

namespace OpenTelemetry.Instrumentation.AWSLambda.Implementation;

internal class AWSMessagingUtils
{
    // SNS attribute types: https://docs.aws.amazon.com/sns/latest/dg/sns-message-attributes.html
    private const string SnsAttributeTypeString = "String";
    private const string SnsAttributeTypeStringArray = "String.Array";
    private const string SnsMessageAttributes = "MessageAttributes";

    /// <summary>
    /// Gets or sets a value indicating whether the parent Activity should be set when SQS message batch is received.
    /// If option is set to true then the parent is set using the last received message otherwise the parent is not set at all.
    /// </summary>
    internal static bool SetParentFromMessageBatch { get; set; }

    internal static (PropagationContext ParentContext, IEnumerable<ActivityLink>? Links) ExtractParentContext(SQSEvent sqsEvent)
    {
        if (sqsEvent?.Records == null)
        {
            return (default, null);
        }

        // We choose the last message (record) as the carrier to set the parent.
        var parentRecord = SetParentFromMessageBatch ? sqsEvent.Records.LastOrDefault() : null;
        var parentContext = (parentRecord != null) ? ExtractParentContext(parentRecord) : default;

        var links = new List<ActivityLink>();
        foreach (var record in sqsEvent.Records)
        {
            var context = ReferenceEquals(record, parentRecord) ? parentContext : ExtractParentContext(record);
            if (context != default)
            {
                links.Add(new ActivityLink(context.ActivityContext));
            }
        }

        return (parentContext, links);
    }

    internal static PropagationContext ExtractParentContext(SQSEvent.SQSMessage sqsMessage)
    {
        if (sqsMessage?.MessageAttributes == null)
        {
            return default;
        }

        var parentContext = Propagators.DefaultTextMapPropagator.Extract(default, sqsMessage.MessageAttributes, SqsMessageAttributeGetter);
        if (parentContext == default)
        {
            // SQS subscribed to SNS topic with raw delivery disabled case, i.e. SNS record serialized into SQS body.
            // https://docs.aws.amazon.com/sns/latest/dg/sns-large-payload-raw-message-delivery.html
            var snsMessage = GetSnsMessage(sqsMessage);
            parentContext = ExtractParentContext(snsMessage);
        }

        return parentContext;
    }

    internal static PropagationContext ExtractParentContext(SNSEvent snsEvent)
    {
        // We assume there can be only a single SNS record (message) and records list is kept in the model consistency.
        // See https://aws.amazon.com/sns/faqs/#Reliability for details.
        var record = snsEvent?.Records?.LastOrDefault();
        return ExtractParentContext(record);
    }

    internal static PropagationContext ExtractParentContext(SNSEvent.SNSRecord? record)
    {
        return (record?.Sns?.MessageAttributes != null) ?
            Propagators.DefaultTextMapPropagator.Extract(default, record.Sns.MessageAttributes, SnsMessageAttributeGetter) :
            default;
    }

    internal static PropagationContext ExtractParentContext(SNSEvent.SNSMessage? message)
    {
        return (message?.MessageAttributes != null) ?
            Propagators.DefaultTextMapPropagator.Extract(default, message.MessageAttributes, SnsMessageAttributeGetter) :
            default;
    }

    private static IEnumerable<string>? SqsMessageAttributeGetter(IDictionary<string, SQSEvent.MessageAttribute> attributes, string attributeName)
    {
        return !attributes.TryGetValue(attributeName, out var attribute) ? null :
            attribute?.StringValue != null ? new[] { attribute.StringValue } :
            attribute?.StringListValues;
    }

    private static IEnumerable<string>? SnsMessageAttributeGetter(IDictionary<string, SNSEvent.MessageAttribute> attributes, string attributeName)
    {
        return !attributes.TryGetValue(attributeName, out var attribute)
            ? null
            : attribute?.Type switch
            {
                SnsAttributeTypeString when attribute.Value != null => [attribute.Value],
                SnsAttributeTypeStringArray when attribute.Value != null =>
                    attribute.Value.Split(','), // Multiple values are stored as CSV (https://docs.aws.amazon.com/sns/latest/dg/sns-message-attributes.html).
                _ => null,
            };
    }

    private static SNSEvent.SNSMessage? GetSnsMessage(SQSEvent.SQSMessage sqsMessage)
    {
        SNSEvent.SNSMessage? snsMessage = null;

        var body = sqsMessage.Body;
        if (body != null &&
#if NET
            body.TrimStart().StartsWith('{') &&
            body.Contains(SnsMessageAttributes, StringComparison.Ordinal))
#else
            body.TrimStart().StartsWith("{", StringComparison.Ordinal) &&
            body.Contains(SnsMessageAttributes))
#endif
        {
            try
            {
                snsMessage = JsonSerializer.Deserialize(body, SourceGenerationContext.Default.SNSMessage);
            }
            catch (Exception)
            {
                // TODO: log exception.
                return null;
            }
        }

        return snsMessage;
    }
}
