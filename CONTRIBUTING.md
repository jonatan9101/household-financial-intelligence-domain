# Contributing

Small project, small process. Follow these rules and the project stays healthy.

## Branch Strategy

- One branch per milestone: `feat/m{n}-<milestone>` (e.g. `feat/m6-supabase-authentication`).
- Branch from the base that already contains the previous milestone (stacked PRs).
- Open one Pull Request per milestone against that base.
- Never commit directly to `main`.

## Before You Start

- Read `AGENTS.md` and the documentation in the order listed in `docs/README_FOR_AI.md`.
- Never invent business rules. If documentation is ambiguous, stop and ask.
- Implement one capability at a time. Complete the current milestone before starting another.

## Build

```sh
dotnet build HouseholdFinancialIntelligence.slnx
```

Must complete with 0 warnings and 0 errors.

## Tests

```sh
dotnet test HouseholdFinancialIntelligence.slnx
```

All tests must pass. Business rules are tested with xUnit + FluentAssertions (`Given_When_Then`). Target: 100% Domain coverage. Do not test EF Core, ASP.NET, or dependency injection.

## Reviews

Every PR passes both reviews before being opened:

1. **Architecture Review** — validate DDD, Clean Architecture, simplicity, and MVP scope (`.opencode/skills/architecture-reviewer.md`).
2. **PR Review** — validate build, tests, architecture, DDD, SOLID, clean code, simplicity, security, EF Core, and documentation (`.opencode/skills/pr-reviewer.md`).

Fix any FAIL section before opening the PR.

## PR Checklist

- [ ] Branch is based on the correct previous milestone
- [ ] Build passes with 0 warnings/errors
- [ ] All tests pass
- [ ] Domain coverage stays at 100%
- [ ] Architecture Review passed
- [ ] PR Review passed
- [ ] Documentation updated (IMPLEMENTATION_PLAN.md, ADRs, runbook) if behavior changed
- [ ] No secrets committed
- [ ] No unrelated changes included