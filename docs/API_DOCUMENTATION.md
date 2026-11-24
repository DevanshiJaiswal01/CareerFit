
# CareerFit - API Documentation

## 1. Introduction
CareerFit uses ASP.NET Core Web APIs to connect the frontend with the backend and SQLite database.

## 2. Authentication APIs

### Student Registration
- Method: POST
- Endpoint: /api/auth/register
- Purpose: Creates a new student account.

### Student Login
- Method: POST
- Endpoint: /api/auth/login
- Purpose: Verifies student login credentials.

## 3. Student Profile APIs

### Get Student Profile
- Method: GET
- Endpoint: /api/profile?email={email}
- Purpose: Retrieves the student's profile information.

### Update Student Profile
- Method: POST
- Endpoint: /api/profile
- Purpose: Saves updated student profile information.

## 4. Skill Assessment API

### Save Student Skills
- Method: POST
- Endpoint: /api/skills
- Purpose: Saves or updates the student's skill ratings.

Supported skills:
- Python
- C#
- SQL
- HTML
- JavaScript
- Communication
- Problem Solving

## 5. Interests API

### Save Student Interests
- Method: POST
- Endpoint: /api/interests
- Purpose: Saves the career interests selected by the student.

## 6. Career Recommendation API

### Generate Recommendations
- Method: GET
- Endpoint: /api/recommendations?email={email}
- Purpose: Retrieves career recommendations using the student's saved skills and interests.

## 7. API Integration
The frontend communicates with the backend using JavaScript Fetch API requests. ASP.NET Core processes these requests and interacts with the SQLite database.
