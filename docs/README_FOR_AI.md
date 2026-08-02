# AI Development Guide

This repository follows Domain-Driven Design.

The Domain Model is the source of truth.

Priority order:

1. ADR
2. Domain Model
3. Strategic Design
4. Business
5. Architecture

Never generate code that contradicts these documents.

If documentation is incomplete:

STOP.

Do not guess.

Request clarification.

Business rules always belong to the Domain.

Repositories never contain business logic.

Controllers are thin.

Application orchestrates.

Infrastructure implements.

Prefer simplicity over abstractions.

YAGNI always wins.

KISS always wins.

If two implementations are valid,

choose the simpler one.