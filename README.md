# Biblical Trivia API

The Biblical Trivia API is a C# application that generates trivia questions based on biblical themes and categories. It uses the Gemini API to dynamically create engaging and educational questions.

## Features

- Generate trivia questions with multiple-choice answers.
- Specify categories and difficulty levels for questions.
- Provides explanations and biblical references for each question.

## Dependencies

The project uses the following NuGet packages:

- `AutoGen` - Provides the core functionality for agent-based architecture.
- `AutoGen.Core` - Core components for building and managing agents.
- `AutoGen.Gemini` - Integration with the Gemini API for generating content.
- `Microsoft.AspNetCore.OpenApi` - For API documentation and Swagger integration.
- `Swashbuckle.AspNetCore` - For generating Swagger UI and API documentation.

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/opazenha/csharp-autogen.git
   ```

2. Navigate to the project directory:
   ```bash
   cd BiblicalTriviaApi
   ```

3. Restore the NuGet packages:
   ```bash
   dotnet restore
   ```

### Environment Setup

Set the Gemini API key as an environment variable:
```bash
export GEMINI_API_KEY=your_api_key_here
```

### Running the API

To run the API locally, use the following command:
```bash
dotnet run
```

The API will be available at `https://localhost:5037/api/trivia/question`.
