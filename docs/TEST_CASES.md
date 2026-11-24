
# CareerFit - Software Testing and Test Cases

## 1. Introduction
This document describes the test cases used to check the main functionalities of the CareerFit application.

## 2. Functional Test Cases

| Test ID | Module | Test Description | Expected Result |
|---|---|---|---|
| TC01 | Registration | Register with valid details | Account is created |
| TC02 | Registration | Submit incomplete details | Validation message appears |
| TC03 | Login | Login with valid credentials | Student dashboard opens |
| TC04 | Login | Enter invalid credentials | Error message appears |
| TC05 | Profile | Update student profile | Profile information is saved |
| TC06 | Skill Assessment | Submit skill ratings | Skill ratings are saved |
| TC07 | Interests | Select and save interests | Selected interests are saved |
| TC08 | Recommendations | Request career recommendations | Suitable careers are displayed |

## 3. Integration Testing
Integration testing checks communication between:
- HTML, CSS and JavaScript frontend.
- ASP.NET Core backend APIs.
- SQLite database.

## 4. Expected Testing Outcome
The application should accept valid student information, store assessment data and generate career recommendations based on saved skills and interests.

## 5. Conclusion
Testing helps verify the functionality and reliability of the CareerFit application.
