# CollabNotes – Real-Time Collaboration Tool

A real-time collaboration tool for shared notes with **user accounts**, **private notes**, and an **invitation system**, built with **ASP.NET Core**, **SignalR**, **CQRS Pattern**, and **Clean Architecture**.

🌐 **Portfolio & Docs:** [https://alexltz0.github.io/CollabNotes/](https://alexltz0.github.io/CollabNotes/)

## Architecture

```
┌──────────────────────────────────────────────────┐
│                   API Layer                       │
│  AuthController · NotesController                 │
│  ExceptionHandlingMiddleware · wwwroot            │
├──────────────────────────────────────────────────┤
│              Application Layer                    │
│  Auth Commands (Register/Login)                   │
│  Note Commands (CRUD + Collaborator Management)   │
│  Queries (Own/Shared Notes, User Search)          │
│  DTOs · Behaviors · Validators · Mappings         │
├──────────────────────────────────────────────────┤
│             Infrastructure Layer                  │
│  EF Core InMemory (Notes + Collaborators)         │
│  JSON User Repository (App_Data/users.json)       │
│  SignalR Hub · RealtimeNotifier                   │
├──────────────────────────────────────────────────┤
│               Domain Layer                        │
│  Entities: Note · User · NoteCollaborator         │
│  Domain Events · Interfaces · Base Classes        │
└──────────────────────────────────────────────────┘
```

## Tech Stack

- **.NET 10** (ASP.NET Core)
- **SignalR** – Real-time communication (WebSockets)
- **MediatR** – CQRS Command/Query Dispatching
- **FluentValidation** – Input validation (Pipeline Behavior)
- **EF Core InMemory** – Note storage (easily swappable)
- **JSON File** – Persistent user storage (`App_Data/users.json`)
- **Vanilla JS Frontend** – Zero framework overhead

## Data Storage

| Data | Location | Persistent? |
|------|----------|-------------|
| User Accounts | `App_Data/users.json` | Yes |
| Notes | EF Core InMemory | No (lost on restart) |
| Collaborator Assignments | EF Core InMemory | No (lost on restart) |

## Authentication

- Registration and login with username + password
- Passwords are hashed with SHA256
- User session via `X-User-Id` header + localStorage
- Automatic re-login when reopening the app

## CQRS Pattern

### Commands (Write Operations)
- `RegisterCommand` → Register a new user
- `LoginCommand` → Authenticate user
- `CreateNoteCommand` → Create a new note (with OwnerId)
- `UpdateNoteContentCommand` → Update content (with access check)
- `UpdateNoteTitleCommand` → Update title (with access check)
- `DeleteNoteCommand` → Delete note (owner only)
- `AddCollaboratorCommand` → Invite collaborator by username (owner only)
- `RemoveCollaboratorCommand` → Remove collaborator (owner only)

### Queries (Read Operations)
- `GetMyNotesQuery` → Fetch own notes
- `GetSharedNotesQuery` → Fetch notes shared with me
- `GetNoteByIdQuery` → Fetch single note (with access check)
- `SearchUsersQuery` → Search users

## SignalR Events

| Event | Direction | Description |
|-------|-----------|-------------|
| `NoteCreated` | Server → All | A new note was created |
| `NoteUpdated` | Server → Group | Note was updated |
| `NoteDeleted` | Server → All | Note was deleted |
| `UserJoined` | Server → Group | User joined a note |
| `UserLeft` | Server → Group | User left a note |
| `ContentEdited` | Client ↔ Group | Real-time text changes |
| `TitleEdited` | Client ↔ Group | Real-time title changes |
| `CollaboratorAdded` | Server → User | Invitation to a note |
| `CollaboratorRemoved` | Server → User | Access to note revoked |

## API Endpoints

### Auth
| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login |
| GET | `/api/auth/search?q=` | Search users |

### Notes
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/notes/my` | Fetch own notes |
| GET | `/api/notes/shared` | Fetch shared notes |
| GET | `/api/notes/{id}` | Fetch single note |
| POST | `/api/notes` | Create a new note |
| PUT | `/api/notes/{id}/content` | Update content |
| PUT | `/api/notes/{id}/title` | Update title |
| DELETE | `/api/notes/{id}` | Delete note (owner only) |
| POST | `/api/notes/{id}/collaborators` | Invite collaborator |
| DELETE | `/api/notes/{id}/collaborators/{userId}` | Remove collaborator |

## Getting Started

```bash
dotnet run --project src/CollabNotes.API --urls "http://localhost:5050"
```

Then open in browser: `http://localhost:5050`

## Features

- **User Accounts** – Registration and login with JSON-based persistence
- **Private Notes** – Notes are private by default, visible only to the creator
- **Invitation System** – Invite other users to notes by username
- **Tabbed UI** – Separation between "My Notes" and "Shared with Me"
- **Real-Time Collaboration** – Changes are instantly synced to all collaborators
- **Live Notifications** – Invitations and access revocations appear in real-time
- **Active User Display** – Per-note with colored avatars
- **Typing Indicator** ("User is typing...")
- **Auto-Save** (Debounced, 800ms)
- **Access Control** – Owner-only for delete, invite, and remove
- **CQRS with MediatR** Pipeline + Validation Behavior
- **Error Handling** via Exception Middleware
- **Clean Architecture** with strict Dependency Rule
