// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Abstractions
{
    /// <summary>
    /// Details of the telemetry event.
    /// </summary>
    /// <remarks>
    /// This implementation is not meant to be thread-safe. This implementation would either need to be overridden or
    /// usage should not be concurrently operated on.
    /// </remarks>
    public abstract class TelemetryEventDetails
    {
        /// <summary>
        /// The underlying properties making up the <see cref="TelemetryEventDetails"/>.
        /// </summary>
        protected internal IDictionary<string, object> PropertyValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Name of the telemetry event, should be unique between events.
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// Properties which describe the event.
        /// </summary>
        public virtual IReadOnlyDictionary<string, object> Properties
        {
            get
            {
                return (IReadOnlyDictionary<string, object>)PropertyValues;
            }
        }

        /// <summary>
        /// Sets a property on the event details.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <param name="value">Property value.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        public virtual void SetProperty(
            string key,
            string value)
        {
            SetPropertyCore(key, value);
        }

        /// <summary>
        /// Sets a property on the event details.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <param name="value">Property value.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        public virtual void SetProperty(
            string key,
            long value)
        {
            SetPropertyCore(key, value);
        }

        /// <summary>
        /// Sets a property on the event details.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <param name="value">Property value.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        public virtual void SetProperty(
            string key,
            bool value)
        {
            SetPropertyCore(key, value);
        }

        /// <summary>
        /// Sets a property on the event details.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <param name="value">Property value.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        public virtual void SetProperty(
            string key,
            DateTime value)
        {
            SetPropertyCore(key, value);
        }

        /// <summary>
        /// Sets a property on the event details.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <param name="value">Property value.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        public virtual void SetProperty(
            string key,
            double value)
        {
            SetPropertyCore(key, value);
        }

        /// <summary>
        /// Sets a property on the event details.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <param name="value">Property value.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        public virtual void SetProperty(
            string key,
            Guid value)
        {
            SetPropertyCore(key, value);
        }

        private void SetPropertyCore(
            string key,
            object value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            PropertyValues[key] = value;
        }
    }
}
