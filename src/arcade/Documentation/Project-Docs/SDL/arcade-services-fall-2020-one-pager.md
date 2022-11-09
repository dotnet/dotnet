### Stakeholders

Stakeholders are: Chris Bohm as SDL owner but also whole .NET Engineering Services team as this work relates to every part of our codebase.

### Risk

Biggest potential risks are:
- Huge surface area to cover - Arcade Services contains many different services, tools and libraries. They all have to be assessed separately. This will also require a lot of investigative work especially in less known parts of our infrastructure.
- Mission Control service - it was not deployed since a long time and we have to make it compilable and deployable again to fix reported SDL vulnerabilities
- Risk of big changes to our services for compliance reasons that will either consume a lot of time or require us to do big breaking changes to our services.
- We have 60 days to fix the found vulnerabilities so there is also a risk of running out of time. Not meeting this deadline could potentially cause some legal/compliance problems for our team.

### Serviceability

This epic will change only those parts that are found to be non-compliant. We don't expect many changes to testing, deployment and servicability of our components. In cases where it will be needed we will try to document the process inside relevant services. We will also prepare the documentation summarizing the work done and tips for future SDL rounds.

#### Rollout and Deployment

This epic does not introduce any new componentss so there won't be any major changes to rollout and deployments. The only exceptions are:
- Mission Control - it haven't been deployed since long time so we will need to get the deployment scripts to working state again and deploy it.
- Grafana - we need changes to deployment scripts to be able to deploy specific version of Grafana instead of the latest one found in package repository. This is needed so that version of Grafana in Component Governance matches the actual instance.

### Usage Telemetry

There is no telemetry to track in this epic. The only metric is that we need to close all work items in SDL assessments.

### Monitoring 

No new monitoring is needed.

### FR Hand off

There is no FR hand off required but we will prepare documentaton summarizing what has been done and put it in dotnet/core-eng wiki.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CSDL%5Carcade-services-fall-2020-one-pager.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CSDL%5Carcade-services-fall-2020-one-pager.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CSDL%5Carcade-services-fall-2020-one-pager.md)</sub>
<!-- End Generated Content-->
