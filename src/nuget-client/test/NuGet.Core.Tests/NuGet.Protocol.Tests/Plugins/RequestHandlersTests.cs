// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class RequestHandlersTests
    {
        private readonly IRequestHandler _handler;
        private readonly RequestHandlers _handlers;

        public RequestHandlersTests()
        {
            _handler = Mock.Of<IRequestHandler>();
            _handlers = new RequestHandlers();
        }

        [Fact]
        public void AddOrUpdate_ThrowsForNullAddHandlerFunc()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => _handlers.AddOrUpdate(
                    MessageMethod.Handshake,
                    addHandlerFunc: null,
                    updateHandlerFunc: oldHandler => oldHandler));

            Assert.Equal("addHandlerFunc", exception.ParamName);
        }

        [Fact]
        public void AddOrUpdate_ThrowsForNullUpdateHandlerFunc()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => _handlers.AddOrUpdate(
                    MessageMethod.Handshake,
                    () => Mock.Of<IRequestHandler>(),
                    updateHandlerFunc: null));

            Assert.Equal("updateHandlerFunc", exception.ParamName);
        }

        [Fact]
        public void AddOrUpdate_AddsIfDoesNotAlreadyExist()
        {
            var handler = Mock.Of<IRequestHandler>();

            _handlers.AddOrUpdate(MessageMethod.Handshake, () => handler, oldHandler => handler);

            IRequestHandler actualHandler;
            var wasAdded = _handlers.TryGet(MessageMethod.Handshake, out actualHandler);

            Assert.True(wasAdded);
            Assert.Same(handler, actualHandler);
        }

        [Fact]
        public void AddOrUpdate_UpdatesIfAlreadyExists()
        {
            var firstHandler = Mock.Of<IRequestHandler>();
            var secondHandler = Mock.Of<IRequestHandler>();

            _handlers.AddOrUpdate(MessageMethod.Handshake, () => firstHandler, h => firstHandler);
            _handlers.AddOrUpdate(MessageMethod.Handshake, () => secondHandler, h => secondHandler);

            IRequestHandler actualHandler;
            var wasUpdated = _handlers.TryGet(MessageMethod.Handshake, out actualHandler);

            Assert.True(wasUpdated);
            Assert.Same(secondHandler, actualHandler);
        }

        [Fact]
        public void TryAdd_ThrowsForNullHandler()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => _handlers.TryAdd(MessageMethod.Handshake, handler: null));

            Assert.Equal("handler", exception.ParamName);
        }

        [Fact]
        public void TryAdd_ReturnsTrueIfAdded()
        {
            var wasAdded = _handlers.TryAdd(MessageMethod.Handshake, _handler);

            Assert.True(wasAdded);
        }

        [Fact]
        public void TryAdd_ReturnsFalseIfNotAdded()
        {
            _handlers.TryAdd(MessageMethod.Handshake, _handler);
            var wasAdded = _handlers.TryAdd(MessageMethod.Handshake, _handler);

            Assert.False(wasAdded);
        }

        [Fact]
        public void TryGet_ReturnsTrueIfGotten()
        {
            _handlers.TryAdd(MessageMethod.Handshake, _handler);

            IRequestHandler handler;
            var wasGotten = _handlers.TryGet(MessageMethod.Handshake, out handler);

            Assert.True(wasGotten);
            Assert.Same(_handler, handler);
        }

        [Fact]
        public void TryGet_ReturnsFalseIfNotGotten()
        {
            IRequestHandler handler;
            var wasGotten = _handlers.TryGet(MessageMethod.Handshake, out handler);

            Assert.False(wasGotten);
            Assert.Null(handler);
        }

        [Fact]
        public void TryRemove_ReturnsTrueIfRemoved()
        {
            _handlers.TryAdd(MessageMethod.Handshake, _handler);

            var wasRemoved = _handlers.TryRemove(MessageMethod.Handshake);

            Assert.True(wasRemoved);
        }

        [Fact]
        public void TryRemove_ReturnsFalseIfNotRemoved()
        {
            var wasRemoved = _handlers.TryRemove(MessageMethod.Handshake);

            Assert.False(wasRemoved);
        }
    }
}
