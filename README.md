# Wordle_Backend
C# Backend for a simple Wordle game

MyApi/
│
├── Controllers/
│   └── TodoController.cs
│
├── Services/
│   ├── Interfaces/
│   │   └── ITodoService.cs
│   └── TodoService.cs
│
├── Models/
│   └── TodoItem.cs
│
├── Dtos/
│   ├── CreateTodoDto.cs
│   └── TodoDto.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Program.cs
├── appsettings.json
└── MyApi.csproj

React
  |
  |  POST /api/todos
  |  JSON (CreateTodoDto)
  v
Controller
  |
  |  kalder service
  v
Service
  |
  |  bruger DbContext
  v
Database

┌─────────┐
│ React   │
│ (JSON)  │
└────┬────┘
     │ CreateTodoDto
     ▼
┌────────────┐
│ Controller │
│  (HTTP)   │
└────┬──────┘
     │
     ▼
┌────────────┐
│  Service   │
│ (Logic)   │
└────┬──────┘
     │
     ▼
┌────────────┐
│ DbContext  │
│ (EF Core) │
└────┬──────┘
     │
     ▼
┌────────────┐
│ Database   │
└────────────┘
