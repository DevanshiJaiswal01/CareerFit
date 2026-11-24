using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("CareerFitPolicy");

app.UseCors("Frontend");

//string connectionString = "Data Source=careerfit.db";
var dbPath = System.IO.Path.Combine(
    builder.Environment.ContentRootPath,
    "careerfit.db"
);

string connectionString = $"Data Source={dbPath}";
Console.WriteLine("CareerFit Database: " + dbPath);

// Create database and tables
await using (var connection = new SqliteConnection(connectionString))
{
    await connection.OpenAsync();

    var command = connection.CreateCommand();

    command.CommandText = """
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Email TEXT NOT NULL UNIQUE,
            PasswordSalt BLOB NOT NULL,
            PasswordHash BLOB NOT NULL
        );

        CREATE TABLE IF NOT EXISTS StudentSkills (
            Email TEXT PRIMARY KEY,
            Python INTEGER NOT NULL,
            Csharp INTEGER NOT NULL,
            Sql INTEGER NOT NULL,
            Html INTEGER NOT NULL,
            Javascript INTEGER NOT NULL,
            Communication INTEGER NOT NULL,
            ProblemSolving INTEGER NOT NULL,
            UpdatedAt TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS StudentInterests (
            Email TEXT PRIMARY KEY,
            Interests TEXT NOT NULL,
            UpdatedAt TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS StudentProfiles (
            Email TEXT PRIMARY KEY,
            Education TEXT NOT NULL,
            CGPA REAL,
            FOREIGN KEY (Email) REFERENCES Users(Email)
        );
        """;

    

    await command.ExecuteNonQueryAsync();
}


    

// Test API
app.MapGet("/", () => "CareerFit API is running!");

// Student Registration
app.MapPost("/api/auth/register", async (RegisterRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) ||
        string.IsNullOrWhiteSpace(request.Email) ||
        string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new { message = "All fields are required." });
    }

    if (request.Password.Length < 8)
    {
        return Results.BadRequest(new { message = "Password must contain at least 8 characters." });
    }

    string email = request.Email.Trim().ToLowerInvariant();

    await using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var check = connection.CreateCommand();
    check.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = $email";
    check.Parameters.AddWithValue("$email", email);

    long count = Convert.ToInt64(await check.ExecuteScalarAsync());

    if (count > 0)
    {
        return Results.Conflict(new { message = "Email already registered." });
    }

    byte[] salt = RandomNumberGenerator.GetBytes(16);

    byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
        request.Password,
        salt,
        100000,
        HashAlgorithmName.SHA256,
        32);

    var insert = connection.CreateCommand();

    insert.CommandText = """
        INSERT INTO Users (Name, Email, PasswordSalt, PasswordHash)
        VALUES ($name, $email, $salt, $hash)
        """;

    insert.Parameters.AddWithValue("$name", request.Name.Trim());
    insert.Parameters.AddWithValue("$email", email);
    insert.Parameters.AddWithValue("$salt", salt);
    insert.Parameters.AddWithValue("$hash", hash);

    await insert.ExecuteNonQueryAsync();

    return Results.Ok(new { message = "Registration successful!" });
});

// Student Login
app.MapPost("/api/auth/login", async (LoginRequest request) =>
{
    string email = request.Email.Trim().ToLowerInvariant();

    await using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var command = connection.CreateCommand();

    command.CommandText = """

    SELECT Name, PasswordSalt, PasswordHash
    FROM Users
    WHERE Email = $email
    """;

        

        

    command.Parameters.AddWithValue("$email", email);

    await using var reader = await command.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
    {
        return Results.Unauthorized();
    }

    string name = reader.GetString(0);
    byte[] salt = reader.GetFieldValue<byte[]>(1);
    byte[] savedHash = reader.GetFieldValue<byte[]>(2);

    byte[] enteredHash = Rfc2898DeriveBytes.Pbkdf2(
        request.Password,
        salt,
        100000,
        HashAlgorithmName.SHA256,
        32);

    if (!CryptographicOperations.FixedTimeEquals(savedHash, enteredHash))
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new
    {
        message = "Login successful!",
        studentName = name
    });
});

app.MapPost("/api/skills", async (SkillRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest(new { message = "Student email is required." });

    int[] scores =
    {
        request.Python, request.Csharp, request.Sql,
        request.Html, request.Javascript,
        request.Communication, request.ProblemSolving
    };

    if (scores.Any(score => score < 1 || score > 5))
        return Results.BadRequest(new { message = "Skill ratings must be between 1 and 5." });

    await using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var check = connection.CreateCommand();
   check.CommandText = "SELECT COUNT(*) FROM Users WHERE LOWER(TRIM(Email)) = $email";
   // check.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = $email";
    check.Parameters.AddWithValue("$email", request.Email.Trim().ToLowerInvariant());

    long userCount = Convert.ToInt64(await check.ExecuteScalarAsync());

    if (userCount == 0)
        return Results.NotFound(new { message = "Student account not found. Please login again." });

    var save = connection.CreateCommand();

    save.CommandText = """
        INSERT INTO StudentSkills
        (Email, Python, Csharp, Sql, Html, Javascript,
         Communication, ProblemSolving, UpdatedAt)
        VALUES
        ($email, $python, $csharp, $sql, $html, $javascript,
         $communication, $problemSolving, $updatedAt)
        ON CONFLICT(Email) DO UPDATE SET
            Python = excluded.Python,
            Csharp = excluded.Csharp,
            Sql = excluded.Sql,
            Html = excluded.Html,
            Javascript = excluded.Javascript,
            Communication = excluded.Communication,
            ProblemSolving = excluded.ProblemSolving,
            UpdatedAt = excluded.UpdatedAt;
        """;

    save.Parameters.AddWithValue("$email", request.Email.Trim().ToLowerInvariant());
    save.Parameters.AddWithValue("$python", request.Python);
    save.Parameters.AddWithValue("$csharp", request.Csharp);
    save.Parameters.AddWithValue("$sql", request.Sql);
    save.Parameters.AddWithValue("$html", request.Html);
    save.Parameters.AddWithValue("$javascript", request.Javascript);
    save.Parameters.AddWithValue("$communication", request.Communication);
    save.Parameters.AddWithValue("$problemSolving", request.ProblemSolving);
    save.Parameters.AddWithValue("$updatedAt", DateTime.UtcNow.ToString("O"));

await save.ExecuteNonQueryAsync();

var verify = connection.CreateCommand();

verify.CommandText =
    "SELECT Email FROM StudentSkills WHERE Email = $email";

verify.Parameters.AddWithValue(
    "$email",
    request.Email.Trim().ToLowerInvariant()
);

var savedEmail = await verify.ExecuteScalarAsync();

return Results.Ok(new
{
    message = "Skills saved successfully!",
    savedEmail = savedEmail
});
    //await save.ExecuteNonQueryAsync();

    //return Results.Ok(new { message = "Skills saved successfully!" });
});

app.MapPost("/api/interests", async (InterestRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest(new { message = "Student email is required." });

    var interests = request.Interests;

    if (interests == null || interests.Length == 0)
        return Results.BadRequest(new { message = "Please select at least one interest." });

    string[] allowedInterests =
    {
        "Web Development",
        "Data Analysis",
        "Artificial Intelligence",
        "Cyber Security",
        "Software Development",
        "Cloud Computing",
        "Database Management"
    };

    if (interests.Any(i => !allowedInterests.Contains(i)))
        return Results.BadRequest(new { message = "Invalid interest selected." });

    string email = request.Email.Trim().ToLowerInvariant();

    await using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var check = connection.CreateCommand();
    check.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = $email";
    check.Parameters.AddWithValue("$email", email);

    long userCount = Convert.ToInt64(await check.ExecuteScalarAsync());

    if (userCount == 0)
        return Results.NotFound(new { message = "Student account not found." });

    var save = connection.CreateCommand();

    save.CommandText = """
        INSERT INTO StudentInterests (Email, Interests, UpdatedAt)
        VALUES ($email, $interests, $updatedAt)
        ON CONFLICT(Email) DO UPDATE SET
            Interests = excluded.Interests,
            UpdatedAt = excluded.UpdatedAt;
        """;

    save.Parameters.AddWithValue("$email", email);
    save.Parameters.AddWithValue(
        "$interests",
        System.Text.Json.JsonSerializer.Serialize(interests.Distinct())
    );
    save.Parameters.AddWithValue("$updatedAt", DateTime.UtcNow.ToString("O"));

    await save.ExecuteNonQueryAsync();

    return Results.Ok(new { message = "Interests saved successfully!" });
});


app.MapGet("/api/recommendations", async (string email) =>
{
    if (string.IsNullOrWhiteSpace(email))
        return Results.BadRequest(new { message = "Email is required." });

    email = email.Trim().ToLowerInvariant();

    await using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    // Get student skills
    var skillCommand = connection.CreateCommand();

    skillCommand.CommandText = """
        SELECT Python, Csharp, Sql, Html, Javascript,
               Communication, ProblemSolving
        FROM StudentSkills
        WHERE Email = $email
        """;

    skillCommand.Parameters.AddWithValue("$email", email);

    int python, csharp, sql, html, javascript, communication, problemSolving;

    await using (var reader = await skillCommand.ExecuteReaderAsync())
    {
        if (!await reader.ReadAsync())
            return Results.NotFound(new
            {
                message = "Please complete Skill Assessment first."
            });

        python = reader.GetInt32(0);
        csharp = reader.GetInt32(1);
        sql = reader.GetInt32(2);
        html = reader.GetInt32(3);
        javascript = reader.GetInt32(4);
        communication = reader.GetInt32(5);
        problemSolving = reader.GetInt32(6);
    }

    // Get student interests
    var interestCommand = connection.CreateCommand();

    interestCommand.CommandText =
        "SELECT Interests FROM StudentInterests WHERE Email = $email";

    interestCommand.Parameters.AddWithValue("$email", email);

    var interestData = await interestCommand.ExecuteScalarAsync();

    if (interestData == null)
        return Results.NotFound(new
        {
            message = "Please save your interests first."
        });

    var selectedInterests =
        System.Text.Json.JsonSerializer.Deserialize<string[]>(
            Convert.ToString(interestData) ?? "[]"
        ) ?? Array.Empty<string>();

    // Career matching rules
    var careers = new[]
    {
        new
        {
            Name = "Software Developer",
            Description = "Develop software and applications.",
            Interests = new[] { "Software Development" },
            Skills = new[] { csharp, python, javascript, sql, problemSolving }
        },
        new
        {
            Name = "Web Developer",
            Description = "Create and maintain websites.",
            Interests = new[] { "Web Development" },
            Skills = new[] { html, javascript, python, communication, problemSolving }
        },
        new
        {
            Name = "Data Analyst",
            Description = "Analyze data and generate useful insights.",
            Interests = new[] { "Data Analysis", "Database Management" },
            Skills = new[] { python, sql, communication, problemSolving }
        },
        new
        {
            Name = "AI/ML Engineer",
            Description = "Develop intelligent systems using data and AI.",
            Interests = new[] { "Artificial Intelligence", "Data Analysis" },
            Skills = new[] { python, sql, problemSolving }
        },
        new
        {
            Name = "Cybersecurity Analyst",
            Description = "Protect computer systems and networks.",
            Interests = new[] { "Cyber Security" },
            Skills = new[] { python, csharp, sql, problemSolving }
        },
        new
        {
            Name = "Cloud Engineer",
            Description = "Work with cloud platforms and services.",
            Interests = new[] { "Cloud Computing" },
            Skills = new[] { python, csharp, sql, problemSolving }
        },
        new
        {
            Name = "Database Administrator",
            Description = "Manage and maintain databases.",
            Interests = new[] { "Database Management" },
            Skills = new[] { sql, python, csharp, problemSolving }
        }
    };

    // Calculate career matching percentage
   
var recommendations = careers
    .Where(career => career.Interests.Any(
        interest => selectedInterests.Contains(
            interest,
            StringComparer.OrdinalIgnoreCase
        )
    ))
    .Select(career =>
    {
        int skillScore = (int)Math.Round(
            career.Skills.Average() / 5.0 * 50
        );

        return new
        {
            career = career.Name,
            description = career.Description,
            matchPercentage = 50 + skillScore
        };
    })
    .OrderByDescending(career => career.matchPercentage)
    .Take(3)
    .ToList();

var skillRatings = new[]
{
    new { name = "Python", score = python },
    new { name = "C#", score = csharp },
    new { name = "SQL", score = sql },
    new { name = "HTML / CSS", score = html },
    new { name = "JavaScript", score = javascript },
    new { name = "Communication", score = communication },
    new { name = "Problem Solving", score = problemSolving }
};

var strongSkills = skillRatings
    .Where(skill => skill.score >= 4)
    .Select(skill => skill.name)
    .ToArray();

var weakSkills = skillRatings
    .Where(skill => skill.score <= 2)
    .Select(skill => skill.name)
    .ToArray();

return Results.Ok(new
{
    message = "Career recommendations generated successfully!",
    recommendations = recommendations,
    strongSkills = strongSkills,
    weakSkills = weakSkills,
    interests = selectedInterests
});
    
});

// Get Student Profile
app.MapGet("/api/profile", async (string email) =>
{
    if (string.IsNullOrWhiteSpace(email))
        return Results.BadRequest(new { message = "Email is required." });

    email = email.Trim().ToLowerInvariant();

    await using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var command = connection.CreateCommand();

    command.CommandText = """
        SELECT Users.Name, Users.Email,
               StudentProfiles.Education, StudentProfiles.CGPA
        FROM Users
        LEFT JOIN StudentProfiles
        ON Users.Email = StudentProfiles.Email
        WHERE Users.Email = $email
        """;

    command.Parameters.AddWithValue("$email", email);

    await using var reader = await command.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
        return Results.NotFound(new { message = "Student not found." });

    return Results.Ok(new
    {
        name = reader.GetString(0),
        email = reader.GetString(1),
        education = reader.IsDBNull(2) ? "" : reader.GetString(2),
        cgpa = reader.IsDBNull(3) ? (double?)null : Convert.ToDouble(reader.GetValue(3))
    });
});


// Update Student Profile
app.MapPost("/api/profile", async (ProfileRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) ||
        string.IsNullOrWhiteSpace(request.Email) ||
        string.IsNullOrWhiteSpace(request.Education))
    {
        return Results.BadRequest(new { message = "All required fields must be filled." });
    }

    if (request.CGPA.HasValue &&
        (request.CGPA.Value < 0 || request.CGPA.Value > 10))
    {
        return Results.BadRequest(new { message = "CGPA must be between 0 and 10." });
    }

    string email = request.Email.Trim().ToLowerInvariant();

    await using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var updateUser = connection.CreateCommand();

    updateUser.CommandText =
        "UPDATE Users SET Name = $name WHERE Email = $email";

    updateUser.Parameters.AddWithValue("$name", request.Name.Trim());
    updateUser.Parameters.AddWithValue("$email", email);

    int updated = await updateUser.ExecuteNonQueryAsync();

    if (updated == 0)
        return Results.NotFound(new { message = "Student account not found." });

    var save = connection.CreateCommand();

    save.CommandText = """
        INSERT INTO StudentProfiles (Email, Education, CGPA)
        VALUES ($email, $education, $cgpa)
        ON CONFLICT(Email) DO UPDATE SET
            Education = excluded.Education,
            CGPA = excluded.CGPA;
        """;

    save.Parameters.AddWithValue("$email", email);
    save.Parameters.AddWithValue("$education", request.Education);
    save.Parameters.AddWithValue(
        "$cgpa",
        (object?)request.CGPA ?? DBNull.Value
    );

    await save.ExecuteNonQueryAsync();

    return Results.Ok(new { message = "Profile updated successfully!" });
});


app.Run();

public record RegisterRequest(string Name, string Email, string Password);

public record LoginRequest(string Email, string Password);

public record SkillRequest(
    string Email,
    int Python,
    int Csharp,
    int Sql,
    int Html,
    int Javascript,
    int Communication,
    int ProblemSolving
);
public record InterestRequest(
    string Email,
    string[]? Interests
);
public record ProfileRequest(
    string Name,
    string Email,
    string Education,
    double? CGPA
);