// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Common
{
    /// <summary>
    /// Simple token replacement system for content files.
    /// </summary>
    public static class Preprocessor
    {
        /// <summary>
        /// Asynchronously performs token replacement on a file stream.
        /// </summary>
        /// <param name="streamTaskFactory">A stream task factory.</param>
        /// <param name="tokenReplacement">A token replacement function.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="string" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="streamTaskFactory" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="tokenReplacement" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public static async Task<string> ProcessAsync(
            Func<Task<Stream>> streamTaskFactory,
            Func<string, string> tokenReplacement,
            CancellationToken cancellationToken)
        {
            if (streamTaskFactory == null)
            {
                throw new ArgumentNullException(nameof(streamTaskFactory));
            }

            if (tokenReplacement == null)
            {
                throw new ArgumentNullException(nameof(tokenReplacement));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var stream = await streamTaskFactory())
            {
                return Process(stream, tokenReplacement);
            }
        }

        /// <summary>
        /// Performs token replacement on a stream and returns the result.
        /// </summary>
        /// <param name="stream">A stream.</param>
        /// <param name="tokenReplacement">A token replacement funciton.</param>
        /// <returns>The token-replaced stream content.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="stream" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="tokenReplacement" />
        /// is <see langword="null" />.</exception>
        public static string Process(Stream stream, Func<string, string> tokenReplacement)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (tokenReplacement == null)
            {
                throw new ArgumentNullException(nameof(tokenReplacement));
            }

            string text;
            using (var streamReader = new StreamReader(stream))
            {
                text = streamReader.ReadToEnd();
            }

            var tokenizer = new Tokenizer(text);
            var result = new StringBuilder();
            for (; ; )
            {
                var token = tokenizer.Read();
                if (token == null)
                {
                    break;
                }

                if (token.Category == TokenCategory.Variable)
                {
                    var replaced = tokenReplacement(token.Value);
                    result.Append(replaced);
                }
                else
                {
                    result.Append(token.Value);
                }
            }

            return result.ToString();
        }
    }
}
