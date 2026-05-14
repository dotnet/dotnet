// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.AWS;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Instrumentation.AWSLambda.Implementation;

internal sealed class AWSLambdaResourceDetector : IResourceDetector
{
    private readonly AWSSemanticConventions semanticConventionBuilder;

    public AWSLambdaResourceDetector(AWSSemanticConventions semanticConventionBuilder)
    {
        this.semanticConventionBuilder = semanticConventionBuilder;
    }

    /// <summary>
    /// Detect the resource attributes for AWS Lambda.
    /// </summary>
    /// <returns>Detected resource.</returns>
    public Resource Detect()
    {
        var resourceAttributes =
            this.semanticConventionBuilder
                .AttributeBuilder
                .AddAttributeCloudProviderIsAWS()
                .AddAttributeCloudRegion(AWSLambdaUtils.GetAWSRegion())
                .AddAttributeFaasName(AWSLambdaUtils.GetFunctionName())
                .AddAttributeFaasVersion(AWSLambdaUtils.GetFunctionVersion())
                .AddAttributeFaasInstance(AWSLambdaUtils.GetFunctionInstance())
                .AddAttributeFaasMaxMemory(AWSLambdaUtils.GetFunctionMemorySize())
                .Build();

        return new Resource(resourceAttributes);
    }
}
