# Policy for setting up Grafana alerts

This document describes best practices for setting up monitoring alerts in Grafana.
The north star for our Grafana alerts is that:

- Alerts are actionable
- Alerts are self-explanatory and approachable
  - By approachable we mean that even non-SMEs should be able to get an idea on the next steps or at least have a point of contact to go to.
- Person handling the alert understands the rough impact and urgency and is able to communicate this to our partners
- Alert investigation and potential mitigation can happen faster as useful links/resources/documentation are available in alert's guide

## Policy

Whenever you're setting up a new alert in Grafana make sure that:

- Panels in Grafana dashboards have a description that explains what the graph/chart/table/panel shows and why we measure that particular metric.  
    Example:  
    ![image](https://user-images.githubusercontent.com/7013027/156360267-4f92d4c3-c136-4dc3-b559-71c2dc5bc3af.png)
- For every Grafana alert there is a page with alert guide tagged as `[Alert]` in the [internal wiki](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/884/Wiki-for-Grafana-Alerts) dedicated to this specific alert
  - The page is marked as `[Alert]` and contains the the category tag `!!Category-Alerts` so that is is listed in the [Alerts category page](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/925/Category-Alerts)
  - The page contains information about the alert, why it fired, what's the impact/urgency, what actions to take and more. Please see a [more exhaustive list below](#recommended-content-of-guides-to-alerts).
- The panel that the alert is associated with contains a link to the alert page in its description (the grey panel description mentioned above).  
    Example:  
    ![image](https://user-images.githubusercontent.com/7013027/184638967-e8e9cae7-3095-4711-86b2-29c56e3af1e2.png)
- The body of the alert contains a link to the appropriate wiki page
  - It's much easier to maintain the wiki page than the alert's description
  - If the alert fires repeatedly (uses one of the notification channels with periodic reminders), include this information in the alert's description too.  
    Example:  
    ![image](https://user-images.githubusercontent.com/7013027/169300812-574c0ee7-ecc9-4e25-b163-4e5b64d43cd0.png)


## Recommended content of guides to alerts

The actual content of the guide will differ for every alert but here are some useful pointers it should include.

**Affected area description**
A brief description of the service or area of service that the alert is involved in to give context to whoever deals with the alert.

**Alert explanation**  
Short description of what we are measuring, when and why we alert and what it means for our systems (urgency, impact..). We can list affected components and describe symptoms which can help answer FR questions during outages.

**Actions to take**  
Some alerts have obvious next steps - go look at this chart, check the heartbeats table, query Kusto in a certain way. Step-by-step instructions are always great.

Other actions can be communication towards our partners when we know the impact. Common actions can get their own page and be linked from several alerts.

**Usual causes/remediation**  
Some problems just come back and this is a piece of information that is hard to get thanks to the way we do FR rotation since we don't have the contiunity insight. Feel free to mention this characteristic if we keep seeing the same thing and how it gets usually resolved. Mention whether it's transient or outside of our control completely.

**Links to related resources**  
These can be repos, specific code, Azure resources, 3rd party systems and other. This should speed up investigations for obvious reasons but also simply prevent mistakes such as searching in the wrong Azure subscription, rebooting the wrong SF cluster, navigating to a wrong storage account when looking for SQL tables..

**List of SMEs**  
Do you know that this whole area was implemented by Billy? Is Billy still around? Well.. add them so that they can help if things go bananas.

**Related documentation / wiki pages**  
We have a tons of useful docs, don't hesitate to link them! This can include common tasks such as FR Operations wiki pages that apply to any part of our system. It can link to a one pager or an epic too.
