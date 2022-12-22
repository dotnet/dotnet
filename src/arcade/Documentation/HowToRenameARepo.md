# Renaming or Moving a Repository

When a repo is renamed or moved, a number of things need to be done to ensure a seamless transition. These updates do not need to be done in any particular order

- Rename internal AzDO repository. Form should be: `<github org>-<repo name>`
- Rename instance of the github repo name in the classic Maestro's subscriptions file https://github.com/dotnet/versions/blob/master/Maestro/subscriptions.json
- Update Maestro default channels to point to the new repo name
  ```
  darc get-default-channels
  <for each default channel on that repo>
  darc delete-default-channel --id <id>
  darc add-default-channel --repo <new repo name> --branch <original branch name> --channel <original channel name>
  ```
- Update Maestro subscriptions to pull from/to the new repo name.
  ```
  <update subscriptions sourcing from the repo>
  darc get-subscriptions --source-repo <original repo url>
  <for each subscription>
  darc update-subscription --id <subscription id>

  <update subscriptions targeting the repo>
  darc get-subscriptions --target-repo <original repo url>
  <foreach subscription>
  darc add-subscription
    <fill out form with new url and original parameters>
  
  darc delete-subscriptions --target-repo <original repo url>
  ```
- Add appropriate repository policies for the new repo name for any batched subscriptions
  ```
  darc get-repository-policies --repo <original repo url>
  <for each original repository policy on active branches>
  darc add-repository-policy
  ```
- Update the `repositories` variable in GatherDropOperation to point to the new repo name, if that repo is present in the list: https://github.com/dotnet/arcade-services/blob/d2ef862c8262c3945a0c2098c3307ae9e264124a/src/Microsoft.DotNet.Darc/src/Darc/Operations/GatherDropOperation.cs#L336-L380.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CHowToRenameARepo.md)](https://helix.dot.net/f/p/5?p=Documentation%5CHowToRenameARepo.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CHowToRenameARepo.md)</sub>
<!-- End Generated Content-->
