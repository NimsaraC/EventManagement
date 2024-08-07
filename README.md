# Event Management System

Welcome to the Event Management System repository! This project is designed to provide a comprehensive solution for managing events, including user authentication, event creation and management, attendee registration, ticket management, event scheduling, analytics and reporting, communication tools, and feedback collection.

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Setup Instructions](#setup-instructions)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Project Overview

The Event Management System is a web application built using ASP.NET Core MVC. It provides a user-friendly platform for organizing and managing events of various scales. The system allows event organizers to create events, manage tickets, track sales, communicate with attendees, and collect feedback post-event.

## Features

- **User Authentication**: Secure registration and login system.
- **Event Creation and Management**: Create, update, and delete events with detailed information.
- **Attendee Registration**: Users can register for events and receive tickets.
- **Ticket Management**: Multiple ticket types, pricing, and availability management.
- **Event Scheduling**: Schedule events with date, time, and venue information.
- **Analytics and Reporting**: Track event performance with sales and attendance analytics.
- **Communication Tools**: Send notifications and updates to attendees.
- **Feedback Collection**: Gather feedback from attendees after the event.

## Technologies Used

- **ASP.NET Core MVC**: The core framework used for building the application.
- **Entity Framework Core**: For database management and ORM.
- **SQL Server**: The database engine used for storing data.
- **Bootstrap**: For responsive and mobile-first design.
- **JavaScript/jQuery**: For client-side scripting and AJAX functionalities.
- **CSS3/HTML5**: For front-end design and structure.

## Setup Instructions

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/NimsaraC/EventManagement.git
   cd EventManagement
   ```

2. **Configure the Database**:
   - Ensure you have SQL Server installed and running.
   - Update the `appsettings.json` file with your SQL Server connection string.

3. **Apply Migrations**:
   ```bash
   dotnet ef database update
   ```

4. **Run the Application**:
   ```bash
   dotnet run
   ```

5. **Access the Application**:
   Open your browser and navigate to `https://localhost:5001` (or the configured URL).

## Usage

- **Event Organizers**: Can create and manage events, track ticket sales, communicate with attendees, and view analytics.
- **Attendees**: Can browse available events, register for them, and receive tickets via email.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature-name`).
3. Commit your changes (`git commit -m 'Add some feature'`).
4. Push to the branch (`git push origin feature/your-feature-name`).
5. Open a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For any questions or inquiries, please contact [NimsaraC](https://github.com/NimsaraC).
