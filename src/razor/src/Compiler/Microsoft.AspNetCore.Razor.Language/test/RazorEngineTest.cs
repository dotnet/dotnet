// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Moq;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language;

public class RazorEngineTest
{
    [Fact]
    public void Ctor_InitializesPhasesAndFeatures()
    {
        // Arrange
        var features = ImmutableArray.Create(
            Mock.Of<IRazorEngineFeature>(),
            Mock.Of<IRazorEngineFeature>());

        var phases = ImmutableArray.Create(
            Mock.Of<IRazorEnginePhase>(),
            Mock.Of<IRazorEnginePhase>());

        // Act
        var engine = new RazorEngine(features, phases);

        // Assert
        foreach (var feature in features)
        {
            Assert.Same(engine, feature.Engine);
        }

        foreach (var phase in phases)
        {
            Assert.Same(engine, phase.Engine);
        }
    }

    [Fact]
    public void Process_CallsAllPhases()
    {
        // Arrange
        var features = ImmutableArray.Create(
            Mock.Of<IRazorEngineFeature>(),
            Mock.Of<IRazorEngineFeature>());

        var phases = ImmutableArray.Create(
            Mock.Of<IRazorEnginePhase>(),
            Mock.Of<IRazorEnginePhase>());

        var engine = new RazorEngine(features, phases);
        var document = TestRazorCodeDocument.CreateEmpty();

        // Act
        engine.Process(document);

        // Assert
        foreach (var phase in phases)
        {
            var mock = Mock.Get(phase);
            mock.Verify(p => p.Execute(document), Times.Once());
        }
    }
}
