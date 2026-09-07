// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.Build.Tasks.Feed;

/// <summary>
/// Represents the body of a request sent when creating a new feed.
/// </summary>
/// <remarks>>
/// When creating a new feed, we want to set up permissions based on the org and project.
/// Right now, only dnceng's public and internal projects are supported.
/// New feeds automatically get the feed administrators and project collection administrators as owners,
/// but we want to automatically add some additional permissions so that the publishing identity can push to them,
/// and build services and organization users can read from them.
///
/// Note that there are two ways of providing read access to the feed:
/// 1. Providing explicit access in the permissions list to the "Project Collection Valid Users"
/// 2. Updating the Local feed view to allow 'collection' users access.
///
/// The second is probably preferrable from a an AzDO pattern and usage standpoint. BUT the AzDO API has a drawback where
/// the create feed operation cannot create the local view with the appropriate access. Instead, it must be updated after the
/// feed is created. This would be fine except that updating a feed's permissions requires administrative permissions, while
/// creating a feed only requires contributor permissions. This would require passing around a PAT with management permissions,
/// instead of just r/w permissions, which is not ideal.
/// </remarks>
public class AzureDevOpsArtifactFeed
{
    public AzureDevOpsArtifactFeed(string name, string organization, string project)
    {
        Name = name;
        switch (organization)
        {
            case "dnceng":
                switch (project)
                {
                    case "public":
                    case "internal":
                        Permissions = new List<AzureDevOpsFeedPermission>
                        {
                            // internal Build Service
                            new AzureDevOpsFeedPermission("Microsoft.TeamFoundation.ServiceIdentity;116cce53-b859-4624-9a95-934af41eccef:Build:7ea9116e-9fac-403d-b258-b31fcf1bb293", "reader"),
                            // Project Collection Build Service
                            new AzureDevOpsFeedPermission("Microsoft.TeamFoundation.ServiceIdentity;116cce53-b859-4624-9a95-934af41eccef:Build:b55de4ed-4b5a-4215-a8e4-0a0a5f71e7d8", "reader"),
                            // maestro-build-promotion-mi
                            new AzureDevOpsFeedPermission("Microsoft.VisualStudio.Services.Claims.AadServicePrincipal;72f988bf-86f1-41af-91ab-2d7cd011db47\\2e3264a8-9ab9-408e-a29f-4de38d20b852", "contributor"),
                            // Project Collection value users (see class comment for info)
                            new AzureDevOpsFeedPermission("Microsoft.TeamFoundation.Identity;S-1-9-1551374245-3991166389-1514870082-2833517066-1601300440-0-0-0-0-3", "reader"),
                        };
                        break;
                    default:
                        throw new NotImplementedException($"Project '{project}' within organization '{organization}' contains no feed permissions information.");
                }
                break;
            default:
                //  Use the default permissions
                Permissions = null;
                break;

        }
    }

    public string Name { get; set; }

    public List<AzureDevOpsFeedPermission> Permissions { get; private set; }
}
