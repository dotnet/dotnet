---
name: validate-skills
description: Validate that commands documented in skill files actually work. Use when creating, updating, or reviewing skills to ensure all documented commands exit with code 0.
---

# Validating Skills

Verify every executable command in a skill runs successfully on the current OS.

## When to Use

- After creating or updating a skill that contains executable commands
- During skill review to catch stale or broken instructions
- When switching OS (e.g. Windows → Linux) to confirm cross-platform commands

## Procedure

### 1. Detect Current OS

Determine which platform commands to extract:

```powershell
# PowerShell (Windows)
$os = "Windows"
```

```bash
# Bash (Linux / macOS)
OS=$(uname -s)   # "Linux" or "Darwin"
```

### 2. Extract Commands

Parse the target skill's `SKILL.md` and list every shell command for the detected OS:

- Many skills document commands in tables with **Windows** and **Linux / macOS** columns. Pick the column matching your OS.
- If a command contains comments like `# Windows` or `# Linux / macOS`, only run the one for your OS.
- **Placeholder substitution:** Replace obvious placeholders (e.g. `<path-to-csproj>`, `<skill-name>`) with real values from the repo. If no sensible value exists, skip the command.
- **Adapt cross-platform commands:** Commands like `ls -la` should be adapted to PowerShell equivalents (`Get-ChildItem`) on Windows when no native Windows command is documented.

### 3. Track Results

Use the SQL tool to create a tracking table:

```sql
CREATE TABLE skill_commands (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  skill TEXT NOT NULL,
  command TEXT NOT NULL,
  expected_exit INTEGER DEFAULT 0,
  actual_exit INTEGER,
  status TEXT DEFAULT 'pending',
  notes TEXT
);
```

### 4. Run Each Command

For every extracted command:

1. Run it from the repo root
2. Record the exit code
3. Classify the result:
   - **Exit 0 → PASS**
   - **Non-zero + environment issue (missing SDK, no internet) → ENV_ISSUE**
   - **Non-zero + command/docs wrong → ERROR**

### 5. Safety Rules

> **CRITICAL:** Never run unfiltered integration/acceptance tests. They take hours.
> - `test.sh --integrationTest` or `test.cmd -Integration` **MUST** include a `--filter` or `-p` flag.
> - `test.sh -p smoke` is acceptable (scoped to smoke tests), but expect it to be slow.

### 6. Report

After all commands finish, print a summary:

```
=== Skill Validation Report ===
Skill: <skill-name>
OS: <Linux|Darwin|Windows>
Commands tested: N
PASS: X
ENV_ISSUE: Y (list with reasons)
ERROR: Z (list failed commands with exit codes)
```

### 7. Fix or Flag

- **ERROR (documentation bug):** Update the skill's `SKILL.md` to fix the command.
- **ENV_ISSUE:** Add a troubleshooting note to the skill if the environment prerequisite is not already documented.
- **PASS:** No action needed.

## Ordering Tips

- Run restore/build before tests (tests depend on build output)
- Run the cheapest commands first to fail fast
- Batch independent test commands in parallel when possible
