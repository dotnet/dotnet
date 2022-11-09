# Signed dnceng.visualstudio.com builds

Dnceng.visualstudio.com does not have support for signed builds.  

Code should still be mirrored to dnceng.visualstudio.com/internal as outlined in the [Azure DevOps Guidance](https://github.com/dotnet/arcade/blob/main/Documentation/AzureDevOps/VSTSGuidance.md#projects).

## Task based build definitions

If your build definition is task based, then the build definition for signing should be created in devdiv.visualstudio.com with an "External Git" source which references the dnceng.visualstudio.com/internal git repository

1. Select a source: External Git
2. Change the Connection to "New Service Endpoint"
    - User name: dotnet-bot@microsoft.com
    - Password / Token Key: Listed in "EngKeyVault" as "dn-bot-dnceng-build-rw-code-rw"
      - You can get Read access to "EngKeyVault" by joining the "DncEngKvRead" [security group](https://idweb/identitymanagement/aspx/groups/AllGroups.aspx)
        - Note: It may take a few hours for permissions to propagate

## Yaml based build definitions

If your build definition is yaml based, then the build definition for signing should be created in devdiv.visualstudio.com, but your code should **also be mirrored into devdiv.visualstudio.com** and the DevDiv Git source should be used for building.  Yaml is only supported for source code from the same project or from specific providers (like GitHub), it is not supported for source code from an external Git source (or other project collection).

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CVSTS%5Csigned-dnceng.visualstudio.com-builds.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CVSTS%5Csigned-dnceng.visualstudio.com-builds.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CVSTS%5Csigned-dnceng.visualstudio.com-builds.md)</sub>
<!-- End Generated Content-->
