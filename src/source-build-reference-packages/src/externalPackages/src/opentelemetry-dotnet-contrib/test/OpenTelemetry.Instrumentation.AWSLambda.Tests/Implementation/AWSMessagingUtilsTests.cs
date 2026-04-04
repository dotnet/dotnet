// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Lambda.SQSEvents;
using OpenTelemetry.Instrumentation.AWSLambda.Implementation;
using OpenTelemetry.Trace;
using Xunit;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace OpenTelemetry.Instrumentation.AWSLambda.Tests.Implementation;

[Collection("TracerProviderDependent")]
public class AWSMessagingUtilsTests : IDisposable
{
    private const string TraceId = "0af7651916cd43dd8448eb211c80319c";
    private const string SpanId1 = "b9c7c989f97918e1";
    private const string SpanId2 = "b9c7c989f97918e2";

    private readonly TracerProvider? tracerProvider;

    public AWSMessagingUtilsTests()
    {
        this.tracerProvider = Sdk.CreateTracerProviderBuilder()
            .Build();
    }

    public void Dispose()
    {
        this.tracerProvider?.Dispose();
    }

    [Fact]
    public void ExtractParentContext_SetParentFromMessageBatchIsDisabled_ParentIsNotSet()
    {
        AWSMessagingUtils.SetParentFromMessageBatch = false;
        var sqsEvent = CreateSqsEventWithMessages([SpanId1, SpanId2]);

        (var parentContext, var links) = AWSMessagingUtils.ExtractParentContext(sqsEvent);

        Assert.Equal(default, parentContext);
        Assert.Equal(2, links!.Count());
    }

    [Fact]
    public void ExtractParentContext_SetParentFromMessageBatchIsEnabled_ParentIsSetFromLastMessage()
    {
        AWSMessagingUtils.SetParentFromMessageBatch = true;
        var sqsEvent = CreateSqsEventWithMessages([SpanId1, SpanId2]);

        (var parentContext, var links) = AWSMessagingUtils.ExtractParentContext(sqsEvent);

        Assert.NotEqual(default, parentContext);
        Assert.Equal(SpanId2, parentContext.ActivityContext.SpanId.ToHexString());
        Assert.Equal(2, links?.Count());
    }

    [Fact]
    public void ExtractParentContext_SetParentFromMessageBatchIsEnabled_ParentIsSetFromSnsMessage()
    {
        AWSMessagingUtils.SetParentFromMessageBatch = true;
        var sqsEvent = new SQSEvent
        {
            Records =
            [
                new SQSMessage
                {
                    MessageAttributes = [],

#pragma warning disable format // dotnet-format butchers the raw string & all following code (use dotnet format instead?)
                    Body = /*lang=json,strict*/ """
                    {
                      "Type" : "Notification",
                      "MessageId" : "f91f7f8e-77cc-51e7-ad08-231055044066",
                      "TopicArn" : "arn:aws:sqs:us-east-1:123456789012:foo-bar-test-queue",
                      "Subject" : "testsub",
                      "Message" : "{\"This JSON string\": \"is in the SNS body\"}",
                      "Timestamp" : "2023-03-29T11:27:04.056Z",
                      "SignatureVersion" : "1",
                      "Signature" : "base64string/redacted",
                      "SigningCertURL" : "https://sns.us-east-1.amazonaws.com/SimpleNotificationService-1234567abc123def1234567890123467.pem",
                      "UnsubscribeURL" : "https://sns.us-east-1.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sqs:us-east-1:123456789012:foo-bar-test-queue:123abcde-1234-1abc-1234-123456abcdef",
                      "MessageAttributes" : {
                        "traceparent" : {"Type":"String","Value":"00-0af7651916cd43dd8448eb211c80319c-b9c7c989f97918e1-00"}
                      }
                    }
                    """,
                },
            ],
        };

        (var parentContext, var links) = AWSMessagingUtils.ExtractParentContext(sqsEvent);

        Assert.NotEqual(default, parentContext);
        Assert.Equal(SpanId1, parentContext.ActivityContext.SpanId.ToHexString());
        Assert.Single(links!);
    }

    private static SQSEvent CreateSqsEventWithMessages(string[] spans)
    {
        var @event = new SQSEvent { Records = [] };
        for (var i = 0; i < spans.Length; i++)
        {
            var message = new SQSMessage { MessageAttributes = [] };
            message.MessageAttributes.Add("traceparent", new MessageAttribute { StringValue = $"00-{TraceId}-{spans[i]}-01" });
            message.MessageAttributes.Add("tracestate", new MessageAttribute { StringValue = $"k1=v1,k2=v2" });
            @event.Records.Add(message);
        }

        return @event;
    }
}
