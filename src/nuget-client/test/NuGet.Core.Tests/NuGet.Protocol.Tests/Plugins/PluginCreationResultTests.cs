// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginCreationResultTests
    {
        [Fact]
        public void Constructor_PluginClaims_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginCreationResult(
                    plugin: null,
                    utilities: Mock.Of<IPluginMulticlientUtilities>(),
                    claims: new List<OperationClaim>()));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void Constructor_PluginClaims_ThrowsForNullPluginMulticlientUtilities()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginCreationResult(
                    Mock.Of<IPlugin>(),
                    utilities: null,
                    claims: new List<OperationClaim>()));

            Assert.Equal("utilities", exception.ParamName);
        }

        [Fact]
        public void Constructor_PluginClaims_ThrowsForNullClaims()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginCreationResult(
                    Mock.Of<IPlugin>(),
                    Mock.Of<IPluginMulticlientUtilities>(),
                    claims: null));

            Assert.Equal("claims", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_Message_ThrowsForNullOrEmptyMessage(string message)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PluginCreationResult(message));

            Assert.Equal("message", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_Message_Exception_ThrowsForNullOrEmptyMessage(string message)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PluginCreationResult(message, exception: new Exception()));

            Assert.Equal("message", exception.ParamName);
        }

        [Fact]
        public void Constructor_Message_Exception_ThrowsForNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginCreationResult("a", exception: null));

            Assert.Equal("exception", exception.ParamName);
        }

        [Fact]
        public void Constructor_PluginClaims_InitializesProperties()
        {
            var plugin = Mock.Of<IPlugin>();
            var utilities = Mock.Of<IPluginMulticlientUtilities>();
            var claims = new List<OperationClaim>();

            var result = new PluginCreationResult(plugin, utilities, claims);

            Assert.Same(plugin, result.Plugin);
            Assert.Same(utilities, result.PluginMulticlientUtilities);
            Assert.Same(claims, result.Claims);
            Assert.Null(result.Message);
        }

        [Fact]
        public void Constructor_Message_InitializesProperties()
        {
            var result = new PluginCreationResult(message: "a");

            Assert.Null(result.Plugin);
            Assert.Null(result.PluginMulticlientUtilities);
            Assert.Null(result.Claims);
            Assert.Equal("a", result.Message);
            Assert.Null(result.Exception);
        }

        [Fact]
        public void Constructor_Message_Exception_InitializesProperties()
        {
            var exception = new Exception();
            var result = new PluginCreationResult(message: "a", exception: exception);

            Assert.Null(result.Plugin);
            Assert.Null(result.PluginMulticlientUtilities);
            Assert.Null(result.Claims);
            Assert.Equal("a", result.Message);
            Assert.Same(exception, result.Exception);
        }
    }
}
