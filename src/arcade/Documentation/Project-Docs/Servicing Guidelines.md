# Placeholder for Servicing Guidelines

From One-Pager Guidelines 

### Serviceability

- How will the components that make up this epic be tested? 
- How will we have confidence in the deployments/shipping of the components of this epic? 
- Identifying secrets (e.g. PATs, certificates, et cetera) that will be used (new ones to be created; existing ones to be used).
    - Instructions for rotating secret (if the secret is new)
- Does this change any existing SDL threat or data privacy models? (models can be found in [sharepoint](https://microsoft.sharepoint.com/teams/netfx/engineering/Shared%20Documents/Forms/AllItems.aspx?FolderCTID=0x01200053A84D1D9752264EB84A423D43EE2F05&viewid=6e9ff2b3%2D49b8%2D468b%2Db0d3%2Db1652e0bbdd3&id=%2Fteams%2Fnetfx%2Fengineering%2FShared%20Documents%2FSecurity%20Docs) folder)
- Does this require a new SDL threat or data privacy models?
- Steps for setting up repro/test/dev environments?

#### Rollout and Deployment
- How will we roll this out safely into production?
    - Are we deprecating something else?
- How often and with what means we will deploy this?
- What needs to be deployed and where?
- What are the risks when doing it?
- What are the dependencies when rolling out?

### Monitoring 
- Is there existing monitoring that will be used by this epic? 
- If new monitoring is needed, it should be defined and alerting thresholds should be set up. 

### FR Hand off
- What documentation/information needs to be provided to FR so the team as a whole is successful in maintaining these changes? 
- If you have created new monitoring rules - what tools/processes should FR use to troubleshoot alerts
- If existing monitoring is used, do the parameters need to be updated to accommodiate these new updates


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CServicing%20Guidelines.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CServicing%20Guidelines.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CServicing%20Guidelines.md)</sub>
<!-- End Generated Content-->
