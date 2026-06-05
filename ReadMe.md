# DotNote

`TODO - give this a better summary once completed`

A desktop note-taking application inspired by Evernote. Users can create notebooks and notes, and edit content using a rich text editor with a custom formatting toolbar.

## Features

`TODO - add to this as I add more features`

* User auth provided through Firebase Authentication
* Create and manage notebooks
* Create, edit, and delete notes
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
User
 1
 └── * Notebooks
        1
        └── * Notes
```

Relationships:

* One User → Many Notebooks
* One Notebook → Many Notes

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

### Azure Speech Configuration

This portfolio project uses a gitignored `appsettings.json` file to store Azure Speech Service configuration values to ensure secrets remain secure.

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
    "ApiKey": "YOUR_KEY_HERE",
    "AuthDomain": "NOT_NEEDED",
    "ProjectId": "NOT_NEEDED",
    "StorageBucket": "NOT_NEEDED",
    "MessagingSenderId": "NOT_NEEDED",
    "AppId": "NOT_NEEDED",
    "MeasurementId": "NOT_NEEDED",
    "DatabaseURL": "YOUR_DB_URL_HERE"
  }
}

```

4. Build and run the application.

## Possible extensions


### Must Do

- Add more modern implicit styles and/or a style resource

- Hide passwords

- Store and retrieve user details (first name, surname, etc.)

- If no note selected, disable the notes tab

### Optional

- Make rename only show textbox for the row that was clicked (probably move that code into a new NotebookVM?)

- Add a edit user option?

- Probably add at least some unit tests

- add a colour picker for the font colour?