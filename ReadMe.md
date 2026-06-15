# DotNote

A desktop note-taking application inspired by Evernote. Users can create notebooks and notes, and edit content using a rich text editor with a custom formatting toolbar.

## Features

* User auth provided through Firebase Authentication (register, login, reset password)
* Create and manage notebooks
* Create, edit, and delete notes
* Create, and edit User Profile
* Rich text editing and formatting
* Remote data persistence using Firebase Realtime Database
* Remote file persistence using Azure Blob Storage
* Speech-to-text integration using Azure Speech Services

## Screenshots

`TODO - add here once app is finished`

## Architecture

### ERD

`TODO - put a high level ERD here`

### Data Model

```text
User 1 - 1 UserDetails
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

The application follows the MVVM (Model-View-ViewModel) architectural pattern, providing separation of concerns between the UI, business logic, and data layers.

## Technologies

* WPF
* C#
* .NET 10
* SQLite
* Blend Animations
* Firebase Auth
* Firebase Realtime Data Storage
* Azure Speech Services
* Azure Blob Storage
* Dependency Injection
* AutoMapper

### Azure Speech Configuration

This portfolio project uses a gitignored `appsettings.json` file to store (Azure and Firebase) configuration values to ensure secrets remain secure.

However, in a production environment, sensitive information such as API keys would typically be managed through environment variables, a secret management service, or a backend API. 

For a locally running desktop application intended for demonstration purposes, those approaches were considered outside the scope of this project.

## Running the Application

1. Clone the repository.
2. Create an `appsettings.json` file in the project root.
3. Add your own Azure Speech Service configuration and Firebase WebApp configuration:

```json
{
  "SpeechToText": {
    "SubscriptionKey": "YOUR_KEY_HERE",
    "Region": "YOUR_REGION_HERE"
  },
  "Firebase": {
    "ApiKey": "YOUR_WEBAPP_KEY_HERE",
    "DatabaseURL": "YOUR_DB_URL_HERE"
  }
}

```

4. Build and run the application.

## Possible extensions

### Must Do

- nothing atm

### Optional
- add a colour picker for the font colour?

- maybe add profile picture upload in the profile view
  - and display a mini icon of that profile picture in the toolbar?