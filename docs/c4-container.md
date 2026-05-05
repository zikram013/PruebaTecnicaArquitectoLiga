```md
# C4 Container Diagram

```mermaid
flowchart TB
    User[Administrative Users]
    Fan[Fans]

    Api[ASP.NET Core Web API]
    Transfer[Transfer Management Module]
    Squad[Squad Management Module]
    Finance[Financial Management Module]
    Competition[Competition Management Module]
    Analytics[Performance Analytics Module]
    FanPortal[Fan Engagement Module]

    Bus[Message Bus]
    Db[(Operational Database)]
    ReadDb[(Read Models / Reporting Store)]

    Payment[Payment Provider]
    Notification[Notification Provider]
    Legacy[Legacy Systems SAP / Oracle]

    User --> Api
    Fan --> Api

    Api --> Transfer
    Api --> Squad
    Api --> Finance
    Api --> Competition
    Api --> FanPortal

    Transfer --> Bus
    Finance --> Bus
    Squad --> Bus
    Competition --> Bus

    Transfer --> Db
    Squad --> Db
    Finance --> Db
    Competition --> Db
    FanPortal --> Db

    Analytics --> ReadDb
    Bus --> Analytics

    Finance --> Payment
    FanPortal --> Notification
    Finance --> Legacy
