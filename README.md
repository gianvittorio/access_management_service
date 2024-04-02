# Access Management Service

## Overview
The following project is a simple access management API, that allows users signing in, either as normal or b2b user.
## Assumptions
- Signup API will create a new user if the email is not already registered.

## Data Model
We basically have three entities:
1. <strong>User</strong> - It stores user personal information;
2. <strong>Employee</strong> - Which is also a user, adding job/company related information to it;
3. <strong>EligibilityMetadata</strong> - It stores user eligibility information, such as a file that contains employees information;

<img width="798" alt="Captura de Tela 2024-04-02 às 10 02 05" src="https://github.com/gianvittorio/access_management_service/assets/8211552/b97788c1-d9b4-4fbb-a613-3953f3a8f028">

## API
We essentially have only two API's:
1. <strong>Signup</strong> - "/api/access_management/v1/pub/self_signup"
2. <strong>Eligibility</strong> - "/api/access_management/v1/pub/employer_signup"

API contracts as follows:

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "Signup API Request Schema",
  "type": "object",
  "properties": {
    "email": {
      "type": "string"
    },
    "password": {
      "type": "string"
    },
    "country": {
      "type": "string"
    },
    "full_name": {
      "type": "string"
    },
    "employer_name": {
      "type": "string"
    },
    "salary": {
      "type": "string"
    }
  },
  "required": [
    "email",
    "password"
  ]
}
```

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "Signup API Response Schema",
  "type": "object",
  "properties": {
    "user_id": {
      "type": "number"
    },
    "access_type": {
      "type": "string"
    }
  },
  "required": [
    "user_id",
    "access_type"
  ]
}
```

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "Eligibility API Request Schema",
  "type": "object",
  "properties": {
    "file": {
      "type": "string"
    },
    "employer_name": {
      "type": "string"
    }
  },
  "required": [
    "file",
    "employer_name"
  ]
}
```

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "Generated schema for Root",
  "type": "object",
  "properties": {
    "processed_lines": {
      "type": "array",
      "items": {
        "type": "string"
      }
    },
    "skipped_lines": {
      "type": "array",
      "items": {
        "type": "string"
      }
    }
  },
  "required": [
    "processed_lines",
    "skipped_lines"
  ]
}
```

## Architecture

### Monolithic Architecture 
We begin with the simplest approach to fulfill our functional requirements and we iterate from it to address the non-functional ones.
A single monolith comprised with a Postgres database is a good start, since we have a simple somewhat relational schema:
<img width="451" alt="Captura de Tela 2024-04-02 às 10 31 44" src="https://github.com/gianvittorio/access_management_service/assets/8211552/6742dcb1-ecec-40fc-887e-9a661ebe0acb">

### Microservices Architecture
In order to achieve higher availability and throughput, through horizontal scaling, we break the data model (responsibility segregation) into two different services, , comprised with their own storage:
1. <strong>User Service</strong> - in charge for User model
2. <strong>Employer Service</strong> - in charge for employer and eligibility model
Therefore <strong>Access Management Service</strong> acts as a mere <strong>API Gateway</strong>, in charge for retrieving information from <strong>User Service</strong> and <strong>Employer Service</strong>. In order to avoid unnecessary roundtrips and possibly overwhelming such outbound services, we setup a cache layer in front of both of them. Access Management Service will try reading from cache and, in case of a miss, it will call User/Employer service and persist it in the cache (write aside). Since most data does not seem to change very often, it can safely be cached. A TTL with daily granularity sounds suitable.
<img width="740" alt="Captura de Tela 2024-04-02 às 10 31 28" src="https://github.com/gianvittorio/access_management_service/assets/8211552/1b7076f6-1f4a-4955-97fb-eb825173ae5c">

### Follow Up
1. Federating and sharding both User and Employer schemas;
2. Delegate csv file download and processing to a document search engine, or perhaps parallelize it with Hadoop or Spark, in order to avoid memory dumps and speed up the processing

## Tech Stack
The following tech stack was used:
1. <strong>ASP.Net C# 8.x</strong> - Web Application
2. <strong>Wiremock</strong> - Mock server for outbound API calls
3. <strong>Postgres</strong> - Relational Database
4. <strong>Redis</strong> - In-Memory Database / Distributed Cache
5. <strong>Docker / Docker-Compose</strong> - Container Runtime Sandbox

The project is structured as a 4-tiered stack:
1. <strong>Application</strong> - Handles routing and http requests/response;
2. <strong>Middleware</strong> - Handles request/response filtering, error handling, and so on;
3. <strong>Service</strong> - Handles core business logic, interfacing with persistence and outbound services and API's
4. <strong>Persistence</strong> - Handles data storage
<img width="213" alt="Captura de Tela 2024-04-02 às 14 22 37" src="https://github.com/gianvittorio/access_management_service/assets/8211552/45112d4b-cea8-4b7a-a505-ba00df208950">

### Important Remarks
1. Microservices architecture was used;
2. API calls to both User and Employer Service are mocked by Wiremock;
3. User data is cached, whereas Eligibility metadata is not, being stored as 'EligibilityMetadata' table in Postgres instead. That just so we do not have to hard-code eligibility file's url in Wiremock mapping files;
4. No TTL was set for the cache at the moment, for simplicity;
5. Despite eligibility file already being buffered as a memory stream, default buffer size for HttpClient was not overridden to 255Mb, but should be fairly easy to;
6. Unit testing for password validation and access management's api calls and caching were skipped due to time constraints, so was integration testing for the whole API

## Bootstrap
The whole application is containerized and can easily spinned by running:
```
docker-compose up -d --build
```
and be gracefully shutdown by:
```
docker-compose down
```

## Testing
There is an eligibiliy file available to download in my personal GDrive:
```
https://docs.google.com/spreadsheets/d/18uwZlO6nq16_RDKSmiIvmGvoUsHCgmBTtNXU5m933-Y/export?exportFormat=csv
```

Signup API usage can be tested with:
```
curl --location 'http://localhost:8080/api/access_management/v1/pub/self_signup' \
--header 'Content-Type: application/json' \
--data-raw '{
    "email": "john.doe@nowhere.net",
    "password": "1L0v3Ch33t0s!",
    "country": "USA",
    "full_name": "John Doe",
    "birth_date": "1990-02-28",
    "employer_name": "some_company",
    "salary": "10000"
}'
```
and Eligibility API with:
```
curl --location 'http://localhost:8080/api/access_management/v1/pub/employer_signup' \
--header 'Content-Type: application/json' \
--data '{
    "file": "https://docs.google.com/spreadsheets/d/18uwZlO6nq16_RDKSmiIvmGvoUsHCgmBTtNXU5m933-Y/export?exportFormat=csv",
    "employer_name": "some_company"
}'
```









