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

- Add more modern styles + central style sheet

- Convert everything into full MVVM as it seems View and ViewModle are mixed rn with bindings to buttons etc. instead of going into the viewmodel

- Could the DisplayNote and DisplayNotebook be done better, eg have the whole thing bound instead of having to set the context within? Check weather App and see

- Hide passwords

- Store user details (first name, etc.)

- If no note selected, hide the notes tab

### Optional

- Change the Speech to Text to be a toggle button instead of a once off

- Maybe add design time data like in the MVVM app (probably requires binding improved which should be done anyway)

- Make rename only show textbox for the row that was clicked (probably move that code into a new NotebookVM?)

- Login VS Register, how could the toggling of visiblity be done better, maybe a state machine and enums for the state or is that over engineering (same thing for Visibility of the text edits for renaming)?

- Add a edit user option?

- Probably add at least some unit tests

- Add an option to use offline vs online mode, where offline mode (after logging in) users the local sqlite db instead

- then have any actions which are done in offline mode (eg all the gets) also update the local sqlite db

- add a colour picker for the font colour?