Microsoft.DotNet.Deployment.Tasks.Links
===============================

Contains tasks that manages aka.ms links.

## Common Required Task Parameters

- **ClientId** - Application ID authorized to update a link
- **ClientSecret** - Token for application authorized to update a link
- **Tenant** - `aka.ms` tenant.

## CreateAkaMSLinks

Creates or updates a new aka.ms link.  Links may have a short url, which is a short string that may contain a number of forward slashes.  Examples:
- `aka.ms/helloworld`
- `aka.ms/dotnet/nightly/sdk/2.1.4xx`

### Parameters
- **Owners** - Owners of the link.  Must be valid aliases, optionally semicolon deliminated.
- **CreatedBy** - Creator of the link.  Must be valid alias.
- **TargetUrl** - Target url of the link
- **ShortUrl** - Short url
- **Description (optional)** - Description of link
- **GroupOwner (optional)** - Mail enabled security group (Owners still required)
- **Overwrite (optional, defaults to true)** - If the link already exists, overwrite it with a new target.  Otherwise fail the task.

## DeleteAkaMSLinks

Removes an `aka.ms` link

### Parameters
- **ShortUrl** - Short url that should be removed
