// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Common.Test
{
    public class PreprocessorTests
    {
        [Fact]
        public void Process_ThrowsForNullStream()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => Preprocessor.Process(stream: null!, tokenReplacement: t => t));

            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Process_ThrowsForNullTokenReplacement()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => Preprocessor.Process(Stream.Null, tokenReplacement: null!));

            Assert.Equal("tokenReplacement", exception.ParamName);
        }

        [Fact]
        public void Process_ReplacesNoTokens()
        {
            var rawText = "a b c";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawText)))
            {
                var actualResult = Preprocessor.Process(stream, token => token);

                Assert.Equal(rawText, actualResult);
            }
        }

        [Fact]
        public void Process_ReplacesOneToken()
        {
            var rawText = "a $b$ c";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawText)))
            {
                var actualResult = Preprocessor.Process(stream, token => "d");

                Assert.Equal("a d c", actualResult);
            }
        }

        [Fact]
        public void Process_ReplacesTwoTokens()
        {
            var rawText = "$a$ b $c$";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawText)))
            {
                var actualResult = Preprocessor.Process(stream, token =>
                    {
                        switch (token)
                        {
                            case "a":
                                return "d";

                            case "c":
                                return "e";

                            default:
                                return "f";
                        }
                    });

                Assert.Equal("d b e", actualResult);
            }
        }

        [Fact]
        public async Task ProcessAsync_ThrowsForNullStreamTaskFactory()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => Preprocessor.ProcessAsync(
                    streamTaskFactory: null!,
                    tokenReplacement: t => t,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("streamTaskFactory", exception.ParamName);
        }

        [Fact]
        public async Task ProcessAsync_ThrowsForNullTokenReplacement()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => Preprocessor.ProcessAsync(
                    () => Task.FromResult(Stream.Null),
                    tokenReplacement: null!,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("tokenReplacement", exception.ParamName);
        }

        [Fact]
        public async Task ProcessAsync_ThrowsIfCanelled()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => Preprocessor.ProcessAsync(
                    () => Task.FromResult(Stream.Null),
                    t => t,
                    new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task ProcessAsync_ReplacesNoTokens()
        {
            var rawText = "a b c";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawText)))
            {
                var actualResult = await Preprocessor.ProcessAsync(
                    () => Task.FromResult<Stream>(stream),
                    token => token,
                    CancellationToken.None);

                Assert.Equal(rawText, actualResult);
            }
        }

        [Fact]
        public async Task ProcessAsync_ReplacesOneToken()
        {
            var rawText = "a $b$ c";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawText)))
            {
                var actualResult = await Preprocessor.ProcessAsync(
                    () => Task.FromResult<Stream>(stream),
                    token => "d",
                    CancellationToken.None);

                Assert.Equal("a d c", actualResult);
            }
        }

        [Fact]
        public async Task ProcessAsync_ReplacesTwoTokens()
        {
            var rawText = "$a$ b $c$";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawText)))
            {
                var actualResult = await Preprocessor.ProcessAsync(
                    () => Task.FromResult<Stream>(stream),
                    token =>
                        {
                            switch (token)
                            {
                                case "a":
                                    return "d";

                                case "c":
                                    return "e";

                                default:
                                    return "f";
                            }
                        },
                    CancellationToken.None);

                Assert.Equal("d b e", actualResult);
            }
        }
    }
}
