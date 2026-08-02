# Event Storming

## Main Flow

```mermaid
flowchart LR
A[Authorize Financial Source]
-->B[Import Financial Document]
-->C[Interpret Document]
-->D[Register Financial Movement]
-->E[Movement Posted]
-->F[Update Read Models]
```

## Main Domain Events

- FinancialSourceAuthorized
- FinancialDocumentImported
- FinancialMovementRegistered
- MovementPosted
- BudgetUpdated
- FinancialHealthUpdated
