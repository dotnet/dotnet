# Custom Version of Xunit Assert

## Origin/Attribution

This is a fork of the code in https://github.com/xunit/assert.xunit for building the
`Microsoft.DotNet.XUnitAssert` NuGet package. See `../../THIRD-PARTY-NOTICES.TXT` for the license for this code.

## Updating

This repository is a "github subtree" of the assert.xunit repo. Follow these steps to update the code:

1. Find what version you want to update to. This can be a tag or a commit on the assert.xunit repo.
2. Run the pull command. From the root of the repo run: `git subtree pull --squash --prefix src/Microsoft.DotNet.XUnitAssert/src https://github.com/xunit/assert.xunit <YOUR-PREFERRED-VERSION>`
3. Resolve merge commits.
4. Commit the result.
5. Get someone with admin permissions to **Merge** (not squash or rebase) the results. Git subtree does not like squash.

## Purpose

This copy of assert.xunit is intended to be AOT-compatible and contains breaking changes from the
original code. In general, code which relied on reflection or dynamic code generation has been
removed in this fork.
