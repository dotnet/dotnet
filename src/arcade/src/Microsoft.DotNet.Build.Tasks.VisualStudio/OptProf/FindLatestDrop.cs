// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    /// <summary>
    /// Find the latest drop in a JSON list of VS drops.
    /// </summary>
    public sealed class FindLatestDrop : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to JSON file containing list of drops.
        /// </summary>
        [Required]
        public string DropListPath { get; set; }

        /// <summary>
        /// The name of the latest drop.
        /// </summary>
        [Output]
        public string DropName { get; private set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            try
            {
                DropName = GetLatestDropName(File.ReadAllText(DropListPath));
            }
            catch (Exception e)
            {
                Log.LogError($"Error parsing file '{DropListPath}': {e.Message}");
                return;
            }
        }

        internal static string GetLatestDropName(string json)
        {
            JObject candidate = null;
            DateTime latest = default;

            JArray drops = JArray.Parse(json);

            if (drops.Count == 0)
            {
                throw new ApplicationException("No drops matching the specified prefix were returned");
            }

            foreach (JObject item in drops)
            {
                var timestampToken = item["CreatedDateUtc"];
                if (timestampToken?.Type != JTokenType.Date)
                {
                    throw new ApplicationException($"Invalid timestamp: '{item["CreatedDateUtc"]}'");
                }

                var timestamp = timestampToken.Value<DateTime>();

                if (string.IsNullOrEmpty((string)item["Name"]))
                {
                    throw new ApplicationException($"Drop has no name: {item}");
                }

                if (candidate == null || (bool)item["UploadComplete"] && !(bool)item["DeletePending"] && timestamp > latest)
                {
                    candidate = item;
                    latest = timestamp;
                }
            }

            if (candidate == null)
            {
                throw new ApplicationException($"No complete, undeleted drops found");
            }

            return (string)candidate["Name"];
        }
    }
}
