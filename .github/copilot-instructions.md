# Git Commit Message Instructions

When generating Git commit messages, follow these rules:

- Write all commit messages in **English**.
- Keep messages **short, clear, and objective**.
- Focus on the **main change** introduced by the commit.
- Describe the **intent and impact of the change**, rather than simply summarizing the code diff.
- Use an **imperative verb**, such as `Add`, `Fix`, `Update`, `Remove`, `Refactor`, or `Improve`.
- Describe **what was changed** without unnecessary implementation details.
- Do not mention every modified file or minor change.
- Prefer a **single concise line** whenever possible.
- If the commit contains multiple meaningful changes, it may be organized into **short topics or bullet points**, but only when necessary.
- Avoid generic messages such as `Update code`, `Fix changes`, or `Minor improvements`.
- Do not include issue numbers unless they are explicitly provided.
- The commit message must be understandable without requiring the reader to inspect the diff.
- Prioritize **clarity and usefulness in the Git history**.
- Avoid unnecessary verbosity, repetition, and implementation-specific details.

The commit message should make the **main change stand out immediately**.

## Examples

Good commit messages:

- `Add validation for user registration`
- `Fix null reference in payment processing`
- `Update customer API response model`
- `Remove deprecated authentication flow`
- `Refactor order processing service`
- `Improve error handling in file upload`
