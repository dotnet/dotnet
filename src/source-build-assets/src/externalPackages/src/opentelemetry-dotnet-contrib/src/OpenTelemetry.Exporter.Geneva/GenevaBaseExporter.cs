// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Geneva;

/// <summary>
/// GenevaExporter base class.
/// </summary>
/// <typeparam name="T">The type of object to be exported.</typeparam>
public abstract class GenevaBaseExporter<T> : BaseExporter<T>
    where T : class
{
}
