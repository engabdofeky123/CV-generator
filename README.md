# AI-Powered CV Generator

An AI-powered CV Generator built with **ASP.NET Core MVC** that helps users create professional, ATS-friendly resumes using AI. The application collects user information, improves CV content based on the provided details, and generates a structured resume that can be exported as a PDF.

## 🚀 Features

- **AI-Powered CV Generation:** Generate professional CV content using AI.
- **ATS-Friendly Resumes:** Improve CV structure and content to make it more suitable for Applicant Tracking Systems (ATS).
- **Smart Content Enhancement:** Rewrite and improve skills, work experience, and project descriptions while preserving the user's original facts.
- **Structured CV Data:** Organize resume information into sections such as personal information, professional summary, skills, experience, projects, education, and certifications.
- **Job Description Optimization:** Provide a target job description to help tailor CV content to a specific role.
- **PDF Export:** Generate downloadable PDF resumes.
- **Responsive User Interface:** Build and edit CVs through a user-friendly interface.
- **AI Response Validation:** Validate and process structured AI responses before using them in the application.
- **Error Handling:** Handle invalid inputs and AI service failures gracefully.

## 🛠️ Technologies Used

- **C#**
- **ASP.NET Core MVC**
- **Razor Views**
- **HTML5 & CSS3**
- **REST APIs**
- **OpenRouter AI API**
- **JSON**
- **Dependency Injection**
- **ViewModels & Service Layer**

## 🏗️ Architecture & Design

The project follows a service-oriented approach within ASP.NET Core MVC to keep responsibilities separated and make the application easier to maintain.

- **Controllers:** Handle HTTP requests and coordinate application workflows.
- **Services:** Encapsulate AI integration, prompt generation, and CV processing.
- **ViewModels:** Transfer and validate data between the UI and application logic.
- **AI Integration:** Send structured prompts to the AI provider and process the returned JSON.
- **PDF Generation:** Convert structured CV data into a downloadable PDF document.
- **Dependency Injection:** Manage service dependencies through the built-in ASP.NET Core container.

## 🔄 How It Works

1. The user enters personal and professional information.
2. The application prepares a structured prompt containing the supplied CV details and, optionally, a target job description.
3. The AI service generates or improves the CV content.
4. The application processes and validates the AI response.
5. The generated content is organized into structured CV sections.
6. The user can export the final CV as a PDF.

## 📄 CV Sections

The generated resume can include:

- Personal Information
- Professional Summary
- Technical and Soft Skills
- Work Experience
- Projects
- Education
- Certifications

## ⚙️ Getting Started

### Prerequisites

- .NET SDK compatible with the project
- Visual Studio 2022 or Visual Studio Code
- An OpenRouter API key
- Internet connection for AI-powered functionality

### Installation

**1. Clone the repository**

```bash
git clone https://github.com/engabdofeky123/CV-Generator.git
```

**2. Navigate to the project directory**

```bash
cd CV-Generator
```

**3. Configure your API key**

Store your OpenRouter API key using .NET User Secrets during development:

```bash
dotnet user-secrets init
dotnet user-secrets set "OpenRouter:ApiKey" "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"
```

Alternatively, configure the key through an environment variable if the application is set up to read it.

**4. Restore dependencies**

```bash
dotnet restore
```

**5. Run the application**

Navigate to the directory containing the MVC project file and execute:

```bash
dotnet run
```

Open the local URL displayed in the terminal.

## 🔐 Security

- Keep API keys and other secrets outside source control.
- Never commit real API keys to GitHub.
- Validate user input before processing it.
- Handle external AI API errors without exposing sensitive configuration details.
- Review AI-generated content before using it in job applications.

## 🎯 Project Goals

This project demonstrates practical experience with:

- Building web applications using ASP.NET Core MVC.
- Integrating external AI services through REST APIs.
- Designing structured prompts for AI-generated content.
- Processing and validating JSON responses.
- Applying separation of concerns through services and ViewModels.
- Generating downloadable PDF documents.
- Handling configuration, errors, and external service dependencies.

## 🔮 Future Improvements

- Add user accounts and saved CV management.
- Support multiple professional CV templates.
- Add DOCX export.
- Improve CV customization for different job descriptions.
- Add automated tests for AI response processing and PDF generation.
- Deploy the application to a public hosting platform.

## 👨‍💻 Author

**Abdelrahman Mohammed**

ASP.NET Backend Developer | Freelancer | Building Scalable & Secure Web Applications

- **GitHub:** [engabdofeky123](https://github.com/engabdofeky123)

---

*Built with ASP.NET Core MVC, C#, and AI to make professional CV creation easier.*
