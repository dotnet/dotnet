// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json;

namespace OpenTelemetry.Exporter.OneCollector;

internal sealed class ExtensionFieldInformation
{
    public string? ExtensionName;
    public JsonEncodedText EncodedExtensionName;
    public string? FieldName;
    public JsonEncodedText EncodedFieldName;

    public bool IsValid => this.ExtensionName != null;
}
