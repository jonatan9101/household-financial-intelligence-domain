# Ubiquitous Language

---

# Purpose

This document defines the common language shared by domain experts, product owners, architects and developers.

The Ubiquitous Language is the official business vocabulary of the Household Financial Intelligence (HFI) domain.

Every business conversation, document, API, domain model and source code artifact must use the terminology defined here.

Whenever two terms describe the same concept, one must be selected as the canonical business term.

---

# Domain Vocabulary

## Household

A group of people who manage their finances together.

A Household represents the primary business boundary of the platform.

---

## Member

A person belonging to a Household.

Members participate in financial activities according to their assigned role.

---

## Financial Source

A system capable of providing financial information.

Examples include:

* Email Provider
* Banking API
* Credit Card Provider
* Digital Wallet
* Payment Platform

The domain never refers to specific technologies.

---

## Financial Document

A document containing financial evidence.

Examples include:

* payment confirmation
* invoice
* bank notification
* purchase receipt

A Financial Document represents evidence.

It is not yet a financial movement.

---

## Financial Movement

A confirmed financial fact belonging to a Household.

Examples include:

* purchase
* income
* refund
* transfer
* bank fee

Financial Movements are immutable.

---

## Financial Account

A financial instrument owned by a Household member.

Examples include:

* checking account
* savings account
* credit card
* digital wallet

---

## Financial Fact

An immutable event that occurred in the real world.

Facts never change.

Facts can only become invalid.

---

## Interpretation

Business meaning assigned to a Financial Fact.

Interpretations may evolve.

Examples include:

* movement category
* merchant normalization
* spending purpose

---

## Financial Knowledge

Business information derived from facts and interpretations.

Examples include:

* monthly spending
* financial health
* budget execution
* spending trends

Knowledge is never considered the source of truth.

---

## Financial Advice

A recommendation generated from financial knowledge.

Advice depends on the current business context.

Advice is never permanent.

---

## Budget

A spending limit defined for a given period.

Budgets are business commitments rather than accounting entities.

---

## Financial Goal

A desired financial outcome defined by a Household.

Examples include:

* emergency fund
* vacation
* home purchase
* education

---

# Canonical Terms

The following terms are considered canonical throughout the platform.

| Canonical Term      | Avoid                 |
| ------------------- | --------------------- |
| Household           | Family Group          |
| Member              | User                  |
| Financial Source    | Gmail Connector       |
| Financial Document  | Email                 |
| Financial Movement  | Transaction           |
| Financial Fact      | Record                |
| Interpretation      | Categorization        |
| Financial Knowledge | Analytics             |
| Financial Advice    | Recommendation Engine |

---

# Business Language

The domain should naturally support business conversations.

Examples:

✔️ Register a Financial Movement.

✔️ Interpret a Financial Document.

✔️ Preserve Financial Facts.

✔️ Generate Financial Knowledge.

✔️ Provide Financial Advice.

Avoid technical language during business discussions.

Examples:

✘ Parse Email

✘ Read Gmail

✘ Execute Lambda

✘ Query PostgreSQL

---

# Naming Principles

Business concepts should:

* represent business meaning,
* remain technology independent,
* be understandable by domain experts,
* remain stable over time.

Technology-specific names are prohibited inside the domain model.

---

# Domain Language Evolution

The Ubiquitous Language is expected to evolve.

Whenever a better business term emerges:

1. Validate the concept with domain experts.
2. Update this document.
3. Update the Domain Model.
4. Update the implementation.

The Blueprint is the authoritative source.

---

# Relationship with the Blueprint

Every chapter in the Domain Blueprint uses the terminology defined in this document.

No chapter may introduce new business concepts without first updating the Ubiquitous Language.
