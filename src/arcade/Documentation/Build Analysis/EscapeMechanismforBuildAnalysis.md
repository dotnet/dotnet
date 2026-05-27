## Escape Mechanism for Build Analysis

There may be scenarios where it is desirable to change the status of a failed build analysis to a passing state, particularly if there are reasons to merge the pull request and you are confident that it is necessary and safe to do so.

This can be achieved by adding a comment to the pull request with the following content:

```
/ba-g <reason>
```
This command is composed of two parts:

- /ba-g: **Required** command to change the build analysis to green.
- reason: **Required** reason for the change. This reason will be added to the build analysis check run.
Please note that providing a reason is mandatory. If you do not provide a reason, the command will be ignored.


Step by step:
1. Navigate to your pull request.
2. On the conversation tab at the bottom of the page, type the command in the comment box.
    ```
    /ba-g <reason>
    ```
3. Post the comment.
4. Wait a few minutes until the request is processed and your build analysis is updated.