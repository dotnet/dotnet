// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonTestClass
    {
        public static readonly string ClassName = typeof(JsonTestClass).ToString();
        private IList<object> _listObject;// = new List<object>();
        private IList<string> _listString;// = new List<string>();

        /// <summary>
        /// When deserializing from JSON any properties that are not defined will be placed here.
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData(ReadData = true, WriteData = true)]
        [System.Text.Json.Serialization.JsonExtensionData]
        public virtual IDictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

        public bool? Boolean { get; set; }

        public double? Double { get; set; }

        public int? Int { get; set; }

        public IList<object> ListObject
        {
            get { return _listObject; }
            set { _listObject = value ?? throw LogHelper.LogArgumentNullException(nameof(value)); }
        }

        public IList<string> ListString
        {
            get { return _listString; }
            set { _listString = value ?? throw LogHelper.LogArgumentNullException(nameof(value)); }
        }

        public string String { get; set; }

        public bool ShouldSerializeAdditionalData()
        {
            return AdditionalData.Count > 0;
        }

        public bool ShouldSerializeBoolean()
        {
            return Boolean.HasValue;
        }

        public bool ShouldSerializeDouble()
        {
            return Double.HasValue;
        }
        public bool ShouldSerializeInt()
        {
            return Int.HasValue;
        }
        public bool ShouldSerializeListObject()
        {
            return ListObject != null && ListObject.Count > 0;
        }

        public bool ShouldSerializeListString()
        {
            return ListString != null && ListString.Count > 0;
        }

        public bool ShouldSerializeString()
        {
            return String != null;
        }
    }
}
