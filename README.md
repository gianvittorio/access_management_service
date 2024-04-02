# Access Management Service

## Overview
The following project is a simple access management API, that allows users signing in, either as normal or b2b user.

## Assumptions
- Signup API will create a new user if the email is not already registered.

## Data Model
We basically have two entities:
1. User - It stores user personal information;
2. Eligibility metadata - It stores user eligibility information, such as a file that contains employees information;

