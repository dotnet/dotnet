// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.UnifiedBuild.Tasks
{
    // Takes a path to a path to a json file and a
    // string that represents a dotted path to an attribute
    // and updates that attribute with the new value provided. 
    public class UpdateJson : Task
    {
        [Required]
        public string JsonFilePath { get; set; }

        [Required]
        public string PathToAttribute { get; set; }

        // New attribute value. May be null. If null,
        // the token is removed.
        public string NewAttributeValue { get; set; }

        public bool SkipUpdateIfMissingKey { get; set; }

        public override bool Execute()
        {
            // Using a character that isn't allowed in the package id
            const char Delimiter = ':';

            string json = File.ReadAllText(JsonFilePath);
            string newLineChars = FileUtilities.DetectNewLineChars(json);
            JsonNode jsonNode = JsonNode.Parse(json);

            string[] escapedPathToAttributeParts = PathToAttribute.Split(Delimiter);
            for (int i = 0; i < escapedPathToAttributeParts.Length; ++i)
            {
                escapedPathToAttributeParts[i] = escapedPathToAttributeParts[i];
            }
            UpdateAttribute(jsonNode, escapedPathToAttributeParts, NewAttributeValue);

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(JsonFilePath, FileUtilities.NormalizeNewLineChars(jsonNode.ToJsonString(options), newLineChars));
            return true;
        }

        private void UpdateAttribute(JsonNode jsonNode, string[] path, string newValue)
        {
            string pathItem = path[0];
            JsonNode current = jsonNode as JsonObject;
            
            if (current is JsonObject jsonObject && !jsonObject.ContainsKey(pathItem))
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
                if (newValue == null)
                {
                    if (current is JsonObject obj && obj.ContainsKey(pathItem))
                    {
                        obj.Remove(pathItem);
                    }
                }
                else
                {
                    if (current is JsonObject obj)
                    {
                        obj[pathItem] = JsonValue.Create(newValue);
                    }
                }
                return;
            }

            UpdateAttribute(((JsonObject)current)[pathItem], path.Skip(1).ToArray(), newValue);
        }
    }
}
