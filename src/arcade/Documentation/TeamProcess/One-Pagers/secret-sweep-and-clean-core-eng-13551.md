# Dnceng Secret Sweep n' Clean 

## Summary

The .NET Engineering services team manages multiple resources that are
necessary for the development, servicing and release of the .NET
product. Several parts of the engineering and product infrastructure
depend on secrets that allow access to sensitive resources.

The Engineering services team has already done extensive work to enforce
the security of these secrets:

-   Any resources that can be accessed by external contributors to the
    .NET product don\'t have access to secrets (helix public queues, PR
    builds in dnceng public instance)

-   All secrets are stored in a Key Vault, and rotated on a regular
    cadence, partially without human interaction via the secret manager.

-   Secrets can be revoked at any moment in case of a suspected breach.

-   All secrets required by build and release pipelines are stored in
    variable groups, with a backing key vault.

-   Access to Key vaults is (mostly) restricted to the Engineering
    Services team.

The company-wide increased focus on Security brings the need to do some
additional work in our secret management practices to ensure that we
follow several key principles.

-   **Least privilege**: Secrets should provide only the minimum
    required level of access to sensitive resources. At the same time, a
    secret should only allow access to a very limited set of resources.

-   **Zero trust**: Secrets can only be accessed by resources/people who
    have a compelling need to access them. Not all secrets need to be
    accessed by every member of the team.

-   **Minimizing shared information**: Resources and infrastructure
    components should only access the secrets they need for their
    functioning and no others.

Keeping these principles in mind, there's a need to:

1.  Perform a point-in-time audit of our existing secrets and
    secret-adjacent resources.

2.  Create new processes and guidelines on how to perform future audits
    and cleanup.

3.  Develop improvements to our infrastructure in such a way that we can
    uphold these principles by default when possible.

## Stakeholders

-   .NET Engineering Services team (contact: \@dnceng)

-   All .NET Engineering Services partners

## Risks

**What are the unknowns?**

-   By changing the layout, scope and access level of our secrets, there
    is a risk that we will break existing repo workflows or
    infrastructure that we are not aware of. This will require providing
    guidance and alternatives to teams that are using secrets in a way
    that we didn't intend to. This is also an opportunity for
    improvement and increased visibility into these hidden workflows.

-   There's a risk that restricting the access level of certain secrets,
    some parts of our infrastructure will temporarily stop working while
    we uncover discrepancies between what we think a secret does, and
    how it's used. For example, we might uncover Personal Access Tokens
    (PATs) that carry more access than their names imply.

**Are there any POCs (proof of concepts) that need to be built for this
work?**

Probably not. We will continue using and extending the existing secret
manager tooling.

**What dependencies will this work have? Are the dependencies currently
in a state that the functionality in the work can consume them now, or
will they need to be updated?**

This work shouldn't depend on any other work. The audit and cleanup can
happen at the same time as we develop new functionalities in the secret
manager. There will be a need to update various parts of our
infrastructure to account for any Key Vault layout changes and
permission guidelines that come as part of this work.

**Will the new implementation of any existing functionality cause
breaking changes for existing consumers?**

Yes. There is a good chance that restricting the access and scope of
certain secrets will break existing tooling and workflows that we are
not directly involved in, as well as places where we not using secrets
in the way we believe they are being used. As we both start enforcing
that every variable group needs to be sourced by a key vault, and we
restrict PAT scopes further, secret names will inevitably change, which
will cause the need for changes to YAML build definitions that rely on
the changed secrets.

We will restrict variable groups to only be used by specific pipelines,
and this will most likely require reauthorization on the first run of
every pipeline that uses the variable group after the change.

**Is there a goal to have this work completed by, and what is the risk
of not hitting that date? (e.g. missed OKRs, increased pain-points for
consumers, functionality is required for the next product release, et
cetera)**

This work should be able to be completed in parallel with the product
development and the development of new features to our infrastructure,
and any impact should be brief. However, we should aim to complete this
work early in the .NET 7 development cycle to ensure that any new
infrastructure and product changes can take advantage of any new
guidelines and enhanced security that comes from this work.

## Open Questions

-   Should we touch any product specific azure resources and key vaults?

-   What kind of access to resources will we need to give to different
    ICs that are not part of the dnceng team?

-   Is attempting to manage Azure DevOps service connections in scope
    for this epic? Managing these service connections has been a pain
    point as the personal access tokens that back them expire without
    any notification.

## Components to change

The components that we have identified that will need to be audited or
updated are:

### Azure Key vaults

We want to make changes to the layout and access policies of our key
vaults, and the individual secrets contained within them. The following
subscriptions are in scope for this work:

-   Dotnet Engineering Services

-   Dnceng-InternalTooling

-   HelixProd

-   HelixStaging

**High-level activities**

1.  Split our existing vaults in such a way that we have a separation
    between vaults that hold secrets needed to be accessed by
    automation, from vaults that hold secrets that should be accessed by
    humans. If there are any secrets identified as required by both
    types of users (such as a limited set of SAS tokens for storage
    account access), we should add them to the human accessible key
    vaults. This way we can restrict access to the automation-only key
    vaults. Some examples of each kind of secret are:

    1.  **Secrets required by automation only:**

        -   Connection strings to databases

        -   Azure storage account keys and SAS tokens

        -   GitHub, Helix, Maestro and Azure DevOps Tokens belonging to bot
        users

        -   Proxy feed URLs for 2.1 servicing

        -   Aka.ms secrets

        -   Other secrets accessed by build and signing pipeline

        -   GitHub Application client secrets

    2.  **Secrets required by humans**

        -   Credentials to bot accounts (one time password seeds and
            recovery codes, usernames, passwords)

        -   SAS tokens for storage accounts that dnceng users don't have
            access to, and for partner team access to dnceng storage
            accounts (Helix, OSOB related).

2.  Split vaults that hold secrets that grant a high level of access to
    resources, from vaults that hold secrets that grant more granular
    access. Secrets like storage account keys and bot login information
    allow for the creation of other secrets. We shouldn't keep bot
    usernames and passwords in the same vaults that hold secrets
    generated from those identities, and we shouldn't hold Storage
    account keys and SAS tokens in the same vault. This grants us the
    ability to restrict user access to the more granular resources and
    tighten the access policies for secrets that grant a high level of
    permissions. Only dnceng administrators should have access to the
    broad access secrets.

3.  Make sure each one of our services has access to the least number of
    vaults possible. This is something we already attempt with service
    fabric clusters only having access to a single vault per
    environment, but we should ensure this is the case for all our
    infrastructure.

4.  Eliminate duplication of secrets where possible. There are bound to
    be scenarios where duplication is necessary, but we should opt for
    generating multiple secrets that do the same thing (such as PATs
    with duplicate scopes, SAS tokens to storage accounts), so that they
    can be rotated or revoked individually without affecting multiple
    resources or separate components of the infrastructure.

### Personal Access tokens

We rely on various PATs to perform operations on behalf of our bot users
in several parts of our infrastructure across multiple GitHub and azure
DevOps organizations. Managing these PATs has historically proven
painful, and the impact of mismanaging them is very high, as usually our
bots have very high levels of access to most of our infrastructure.

**High-level activities**

1.  Make sure that PATs only have only the minimum required set of
    scopes for the functionality they perform.

2.  Create a sustainable process to generate Azure DevOps PATs for
    multiple organizations in such a way that 1ES automation
    (<https://www.1eswiki.com/wiki/Automated_re-scoping_of_Personal_Access_Tokens>)
    will not change them underneath us.

3.  Make it easier to generate PATs for our bots, such that we can
    encourage the creation and management of multiple PATs with the same
    scopes as opposed to reusing the same PAT for multiple purposes.

4.  Ensure that every PAT used by infrastructure is performing
    operations on behalf of our bot users, and not individual team
    members.

5.  Make sure that every PAT is accounted for in a secret manager
    manifest so that it can be managed by automation.

6.  Enforce the naming convention of \<User-Organizations-Scopes> for
    existing and future PATs.

### Azure DevOps Variable Groups

The way that we pipe secrets from our key vaults to be used by build and
release pipelines is through azure devops variable groups. As their name
implies, these components can group variables in a logical collection.
These groups, and the variables contained within variables can then be
referenced in yaml pipelines and the classic editor.

As part of this effort, we will audit every variable group in the dnceng
organization, as well as every matching variable group that we own in
the devdiv azure devops instance.

**High-level activities**

1.  Ensure all variables groups that hold secrets are linked to a key
    vault. If the variable group is not owned by the engineering
    services team, work with the variable group owners to find the best
    location for a vault to store the secrets. This will mean that
    variable groups that currently mix secret and non-secret variables
    should be split into two groups.

2.  Change all variable groups so that they are not granted access to
    every pipeline in the Azure DevOps organization where they live, and
    instead individual pipelines should be granted access on a "as
    needed" basis.

3.  Audit pipelines and yaml templates to make sure they only request
    access to the minimal set of variable groups they need to work
    properly.

4.  Write tooling to enforce the new policies. We will need tools /
    scripts that:

    a.  Identify variable groups that are not linked to vaults

    b.  Identify secrets that are present in variable groups but are not
        used by any pipeline

### Azure Storage Accounts

The .NET engineering infrastructure relies on several storage Azure
storage accounts so that our services and pipelines run properly. As
part of this effort, we should make sure that broad access to storage
accounts is only granted to administrators, while we improve the tooling
needed to minimize the effects of cycling storage account keys.

For the purposes of this work, we will make a distinction between
infrastructure development and product release storage accounts.

**Product release storage accounts** refer to the storage accounts
where we upload bits that we will end up releasing to the public,
servicing releases, as well as the source-build tarballs that we share
with RedHat. Dnceng has limited access to these accounts, as they are
hosted in subscriptions outside of general access. We want to focus most
of our effort in this group of storage accounts.

The storage accounts that fall in this group are:

-   Dotnetcli

-   DotnetcliMSRC

-   DotnetCliChecksums

-   DotnetCliChecksumsMSRC

-   Dotnetbuilds

-   DotnetFeed

-   DotnetFeedMRC

**Infrastructure storage accounts** refer to storage accounts that are
needed for the functioning of services and infrastructure owned by
dnceng, for example the Helix service and autoscaler, and the
Helix-machines repository.

**High-level activities**

1.  Audit our infrastructure for places where we use storage account
    keys for access and try to replace them with SAS tokens instead.
    Update all places where it's reasonable to make such changes.

2.  Audit existing SAS tokens in our key vaults to make sure that they
    only have the least required access to the resources they grant
    access to, and to make sure the secret names match the access level
    the sas tokens grant.

3.  As specified in the Azure Key Vault section, split the vaults where
    we store storage account keys from the vaults where we store SAS
    tokens. No individuals in dnceng or the product teams should have
    access to the vault where we store the keys. In cases where these
    secrets need to be cycled, access to these vaults should be granted
    either JIT, or there should be breadcrumbs so that devs know which
    users have access to the resources.

4.  Cycle all keys for product release storage accounts once they are in
    their own vault.

5.  Create SAS tokens managed by the secret manager tooling to grant
    restricted access to individual containers in these storage
    accounts.

6.  Write guidance for access policies to the product release storage
    accounts. Create additional storage accounts for non-release
    workflows that have historically used other storage accounts without
    a compelling reason to do so.

### Secret Manager

Secret manager is the tooling we built to be able to manage secrets in
an automated fashion. Part of the work in this effort will require
additions to secret manager to achieve the level of enforcement and
modularity that we want to add to our key vaults and secrets.

**High-level activities**

1.  Improve the usability and documentation around secret manager, with
    usage examples and different examples of commands that should run
    for different scenarios. As well as detailed documentation on how to
    onboard each type of supported secret.

2.  Add the understanding of "dependent secrets" to secret manager. A
    dependent secret is a secret that should be regenerated once another
    secret is cycled or revoked. An example of this scenario is that all
    SAS tokens that belong to a storage account need to be regenerated
    once both keys to a storage account are cycled.

3.  Add the ability to automatically generate PATs through the tool
    based on metadata in the secret manifests that shows which user,
    which organizations and scopes PATs should be generated with.

4.  Add the ability to enforce naming conventions in secrets in the
    manifests. We can make secret manager fail pipelines if it detects a
    secret with an incorrect naming convention.

6.  We need to make a pass through all the existing secret manifests to
    make sure there is enough information in each one of them for secret
    types that the tool can't rotate automatically, such as aka.ms
    secrets.

## Serviceability

#### How will the components that make up this epic be tested?

For the required changes to secret manager, we will use and extend the
existing tests to make sure that any new scenarios don't break existing
functionality.

For the secret audit and restructuring of vaults, it is difficult to
test whether a change to a secret will have a negative impact on where
it's used, so we will rely on a deployment strategy that minimizes the
impact of any scope or permission changes we perform. For secrets that
are used inside our different services and infrastructure, we will
depend on the test coverage of the different services to make sure that
changes to secrets do not cause breaking changes to the services.

#### How will we have confidence in the deployments/shipping of the components of this work?

Secrets used by our services will be tested as part of the service
deployments themselves. Changes to secrets used by pipelines will be
tested by running the pipelines that use them.

## Rollout and Deployment

### How will we roll this out safely into production?

Changes to secret manager will rely on running the new functionality
against the staging versions of the manifests for the different
services. There is usually some discrepancy between the staging and
production secrets but making sure the staging services still work with
updated manifests is a good start.

### How often and with what means will we deploy this?

Changes to secret manager will be deployed weekly as part of the
arcade-services deployments. Changes to the layout of vaults and secrets
themselves will need to be deployed in stages:

**For new vaults/secrets created as part of this epic**:

1.  Create copies of the existing secrets in their new locations

2.  Inform partners that we will be working on a particular set of
    secrets, and to expect some disruption while we make the switch

3.  Change references to the secrets to pull from their new locations

4.  Ensure the secrets work when pulled from the new locations

5.  Delete the old secrets / vaults.

**For existing secrets that lose some privilege**

1.  Inform partners that changes to secrets are going to be performed,
    and to let us know if they see any of their workflows affected in a
    negative way

2.  Monitor the pipelines and services that use the secrets that got
    rescoped to make sure they are still functioning.

### What needs to be deployed and where?

Code changes to secret manager will be deployed via arcade-services,
where the tool lives.

New key vaults will be deployed to the dnceng owned subscriptions,
depending on where the services that use the vaults live.

### What are the risks when deploying?

There are multiple risks when trying to make changes to secrets:

-   Restricting access to storage accounts may break some teams'
    workflows. We should work with affected teams to provide guidance
    for how to securely access the resources they were previously used
    to accessing without restriction.

-   Changing the scopes of PATs has the risk of breaking infrastructure
    that assumed the PATs had undocumented scopes. We should regenerate
    any PATs and secrets with the proper scopes when this happens.

-   Changes to pipeline access for variable groups will need to be
    accompanied by manual authorization of every pipeline that uses the
    variable group. We should write instructions for the entire dnceng
    team, so they know how to evaluate and perform this authorization.

## FR Handoff

### What documentation/information needs to be provided to FR so the team is successful in maintaining these changes?

Part of the epic work involves better documentation and guides for the
usage of secret manager. We will also write instructions for any
potentially disruptive changes to secret access and how to deal with
them.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Csecret-sweep-and-clean-core-eng-13551.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Csecret-sweep-and-clean-core-eng-13551.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Csecret-sweep-and-clean-core-eng-13551.md)</sub>
<!-- End Generated Content-->
