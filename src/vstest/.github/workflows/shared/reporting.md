## Report Formatting

If a formatting fragment was imported above, follow its content structure and formatting guidelines.

## Reporting Workflow Run Information

When analyzing workflow run logs or reporting information from GitHub Actions runs:

### 1. Workflow Run ID Formatting

**Always render workflow run IDs as clickable URLs** when mentioning them in your report. The workflow run data includes a `url` field that provides the full GitHub Actions run page URL.

**Format:**

`````markdown
[§12345](https://github.com/owner/repo/actions/runs/12345)
`````

**Example:**

`````markdown
Analysis based on [§456789](https://github.com/github/gh-aw/actions/runs/456789)
`````

### 2. Document References for Workflow Runs

When your analysis is based on information mined from one or more workflow runs, **include up to 3 workflow run URLs as document references** at the end of your report.

**Format:**

`````markdown
---

**References:**
- [§12345](https://github.com/owner/repo/actions/runs/12345)
- [§12346](https://github.com/owner/repo/actions/runs/12346)
- [§12347](https://github.com/owner/repo/actions/runs/12347)
`````

**Guidelines:**

- Include **maximum 3 references** to keep reports concise
- Choose the most relevant or representative runs (e.g., failed runs, high-cost runs, or runs with significant findings)
- Always use the actual URL from the workflow run data (specifically, use the `url` field from `RunData` or the `RunURL` field from `ErrorSummary`)
- If analyzing more than 3 runs, select the most important ones for references
