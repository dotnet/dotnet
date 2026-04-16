// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks
{
    public class UpdateJson : Task
    {
        [Required]
        public string JsonFilePath { get; set; }

        [Required]
        public string PathToAttribute { get; set; }

        [Required]
        public string NewAttributeValue { get; set; }

        public bool SkipUpdateIfMissingKey { get; set; }

        public override bool Execute()
        {
            string jsonText = File.ReadAllText(JsonFilePath);
            JsonNode jsonNode = JsonNode.Parse(jsonText);

            string[] escapedPathToAttributeParts = PathToAttribute.Replace("\\.", "\x1F").Split('.');
            for (int i = 0; i < escapedPathToAttributeParts.Length; ++i)
            {
                escapedPathToAttributeParts[i] = escapedPathToAttributeParts[i].Replace("\x1F", ".");
            }

            UpdateAttribute(jsonNode, escapedPathToAttributeParts, NewAttributeValue);

            File.WriteAllText(JsonFilePath, jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
            return true;
        }

        private void UpdateAttribute(JsonNode node, string[] path, string newValue)
        {
            string pathItem = path[0];

            if (node is not JsonObject obj || !obj.ContainsKey(pathItem))
            {
                string message = $"Path item [{nameof(PathToAttribute)}] not found in json file.";
                if (SkipUpdateIfMissingKey)
                {
                    Log.LogMessage(MessageImportance.Low, $"Skipping update: {message} {pathItem}");
                    return;
                }
                throw new ArgumentException(message, pathItem);
            }

            if (path.Length == 1)
            {
                obj[pathItem] = newValue;
                return;
            }

            UpdateAttribute(obj[pathItem], path.Skip(1).ToArray(), newValue);
        }
    }
}
