---
name: "Build Failure Analysis (command)"
description: >-
  Rerun the build-failure analysis on a pull request when a maintainer comments
  `/analyze-build-failure`. Same body as `build-failure-analysis.md` — it does
  NOT rebuild: it inspects the PR's **latest** Azure Pipelines `arcade-pr`
  build and, **only when that latest build has failed** (it stops if the
  newest build is still running or has succeeded), downloads the binary logs
  that build already produced (all build legs) and delegates to the
  `build-failure-analyst` agent (which queries the binlogs live via the
  containerized `binlog-mcp` MCP server). Useful when a previous run was
  cancelled, the analysis comment was dismissed, or the agent needs another
  pass. Like the auto workflow it performs **no build**; the generated jobs do
  check out the repository (and, for the slash-command event, the PR branch)
  for agent tooling only — the PR's code is never built or executed.

on:
  slash_command:
    name: analyze-build-failure
    events: [pull_request_comment]
  roles: [admin, maintainer, write]
  reaction: "eyes"
  # Gate the AI pipeline on the fetch job so the agent only runs when a binlog
  # was actually retrieved from a failed Azure DevOps build.
  needs: [fetch-binlog]

# Skip activation (and the agent) unless a binlog was retrieved — e.g. if the
# PR's latest Azure DevOps build did not fail, or the PR is out of scope.
if: needs.fetch-binlog.outputs.binlog-found == 'true'

# Least-privilege for the workflow/agent jobs. The agent runs read-only; it
# does NOT post directly. All PR writes it produces (summary comment + inline
# review suggestions) go through gh-aw **safe-outputs**, which the compiler
# emits as a separate `safe_outputs` job granted `pull-requests: write` +
# `issues: write` in the generated lock. (The slash-command trigger also adds
# an acknowledgement reaction to the command comment; gh-aw emits that in its
# own generated job with the scope it needs — it is not driven by this agent
# job.) Keep `pull-requests: read` here so the AI agent job stays
# least-privilege — do NOT raise it to `write`, that would hand PR-write scope
# to the agent job unnecessarily.
#
# Do NOT add `copilot-requests: write` here. That permission switches gh-aw's
# generated lock from `COPILOT_GITHUB_TOKEN: ${{ secrets.COPILOT_GITHUB_TOKEN }}`
# to `${{ github.token }}`, and the ephemeral Actions token is not entitled for
# inference against api.githubcopilot.com in this org — every agent run then
# dies in ~2s with "Authentication failed with provider ... (HTTP 403)" on both
# /models and /chat/completions, before it reads the prompt or opens a binlog.
# `update-default-versions.md` omits it and works; keep this consistent.
permissions:
  contents: read
  pull-requests: read

concurrency:
  # Distinct from the automatic workflow's group (`build-failure-analysis-<pr>`).
  # Concurrency groups are repository-global, so sharing the name made the two
  # workflows cancel each other for the same PR: a newly failing build would
  # kill an on-demand analysis a maintainer had just asked for. Each still
  # collapses its own repeat invocations for a PR.
  group: build-failure-analysis-cmd-${{ github.event.issue.number || github.event.pull_request.number || fromJSON(github.event.inputs.aw_context || github.event.client_payload.aw_context || '{}').item_number || github.run_id }}
  cancel-in-progress: true

timeout-minutes: 30

network:
  allowed:
    - defaults
    - dotnet

imports:
  - shared/build-failure-analysis-shared.md

# Live binlog access for the agent — see build-failure-analysis.md for the
# rationale. The fetch-binlog job downloads each build leg's binlog from Azure
# DevOps into a directory and uploads it; the agent job downloads it to
# `/tmp/binlogs` and the gh-aw MCP gateway mounts it read-only at
# `/data/binlogs`.
#
# NOT pinned by digest, and that is a gh-aw v0.77.5 limitation, not a choice.
# This container is handed the binlogs of an unmerged, possibly external PR and
# its output is what the agent reports back, so "whatever this tag points at
# today" is a supply-chain decision made by whoever last pushed the tag — and
# the tag does move: it resolved to sha256:9f1e2c3e8281... from 2026-07-16
# until 2026-08-03, when it became
# sha256:ee7b7e5c6e162f3f0061822aa7183260626f1a1e986d04ba9915ab197a37932c.
# v0.77.5 validates `container` against `^[a-zA-Z0-9][a-zA-Z0-9/:_.-]*$`, which
# has no `@`, so `image@sha256:...` is rejected at compile time and the
# generated `download_docker_images.sh` pulls this image by bare tag while every
# other image in the lock is digest-pinned. gh-aw >= v0.83.x resolves and pins
# the digest automatically (verified: microsoft/testfx on v0.83.4 emits
# `digest` + `pinned_image` in its `gh-aw-manifest` and pulls by `@sha256:`), so
# this is fixed by bumping the compiler this repo pins rather than by editing
# this line.
# Refresh/inspect the current digest with:
#   docker buildx imagetools inspect \
#     mcr.microsoft.com/dotnet-buildtools/prereqs:azurelinux-3.0-binlog-mcp-amd64
mcp-servers:
  binlog-mcp:
    container: "mcr.microsoft.com/dotnet-buildtools/prereqs:azurelinux-3.0-binlog-mcp-amd64"
    mounts:
      - "/tmp/binlogs:/data/binlogs:ro"
    allowed: ["*"]

# Custom job that reuses the binlogs from the PR's most recent failed Azure
# DevOps `arcade-pr` build instead of rebuilding. Mirrors the fetch-binlog job
# in build-failure-analysis.md; it locates the build by the PR's merge branch
# (no `check_run` payload is available on a slash command).
jobs:
  fetch-binlog:
    name: Fetch binlogs (Azure Pipelines)
    # Cheap pre-gate. This job is a dependency of gh-aw's `pre_activation`, so it
    # runs BEFORE the role / command-position check. Without a guard it would
    # download hundreds of MB of binlogs on *every* comment in the repository,
    # which any public commenter could trigger repeatedly. This expression is
    # only the free first filter — `author_association` is coarse (in an
    # org-owned repo every org member reports MEMBER regardless of the
    # permission they actually hold here), so the step below resolves the
    # commenter's real repository permission before anything is downloaded.
    # `pre_activation` remains the authoritative role + command-position check,
    # and `activation` additionally requires `binlog-found == 'true'`.
    #
    # KEEP IN SYNC with `roles:` in the frontmatter above. The author_association
    # list here and the permission step below are hand-written restatements of
    # that policy; editing `roles:` does NOT update them, because only
    # `pre_activation` is generated from the frontmatter.
    #
    # `github.event.issue.pull_request` is what keeps plain issue comments out:
    # gh-aw emits no such filter of its own despite `events: [pull_request_comment]`
    # (checked in the generated lock), so PR-only scoping is a property of this
    # hand-written expression rather than something the compiler enforces. It
    # degrades safely without it — `repos/.../pulls/<issue#>` 404s and the script
    # emits no binlog — but it would pay for a runner first.
    #
    # `contains(..., '/analyze-build-failure')` is a substring match anywhere in
    # the body, whereas the authoritative `check_command_position` requires the
    # command to be in a valid position. So a write-access user merely mentioning
    # the command, or editing an old comment that quotes it (`types:` includes
    # `edited`), still starts this job. Workflow `if:` expressions have no
    # regex, and `startsWith` would reject the leading whitespace/newlines gh-aw
    # accepts, so this stays a deliberate over-approximation — but it is now
    # only a cheap pre-filter: the first step of the job reproduces gh-aw's real
    # first-token check and bails out before anything is downloaded.
    if: >-
      github.event.repository.fork == false &&
      github.event.issue.pull_request &&
      contains(fromJSON('["OWNER","MEMBER","COLLABORATOR"]'), github.event.comment.author_association) &&
      contains(github.event.comment.body, '/analyze-build-failure')
    runs-on: ubuntu-latest
    timeout-minutes: 15
    permissions:
      contents: read
      pull-requests: read
    outputs:
      binlog-found: ${{ steps.fetch.outputs.binlog-found }}
      pr-number: ${{ steps.fetch.outputs.pr-number }}
      pr-head-sha: ${{ steps.fetch.outputs.pr-head-sha }}
      pr-merge-sha: ${{ steps.fetch.outputs.pr-merge-sha }}
      ado-build-id: ${{ steps.fetch.outputs.ado-build-id }}
      ado-build-url: ${{ steps.fetch.outputs.ado-build-url }}
    steps:
      # `author_association` in the job-level `if:` cannot tell an org member
      # with read-only access apart from a maintainer, so resolve the real
      # repository permission here — before any download — and match it against
      # the same `roles: [admin, maintainer, write]` this command declares.
      # KEEP IN SYNC with that list.
      #
      # `.permission` is the field to test. The REST docs for this endpoint say
      # it returns the legacy base roles admin|write|read|none, "where the
      # maintain role is mapped to write and the triage role is mapped to read",
      # so `admin|write` is exactly "has push access or better" — precisely the
      # set `roles: [admin, maintainer, write]` describes, with maintainers
      # included.
      #
      # `.role_name` is deliberately NOT consulted. It reports "the name of the
      # assigned role, including custom roles", and a custom organization role
      # only has to avoid the base names read/triage/write/maintain/admin — so
      # matching on it would let a role merely *named* like a privileged one
      # (e.g. a custom `maintainer` inheriting read) pass this gate with no push
      # access at all.
      #
      # On any API failure the response carries no `.permission`, so `perm` ends
      # up empty and the check falls into the deny branch; failing closed is the
      # safe direction for a pre-gate.
      - name: Verify the comment invokes the command and the commenter has write access
        id: perm
        if: github.event_name == 'issue_comment'
        shell: bash
        env:
          GH_TOKEN: ${{ github.token }}
          COMMENTER: ${{ github.event.comment.user.login }}
          COMMENT_BODY: ${{ github.event.comment.body }}
          COMMAND_NAME: "analyze-build-failure"
        run: |
          set +e
          # --- 1. Command position (free; do this before the API call) ------
          # The job-level `if:` can only use `contains()`, a plain substring
          # test, so a comment that merely mentions the command — or an edited
          # old comment quoting it — still reaches this job and pays for the
          # download before `pre_activation` throws the result away. That check
          # runs too late by construction, so reproduce it here.
          #
          # gh-aw trims the body and requires the command to be the FIRST token:
          # `/^\/([a-zA-Z0-9][a-zA-Z0-9._-]*)(?=$|\s)/` over the trimmed text,
          # then an equality comparison on the captured name
          # (actions/setup/js/slash_command_matcher.cjs). `awk 'NF {print $1;
          # exit}'` is the same rule: skip leading whitespace/blank lines, take
          # the first whitespace-delimited token. The token is delimited by
          # whitespace or end-of-input, exactly the `(?=$|\s)` lookahead, so
          # `/analyze-build-failure-now` correctly does NOT match. `tr -d '\r'`
          # is needed because JS `.trim()` and `\s` treat CR as whitespace while
          # awk's default field splitting does not.
          # KEEP IN SYNC with `on.command.name` below.
          first_word=$(printf '%s' "${COMMENT_BODY}" | tr -d '\r' | awk 'NF {print $1; exit}')
          if [ "${first_word}" != "/${COMMAND_NAME}" ]; then
            # Never echo the raw token: it is attacker-controlled and `::`-
            # prefixed text is interpreted by the runner as a workflow command.
            safe_word=$(printf '%s' "${first_word}" | tr -cd 'A-Za-z0-9/._-' | cut -c1-40)
            echo "Comment does not start with '/${COMMAND_NAME}' (first token: '${safe_word}'); skipping the binlog download."
            echo "authorized=false" >> "$GITHUB_OUTPUT"
            exit 0
          fi
          # --- 2. Repository permission -------------------------------------
          # `COMMENTER` is interpolated into an API path and into log output, so
          # give it the same shape check `PR_NUMBER` and `BUILD_ID` get below.
          # GitHub logins are alphanumerics and hyphens; anything else (a bot
          # login such as `github-actions[bot]`, or an empty value) is rejected
          # here instead of being sent to the API.
          if ! printf '%s' "${COMMENTER}" | grep -qE '^[A-Za-z0-9-]+$'; then
            echo "::warning::Commenter login is missing or malformed; skipping the binlog download."
            echo "authorized=false" >> "$GITHUB_OUTPUT"
            exit 0
          fi
          # Read the response first and extract with `jq` rather than using
          # `gh api --jq`: on a non-2xx response `gh` prints the error document
          # to stdout, which `--jq` does not filter, so the raw JSON would end
          # up in `perm` and get echoed into the log. Extracting the field
          # ourselves yields an empty string for any error shape.
          resp=$(gh api "repos/${GITHUB_REPOSITORY}/collaborators/${COMMENTER}/permission" 2>/dev/null)
          perm=$(printf '%s' "${resp}" | jq -r '.permission // empty' 2>/dev/null)
          case "${perm}" in
            admin|write) authorized=true ;;
            *)           authorized=false ;;
          esac
          if [ "${authorized}" = "true" ]; then
            echo "'${COMMENTER}' has '${perm}' access to ${GITHUB_REPOSITORY}; proceeding."
          else
            echo "::warning::'${COMMENTER}' does not have write access to ${GITHUB_REPOSITORY} (resolved permission '${perm:-none}'); skipping the binlog download."
          fi
          echo "authorized=${authorized}" >> "$GITHUB_OUTPUT"

      - name: Download binlogs from the PR's latest failed Azure Pipelines build
        id: fetch
        if: github.event_name != 'issue_comment' || steps.perm.outputs.authorized == 'true'
        shell: bash
        env:
          GH_TOKEN: ${{ github.token }}
          GH_AW_REPO: ${{ github.repository }}
          ADO_API: "https://dev.azure.com/dnceng-public/public/_apis"
          ADO_BUILD_UI: "https://dev.azure.com/dnceng-public/public/_build/results"
          # arcade-pr pipeline definition id in dnceng-public/public.
          ADO_BUILD_DEFINITION_ID: "283"
          PR_NUMBER: ${{ github.event.issue.number || fromJSON(github.event.inputs.aw_context || github.event.client_payload.aw_context || '{}').item_number }}
        run: |
          # Advisory + best-effort. On any gap emit binlog-found=false so the
          # agent pipeline stays inert.
          set +e
          set +o pipefail
          emit_none() { echo "binlog-found=false" >> "$GITHUB_OUTPUT"; exit 0; }

          [ -z "${PR_NUMBER}" ] && { echo "::warning::No PR number resolved from the slash-command event / aw_context."; emit_none; }
          # PR_NUMBER feeds GitHub API paths and the `refs/pull/<n>/merge`
          # branch query; require it numeric so a malformed event/aw_context
          # payload can't reach those URLs with unexpected content.
          if ! printf '%s' "${PR_NUMBER}" | grep -qE '^[0-9]+$'; then
            echo "::warning::Resolved PR number '${PR_NUMBER}' is not numeric; refusing."; emit_none
          fi

          # --- Scope check: only analyse PRs targeting main / release/* ---
          PR_JSON=$(gh api "repos/${GH_AW_REPO}/pulls/${PR_NUMBER}" 2>/dev/null)
          BASE_REF=$(printf '%s' "${PR_JSON}" | jq -r '.base.ref // empty')
          # An empty BASE_REF means the `gh api` call failed or returned no
          # data (rate limit / transient error), NOT that the PR targets an
          # out-of-scope branch. Treat it as a data-resolution failure so a
          # valid PR isn't silently skipped and misreported as base '' out of
          # scope.
          [ -z "${BASE_REF}" ] && { echo "::warning::Could not resolve the base ref for PR #${PR_NUMBER} (GitHub API returned no data); treating as a data-resolution failure, not an out-of-scope branch."; emit_none; }
          HEAD_SHA=$(printf '%s' "${PR_JSON}" | jq -r '.head.sha // empty')
          case "${BASE_REF}" in
            main|release/*) echo "PR #${PR_NUMBER} base '${BASE_REF}' is in scope." ;;
            *) echo "::warning::PR #${PR_NUMBER} base '${BASE_REF}' is out of scope (main, release/*); skipping."; emit_none ;;
          esac

          # --- Find the PR's most recent arcade-pr build (merge ref) ---
          # Query the newest build REGARDLESS of status (queue-time desc). If
          # the newest build is still queued/running — e.g. right after a
          # force-push — skip: analysing an older completed failure now would
          # pair a stale binlog with the PR's current head. Only proceed when
          # the newest build is completed AND failed. The head SHA is then
          # anchored to that build's own revision (below), so links/suggestions
          # always match the analysed binlog.
          builds_json=$(curl -sSL --retry 3 \
            "${ADO_API}/build/builds?definitions=${ADO_BUILD_DEFINITION_ID}&branchName=refs/pull/${PR_NUMBER}/merge&queryOrder=queueTimeDescending&\$top=1&api-version=7.1")
          BUILD_ID=$(printf '%s' "${builds_json}" | jq -r '.value // [] | .[0].id // empty')
          BUILD_STATUS=$(printf '%s' "${builds_json}" | jq -r '.value // [] | .[0].status // empty')
          BUILD_RESULT=$(printf '%s' "${builds_json}" | jq -r '.value // [] | .[0].result // empty')
          echo "Newest arcade-pr build for PR #${PR_NUMBER}: id='${BUILD_ID}' status='${BUILD_STATUS}' result='${BUILD_RESULT}'"
          [ -z "${BUILD_ID}" ] && { echo "::warning::No arcade-pr build found for PR #${PR_NUMBER}."; emit_none; }
          # Require a numeric build id before it feeds subsequent ADO API URLs,
          # so a malformed query response can't inject unexpected path/query.
          if ! printf '%s' "${BUILD_ID}" | grep -qE '^[0-9]+$'; then
            echo "::warning::ADO build id '${BUILD_ID}' is not numeric; refusing."; emit_none
          fi
          if [ "${BUILD_STATUS}" != "completed" ]; then
            echo "::warning::PR #${PR_NUMBER}'s newest arcade-pr build (${BUILD_ID}) is still '${BUILD_STATUS}'; wait for it to finish before analysing."
            emit_none
          fi
          if [ "${BUILD_RESULT}" != "failed" ]; then
            echo "::warning::PR #${PR_NUMBER}'s newest arcade-pr build (${BUILD_ID}) result is '${BUILD_RESULT}', not failed — the failure looks resolved; nothing to analyse."
            emit_none
          fi

          # Require the build's analyzed revision to equal the PR's CURRENT
          # head. gh-aw safe-output review comments carry no `commit_id` (they
          # target the current PR diff), so analyzing a stale revision would
          # misplace/reject inline suggestions. The PR can advance between
          # selecting the build and downloading artifacts, and right after a
          # force-push this query can still return the previous failed build —
          # so re-read the head here and skip if it moved.
          build_json=$(curl -sSL --retry 3 "${ADO_API}/build/builds/${BUILD_ID}?api-version=7.1")
          BUILD_PR_SHA=$(printf '%s' "${build_json}" | jq -r '.triggerInfo["pr.sourceSha"] // empty')
          BUILD_MERGE_SHA=$(printf '%s' "${build_json}" | jq -r '.sourceVersion // empty')
          PR_JSON2=$(gh api "repos/${GH_AW_REPO}/pulls/${PR_NUMBER}" 2>/dev/null)
          CURRENT_HEAD=$(printf '%s' "${PR_JSON2}" | jq -r '.head.sha // empty')
          CURRENT_MERGE=$(printf '%s' "${PR_JSON2}" | jq -r '.merge_commit_sha // empty')
          # Fail CLOSED: if either SHA can't be resolved (transient API failure
          # or missing Azure triggerInfo), skip rather than risk analyzing a
          # stale binlog against the current diff.
          if [ -z "${BUILD_PR_SHA}" ] || [ -z "${CURRENT_HEAD}" ]; then
            echo "::warning::Could not resolve build revision ('${BUILD_PR_SHA}') and/or current PR head ('${CURRENT_HEAD}'); skipping."
            emit_none
          fi
          if [ "${BUILD_PR_SHA}" != "${CURRENT_HEAD}" ]; then
            echo "::warning::Build ${BUILD_ID} analyzed revision '${BUILD_PR_SHA}' but PR #${PR_NUMBER} head is now '${CURRENT_HEAD}'; skipping stale build (a newer build will cover the current revision)."
            emit_none
          fi
          # ADO builds GitHub's `refs/pull/<n>/merge` ref, so build_json.sourceVersion
          # is that merge commit; if the base branch advanced it differs from the
          # PR's current merge_commit_sha even with the head unchanged. Skip stale merges.
          if [ -n "${BUILD_MERGE_SHA}" ] && [ -n "${CURRENT_MERGE}" ] && [ "${BUILD_MERGE_SHA}" != "${CURRENT_MERGE}" ]; then
            echo "::warning::Build ${BUILD_ID} merge revision '${BUILD_MERGE_SHA}' but PR #${PR_NUMBER} current merge is '${CURRENT_MERGE}' (base branch advanced); skipping stale merge."
            emit_none
          fi
          HEAD_SHA="${CURRENT_HEAD}"
          echo "Analyzing build ${BUILD_ID} at PR head revision '${HEAD_SHA}'."

          # --- Download every Logs_Build_* artifact and extract binlogs ---
          artifacts_json=$(curl -sSL --retry 3 "${ADO_API}/build/builds/${BUILD_ID}/artifacts?api-version=7.1")
          mapfile -t names < <(printf '%s' "${artifacts_json}" | jq -r '.value // [] | map(select(.name | test("^Logs_Build_"))) | .[].name')
          [ "${#names[@]}" -eq 0 ] && { echo "::warning::No Logs_Build_* artifacts on build ${BUILD_ID}."; emit_none; }

          # Guards for untrusted PR-produced archives: cap the compressed
          # download and the reported uncompressed size per artifact, bound
          # extraction time, AND enforce a cumulative uncompressed budget across
          # all legs so many individually-small artifacts can't collectively
          # exhaust the runner's disk.
          MAX_ZIP_BYTES=524288000       # 500 MB compressed per artifact
          MAX_UNZIP_BYTES=2147483648    # 2 GB uncompressed per artifact
          MAX_TOTAL_BYTES=4294967296    # 4 GB uncompressed across all artifacts
          TOTAL_BYTES=0
          mkdir -p /tmp/binlogs
          count=0
          staged_legs=0
          ai=0
          for name in "${names[@]}"; do
            # `name` is PR-controlled ADO artifact metadata and the
            # `^Logs_Build_` filter only anchors the prefix, so sanitize it
            # before using it in any on-disk path (guards against `/` or `..`
            # traversal); keep the original `name` for the artifacts_json lookup.
            safe_name=$(printf '%s' "${name}" | tr -c 'A-Za-z0-9._-' '_')
            ai=$((ai + 1))
            url=$(printf '%s' "${artifacts_json}" | jq -r --arg n "${name}" '.value[] | select(.name==$n) | .resource.downloadUrl // empty')
            [ -z "${url}" ] && continue
            rm -rf /tmp/ax /tmp/a.zip
            mkdir -p /tmp/ax
            # Hard-cap the bytes written to disk regardless of Content-Length:
            # stream through `head -c` (cap + 1) and bound total time.
            # `curl_rc` captures curl's OWN status: the pipeline's status is
            # `head`'s (which succeeds even when curl dies mid-transfer), so a
            # timed-out or interrupted download would otherwise be invisible
            # here and only resurface below as an unreadable archive —
            # indistinguishable from a hostile one, and costing the whole leg
            # and with it the entire analysis (see the all-legs guard below).
            # Nothing is appended to the pipeline and errexit is not toggled
            # around it: this step already runs with errexit off (`set +e` at
            # the top), so the pipeline cannot abort it, and a trailing
            # `|| true` would run `true` — itself a command — resetting
            # PIPESTATUS before curl's status could be read.
            curl -sSL --retry 3 --retry-delay 2 --max-time 600 "${url}" 2>/dev/null | head -c $((MAX_ZIP_BYTES + 1)) > /tmp/a.zip
            curl_rc=${PIPESTATUS[0]}
            ZIP_BYTES=$(stat -c%s /tmp/a.zip 2>/dev/null || echo 0)
            if [ "${ZIP_BYTES}" -eq 0 ]; then
              echo "::warning::Skipping ${name}: empty or failed download."; continue
            fi
            if [ "${ZIP_BYTES}" -gt "${MAX_ZIP_BYTES}" ]; then
              echo "::warning::Skipping ${name}: download exceeded ${MAX_ZIP_BYTES} bytes."; continue
            fi
            # Only here can a non-zero curl status mean a genuinely failed
            # transfer: an oversized artifact makes `head` close the pipe early
            # and curl exit on SIGPIPE, and that case was just skipped above.
            if [ "${curl_rc}" -ne 0 ]; then
              echo "::warning::Skipping ${name}: download failed or was truncated (curl exit ${curl_rc})."; continue
            fi
            # Bound the probe with `timeout` so a hostile/huge archive can't
            # hang the runner. `unzip -Zt` prints ONE summary line
            # ("<n> files, <x> bytes uncompressed, <y> bytes compressed")
            # instead of `unzip -l`'s per-entry listing, so the total is read
            # from a fixed column rather than by `tail -1`-ing a table whose
            # last line shifts with the entry list. (Measured on real
            # `Logs_Build_*` legs both forms agree and both are fast — this is
            # a robustness and parsing change, not a speed fix; the observed
            # "failed or timed out" warnings came from unreadable archives,
            # which the curl status check above now reports accurately.) Run
            # the pipeline in a subshell with `pipefail` and FAIL CLOSED on a
            # non-zero exit: if `timeout` kills the probe, its partial output
            # can still end in a numeric column and undercount the total,
            # bypassing the size guard below — so a failed probe skips the
            # artifact rather than trusting the parsed value.
            UNCOMP=$(set -o pipefail; timeout 60 unzip -Zt /tmp/a.zip 2>/dev/null | awk '{print $3}') \
              || { echo "::warning::Skipping ${name}: 'unzip -Zt' failed or timed out; cannot verify uncompressed size."; continue; }
            # Fail safe: if the uncompressed size isn't a plain integer (corrupt
            # zip / unexpected or timed-out `unzip -Zt` output), we can't verify
            # it — skip the artifact rather than let a non-numeric value bypass
            # the `-gt` guard.
            if ! printf '%s' "${UNCOMP}" | grep -qE '^[0-9]+$'; then
              echo "::warning::Skipping ${name}: could not determine uncompressed size (unparseable/timed-out unzip output)."; continue
            fi
            # ZIP64 uncompressed sizes can reach ~20 digits — beyond Bash's
            # signed 64-bit range, where `-gt` (and the cumulative `$((...))`
            # below) error out and, under `set +e`, would let an oversized
            # archive slip past the guard. Any value with more digits than the
            # limit is unambiguously larger, so reject on decimal length first;
            # after this, UNCOMP fits safely in the integer range used below.
            if [ "${#UNCOMP}" -gt "${#MAX_UNZIP_BYTES}" ]; then
              echo "::warning::Skipping ${name}: uncompressed size has ${#UNCOMP} digits, exceeding the ${MAX_UNZIP_BYTES} guard (possible zip bomb)."; continue
            fi
            if [ "${UNCOMP}" -gt "${MAX_UNZIP_BYTES}" ]; then
              echo "::warning::Skipping ${name}: uncompressed size ${UNCOMP} exceeds ${MAX_UNZIP_BYTES} guard (possible zip bomb)."; continue
            fi
            if [ $((TOTAL_BYTES + UNCOMP)) -gt "${MAX_TOTAL_BYTES}" ]; then
              echo "::warning::Cumulative uncompressed budget ${MAX_TOTAL_BYTES} reached at ${name}; stopping extraction."; break
            fi
            # Refuse the archive if any entry path is absolute or has a `..`
            # component (defense-in-depth over unzip's own traversal guard),
            # then extract `*.binlog` entries *preserving* their in-archive
            # paths (no `-j`) under a fresh dir + timeout, so two binlogs that
            # share a basename in different folders don't overwrite each other.
            # Stream the entry listing through `grep` under a `timeout` (no full
            # in-memory buffer of entry names, which a many-entry archive could
            # bloat) and use PIPESTATUS to separate the failure modes: a
            # non-zero listing exit (error/timeout) FAILS CLOSED; a grep match
            # means a suspicious absolute/`..` path.
            timeout 60 unzip -Z1 /tmp/a.zip 2>/dev/null | grep -qE '(^/|(^|/)\.\.(/|$))'
            zscan_rc=("${PIPESTATUS[@]}")
            if [ "${zscan_rc[0]}" -ne 0 ]; then
              echo "::warning::Skipping ${name}: could not list archive entries (unzip -Z1 rc=${zscan_rc[0]})."; continue
            fi
            if [ "${zscan_rc[1]}" -eq 0 ]; then
              echo "::warning::Skipping ${name}: archive has a suspicious (absolute or ..) entry path."; continue
            fi
            timeout 120 unzip -o /tmp/a.zip '*.binlog' -d /tmp/ax >/dev/null 2>&1 \
              || { echo "::warning::Skipping ${name}: extraction failed or timed out."; continue; }
            # Consume the cumulative budget only once the archive actually
            # extracted — not on a suspicious-path or extraction-failure skip
            # above — so a skipped leg can't wrongly exhaust the budget and
            # force later legs to be dropped as "incomplete".
            TOTAL_BYTES=$((TOTAL_BYTES + UNCOMP))
            i=0
            leg_staged=0
            while IFS= read -r bl; do
              [ -f "${bl}" ] || continue
              # Every destination is uniquely prefixed with the artifact index
              # (`ai`) and a per-file counter (`i`), so neither a cross-artifact
              # sanitize collision nor same-basename entries within one archive
              # can overwrite a previously staged leg's binlog. `safe_name` is
              # kept only for readability.
              dest="/tmp/binlogs/${ai}_${i}_${safe_name}.binlog"
              # Only count a staged binlog when the copy actually succeeds —
              # `set +e` is on, so a failed `cp` must not inflate the counts.
              if cp "${bl}" "${dest}"; then
                count=$((count + 1))
                i=$((i + 1))
                leg_staged=1
              else
                echo "::warning::Failed to stage ${bl}; skipping."
              fi
            done < <(find /tmp/ax -type f -name '*.binlog')
            # This leg produced at least one usable binlog.
            [ "${leg_staged}" -eq 1 ] && staged_legs=$((staged_legs + 1))
          done
          echo "Extracted ${count} binlog(s) from ${staged_legs}/${#names[@]} legs into /tmp/binlogs:"
          ls -la /tmp/binlogs || true
          [ "${count}" -eq 0 ] && { echo "::warning::No *.binlog found in any Logs_Build_* artifact of build ${BUILD_ID}."; emit_none; }
          # Fail CLOSED on a partial set: if any Logs_Build_* leg did not yield
          # a usable binlog (download/extract failure, size-guard skip, or no
          # binlog inside), we cannot see every leg. Activating anyway would let
          # the agent treat the retrieved legs as the whole build and possibly
          # mis-classify a real build break in a missing leg as a clean compile /
          # non-build failure. A later build/check will re-trigger the analysis.
          if [ "${staged_legs}" -ne "${#names[@]}" ]; then
            echo "::warning::Only ${staged_legs} of ${#names[@]} Logs_Build_* legs produced a usable binlog; skipping to avoid analyzing an incomplete build (a missing leg could be the one that failed)."
            emit_none
          fi

          # The download/extract loop above can take minutes. Re-read the PR
          # head right before activating and fail CLOSED if it moved or can't
          # be resolved: a force-push during that window would otherwise leave
          # the analyzed binlog stale relative to the current diff (inline
          # comments carry no commit_id and target the current diff).
          LATEST_PR=$(gh api "repos/${GH_AW_REPO}/pulls/${PR_NUMBER}" 2>/dev/null)
          LATEST_HEAD=$(printf '%s' "${LATEST_PR}" | jq -r '.head.sha // empty')
          LATEST_MERGE=$(printf '%s' "${LATEST_PR}" | jq -r '.merge_commit_sha // empty')
          if [ -z "${LATEST_HEAD}" ] || [ "${LATEST_HEAD}" != "${HEAD_SHA}" ]; then
            echo "::warning::PR #${PR_NUMBER} head changed during artifact download ('${HEAD_SHA}' -> '${LATEST_HEAD}') or could not be re-resolved; skipping to avoid posting stale-build suggestions against the new diff."
            emit_none
          fi
          # The base branch may also have advanced during the download; if the
          # merge revision moved from what the build analyzed, skip (stale merge).
          if [ -n "${BUILD_MERGE_SHA}" ] && [ -n "${LATEST_MERGE}" ] && [ "${LATEST_MERGE}" != "${BUILD_MERGE_SHA}" ]; then
            echo "::warning::PR #${PR_NUMBER} merge revision changed during artifact download ('${BUILD_MERGE_SHA}' -> '${LATEST_MERGE}'); skipping stale merge."
            emit_none
          fi

          {
            echo "binlog-found=true"
            echo "pr-number=${PR_NUMBER}"
            echo "pr-head-sha=${HEAD_SHA}"
            echo "pr-merge-sha=${BUILD_MERGE_SHA}"
            echo "ado-build-id=${BUILD_ID}"
            echo "ado-build-url=${ADO_BUILD_UI}?buildId=${BUILD_ID}"
          } >> "$GITHUB_OUTPUT"

      - name: Upload analysis artifact
        if: steps.fetch.outputs.binlog-found == 'true'
        uses: actions/upload-artifact@v7.0.1
        with:
          name: build-failure-analysis-data
          path: /tmp/binlogs
          if-no-files-found: warn
          retention-days: 1

# Steps that run in the agent job. The top-level `if:` gates these on binlogs
# having been retrieved, so the agent never runs without something to analyse.
steps:
  - name: Download analysis artifact
    uses: actions/download-artifact@v8.0.1
    with:
      name: build-failure-analysis-data
      path: /tmp/binlogs

  - name: Export agent context
    shell: bash
    env:
      GH_AW_BINLOG_FOUND_VALUE: ${{ needs.fetch-binlog.outputs.binlog-found }}
      GH_AW_PR_NUMBER_VALUE: ${{ needs.fetch-binlog.outputs.pr-number }}
      GH_AW_PR_HEAD_SHA_VALUE: ${{ needs.fetch-binlog.outputs.pr-head-sha }}
      GH_AW_PR_MERGE_SHA_VALUE: ${{ needs.fetch-binlog.outputs.pr-merge-sha }}
      GH_AW_ADO_BUILD_URL_VALUE: ${{ needs.fetch-binlog.outputs.ado-build-url }}
      GH_AW_GITHUB_WORKSPACE: ${{ github.workspace }}
    run: |
      # See build-failure-analysis.md for the binlog path conventions. The
      # per-leg binlogs are read through the binlog-mcp MCP server (mounted at
      # `/data/binlogs`); GH_AW_BINLOG_HOST_PATH points at the Azure DevOps
      # build for human-facing references.
      BINLOG_DIR="/data/binlogs"
      LIST=""
      if [ "${GH_AW_BINLOG_FOUND_VALUE:-false}" = "true" ] && [ -d /tmp/binlogs ]; then
        for f in /tmp/binlogs/*.binlog; do
          [ -f "$f" ] || continue
          LIST="${LIST}${BINLOG_DIR}/$(basename "$f")"$'\n'
        done
      fi
      FIRST=$(printf '%s' "$LIST" | head -1)
      {
        echo "GH_AW_BUILD_OUTCOME=failure"
        echo "GH_AW_BINLOG_DIR=${BINLOG_DIR}"
        echo "GH_AW_BINLOG_PATH=${FIRST}"
        echo "GH_AW_BINLOG_HOST_PATH=${GH_AW_ADO_BUILD_URL_VALUE}"
        echo "GH_AW_PR_NUMBER=${GH_AW_PR_NUMBER_VALUE}"
        echo "GH_AW_PR_HEAD_SHA=${GH_AW_PR_HEAD_SHA_VALUE}"
        echo "GH_AW_PR_MERGE_SHA=${GH_AW_PR_MERGE_SHA_VALUE}"
        echo "GH_AW_WORKSPACE=${GH_AW_GITHUB_WORKSPACE}"
        echo "GH_AW_BINLOG_LIST<<GH_AW_EOF"
        printf '%s' "$LIST"
        echo "GH_AW_EOF"
      } >> "$GITHUB_ENV"

tools:
  github:
    toolsets: [pull_requests, repos]
  bash:
    - "cat"
    - "head"
    - "tail"
    - "grep"
    - "wc"
    - "sort"
    - "uniq"
    - "ls"
    - "find"
    # binlog-mcp is also mounted as a CLI wrapper (…/mcp-cli/bin/binlog-mcp);
    # allow it so the agent can query the binlogs via the wrapper when it does
    # not call the MCP tool natively.
    - "binlog-mcp:*"

safe-outputs:
  messages:
    footer: "> 🤖 **Automated content by GitHub Copilot.** Generated by the [{workflow_name}]({agentic_workflow_url}) workflow.{ai_credits_suffix} · [◷]({history_link})"
  # This workflow is triggered by an `issue_comment` on a PR, so it HAS a
  # triggering item — and it is the same PR `fetch-binlog` resolves from
  # `github.event.issue.number`. `target: "triggering"` is therefore equivalent
  # by construction to targeting `GH_AW_PR_NUMBER`, while removing the agent's
  # ability to name a different issue/PR: the prompt instruction to use
  # `GH_AW_PR_NUMBER` is guidance, not a boundary, and the agent reads binlogs
  # and PR content from unmerged external code.
  #
  # The auto-trigger workflow must keep `target: "*"`: `check_run` has no
  # triggering item of its own, and the resolved PR number is not reachable
  # there — gh-aw bakes `target` into the generated `safe_outputs` job, whose
  # `needs:` does not include `fetch-binlog`, and the Actions `needs` context
  # exposes only direct dependencies.
  report-failure-as-issue: false
  add-comment:
    max: 5
    target: "triggering"
    # Hiding superseded comments is scoped to the posting workflow's id
    # (`GH_AW_WORKFLOW_ID`, the workflow FILE stem — here
    # `build-failure-analysis-command` vs `build-failure-analysis`), so this
    # workflow only ever hides its own comments: re-running the command leaves
    # the stale automatic analysis visible next to the fresh one. dotnet/sdk
    # fixes that with the object form
    # (`hide-older-comments: {enabled: true, match: [...]}`), but the gh-aw
    # v0.77.5 schema this repo pins types `hide-older-comments` as a
    # `templatable_boolean` with no object/`match` variant, so it cannot be
    # expressed here. Fixed by bumping the compiler, not by editing this line.
    hide-older-comments: true
  create-pull-request-review-comment:
    max: 25
    target: "triggering"
  noop:
    max: 5
    report-as-issue: false
---

<!--
  Body provided by shared/build-failure-analysis-shared.md.
-->
