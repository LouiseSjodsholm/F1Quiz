# Formula 1 Quiz Application

The project is a prototype of a web application to host and participate in Formula 1 quizzes, with features for creating events, answering questions, and viewing leaderboards. Built with .NET and ASP.NET Core, this application allows admins to create quiz events and track participant scores, while users can answer questions and view their scores across multiple events. Designed for friends and family, this project demonstrates full-stack development with .NET and serves as a personal project for managing a Formula 1 quiz.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)

## Features
- **Admin**: Create and manage quiz events, add questions, and define correct answers.
- **Participants**: Answer questions for active events.
- **Leaderboards**:
  - **Event Leaderboard**: View participant scores for the latest event.
  - **Total Leaderboard**: View cumulative scores for all participants across all events.
- **User Accounts**: Register, log in, and track scores associated with user accounts.

## Technologies Used
- **Backend**: .NET 8, ASP.NET Core, Entity Framework Core
- **Frontend**: Razor Pages, HTML, CSS, Bootstrap
- **Database**: SQL Server
- **Authentication**: ASP.NET Identity for user registration and login
- **Version Control**: Git, GitHub

## Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/yourrepository.git

2. **Navigate to the project directory**:
   ```bash
   cd F1Quiz
   
3. **Install dependencies**:
   - Ensure you have [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed.
   - Install SQL Server or configure a compatible database.
     
4. **Set up the database**:
   - Update the connection string in `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Your_SQL_Server_Connection_String"
     }
     ```
   - Apply migrations to create the database schema:
     ```bash
     dotnet ef database update
     ```
5. **Run the application**:
      ```bash
   dotnet run

## Usage
### Admin Functions
- Navigate to the "Create Event" section to add a new quiz event with questions.
- Define correct answers after the event is completed to enable automatic score calculation.
- View leaderboards for each event and cumulative scores across all events.

### Participant Functions
- Register an account and log in.
- Answer questions for active events.
- View scores in the Event Leaderboard and Total Leaderboard section.

## Project structure
- **Controllers**: Manages routing and logic for various pages (e.g., `EventAdminController`, `QuizController`).
- **Models**: Defines the data structures (e.g., `Event`, `Question`, `Response`, `Score`).
- **Repositories**: Contains data access logic for models.
- **Views**: Razor pages for UI.
- **wwwroot**: Contains static assets like CSS and images.
