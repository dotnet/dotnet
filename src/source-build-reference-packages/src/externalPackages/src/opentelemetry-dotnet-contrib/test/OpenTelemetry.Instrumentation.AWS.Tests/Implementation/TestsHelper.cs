// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime;
using Amazon.Runtime.Internal;
using OpenTelemetry.Instrumentation.AWS.Implementation;
using Xunit;
using SNS = Amazon.SimpleNotificationService.Model;
using SQS = Amazon.SQS.Model;

namespace OpenTelemetry.Instrumentation.AWS.Tests.Implementation;

internal static class TestsHelper
{
    /// <summary>
    /// Returns either <see cref="SqsRequestContextHelper.AddAttributes"/> or <see cref="SnsRequestContextHelper.AddAttributes"/>
    /// depending on <paramref name="serviceType"/>.
    /// <para />
    /// This is meant to mimic these logic in <see cref="AWSPropagatorPipelineHandler.AddRequestSpecificInformation"/>.
    /// </summary>
    internal static Action<IRequestContext, IReadOnlyDictionary<string, string>>? CreateAddAttributesAction(string serviceType)
    {
        return serviceType switch
        {
            AWSServiceType.SQSService => SqsRequestContextHelper.AddAttributes,
            AWSServiceType.SNSService => SnsRequestContextHelper.AddAttributes,
            _ => throw new NotSupportedException($"Tests for service type {serviceType} not supported."),
        };
    }

    internal static AmazonWebServiceRequest CreateOriginalRequest(string serviceType, int attributesCount)
    {
        AmazonWebServiceRequest resultRequest;
        var sendRequest = new SQS::SendMessageRequest()
        {
            MessageAttributes = [],
        };
        var publishRequest = new SNS::PublishRequest()
        {
            MessageAttributes = [],
        };
        Action<int> addAttribute;

        switch (serviceType)
        {
            case AWSServiceType.SQSService:
                resultRequest = sendRequest;
                addAttribute = i => sendRequest.MessageAttributes.Add($"name{i}", new SQS::MessageAttributeValue { DataType = "String", StringValue = $"value{i}" });
                break;
            case AWSServiceType.SNSService:
                resultRequest = publishRequest;
                addAttribute = i => publishRequest.MessageAttributes.Add($"name{i}", new SNS::MessageAttributeValue { DataType = "String", StringValue = $"value{i}" });
                break;
            default:
                throw new NotSupportedException($"Tests for service type {serviceType} not supported.");
        }

        for (var i = 1; i <= attributesCount; i++)
        {
            addAttribute(i);
        }

        return resultRequest;
    }

    internal static void AddAttribute(this AmazonWebServiceRequest serviceRequest, string name, string value)
    {
        var publishRequest = serviceRequest as SNS::PublishRequest;
        if (serviceRequest is SQS::SendMessageRequest sendRequest)
        {
            sendRequest.MessageAttributes.Add(name, new SQS::MessageAttributeValue { DataType = "String", StringValue = value });
        }
        else
        {
            publishRequest?.MessageAttributes.Add(name, new SNS::MessageAttributeValue { DataType = "String", StringValue = value });
        }
    }

    internal static void AddStringParameter(this ParameterCollection parameters, string serviceType, string name, string value, int index)
    {
        var prefix = $"{GetNamePrefix(serviceType)}.{index}";
        parameters.Add($"{prefix}.Name", name);
        parameters.Add($"{prefix}.Value.DataType", "String");
        parameters.Add($"{prefix}.Value.StringValue", value);
    }

    internal static void AddStringParameters(this ParameterCollection parameters, string serviceType, AmazonWebServiceRequest serviceRequest)
    {
        var index = 1;
        if (serviceRequest is SQS::SendMessageRequest sendRequest)
        {
            foreach (var a in sendRequest.MessageAttributes)
            {
                AddStringParameter(parameters, serviceType, a.Key, a.Value.StringValue, index++);
            }
        }
        else if (serviceRequest is SNS::PublishRequest publishRequest)
        {
            foreach (var a in publishRequest.MessageAttributes)
            {
                AddStringParameter(parameters, serviceType, a.Key, a.Value.StringValue, index++);
            }
        }
    }

    internal static void AssertMessageParameters(List<KeyValuePair<string, string>> expectedParameters, SQS.SendMessageRequest request)
    {
        foreach (var kvp in expectedParameters)
        {
            Assert.NotNull(request.MessageAttributes);
            Assert.True(request.MessageAttributes.ContainsKey(kvp.Key));

            Assert.Equal(kvp.Value, request.MessageAttributes[kvp.Key].StringValue);
        }
    }

    internal static void AssertMessageParameters(List<KeyValuePair<string, string>> expectedParameters, SNS.PublishRequest request)
    {
        foreach (var kvp in expectedParameters)
        {
            Assert.NotNull(request.MessageAttributes);
            Assert.True(request.MessageAttributes.ContainsKey(kvp.Key));

            Assert.Equal(kvp.Value, request.MessageAttributes[kvp.Key].StringValue);
        }
    }

    private static string GetNamePrefix(string serviceType)
    {
        return serviceType switch
        {
            AWSServiceType.SQSService => "MessageAttribute",
            AWSServiceType.SNSService => "MessageAttributes.entry",
            _ => throw new NotSupportedException($"Tests for service type {serviceType} not supported."),
        };
    }
}
