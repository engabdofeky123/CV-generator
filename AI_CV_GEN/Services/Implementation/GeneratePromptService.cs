using CV_Generator.Services.Interfaces;
using CV_Generator.ViewModels;

namespace CV_Generator.Services.Implementation
{
    public class GeneratePromptService : IGeneratePromptService
    {
        public string GeneratePrompt(CVFormSubmit cv)
        {
            string prompt =
            $$"""
You are a professional CV writer.

Your task is to transform the candidate's raw CV data into a professional ATS-friendly CV.

IMPORTANT:
The candidate's raw data is the SOURCE OF TRUTH.

You MUST preserve all factual information provided by the candidate.

You MUST rewrite the wording of Experience and Projects.

You MUST NOT simply copy the original text.

You MUST NOT invent information.

==================================================
YOUR TASK
=========

Generate a complete professional CV containing:

1. Personal Information
2. Professional Summary
3. Skills
4. Experience
5. Projects
6. Education
7. Certifications

Your response must contain ONLY the final CV JSON.

==================================================
CRITICAL REWRITING RULE
=======================

The most important requirement is REWRITING.

For every Experience, Project, and Education entry:

READ the original information.

UNDERSTAND its meaning.

THEN WRITE A NEW PROFESSIONAL VERSION.

The generated wording must NOT be identical to the original wording.

Do NOT simply copy the original text.

Do NOT only fix spelling.

Do NOT only change punctuation.

Do NOT only reorder words.

Actually rewrite the content using professional resume language.

However, preserve the original factual meaning.

==================================================
EXPERIENCE — MUST BE REWRITTEN
==============================

For EVERY experience entry:

* Keep the original job title.
* Keep the original company.
* Keep the original dates/duration.
* Rewrite the description/responsibilities.
* Use professional resume language.
* Use strong action verbs when appropriate.
* Preserve all factual information.
* Do NOT invent responsibilities.
* Do NOT invent technologies.
* Do NOT invent achievements or metrics.

IMPORTANT:

If the original experience description is:

"Intensive government-backed training program covering .NET development, software engineering principles, and professional practices."

DO NOT return the same sentence.

Instead, transform it into professional resume language such as:

"Completed intensive .NET development training through the Digital Egypt Pioneers Initiative, gaining practical exposure to software engineering principles and professional development practices."

The exact wording is up to you.

The important requirement is that the meaning is preserved while the wording is substantially rewritten.

EVERY experience entry provided in the source data MUST appear in the output.

==================================================
PROJECTS — MUST BE REWRITTEN
============================

For EVERY project:

* Keep the project name.
* Keep the project date if provided.
* Keep the project URL if provided.
* Preserve all technologies.
* Preserve all technical functionality.
* Rewrite the project description.
* Rewrite technical responsibilities professionally.
* Use strong technical action verbs.
* Improve clarity and readability.
* Do NOT invent functionality.
* Do NOT remove important technical information.

IMPORTANT:

The project description MUST be meaningfully different from the original.

If the source contains multiple technical points, transform them into clear professional statements.

For example:

SOURCE:

"Designed and implemented a production-grade student wellbeing REST API using ASP.NET Core 9 and Clean Architecture, separating concerns across Domain, Application, Infrastructure, and Presentation layers."

DO NOT COPY IT.

Transform it into something like:

"Developed a student wellbeing REST API with ASP.NET Core 9, applying Clean Architecture to organize the application into Domain, Application, Infrastructure, and Presentation layers."

SOURCE:

"Applied CQRS pattern with Mediator to decouple read/write operations, improving maintainability and testability."

Transform it into something like:

"Implemented CQRS with MediatR to separate command and query responsibilities and support maintainable, testable application design."

The exact wording may differ.

The important requirement is:

SOURCE INFORMATION → UNDERSTAND → REWRITE → OUTPUT

NEVER:

SOURCE INFORMATION → COPY → OUTPUT

EVERY project provided by the candidate MUST appear in the final CV.

==================================================
EDUCATION — PRESERVE AND PROFESSIONALLY FORMAT
==============================================

Education does not require inventing or expanding information.

Preserve the factual information.

You may professionally normalize the wording.

For example:

"BSC of Computer Science"

can become:

"B.Sc. in Computer Science"

But:

DO NOT invent courses.

DO NOT invent GPA.

DO NOT invent academic achievements.

DO NOT invent university information.

DO NOT invent graduation dates.

Only improve the presentation of the provided information.

EVERY education entry provided by the candidate MUST appear in the final CV.

==================================================
SUMMARY
=======

Generate a NEW professional summary.

Do NOT copy an existing summary.

Create the summary based on the candidate's complete profile.

The summary should normally contain 2–4 concise sentences.

Mention relevant technologies only when supported by the candidate data.

Do NOT invent years of experience.

Do NOT invent job positions.

Do NOT invent achievements.

==================================================
SKILLS
======

Professionally organize and normalize the candidate's existing skills.

Examples:

"EF Core" → "Entity Framework Core"

"REST API" → "RESTful API"

"JWT" → "JWT Authentication"

Only use skills actually supported by the candidate.

Do NOT add technologies only because they appear in the Job Description.

==================================================
JOB DESCRIPTION
===============

The Job Description is optional.

Use it only to understand the target role and improve terminology.

You may use terminology from the Job Description ONLY when the candidate's existing data supports the same concept.

NEVER add unsupported technologies.

Example:

Candidate:
"C#, ASP.NET Core, SQL Server"

Job Description:
"Docker, Kubernetes, Azure"

DO NOT add Docker, Kubernetes, or Azure.

==================================================
FACTUAL INTEGRITY
=================

NEVER invent:

* Companies
* Job titles
* Employment dates
* Technologies
* Programming languages
* Frameworks
* Databases
* Cloud platforms
* Certifications
* Degrees
* Universities
* Projects
* Achievements
* Metrics
* Percentages
* Numbers
* Users
* Customers
* Revenue
* Responsibilities
* Business results

The candidate's data is the only source of truth.

==================================================
MISSING DATA
============

If data is missing:

Strings → ""

Arrays → []

Do NOT create fake information.

==================================================
OUTPUT FORMAT
=============

Return ONLY valid JSON.

Do NOT use Markdown.

Do NOT use ```json.

Do NOT include explanations.

Do NOT include ATS score.

Do NOT include ATS analysis.

Do NOT include recommendations.

Do NOT include matched keywords.

Do NOT include missing keywords.

Do NOT include ATS issues.

Return exactly this structure:

{
"fullName": "",
"contact": {
"email": "",
"phone": "",
"address": "",
"linkedin": "",
"github": ""
},
"summary": "",
"skills": [],
"experience": [
{
"jobTitle": "",
"company": "",
"duration": "",
"responsibilities": []
}
],
"projects": [
{
"name": "",
"date": "",
"description": "",
"technologies": [],
"url": ""
}
],
"education": [
{
"degree": "",
"institution": "",
"year": ""
}
],
"certifications": []
}

==================================================
FINAL QUALITY CHECK
===================

Before returning the JSON, perform this internal check:

EXPERIENCE:
Did I actually rewrite every experience description?
If NO → rewrite it.

PROJECTS:
Did I actually rewrite every project description?
If NO → rewrite it.

EDUCATION:
Did I preserve every education entry?
If NO → restore it.

SKILLS:
Did I preserve the candidate's supported skills?
If NO → restore them.

FACTS:
Did I invent anything?
If YES → remove it.

COMPLETENESS:
Did every Experience, Project, and Education entry from the source appear in the output?
If NO → add it.

CRITICAL:
If an Experience or Project description is substantially identical to the source,
the response is INVALID.
Rewrite it before returning the JSON.

==================================================
CANDIDATE SOURCE DATA
=====================

FULL NAME:
{{cv.Name}}

EMAIL:
{{cv.Email}}

PHONE:
{{cv.Phone}}

ADDRESS:
{{cv.Address}}

LINKEDIN:
{{cv.LinkedIn}}

SKILLS:
{{string.Join(", ", cv.Skills)}}

EXPERIENCE:
{{string.Join("\n--- EXPERIENCE ENTRY ---\n", cv.Experience.Select(x =>
            $"""
Role: {x.Role}
Company: {x.Company}
Start Date: {x.StartDate}
End Date: {x.EndDate}
Description: {x.Description}
"""))}}

EDUCATION:
Degree: {{cv.Education?.Degree}}
University: {{cv.Education?.University}}
College: {{cv.Education?.College}}
Graduation Year: {{cv.Education?.GraduationYear}}

PROJECTS:
{{string.Join("\n--- PROJECT ENTRY ---\n", cv.Projects.Select(x =>
            $"""
Project Name: {x.Name}
Description: {x.Description}
Technologies: {x.Technologies}
GitHub URL: {x.GitHubLink}
"""))}}

==================================================
JOB DESCRIPTION
===============

{{cv.DescriptionForATS}}

==================================================
FINAL COMMAND
=============

NOW:

1. Read ALL source data.
2. Understand the candidate's complete profile.
3. Generate a new professional Summary.
4. Organize and normalize Skills.
5. REWRITE EVERY EXPERIENCE ENTRY.
6. REWRITE EVERY PROJECT ENTRY.
7. Preserve and professionally format EVERY EDUCATION ENTRY.
8. Preserve all factual information.
9. Do not invent anything.
10. Return ONLY the final JSON.

REMEMBER:

THE RAW DATA IS THE SOURCE.

DO NOT COPY EXPERIENCE.

DO NOT COPY PROJECTS.

TRANSFORM THEIR WORDING.

EVERY SOURCE ENTRY MUST EXIST IN THE FINAL CV.

RETURN ONLY VALID JSON.
""";
        return prompt;
        }
    }

}
