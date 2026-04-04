// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

// because this class is shared and public, each project that exposes it must
// ensure that it has a unique namespace, otherwise users could face type collisions
// if they import multiple projects.
#if INSTRUMENTATION_AWSLAMBDA
namespace OpenTelemetry.Instrumentation.AWSLambda;
#elif INSTRUMENTATION_AWS
namespace OpenTelemetry.Instrumentation.AWS;
#elif RESOURCES_AWS
namespace OpenTelemetry.Resources.AWS;
#endif

#pragma warning disable CA1707

/// <summary>
/// <para>
/// Collection of the Open Telemetry Semantic Conventions supported by
/// the OpenTelemetry.*.AWS libraries.  Can be used to pin the version
/// of Semantic Convention emitted.
/// </para>
/// <para>
/// While these libraries are intended for production use, they rely on several
/// Semantic Conventions that are still considered Experimental, meaning they
/// may undergo additional changes before becoming Stable.  This can impact
/// the aggregation and analysis of telemetry signals in environments with
/// multiple applications or microservices. For example, a microservice using
/// an older version of the Semantic Conventions for Http Attributes may emit
/// <c>"http.method"</c> with a value of GET, while a different microservice,
/// using a new version of Semantic Convention may instead emit the GET as
/// <c>"http.request.method"</c>.
/// </para>
/// <para>
/// Future versions the OpenTelemetry.*.AWS libraries will include updates
/// to the Semantic Convention, which may break compatibility with a previous
/// version.
/// </para>
/// <para>
/// The default will remain as <see cref="V1_28_0"/> until the next major version
/// bump.
/// </para>
/// <para>
/// To opt in to automatic upgrades, you can use <see cref="Latest"/>
/// or you can specify a specific version:
/// </para>
/// <para>
/// <code>
/// <![CDATA[
///  using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
///     .AddAWSLambdaConfigurations(opt =>
///     {
///         opt.SemanticConventionVersion = SemanticConventionVersion.V1_29_0;
///     })
///     .Build()!);
/// ]]>
/// </code>
/// </para>
/// <para>
/// For additional details, see:
/// https://opentelemetry.io/docs/specs/otel/versioning-and-stability/.
/// </para>
/// </summary>
/// <remarks>
/// Once a Semantic Convention becomes Stable, OpenTelemetry.*.AWS libraries
/// will remain on that version until the next major version bump.
/// </remarks>
public enum SemanticConventionVersion
{
    /// <summary>
    /// Use Experimental Conventions until they become stable and then
    /// pin to stable.
    /// </summary>
    Latest = 0,

    /// <summary>
    /// Pin to the specific state of all Semantic Conventions as of the 1.28.0
    /// release. See:
    /// https://github.com/open-telemetry/semantic-conventions/releases/tag/v1.28.0.
    /// <para />
    /// This version contains conventions marked Experimental and may change in future versions.
    /// </summary>
    V1_28_0 = 1,

    /// <summary>
    /// Pin to the specific state of all Semantic Conventions as of the 1.29.0
    /// release. See:
    /// https://github.com/open-telemetry/semantic-conventions/releases/tag/v1.29.0.
    /// <para />
    /// This version contains conventions marked Experimental and may change in future versions.
    /// </summary>
    V1_29_0 = 2,
}
