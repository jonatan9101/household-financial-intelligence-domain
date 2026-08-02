# Bounded Contexts

| Context | Responsibility |
|---------|----------------|
| Document Acquisition | Acquire financial documents |
| Document Interpretation | Extract financial information |
| Household Finance | Preserve financial facts |
| Movement Classification | Interpret financial movements |
| Financial Intelligence | Generate knowledge |
| Financial Advisory | Produce recommendations |

## Context Relationship

```mermaid
flowchart LR
DA[Document Acquisition]
-->DI[Document Interpretation]
-->MC[Movement Classification]
-->HF[Household Finance]
-->FI[Financial Intelligence]
-->FA[Financial Advisory]
```
