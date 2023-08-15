// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Common
{
    /// <summary> Represents a NuGet telemetry event data to pass to telemetry service. </summary>
    public class TelemetryEvent
    {
        private IDictionary<string, object?> _properties;
        private IDictionary<string, object?> _piiProperties;

        /// <summary> Creates a new instance of <see cref="TelemetryEvent"/>. </summary>
        /// <param name="eventName"> Event name. </param>
        public TelemetryEvent(string eventName) :
            this(eventName, new Dictionary<string, object?>())
        {
        }

        /// <summary> Creates a new instance of <see cref="TelemetryEvent"/>. </summary>
        /// <param name="eventName"> Event name. </param>
        /// <param name="properties"> Properties to add to the event. </param>
        public TelemetryEvent(string eventName, Dictionary<string, object?> properties)
        {
            Name = eventName;
            _properties = properties;
            _piiProperties = new Dictionary<string, object?>();
        }

        /// <summary> Name of the event. </summary>
        public string Name { get; }

        /// <summary> Number of properties. </summary>
        public int Count => _properties.Count;

        /// <summary> Returns a property with given <paramref name="key"/>. </summary>
        /// <param name="key"> Property key. </param>
        /// <returns> Property value. </returns>
        public object? this[string key]
        {
            get
            {
                if (key != null)
                {
                    _properties.TryGetValue(key, out var value);
                    return value;
                }
                return null;
            }
            set
            {
                _properties[key] = value;
            }
        }

        /// <summary> Complex data properties. </summary>
        public IDictionary<string, object?> ComplexData { get; } = new Dictionary<string, object?>();

        /// <summary> Gets enumerator to enumerate properties. </summary>
        /// <returns> Enumerator over recorded properties. </returns>
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary> Adds personally-identifiable information property. </summary>
        /// <param name="key"> Property key. </param>
        /// <param name="value"> Property value. </param>
        public void AddPiiData(string key, object? value)
        {
            _piiProperties[key] = value;
        }

        /// <summary> Gets personally-identifiable information properties. </summary>
        /// <returns> List of personally-identifiable information properties. </returns>
        public IEnumerable<KeyValuePair<string, object?>> GetPiiData()
        {
            return _piiProperties;
        }
    }
}
