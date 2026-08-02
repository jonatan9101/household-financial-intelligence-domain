# FinancialMovement

> Core Aggregate of the Household Finance Context

## Purpose

FinancialMovement represents an immutable financial fact that affects the financial state of a Household.

It is the core aggregate of the MVP. Every budget, dashboard, forecast, recommendation and financial insight is ultimately derived from FinancialMovement.

## Responsibilities

- Preserve financial facts
- Guarantee transactional consistency
- Preserve original evidence
- Maintain traceability
- Publish domain events

## Out of Scope

- Budget calculations
- Financial health
- Forecasting
- Recommendations
- Analytics

## Aggregate Root

FinancialMovement

## Identity

FinancialMovementId

## Bounded Context

Household Finance
