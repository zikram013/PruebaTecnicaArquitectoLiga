# Bounded Contexts

## 1. Squad Management
Manages players, coaches, staff, and active squad composition.

## 2. Transfer Management
Manages transfer offers, negotiations, approvals, legal traceability, and orchestration of player transfers.

## 3. Financial Management
Manages club budgets, salaries, payments, transfer compensations, and financial validations.

## 4. Competition Management
Manages matches, calendars, fixtures, and results.

## 5. Performance Analytics
Manages statistics, metrics, and performance indicators.

## 6. Facilities Management
Manages stadiums, training centers, and operational facilities.

## 7. Fan Engagement
Manages memberships, ticketing, merchandising, and fan notifications.

## Main Relationships
- Transfer Management depends on Financial Management for budget validation and payment processing.
- Transfer Management depends on Squad Management for squad updates.
- Competition Management produces data consumed by Performance Analytics.
- Competition Management feeds Fan Engagement with match and result information.
