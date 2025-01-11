// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Deployment.Tasks.Links
{
    /// <summary>
    /// Creates or updates, in bulk, a set of aka.ms (redirection) links
    /// </summary>
    public class CreateAkaMSLinks : AkaMSLinksBase
    {
        /// <summary>
        /// Set of links to create
        /// ItemSpec: ShortUrl
        /// Metadata TargetUrl: Target url that the aka.ms/ItemSpec should point to.
        /// Metadata Description (optional): Optional description of the aka.ms link
        /// </summary>
        [Required]
        public ITaskItem[] Links { get; set; }

        /// <summary>
        /// Set of owners of the link. Semicolon delimited if multiple owners
        /// are provided.
        /// </summary>
        [Required]
        public string Owners { get; set; }
        /// <summary>
        /// Creator of the link
        /// </summary>
        [Required]
        public string CreatedBy { get; set; }
        /// <summary>
        /// Group
        /// </summary>
        public string GroupOwner { get; set; }
        /// <summary>
        /// If the links already exists, update it if necessary. If false, a link already exists and points to a different target,
        /// the update will fail.
        /// </summary>
        public bool Overwrite { get; set; } = true;

        public override bool Execute()
        {
            return ExecuteAsync().GetAwaiter().GetResult();
        }

        public async Task<bool> ExecuteAsync()
        {
            // Parse out the Links items.
            List<AkaMSLink> linksToCreate = new List<AkaMSLink>();

            foreach (var link in Links)
            {
                string shortUrl = link.ItemSpec;
                string targetUrl = link.GetMetadata(nameof(AkaMSLink.TargetUrl));
                string description = link.GetMetadata(nameof(AkaMSLink.Description));

                if (string.IsNullOrEmpty(shortUrl))
                {
                    Log.LogError($"Short url (ItemSpec) should not be empty");
                }

                if (string.IsNullOrEmpty(targetUrl))
                {
                    Log.LogError($"TargetUrl (metadata) should not be empty");
                }

                if (Log.HasLoggedErrors)
                {
                    return false;
                }

                linksToCreate.Add(new AkaMSLink(shortUrl, targetUrl, description));
            }

            try
            {
                foreach (var link in linksToCreate)
                {
                    string descriptionString = !string.IsNullOrEmpty(link.Description) ? $" ({link.Description})" : "";
                    Log.LogMessage(MessageImportance.High, $"Creating link aka.ms/{link.ShortUrl} -> {link.TargetUrl}{descriptionString}");
                }
                AkaMSLinkManager manager = CreateAkaMSLinksManager();

                await manager.CreateOrUpdateLinksAsync(linksToCreate, Owners, CreatedBy, GroupOwner, Overwrite);
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
