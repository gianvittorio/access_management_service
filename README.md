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


