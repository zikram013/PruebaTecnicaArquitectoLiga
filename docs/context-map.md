```md
# Context Map

```mermaid
flowchart LR
    Transfer[Transfer Management]
    Squad[Squad Management]
    Finance[Financial Management]
    Competition[Competition Management]
    Analytics[Performance Analytics]
    Facilities[Facilities Management]
    Fan[Fan Engagement]
    Regulatory[Regulatory / Federation Systems]
    Legacy[Legacy Systems SAP / Oracle]

    Transfer -->|Validates budget and payment| Finance
    Transfer -->|Updates player squad| Squad
    Transfer -->|Contract/compliance checks| Regulatory

    Competition -->|Produces match data| Analytics
    Competition -->|Publishes live updates| Fan
    Fan -->|Ticketing and memberships| Finance

    Finance -->|Synchronizes financial data| Legacy
    Facilities -->|Venue availability| Competition
