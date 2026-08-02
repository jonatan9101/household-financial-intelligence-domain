# Budget

> Aggregate responsible for planning spending within a Household.

## Purpose

A Budget defines planned spending for a specific period. It is compared against actual FinancialMovements through read models; it never calculates actual spending itself.

## Responsibilities

- Define spending limits
- Manage budget lifecycle
- Protect planning invariants
- Publish budget events

## Out of Scope

- Posting expenses
- Calculating actual consumption
- Forecasting
- Recommendations
