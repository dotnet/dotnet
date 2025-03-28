﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using Moq;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language;

public class DefaultRazorSyntaxTreePhaseTest
{
    [Fact]
    public void OnInitialized_OrdersPassesInAscendingOrder()
    {
        // Arrange & Act
        var phase = new DefaultRazorSyntaxTreePhase();

        var first = Mock.Of<IRazorSyntaxTreePass>(p => p.Order == 15);
        var second = Mock.Of<IRazorSyntaxTreePass>(p => p.Order == 17);

        var engine = RazorProjectEngine.CreateEmpty(b =>
        {
            b.Phases.Add(phase);

            b.Features.Add(second);
            b.Features.Add(first);
        });

        // Assert
        Assert.Collection(
            phase.Passes,
            p => Assert.Same(first, p),
            p => Assert.Same(second, p));
    }

    [Fact]
    public void Execute_ThrowsForMissingDependency()
    {
        // Arrange
        var phase = new DefaultRazorSyntaxTreePhase();

        var engine = RazorProjectEngine.CreateEmpty(b => b.Phases.Add(phase));

        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(
            () => phase.Execute(codeDocument));
        Assert.Equal(
            $"The '{nameof(DefaultRazorSyntaxTreePhase)}' phase requires a '{nameof(RazorSyntaxTree)}' " +
            $"provided by the '{nameof(RazorCodeDocument)}'.",
            ex.Message);
    }

    [Fact]
    public void Execute_ExecutesPhasesInOrder()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        // We're going to set up mocks to simulate a sequence of passes. We don't care about
        // what's in the trees, we're just going to look at the identity via strict mocks.
        var originalSyntaxTree = RazorSyntaxTree.Parse(codeDocument.Source);
        var firstPassSyntaxTree = RazorSyntaxTree.Parse(codeDocument.Source);
        var secondPassSyntaxTree = RazorSyntaxTree.Parse(codeDocument.Source);
        codeDocument.SetSyntaxTree(originalSyntaxTree);

        var firstPass = new Mock<IRazorSyntaxTreePass>(MockBehavior.Strict);
        firstPass.SetupGet(m => m.Order).Returns(0);

        RazorEngine firstPassEngine = null;
        firstPass
            .SetupGet(m => m.Engine)
            .Returns(() => firstPassEngine);
        firstPass
            .Setup(m => m.Initialize(It.IsAny<RazorEngine>()))
            .Callback((RazorEngine engine) => firstPassEngine = engine);

        firstPass.Setup(m => m.Execute(codeDocument, originalSyntaxTree)).Returns(firstPassSyntaxTree);

        var secondPass = new Mock<IRazorSyntaxTreePass>(MockBehavior.Strict);
        secondPass.SetupGet(m => m.Order).Returns(1);

        RazorEngine secondPassEngine = null;
        secondPass
            .SetupGet(m => m.Engine)
            .Returns(() => secondPassEngine);
        secondPass
            .Setup(m => m.Initialize(It.IsAny<RazorEngine>()))
            .Callback((RazorEngine engine) => secondPassEngine = engine);

        secondPass.Setup(m => m.Execute(codeDocument, firstPassSyntaxTree)).Returns(secondPassSyntaxTree);

        var phase = new DefaultRazorSyntaxTreePhase();

        var engine = RazorProjectEngine.CreateEmpty(b =>
        {
            b.Phases.Add(phase);

            b.Features.Add(firstPass.Object);
            b.Features.Add(secondPass.Object);
        });

        // Act
        phase.Execute(codeDocument);

        // Assert
        Assert.Same(secondPassSyntaxTree, codeDocument.GetSyntaxTree());
    }
}
