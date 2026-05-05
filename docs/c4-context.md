# C4 Context Diagram

```mermaid
flowchart TB
    ClubAdmin[Club Administrator]
    SportingDirector[Sporting Director]
    FinanceOfficer[Finance Officer]
    Coach[Coach / Staff]
    Fan[Fan]

    Platform[Sports Club Management Platform]

    SAP[SAP]
    Oracle[Oracle]
    PaymentProvider[Payment Provider]
    NotificationProvider[Notification Provider]
    IdentityProvider[Identity Provider]
    Federation[Sports Federation / Regulatory Systems]

    ClubAdmin -->|Manage club operations| Platform
    SportingDirector -->|Manage transfers and squad planning| Platform
    FinanceOfficer -->|Manage budgets and payments| Platform
    Coach -->|Review squad and performance| Platform
    Fan -->|Tickets, memberships, live updates| Platform

    Platform -->|Financial integration| SAP
    Platform -->|Legacy data integration| Oracle
    Platform -->|Transfer payments| PaymentProvider
    Platform -->|Emails, push notifications| NotificationProvider
    Platform -->|Authentication and authorization| IdentityProvider
    Platform -->|Compliance and registration data| Federation
