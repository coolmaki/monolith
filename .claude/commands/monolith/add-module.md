---
name: monolith:add-module
description: Scaffold a new module (Domain/Application/Infrastructure/Endpoints) under src/Monolith.Modules.<name>.
argument-hint: "<name>"
---

# Add Module

Scaffolds a new module by delegating to `scripts/add-module.sh`.

## 1. Validate input

Stop with a clear error message and do not proceed if any check fails.

1. **Name provided.** If the argument is empty or only whitespace, ask the user to supply a name.
2. **PascalCase.** The name must match the regex `^[A-Z][A-Za-z0-9]*$` (starts with an uppercase letter, alphanumeric only). Reject names with hyphens, underscores, dots, spaces, or a lowercase first letter.

## 2. Run the script

From the repository root, run:

```sh
./scripts/add-module.sh <name>
```

Surface the script's output to the user. If it exits non-zero, report the failure rather than attempting a fix.
