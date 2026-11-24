/* =========================
REGISTRATION
========================= */

const registerForm = document.getElementById("registerForm");

if (registerForm) {


registerForm.addEventListener("submit", function(event) {

    event.preventDefault();

    const name =
        document.getElementById("name").value;

    const email =
        document.getElementById("email").value;

    const password =
        document.getElementById("password").value;

    const education =
        document.getElementById("education").value;

    const cgpa =
        document.getElementById("cgpa").value;


    if (
        name === "" ||
        email === "" ||
        password === "" ||
        education === ""
    ) {

        alert("Please fill all required fields.");

        return;
    }


    alert(
        "Registration successful!\n\n" +
        "Welcome " + name
    );

    window.location.href = "login.html";

});


}

/* =========================
LOGIN
========================= */

const loginForm = document.getElementById("loginForm");

if (loginForm) {







}
/* =========================
SKILL ASSESSMENT
========================= */

const skillForm = document.getElementById("skillForm");

if (skillForm) {





}

/* =========================
MY INTERESTS
========================= */

const interestForm =
document.getElementById("interestForm");

if (interestForm) {

// Load previously selected interests

const savedInterests =
    JSON.parse(
        localStorage.getItem("careerInterests")
    ) || [];


// Automatically check saved interests

const checkboxes =
    document.querySelectorAll(
        'input[name="interest"]'
    );


checkboxes.forEach(function(checkbox) {

    if (savedInterests.includes(checkbox.value)) {

        checkbox.checked = true;

    }

});


// Save interests

interestForm.addEventListener(
    "submit",
    function(event) {

        event.preventDefault();


        const selected =
            document.querySelectorAll(
                'input[name="interest"]:checked'
            );


        const error =
            document.getElementById("interestError");


        if (selected.length === 0) {

            error.textContent =
                "Please select at least one interest.";

            return;

        }


        const interests = [];


        selected.forEach(function(item) {

            interests.push(item.value);

        });


        // Save to browser

        localStorage.setItem(
            "careerInterests",
            JSON.stringify(interests)
        );


        error.textContent = "";


        alert(
            "Your interests have been saved successfully!"
        );


        // Stay on My Interests page

    });


}


      

/* =========================
RECOMMENDATION PAGE
========================= */

/* =========================
CAREER RECOMMENDATION
========================= */

const careerName =
document.getElementById("careerName");

if (careerName) {


// Get saved skills

const python =
    Number(localStorage.getItem("python")) || 0;

const csharp =
    Number(localStorage.getItem("csharp")) || 0;

const sql =
    Number(localStorage.getItem("sql")) || 0;

const html =
    Number(localStorage.getItem("html")) || 0;

const javascript =
    Number(localStorage.getItem("javascript")) || 0;

const communication =
    Number(localStorage.getItem("communication")) || 0;

const problemSolving =
    Number(localStorage.getItem("problemSolving")) || 0;


// Get interests

const savedInterests =
    JSON.parse(
        localStorage.getItem("careerInterests")
    ) || [];


/*
   Calculate career scores
*/


// Data Analyst

const dataAnalystScore =
    (
        python +
        sql +
        communication
    ) / 15 * 100;


// Software Developer

const softwareDeveloperScore =
    (
        csharp +
        sql +
        problemSolving
    ) / 15 * 100;


// Web Developer

const webDeveloperScore =
    (
        html +
        javascript +
        communication
    ) / 15 * 100;


// AI

const aiScore =
    (
        python +
        problemSolving +
        sql
    ) / 15 * 100;


/*
   Interest bonus
*/

let finalDataScore =
    dataAnalystScore;

let finalSoftwareScore =
    softwareDeveloperScore;

let finalWebScore =
    webDeveloperScore;

let finalAIScore =
    aiScore;


if (savedInterests.includes("Data Analysis")) {

    finalDataScore += 10;

}


if (savedInterests.includes("Software Development")) {

    finalSoftwareScore += 10;

}


if (savedInterests.includes("Web Development")) {

    finalWebScore += 10;

}


if (savedInterests.includes("Artificial Intelligence")) {

    finalAIScore += 10;

}


/*
   Find highest score
*/

const careers = [

    {
        name: "Data Analyst",
        score: finalDataScore
    },

    {
        name: "Software Developer",
        score: finalSoftwareScore
    },

    {
        name: "Web Developer",
        score: finalWebScore
    },

    {
        name: "AI Engineer",
        score: finalAIScore
    }

];


careers.sort(
    (a, b) => b.score - a.score
);


const bestCareer = careers[0];


/*
   Display result
*/

document.getElementById("careerName")
    .textContent = bestCareer.name;


document.getElementById("matchScore")
    .textContent =
    Math.min(
        Math.round(bestCareer.score),
        100
    ) + "%";


/*
   Strong skills
*/

const skills = [

    {
        name: "Python",
        value: python
    },

    {
        name: "C#",
        value: csharp
    },

    {
        name: "SQL",
        value: sql
    },

    {
        name: "HTML/CSS",
        value: html
    },

    {
        name: "JavaScript",
        value: javascript
    },

    {
        name: "Communication",
        value: communication
    },

    {
        name: "Problem Solving",
        value: problemSolving
    }

];


const strongSkills =
    skills.filter(
        skill => skill.value >= 4
    );


const weakSkills =
    skills.filter(
        skill => skill.value < 4
    );


const strongContainer =
    document.getElementById("strongSkills");


const weakContainer =
    document.getElementById("weakSkills");


strongContainer.innerHTML =
    strongSkills.length > 0
    ? strongSkills.map(
        skill =>
            `<span class="skill-tag">
                ✓ ${skill.name}
            </span>`
      ).join("")
    : "<p>No strong skills found yet.";


weakContainer.innerHTML =
    weakSkills.length > 0
    ? weakSkills.map(
        skill =>
            `<span class="weak-tag">
                ⚠ ${skill.name}
            </span>`
      ).join("")
    : "<p>Great! Your skills are well balanced.";


/*
   Display interests
*/

const interestContainer =
    document.getElementById("userInterests");


if (savedInterests.length > 0) {

    interestContainer.innerHTML =
        savedInterests.map(
            interest =>
                `<span class="interest-tag">
                    ❤️ ${interest}
                </span>`
        ).join("");

} else {

    interestContainer.innerHTML =
        "<p>No interests selected.</p>";

}


}

/* Dashboard button */

function goToDashboard() {


window.location.href =
    "dashboard.html";


}


// CareerFit Registration

document.addEventListener("DOMContentLoaded", function () {

    const registerForm = document.getElementById("registerForm");

    if (!registerForm) return;

    registerForm.addEventListener("submit", async function (event) {

        event.preventDefault();

        const name = document.getElementById("name").value;
        const email = document.getElementById("email").value;
        const password = document.getElementById("password").value;

        const message = document.getElementById("registerMessage");

        message.textContent = "Creating your account...";

        try {

            const response = await fetch(
                "http://localhost:5001/api/auth/register",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        name: name,
                        email: email,
                        password: password
                    })
                }
            );

            const result = await response.json();
           // alert("Database saved email: " + result.savedEmail);
           //alert(JSON.stringify(result));

            if (!response.ok) {
                message.style.color = "red";
                message.textContent = result.message || "Registration failed.";
                return;
            }

            message.style.color = "green";
            message.textContent = "Registration successful!";

            registerForm.reset();

        } catch (error) {

            message.style.color = "red";
            message.textContent = "Cannot connect to CareerFit server.";

            console.error(error);
        }

    });

});

// CareerFit Login

document.addEventListener("DOMContentLoaded", function () {

    const loginForm = document.getElementById("loginForm");

    if (!loginForm) return;

    loginForm.addEventListener("submit", async function (event) {

        event.preventDefault();

        const email = document.getElementById("loginEmail").value;
        const password = document.getElementById("loginPassword").value;
        const message = document.getElementById("loginMessage");

        message.textContent = "Checking login details...";

        try {

            const response = await fetch(
                "http://localhost:5001/api/auth/login",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        email: email,
                        password: password
                    })
                }
            );

            if (response.status === 401) {
                message.style.color = "red";
                message.textContent = "Invalid email or password.";
                return;
            }

            const result = await response.json();

            if (!response.ok) {
                message.style.color = "red";
                message.textContent = "Login failed.";
                return;
            }

            localStorage.setItem("studentName", result.studentName);
         localStorage.setItem("studentEmail", email);

            message.style.color = "green";
            message.textContent = "Login successful!";

            setTimeout(function () {
                window.location.href = "dashboard.html";
            }, 1000);

        } catch (error) {

            message.style.color = "red";
            message.textContent = "Cannot connect to CareerFit server.";

            console.error(error);
        }

    });
});

// CareerFit Skill Assessment

document.addEventListener("DOMContentLoaded", function () {

    const skillForm = document.getElementById("skillForm");

    if (!skillForm) return;
    console.log("SKILLS FORM FOUND");

    skillForm.addEventListener("submit", async function (event) {

        event.preventDefault();
        console.log("SKILLS SUBMIT FIRED");

        const email = localStorage.getItem("studentEmail");
     //alert("Email used for Skills: " + email);
     
        const message = document.getElementById("skillMessage");

        document.body.appendChild(message);

Object.assign(message.style, {
    display: "block",
    visibility: "visible",
    opacity: "1",
    position: "fixed",
    bottom: "20px",
    left: "50%",
    transform: "translateX(-50%)",
    zIndex: "999999",
    background: "white",
    padding: "12px 20px",
    borderRadius: "8px"
});
        if (!email) {
            message.textContent = "Please login again before saving your skills.";
            message.style.color = "red";
            return;
        }

        const skills = {
            email: email,
            python: Number(document.getElementById("python").value),
            csharp: Number(document.getElementById("csharp").value),
            sql: Number(document.getElementById("sql").value),
            html: Number(document.getElementById("html").value),
            javascript: Number(document.getElementById("javascript").value),
            communication: Number(document.getElementById("communication").value),
            problemSolving: Number(document.getElementById("problemSolving").value)
        };
//console.log("Email sent with skills:", skills.email);
        message.textContent = "Saving your skills...";

        try {
            const response = await fetch(
                "http://localhost:5001/api/skills",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(skills)
                }
            );

            const result = await response.json();
            //alert(JSON.stringify(result));

            
if (!response.ok) {
    throw new Error(result.message || "Could not save skills.");
}

message.style.display = "block";
message.style.visibility = "visible";
message.style.color = "green";
message.textContent = "Skills saved successfully!";

} catch (error) {
    message.style.display = "block";
    message.style.visibility = "visible";
    message.style.color = "red";
    message.textContent = error.message;
    console.error("Skill saving error:", error);
}

    });

});



document.addEventListener("DOMContentLoaded", async function () {

    const careerName = document.getElementById("careerName");

    if (!careerName) return;

    const email = localStorage.getItem("studentEmail");
    console.log("Email used for recommendations:", email);
    const matchScore = document.getElementById("matchScore");

    if (!email) {
        careerName.textContent = "Please login again";
        return;
    }

    try {

        const response = await fetch(
            "http://localhost:5001/api/recommendations?email=" +
            encodeURIComponent(email)
        );

        const result = await response.json();

        if (!response.ok) {
            careerName.textContent =
                result.message || "Unable to generate recommendations.";
            return;
        }

        const recommendations = result.recommendations;

        if (!recommendations || recommendations.length === 0) {
            careerName.textContent = "No recommendations found";
            return;
        }

        // Display the best career match
        const bestCareer = recommendations[0];

        careerName.textContent = bestCareer.career;
        matchScore.textContent =
            bestCareer.matchPercentage + "%";

        // Display skills and interests
        function showItems(id, items, emptyMessage) {

            const container = document.getElementById(id);

            if (!container) return;

            container.replaceChildren();

            if (!items || items.length === 0) {
                container.textContent = emptyMessage;
                return;
            }

            items.forEach(item => {
                const span = document.createElement("span");
                span.textContent = item;
                container.appendChild(span);
            });
        }

        showItems("strongSkills", result.strongSkills,
            "No strong skills identified yet.");

        showItems("weakSkills", result.weakSkills,
            "No skills to improve identified.");

        showItems("userInterests", result.interests,
            "No interests selected.");

        // Display other career recommendations
        const otherCareerList =
            document.getElementById("otherCareerList");

        if (otherCareerList) {

            otherCareerList.replaceChildren();

            recommendations.slice(1).forEach(career => {

                const card = document.createElement("div");

                const heading = document.createElement("h4");
                heading.textContent = career.career;

                const description = document.createElement("p");
                description.textContent = career.description;

                const percentage = document.createElement("p");
                percentage.textContent =
                    "Match: " + career.matchPercentage + "%";

                card.append(heading, description, percentage);
                otherCareerList.appendChild(card);

            });
        }

    } catch (error) {

        careerName.textContent = "Unable to connect to server.";
        console.error(error);

    }

});

// CareerFit Profile

document.addEventListener("DOMContentLoaded", async function () {

    const profileForm = document.getElementById("profileForm");

    if (!profileForm) return;

    const email = localStorage.getItem("studentEmail");
    const message = document.getElementById("profileMessage");

    if (!email) {
        message.textContent = "Please login again.";
        message.style.color = "red";
        return;
    }

    document.getElementById("profileEmail").value = email;

    // Load existing profile details
    try {

        const response = await fetch(
            "http://localhost:5001/api/profile?email=" +
            encodeURIComponent(email)
        );

        if (response.ok) {

            const profile = await response.json();

            document.getElementById("profileName").value = profile.name;
            document.getElementById("profileEducation").value =
                profile.education || "";

            document.getElementById("profileCgpa").value =
                profile.cgpa ?? "";

        }

    } catch (error) {
        console.error("Profile loading error:", error);
    }

    // Update profile
    profileForm.addEventListener("submit", async function (event) {

        event.preventDefault();

        const name = document.getElementById("profileName").value.trim();
        const education = document.getElementById("profileEducation").value;
        const cgpaInput = document.getElementById("profileCgpa").value;

        const profileData = {
            name: name,
            email: email,
            education: education,
            cgpa: cgpaInput === "" ? null : Number(cgpaInput)
        };

        message.textContent = "Updating profile...";

        try {

            const response = await fetch(
                "http://localhost:5001/api/profile",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(profileData)
                }
            );

            const result = await response.json();

            if (!response.ok) {
                throw new Error(result.message || "Profile update failed.");
            }

            localStorage.setItem("studentName", name);

            message.style.color = "green";
            message.textContent = "Profile updated successfully!";

        } catch (error) {

            message.style.color = "red";
            message.textContent = error.message;

            console.error(error);
        }

    });


});