---
name: breaking-change
description: 'Drive the .NET SDK breaking-change documentation-and-notification process for a NuGet change that has already been implemented. Invoke this once a NuGet/Home issue (or its implementing NuGet.Client PR) is labeled `Breaking-Change`. It makes sure the correct documentation issues are filed/completed (the NuGet NUxxxx reference-docs issue and the official dotnet/docs breaking-change issue) and helps author the draft notification email to the .NET Breaking Change Notifications alias. Trigger on phrases like "breaking change process", "file the breaking change doc issue", "the issue got the Breaking-Change label", "draft the breaking change email", or "notify dotnetbcn".'
---

# NuGet Breaking Change — Docs & Notification Skill

Invoke this skill **after** a NuGet breaking change has been designed/implemented, once its
[NuGet/Home](https://github.com/NuGet/Home/issues) issue (or the implementing NuGet.Client PR) is labeled
`Breaking-Change`. This skill does **not** write product code. Its job is the paperwork the
[dotnet SDK breaking-change guidelines](https://github.com/dotnet/sdk/blob/main/documentation/project-docs/breaking-change-guidelines.md)
require:

1. Make sure the **correct documentation issues** are filed and filled out.
2. Help author the **draft notification email** to the .NET Breaking Change Notifications alias.

> Worked reference: Home #14949 "Pack warning when Package ID doesn't meet the standards" (NU5052), design
> Home PR #14950, implemented in NuGet.Client PR #7487, reference-docs issue Home #14953, announcement
> NuGet/Announcements #75. Mirror these for a new change.

---

## The two documentation issues

A NuGet SDK breaking change needs documentation in **two** places. Confirm both exist and are complete:

### 1. NuGet reference-docs (tracking issue in NuGet/Home + page in NuGet/docs.microsoft.com-nuget)
The user-facing docs page for the diagnostic. Two moving parts, both of which must be **done** — do not rely on
the tracking issue alone:

- **Tracking issue** in **NuGet/Home**, `Type:Docs` + the relevant `Functionality:*` (e.g. `Functionality:Pack`).
- **The actual docs page** lives in the source repo **`NuGet/docs.microsoft.com-nuget`** at
  `docs/reference/errors-and-warnings/NU<XXXX>.md` (plus an index row in `docs/reference/Errors-and-Warnings.md`),
  and publishes to `https://learn.microsoft.com/en-us/nuget/reference/errors-and-warnings/nu<xxxx>`.

**"Done" requires the docs PR merged AND the learn.microsoft.com page live** — the Home issue being open or even
closed is only a signal, not proof. Verify all of the following (search all states, including closed/merged):

```
gh search issues --repo NuGet/Home "NU<XXXX>" --label "Type:Docs" --state all
gh search prs    --repo NuGet/docs.microsoft.com-nuget "NU<XXXX>" --state all
```
- Also fetch the live page (a **404 means not published / not done**):
  `https://learn.microsoft.com/en-us/nuget/reference/errors-and-warnings/nu<xxxx>`.
- Also check the implementing NuGet.Client PR body's "update docs" checklist line — it usually links the docs issue.

If the docs PR is still open (or none exists), the NuGet docs part is **not done**: report the open docs PR /
missing page. Only file a new NuGet/Home `Type:Docs` issue if none exists. Confirm the page covers **what triggers
the diagnostic** and how to fix it. (For a **warning**, the opt-out is implied — `NoWarn`/`SdkAnalysisLevel`
suppression is standard, so the page need not document it; call out an opt-out explicitly only for an error or a
behavioral change with a non-obvious switch, and a removal/cut-over timeline only if one exists — see note below.)
Example: Home #14953 "Document NU5052" (tracking) + docs page PR
https://github.com/NuGet/docs.microsoft.com-nuget/pull/3595.

### 2. Official .NET breaking-change issue (dotnet/docs)
The change must also be recorded in the central .NET breaking-changes list. File an issue in
[dotnet/docs](https://github.com/dotnet/docs) using its **".NET breaking change"** template
(`.github/ISSUE_TEMPLATE/02-breaking-change.yml`). Fetch the live template before drafting — its dropdown
values (Version, Feature area) change each release:

```
gh api repos/dotnet/docs/contents/.github/ISSUE_TEMPLATE/02-breaking-change.yml -H "Accept: application/vnd.github.raw"
```

Before creating it, **search dotnet/docs for an existing `breaking-change` issue** referencing this change/PR and
stop if one exists (don't duplicate).

---

## Step 1 — Gather context

Read, from the labeled item:
- The **NuGet/Home issue**: body, labels, linked design (Home PR), and the announcement issue if any.
- The **implementing NuGet.Client PR(s)**: title, body, diff, the diagnostic id/message, and the gating
  (`SdkAnalysisLevelMinimums.Vxx_x_xxx`, whether it's SDK-style-only via `UsingMicrosoftNETSdk`).
- The **opt-out mechanism** and the **release** it ships in.

## Step 2 — Determine the shipping .NET SDK version

The dotnet/docs template requires the exact version (e.g. ".NET 11 Preview 3" / GA). Derive it from the
`SdkAnalysisLevel` gate and the NuGet↔SDK release mapping (NuGet dev branch → SDK it inserts into; e.g.
`V11_0_100` ⇒ the .NET 11 SDK). Confirm the milestone from the release branch the fix shipped in. If uncertain,
use "Other (please put exact version in description textbox)" and state the version in the description.

## Step 3 — Author the dotnet/docs breaking-change issue

Produce clean markdown matching the template sections (do **not** output the YAML):

- **Title**: `[Breaking change]: <concise user-facing summary>` (not just the PR title).
- **Description**: brief summary + link to the Home issue and implementing PR.
- **Version**: from Step 2.
- **Previous behavior**: what pack/restore/CLI did before (with a small example if useful).
- **New behavior**: the new diagnostic/behavior, its severity, and the exact message/id (e.g. `NU5052`).
- **Type of breaking change**: usually **Behavioral change** for a new warning/error; mark source/binary
  incompatible only if it actually is.
- **Reason for change**: motivation from the Home issue/announcement.
- **Recommended action**: how to fix (e.g. rename the package ID). For a warning the opt-out is implied
  (standard `NoWarn`/`SdkAnalysisLevel` suppression) — mention it briefly at most; document an opt-out explicitly
  only for errors or non-obvious switches. Include a removal/escalate-to-error timeline **only if one exists** —
  many NuGet warnings have no planned removal and simply stay (per the dotnet guidelines, that time may be
  *never*). Do not invent a "TBD".
- **Feature area**: `SDK` (NuGet ships in the SDK).
- **Affected APIs**: usually none for a build-time diagnostic — state "N/A (build-time NuGet diagnostic)".

Present the draft for review before creating the issue. Default to **draft mode** unless the user explicitly
says to file it. When filing, prefer a pre-filled `https://github.com/dotnet/docs/issues/new?...` link (title +
`breaking-change` label from the template) so it's one click for the user.

## Step 4 — Draft the notification email

The dotnet/docs template ends with a note to email a link to the breaking-change issue to the .NET Breaking
Change Notifications alias. **For NuGet, send to `dotnetsdkbcn@service.microsoft.com`** (not the generic
`dotnetbcn@microsoft.com` shown in the template). After the dotnet/docs issue exists (or as a draft alongside
it), author that email.

**Format the email as HTML** (`body.contentType = "html"`), not a plain-text blob — it renders in Outlook and is
what the user actually sends. Use short **paragraphs** (not one long block), **bold** the labels ("What
changed", "Who's affected", "Opt-out"), put identifiers/switches in `<code>` (e.g. `dotnet pack`, `NU5052`,
`<NoWarn>NU5052</NoWarn>`, `SdkAnalysisLevel 11.0.100`), and add a little color for scannability — e.g. the
diagnostic id in an accent color and code snippets on a light-tinted background. Keep it professional and
restrained (one or two accent colors, inline styles only; assume no external CSS). A `References` list at the
bottom links the doc issue, tracking issue, and announcement. Structure to fill:

- **What changed** — one line incl. diagnostic id (e.g. NU5052) and severity.
- **Who's affected** — e.g. package authors whose package IDs use characters outside the restricted set, packing on the .NET 11 SDK.
- **Opt-out** — e.g. `<NoWarn>NU5052</NoWarn>`, or set `SdkAnalysisLevel` below the gate (e.g. 11.0.100).
- **References** — Breaking change doc issue (dotnet/docs URL), Tracking issue (NuGet/Home URL), Announcement (NuGet/Announcements URL, if any).

Subject: `[Breaking change] NuGet: <concise summary> (.NET <version>)`.

Keep it short and factual. Fill every `<...>` from Steps 1-3; do not leave placeholders in the final draft.
Present it for the user to review and send — this skill drafts the email, the user sends it.

> Tooling note: when drafting the mail via the mailbox API (Graph `POST /me/messages`), the `create` path is
> reliable; the returned message `id` comes back base64url-mangled, so in this environment `update`/`delete` by
> that id may fail. If you need to revise a draft, prefer **re-creating** it with the corrected HTML rather than
> patching, and tell the user which stale draft to delete.

---

## Checklist to close out

- [ ] `Breaking-Change` label on the Home issue **and** on the implementing PR (the PR label triggers the bot's
      instructions). Add it if missing.
- [ ] NuGet reference-docs **done**: NuGet/docs.microsoft.com-nuget PR merged **and** the
      `learn.microsoft.com/.../errors-and-warnings/nu<xxxx>` page is live (404 = not done); Home `Type:Docs`
      tracking issue resolved. Covers what triggers it and how to fix it (opt-out is implied for warnings).
- [ ] dotnet/docs breaking-change issue filed (no duplicate) using the current template.
- [ ] Notification email to `dotnetsdkbcn@service.microsoft.com` drafted (and sent by the user) linking the docs issue.

## Conventions

- NuGet product issues live in **NuGet/Home**; official breaking-change docs go in **dotnet/docs**.
- This skill is documentation + notification only — for the code/gating mechanics see `docs/feature-guide.md`
  (SdkAnalysisLevel) and the reference PR #7487.
- Default to **draft mode**: show issue bodies and the email for review; only create/file on explicit request.
- Use the `create-pr` skill for any PR work; do not re-implement `gh` PR mechanics here.
