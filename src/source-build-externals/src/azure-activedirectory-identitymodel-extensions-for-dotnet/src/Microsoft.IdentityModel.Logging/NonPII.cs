//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// An internal structure that is used to mark an argument as NonPII.
    /// Arguments wrapped with a NonPII structure will be considered as NonPII in the message logging process.
    /// </summary>
    internal struct NonPII
    {
        /// <summary>
        /// Argument wrapped with a <see cref="NonPII"/> structure is considered as NonPII in the message logging process.
        /// </summary>
        public object Argument { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="NonPII"/> that wraps the <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">An argument that is considered as NonPII.</param>
        public NonPII(object argument)
        {
            Argument = argument;
        }

        /// <summary>
        /// Returns a string that represents the <see cref="Argument"/>.
        /// </summary>
        /// <returns><c>Null</c> if the <see cref="Argument"/> is <c>null</c>, otherwise calls <see cref="System.ValueType.ToString()"/> method of the <see cref="Argument"/>.</returns>
        public override string ToString()
        {
            return Argument?.ToString() ?? "Null";
        }
    }
}
