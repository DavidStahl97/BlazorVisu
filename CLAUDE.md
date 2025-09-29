# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BlazorVisu is a .NET 8 Blazor Server application using Radzen UI components for visualization. The project uses the latest C# 12 language features and provides a modern web application foundation.

## Development Commands

```bash
# Restore dependencies (solution level)
dotnet restore

# Build the solution
dotnet build

# Run the project in development mode
dotnet run --project BlazorVisu

# Run in watch mode (auto-reload on changes)
dotnet watch run --project BlazorVisu

# Run tests
dotnet test

# Publish for production
dotnet publish BlazorVisu -c Release
```

## Project Structure

- **BlazorVisu.sln** - Visual Studio solution file
- **BlazorVisu/** - Main project directory
  - **Components/** - Blazor components
    - **Layout/** - Layout components (MainLayout.razor)
    - **Pages/** - Page components (Home, Counter, Weather)
  - **Pages/** - Razor pages (_Host.cshtml, _Layout.cshtml)
  - **wwwroot/** - Static web assets (CSS, JS)
  - **Program.cs** - Application entry point with Radzen configuration
  - **App.razor** - Root application component
  - **BlazorVisu.csproj** - Project file

## Technology Stack

- **.NET 8** with C# 12
- **Blazor Server** - Server-side rendering with SignalR
- **Radzen Blazor Components** - UI component library
- **Radzen.Blazor 4.32.0** - Latest Radzen package

## Key Features

- Responsive layout with sidebar navigation using RadzenLayout
- Sample pages demonstrating Radzen components:
  - Home page with cards and buttons
  - Counter with progress visualization
  - Weather data grid with filtering and paging
- Material Design theme from Radzen
- Server-side prerendering for better performance