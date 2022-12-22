// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A progress notification.
    /// </summary>
    public sealed class Progress
    {
        /// <summary>
        /// Gets the progress percentage.
        /// </summary>
        public double? Percentage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Progress" /> class.
        /// </summary>
        /// <param name="percentage">The progress percentage.</param>
        [JsonConstructor]
        public Progress(double? percentage = null)
        {
            if (!IsValidPercentage(percentage))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(percentage));
            }

            Percentage = percentage;
        }

        private static bool IsValidPercentage(double? percentage)
        {
            if (percentage.HasValue &&
                (double.IsNaN(percentage.Value) || double.IsInfinity(percentage.Value) ||
                percentage.Value < 0 || percentage.Value > 1))
            {
                return false;
            }

            return true;
        }
    }
}
