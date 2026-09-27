# Legacy Helix runner compatibility

This directory contains a temporary compatibility shim for payloads that still
invoke the legacy Python `scriptrunner.py`. It is not a general-purpose Helix
Python client and must not be used by new work-item implementations.

On legacy machines, Python prefers the installed regular `helix` package. On
machines using the AOT Helix client, these files provide only the runner
surface needed to execute a script and stage its logs and results beneath
`HELIX_WORKITEM_UPLOAD_ROOT`. The AOT client remains responsible for storage
credentials, uploads, retries, and Helix events.

The shim can be removed when:

1. No submitted correlation payload invokes the legacy `scriptrunner.py`.
2. Supported queues no longer depend on the installed Python `helix` package.
3. Known JobSender consumers have migrated to direct work-item commands or a
   supported replacement runner.
4. Staging validation confirms that removing the packaged files does not
   regress console logs, script logs, or result artifacts.

`continuationrunner.py` is intentionally unsupported because it directly
creates secondary work items and depends on queue and storage credentials.
