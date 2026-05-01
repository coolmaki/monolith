---
name: command-creator
description: Create a new Claude Code slash command in .claude/commands/. Use when the user asks to create, add, write, or scaffold a new slash command, a /command, or a .claude/commands file. Generates YAML frontmatter (description, argument-hint, allowed-tools) and an XML-tagged body using this project's standard tags — <objective>, <context>, <process>, and optionally <template>/<examples>. Do NOT use for subagents (.claude/agents/), skills (.claude/skills/), or editing an existing command.
---
<objective>
Scaffold a new slash command file at `.claude/commands/[<namespace>/]<name>.md` matching this project's conventions: YAML frontmatter plus an XML-tagged body. Stop at the file write — do not execute the new command.
</objective>

<context>
Inputs arrive as a free-form description in `$ARGUMENTS` or the surrounding conversation (e.g. "add a command that runs the test suite for a given module").

Fields to determine:

| Field | Required | Notes |
|---|---|---|
| `name` | yes | kebab-case. File lives at `.claude/commands/[<namespace>/]<name>.md`. |
| `namespace` | no | Subfolder, e.g. `monolith`. If the command is project-specific, default to `monolith` and confirm. |
| `description` | yes | One short sentence, starts with a verb. Shown in the command picker. |
| `argument-hint` | conditional | Short placeholder (e.g. `"<name>"`, `"<module> [--flag]"`). Omit if the command takes no arguments. |
| `allowed-tools` | recommended | Minimum tools needed (e.g. `Bash`, `Read`, `Edit`, `Write`). Always set if the command runs shell or modifies files. |
| `model` | rare | Only set if the command needs a specific model: `claude-opus-4-7`, `claude-sonnet-4-6`, `claude-haiku-4-5-20251001`. |

Standard body tags, in order: `<objective>` → `<context>` → `<process>`, then optionally `<template>` / `<examples>`.

Reference: `.claude/commands/monolith/add-command-handler.md` is the canonical example — generated commands should look and read like it.
</context>

<process>
1. **Parse intent.** Read `$ARGUMENTS` and the conversation. Fill every field above that you can infer confidently. Note which fields remain ambiguous.

2. **Ask follow-ups (hybrid mode).** Use `AskUserQuestion` only for fields you cannot confidently infer. Bundle related questions in a single call; never ask one at a time.
   - Always confirm `name`, `namespace`, and `allowed-tools` when the command will run Bash or write files.
   - Don't ask about fields you can derive (e.g. `argument-hint` follows from the described inputs).
   - Don't ask about body tags — choose them in step 3.

3. **Choose body tags.**
   - `<objective>` — one or two sentences on what the command does and why.
   - `<context>` — inputs (e.g. `$ARGUMENTS` shape), constraints, repo assumptions.
   - `<process>` — numbered, imperative steps. Be explicit about validation and stop conditions.
   - `<template>` — code, file, or output skeleton. Use only when the command emits a structured artifact.
   - `<examples>` — sample invocation or sample output. Use sparingly.
   - A trivial command may only need `<objective>` + `<process>`. Do not over-tag.

4. **Resolve target path.**
   - With namespace: `.claude/commands/<namespace>/<name>.md`
   - Without: `.claude/commands/<name>.md`
   - If the file already exists, STOP and ask before overwriting.

5. **Write the file** using the skeleton in `<template>`. Omit any frontmatter key or body tag that doesn't apply — don't leave empty placeholders.

6. **Report.** Print the resolved path and the invocation string (e.g. `/monolith:add-command-handler <name>`). Do not execute the new command.

Conventions to enforce on the generated file:
- XML tags over Markdown headers — they give Claude unambiguous boundaries and reduce prompt leakage.
- One verb per `<process>` step; explicit stop conditions on steps that can fail.
- Don't restate the `description` inside the body.
- Least privilege on `allowed-tools`.
</process>

<template>
````markdown
---
name: <namespace>:<name>          # include only when namespaced
description: <one short sentence, starts with a verb>
argument-hint: "<arg-shape>"      # omit when no arguments
allowed-tools:
  - <Tool>                         # e.g. Bash, Read, Edit, Write
---
<objective>
<one or two sentences on what this command does and why>
</objective>

<context>
<input data, $ARGUMENTS shape, repo assumptions, preconditions>
</context>

<process>
1. **<step name>.** <what to do, including validation>
2. **<step name>.** <next step>
   - <sub-bullet for sub-rules or stop conditions>
</process>

<template>
<code or output skeleton — only when the command emits a structured artifact>
</template>

<examples>
<sample invocation or sample output — only when an example clarifies usage>
</examples>
````
</template>
