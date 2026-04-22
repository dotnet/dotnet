// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.AWS;

// disable Style Warnings to improve readability of this specific file.
#pragma warning disable SA1124
#pragma warning disable SA1005
#pragma warning disable SA1514
#pragma warning disable SA1201
#pragma warning disable SA1516

internal partial class AWSSemanticConventions
{
    /// <summary>
    /// Open Telemetry Semantic Conventions as of 1.28.0:
    /// https://github.com/open-telemetry/semantic-conventions/releases/tag/v1.28.0.
    /// </summary>
    private class AWSSemanticConventions_V1_28_0 : AWSSemanticConventionsLegacy
    {
        public override string AttributeAWSDynamoTableName => "aws.dynamodb.table_names";

        // FAAS Attributes
        public override string AttributeFaasID => "cloud.resource_id";
        public override string AttributeFaasExecution => "faas.invocation_id";

        // HTTP Attributes
        [Obsolete("Replaced by <c>http.response.status_code</c>.")]
        public override string AttributeHttpStatusCode => string.Empty; // value no longer written
        [Obsolete("Replaced by <c>url.scheme</c> instead.")]
        public override string AttributeHttpScheme => string.Empty; // value no longer written

        [Obsolete("Split to <c>url.path</c> and `url.query.")]
        public override string AttributeHttpTarget => string.Empty; // value no longer written
        [Obsolete("Replaced by <c>http.request.method</c>.")]
        public override string AttributeHttpMethod => string.Empty; // value no longer written
        public override string AttributeHttpResponseStatusCode => "http.response.status_code";
        public override string AttributeHttpRequestMethod => "http.request.method";

        // NET Attributes
        [Obsolete("Replaced by <c>server.address</c>.")]
        public override string AttributeNetHostName => string.Empty; // value no longer written
        [Obsolete("Replaced by <c>server.port</c>.")]
        public override string AttributeNetHostPort => string.Empty; // value no longer written

        // SERVER Attributes
        public override string AttributeServerAddress => "server.address";
        public override string AttributeServerPort => "server.port";

        // URL Attributes
        public override string AttributeUrlPath => "url.path";
        public override string AttributeUrlQuery => "url.query";
        public override string AttributeUrlScheme => "url.scheme";
    }
}
