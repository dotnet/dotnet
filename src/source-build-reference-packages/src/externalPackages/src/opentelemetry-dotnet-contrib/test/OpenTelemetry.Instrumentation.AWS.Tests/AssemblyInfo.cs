// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

// Avoid mutations to RuntimePipelineCustomizerRegistry.Instance causing flaky tests
[assembly: CollectionBehavior(DisableTestParallelization = true)]
