
# CareerFit - System Architecture

## 1. Introduction
CareerFit follows a three-layer architecture consisting of the presentation layer, application layer and data layer.

## 2. Presentation Layer
The frontend is developed using HTML, CSS and JavaScript.

It provides:
- Student Registration and Login
- Student Dashboard
- Profile Management
- Skill Assessment
- Interest Selection
- Career Recommendation Display

## 3. Application Layer
The backend is developed using C# and ASP.NET Core.

It handles:
- Student authentication
- Profile management
- Skill assessment requests
- Interest management
- Career recommendation processing

## 4. Data Layer
SQLite is used to store and retrieve student information.

The main database tables are:
- Users
- StudentSkills
- StudentInterests

## 5. System Workflow
1. Student registers or logs in.
2. Student completes the profile.
3. Student enters skill ratings.
4. Student selects career interests.
5. Backend retrieves saved information.
6. Recommendation logic processes the information.
7. Frontend displays suitable career options.

## 6. Communication
The frontend communicates with the ASP.NET Core backend through HTTP API requests. The backend performs database operations using SQLite.
