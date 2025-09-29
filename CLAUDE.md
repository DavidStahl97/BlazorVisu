# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BlazorVisu is a .NET 8 Blazor Server application that provides real-time production system visualization using SignalR and Radzen UI components. It simulates a complete production line with machines, switches, and consumers, displaying animated SVG visualizations of component flow and system status.

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

## Application Architecture

### Core Architecture Pattern
- **Blazor Server**: Server-side rendering with SignalR for real-time UI updates
- **Service-Oriented**: Clear separation between business logic (services) and presentation (components)
- **Real-time Communication**: SignalR hubs for live production system updates
- **Component-Based UI**: Modular Blazor components with Radzen UI library

### Key Services

**ProductionService** (`Services/ProductionService.cs`):
- Primary business logic service implementing multiple interfaces:
  - `IProductionService`: Production operations contract
  - `IHostedService`: Background service for continuous updates
  - `ProductionHub`: SignalR hub for real-time client communication
- Simulates production system with automatic updates every 2 seconds
- Generates components every 3 seconds with realistic flow through system
- Handles error simulation (2% chance) with auto-recovery
- Manages component buffers and consumption patterns

### Data Flow Architecture

```
ProductionService (Background Service)
    ↓ (SignalR Hub - "ProductionUpdates" group)
Production.razor (Page Component)
    ↓ (Parameter Binding)
ProductionVisualization.razor (SVG Visualization)
```

### Service Registration Pattern
The application uses a triple registration pattern for the ProductionService:
```csharp
services.AddSingleton<ProductionService>();
services.AddSingleton<IProductionService>(provider => provider.GetRequiredService<ProductionService>());
services.AddHostedService(provider => provider.GetRequiredService<ProductionService>());
```

### Domain Models (`Models/ProductionModels.cs`)

**ProductionSystem**: Main aggregate containing:
- **Machine**: Primary production unit with temperature and efficiency metrics
- **Switch**: Distribution hub routing components to consumers
- **Consumers**: Processing units with buffer management (3 units)
- **ComponentsInTransit**: Real-time tracking of moving components

**Component Types**: TypeA, TypeB, TypeC with color-coded visualization
**Station Status**: Running, Stopped, Error, Maintenance with visual indicators

## Technology Stack

- **.NET 8** with C# 12
- **Blazor Server** with server-side prerendering
- **SignalR** for real-time communication
- **Radzen.Blazor 4.32.0** - Complete UI component library with Material Design theme
- **Serilog.AspNetCore 9.0.0** - Structured logging with file rotation to logs/*.log files
- **Custom SVG animations** for production visualization

## Key Components

### Real-time Visualization
- **ProductionVisualization.razor**: SVG-based animated production flow
- **Interactive Elements**: Clickable stations for detailed information
- **Status Indicators**: Color-coded stations with pulsing animations
- **Component Movement**: Real-time animation of components through system

### SignalR Integration
- Clients automatically join/leave "ProductionUpdates" group
- Proper connection lifecycle management with disposal pattern
- Automatic reconnection handling for robust real-time updates

## Logging Configuration

The application uses Serilog with:
- Console logging for development
- File logging to `logs/blazor-YYYYMMDD.log` next to executable
- Daily rolling files with 7-day retention
- Real-time flushing every second