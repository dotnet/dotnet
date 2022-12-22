# Secret Management
We need a secret management system which allows us to audit and monitor secrets, rotate them in an automated fashion (as much as possible), and manage appropriate / inappropriate secret usage. More specific (and additional requirements) are available [below](#requirements).

## Requirements
Requirements [gist](https://gist.github.com/chcosta/51af24ab8a1cfd303a50d0aa7332e7f0)

## Stakeholders
- The First Responder team
- All of .NET Core Engineering

## Risks
- Another service that we have to manage and monitor
- Adds more policy around secret usage in our services

### Unknowns
- Can we monitor all key vault secret accesses? Azure has access logs for this that log requesting IP. Is that enough?
- There are categories of secrets which don't involve a service accessing key vault for the value (things like account passwords and otp codes), can we monitor those without introducing another layer? If not, is that ok? These should be monitored and managed in a key vault just accessed manually and/or with a tool when a person needs them.
- Can we use a third party tool for secret management? None of them look promising. https://github.com/microsoft/AuthJanitor has a disclamier in their readme about it not being ready for prime time, and it requires deploying a website that requires SDL stuff.

### Proof of Concepts
- https://github.com/microsoft/AuthJanitor exists. We very much don't want a website, but can we use some of this.
- https://www.vaultproject.io/use-cases/secrets-management looks cool but costs money.
- Azure has a sample https://github.com/Azure-Samples/serverless-keyvault-secret-rotation-handling but that is just boiler plate that we might be able to take something from.

## Serviceability
- Tests for rotation of all secrets that can be rotated
- Management system runs for PRs to validate configuration, and changes are validated in staging before deployment
- The tool will not accept customer input, so doesn't affect SDL or threat model. All authentication will be handled by azure cli, so arbitrary people can't mess with secrets they don't already have access to.

## Rollout and Deployment
- We need to deprecate the existing "secret notifier"
- This will be deployed with the existing services

## Usage Telemetry
- Usage will be tracked in application insights

## Monitoring
- Grafana alerts and build results

## FR Hand off
- Will create documentation about
    - How to use the tool
    - What to do when it fails the build
    - What to do when a secret gets leaked or expires
    - How to diagnose the tool when alerts get triggered



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CSecret%20Management%5Cone-pager.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CSecret%20Management%5Cone-pager.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CSecret%20Management%5Cone-pager.md)</sub>
<!-- End Generated Content-->
