# Access Management Service

## Overview
The following project is a simple access management API, that allows users signing in, either as normal or b2b user.

## Assumptions
- Signup API will create a new user if the email is not already registered.

## Data Model
We basically have three entities:
1. User - It stores user personal information;
2. Employee - Which is also a user, adding job/company related information to it;
3. EligibilityMetadata - It stores user eligibility information, such as a file that contains employees information;

<img width="798" alt="Captura de Tela 2024-04-02 às 10 02 05" src="https://github.com/gianvittorio/access_management_service/assets/8211552/b97788c1-d9b4-4fbb-a613-3953f3a8f028">

## API
We essentially have only two API's:
1. Signup - "/api/access_management/v1/pub/self_signup"
2. Eligibility - "/api/access_management/v1/pub/employer_signup"

API contracts are, respectively:

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
    "password",
    "country",
    "full_name",
    "salary"
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
}
```

