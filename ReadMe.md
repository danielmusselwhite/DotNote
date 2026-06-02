# DotNote

`TODO - give this a better summary once completed`

A desktop note-taking application inspired by Evernote. Users can create notebooks and notes, and edit content using a rich text editor with a custom formatting toolbar.

## Features

`TODO - add to this as I add more features`

* Create and manage notebooks
* Create, edit, and delete notes
* Rich text editing and formatting
* Local data persistence using SQLite
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

`TODO - update this and its subsections as more technologies are added`

* WPF
* C#
* .NET 10
* SQLite
* Azure Speech Services

### Azure Speech Configuration

This portfolio project uses a gitignored `appsettings.json` file to store Azure Speech Service configuration values to ensure secrets remain secure.

However, in a production environment, sensitive information such as API keys would typically be managed through environment variables, a secret management service, or a backend API. 

For a locally running desktop application intended for demonstration purposes, those approaches were considered outside the scope of this project.

## Running the Application

1. Clone the repository.
2. Create an `appsettings.json` file in the project root.
3. Add your own Azure Speech Service configuration:

```json
{
  "SpeechToText": {
    "SubscriptionKey": "YOUR_KEY_HERE",
    "Region": "YOUR_REGION_HERE"
  }
}
```

4. Build and run the application.

## Possible extensions


### Must Do

- Add more modern styles + central style sheet

- Convert everything into full MVVM as it seems View and ViewModle are mixed rn with bindings to buttons etc. instead of going into the viewmodel

- Could the DisplayNote and DisplayNotebook be done better, eg have the whole thing bound instead of having to set the context within? Check weather App and see

- Add rename and delete options into the Note as well as Notebooks (probs just do after Firebase integration)

### Optional

- Change the Speech to Text to be a toggle button instead of a once off

- Maybe add design time data like in the MVVM app (probably requires binding improved which should be done anyway)

- Make rename only show textbox for the row that was clicked (probably move that code into a new NotebookVM?)
