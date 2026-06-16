# DotNote

DotNote is a desktop note-taking application inspired by Evernote. Users can register an account and create notebooks and notes using a rich text editor with a custom formatting toolbar, allowing control over font type, size, colour, and styling.

Notes and user files are stored remotely using Azure Blob Storage, while application data is persisted in Firebase Realtime Database. Access to notes is secured so that only the creating user can view and manage their content.

Users can also create a profile and upload a profile picture, which is stored in Azure Blob Storage.

---

## Features

- User authentication via Firebase Authentication (register, login, password reset)
- Create and manage notebooks
- Create, edit, and delete notes
- User profile creation and editing
  - Profile image upload
- Rich text editor with formatting tools (fonts, size, colour, styling)
- Cloud data persistence using Firebase Realtime Database
- File storage using Azure Blob Storage
- Speech-to-text integration via Azure Speech Services

---

## Screenshots

`TODO - add here once app is finished`

## Architecture

### ERD

`TODO - put a high level ERD here`

### Data Model

```text
User 1 ── 1 UserDetails
 1
 └── * Notebooks
        1
        └── * Notes
```

Relationships:

* One User → Many Notebooks
* One Notebook → Many Notes
* One User → One UserDetails

### Design Pattern

The application follows the MVVM (Model-View-ViewModel) architectural pattern, ensuring a clear separation of concerns between UI, business logic, and data access layers.

## Technologies
* Frontend / UI
  * WPF
  * Blend Animations
* Language & Framework
  * C#
  * .NET 10
* Architecture & Patterns
  * MVVM
  * Dependency Injection
  * AutoMapper
* Authentication Services
  * Firebase Authentication
* Database Services
  * Firebase Realtime Database
* Cloud Storage Services
  * Azure Blob Storage
* Other Backend Services
  * Azure Speech Services

## Configuration

This project uses a gitignored appsettings.json file to store Azure and Firebase configuration values locally, ensuring that sensitive credentials are not committed to source control.

In production systems, these values would typically be managed using environment variables, secure secret storage services, or a dedicated backend API. However, for a locally running desktop application intended for demonstration purposes, those approaches were considered outside the scope of this project. However, if this project were to scale, I would create a dedicated backend API that the WPF would communicate with, instead of interacting with the database and blob storage directly.

## Running the Application
1. Clone the repository.
2. Create an appsettings.json file in the project root.
3. Add your Azure Speech Service, Firebase, and Azure Storage configuration:

```json
{
  "SpeechToText": {
    "SubscriptionKey": "YOUR_KEY_HERE",
    "Region": "YOUR_REGION_HERE"
  },
  "Firebase": {
    "ApiKey": "YOUR_WEBAPP_KEY_HERE",
    "DatabaseURL": "YOUR_DB_URL_HERE"
  },
  "AzureStorage": {
    "ConnectionString": "YOUR_CONNECTIONSTRING_HERE",
    "NotesContainerName": "notes",
    "UserPhotosContainerName": "userphotos"
  }
}
```

4. Build and run the application.