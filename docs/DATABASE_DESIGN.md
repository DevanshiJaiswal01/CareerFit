
# CareerFit - Database Design

## 1. Database Overview

CareerFit uses SQLite as its database for storing student account information, skill assessments and career interests.

## 2. Users Table

The Users table stores student registration details.

Fields:
- Id
- Name
- Email
- PasswordSalt
- PasswordHash

The Email field is unique and is used to identify student accounts.

## 3. StudentSkills Table

The StudentSkills table stores the skill assessment ratings submitted by students.

Skill fields:
- Python
- C#
- SQL
- HTML
- JavaScript
- Communication
- Problem Solving

The skill ratings are stored along with the student's email and update information.

## 4. StudentInterests Table

The StudentInterests table stores the career interests selected by students.

Fields:
- Email
- Interests
- UpdatedAt

Selected interests are stored as serialized JSON data.

## 5. Database Operations

CareerFit performs the following database operations:

- Insert student registration details.
- Verify student login details.
- Save and update skill ratings.
- Save and update selected interests.
- Retrieve information for career recommendations.

## 6. Database Purpose

The database connects student information with their skills and interests so that the recommendation system can generate suitable career suggestions.
