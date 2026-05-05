@baseUrl = https://localhost:7227

### Health
GET {{baseUrl}}/api/health

### Clubs
GET {{baseUrl}}/api/clubs

### Players
GET {{baseUrl}}/api/players

### Successful transfer to Club A
POST {{baseUrl}}/api/transfers
Content-Type: application/json

{
  "playerId": 1,
  "destinationClubId": 1,
  "offerAmount": 5000000,
  "salaryProposed": 1200000
}

### Get transfer
GET {{baseUrl}}/api/transfers/1

### Get transfer audit
GET {{baseUrl}}/api/transfers/1/audit

### Failed transfer to Club C because of insufficient budget
POST {{baseUrl}}/api/transfers
Content-Type: application/json

{
  "playerId": 1,
  "destinationClubId": 3,
  "offerAmount": 5000000,
  "salaryProposed": 1200000
}
