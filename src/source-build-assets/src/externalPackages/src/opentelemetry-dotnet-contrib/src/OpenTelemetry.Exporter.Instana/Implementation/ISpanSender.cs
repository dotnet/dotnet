// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Instana.Implementation;

internal interface ISpanSender
{
    void Enqueue(InstanaSpan instanaSpan);
}
