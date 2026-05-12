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
        // Using a character that isn't allowed in the package id
        private const char Delimiter = ':';

        /// <summary>
        /// Path to the json file to update.
        /// </summary>
        [Required]
        public string JsonFilePath { get; set; }

        [Required]
        public string PathToAttribute { get; set; }

        /// <summary>
        /// New attribute value. May be null. If null and
        /// ValuesToUpdate is not provided, the token is removed.
        /// </summary>
        public string NewAttributeValue { get; set; }

        /// <summary>
        /// Expects Identity and Value metadata per item. If provided, the value metadata is used to
        /// replace the json value of PathToAttribute children elements based on a case-insensitive
        /// Identity match.
        /// </summary>
        public ITaskItem[] ValuesToUpdate { get; set; }

        /// <summary>
        /// If true, if the key specified in PathToAttribute is not found, the update will be skipped.
        /// If false, an exception will be thrown if the key is not found.
        /// </summary>
        public bool SkipUpdateIfMissingKey { get; set; }

        public override bool Execute()
        {
            string json = File.ReadAllText(JsonFilePath);
            string newLineChars = FileUtilities.DetectNewLineChars(json);
            JsonNode jsonNode = JsonNode.Parse(json);

            if (!TryNavigateToObject(jsonNode, out JsonObject parent, out string lastKey))
            {
                return true;
            }

            if (ValuesToUpdate is { Length: > 0 })
            {
                if (parent[lastKey] is not JsonObject targetObj)
                {
                    throw new ArgumentException($"The node at '{PathToAttribute}' is not a JSON object.");
                }

                foreach (ITaskItem item in ValuesToUpdate)
                {
                    string matchingKey = targetObj
                        .Select(kvp => kvp.Key)
                        .FirstOrDefault(k => string.Equals(k, item.ItemSpec, StringComparison.OrdinalIgnoreCase));

                    if (matchingKey != null)
                    {
                        targetObj[matchingKey] = item.GetMetadata("Value");
                    }
                }
            }
            else if (NewAttributeValue == null)
            {
                parent.Remove(lastKey);
            }
            else
            {
                parent[lastKey] = NewAttributeValue;
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(JsonFilePath, FileUtilities.NormalizeNewLineChars(jsonNode.ToJsonString(options), newLineChars));
            return true;
        }

        /// <summary>
        /// Walks the path and returns the parent JsonObject and the final key segment.
        /// Returns false (and logs/throws) if any intermediate segment is missing.
        /// </summary>
        private bool TryNavigateToObject(JsonNode node, out JsonObject parent, out string lastKey)
        {
            string[] pathParts = PathToAttribute.Split(Delimiter);
            parent = null;
            lastKey = pathParts[^1];

            JsonNode current = node;
            foreach (string pathItem in pathParts[..^1])
            {
                if (current is not JsonObject jsonObj || !jsonObj.ContainsKey(pathItem))
                {
                    return HandleMissingKey(pathItem);
                }
                current = jsonObj[pathItem];
            }

            if (current is not JsonObject parentObj || !parentObj.ContainsKey(lastKey))
            {
                return HandleMissingKey(lastKey);
            }

            parent = parentObj;
            return true;
        }

        private bool HandleMissingKey(string pathItem)
        {
            string message = $"Path item [{nameof(PathToAttribute)}] not found in json file.";
            if (SkipUpdateIfMissingKey)
            {
                Log.LogMessage(MessageImportance.Low, $"Skipping update: {message} {pathItem}");
                return false;
            }
            throw new ArgumentException(message, pathItem);
        }
    }
}
