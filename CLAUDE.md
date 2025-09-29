# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BlazorVisu is a .NET Blazor visualization project. This repository is currently in its initial state with only basic configuration files.

## Development Commands

Since this is a new Blazor project, the typical .NET development commands will apply once the project structure is created:

```bash
# Create a new Blazor Server project (if needed)
dotnet new blazorserver -n BlazorVisu

# Create a new Blazor WebAssembly project (if needed)
dotnet new blazorwasm -n BlazorVisu

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the project in development mode
dotnet run

# Run tests
dotnet test

# Publish for production
dotnet publish -c Release
```

## Project Structure

This repository currently contains only:
- README.md - Basic project description
- LICENSE - Project license
- .gitignore - Standard Visual Studio/.NET gitignore configuration

The project appears to be set up for Blazor development based on the name "BlazorVisu" and the Visual Studio gitignore configuration. Once source code is added, the typical Blazor project structure would include:
- Pages/ - Blazor pages and components
- Components/ - Reusable Blazor components  
- Models/ - Data models
- Services/ - Business logic services
- wwwroot/ - Static web assets

## Notes

This is a newly initialized repository. When adding the actual Blazor project files, ensure to follow Blazor conventions for component organization and naming.