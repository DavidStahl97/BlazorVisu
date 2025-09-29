# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BlazorVisu is a .NET 8 Blazor Server application providing static production system visualization with selective transport control. The application displays a configurable production topology using SVG animations and allows users to control which transport routes are active through a dedicated control interface.

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

# Publish for production
dotnet publish BlazorVisu -c Release
```

## Application Architecture

### Core Architecture Pattern
- **Static Visualization**: Minimal Blazor Server application focused purely on visualization
- **JSON-Configured Topology**: Production system layout defined in external configuration file
- **Event-Driven Transport Control**: Real-time selective route animation through service events
- **Fullscreen SVG Display**: Immersive production visualization without UI chrome

### Key Services

**ProductionService** (`Services/ProductionService.cs`):
- Minimal service providing only `GetCurrentState()` for static data
- Loads production topology from `wwwroot/production-config.json`
- Manages active transport routes with `SetTransportRoute()` and `ClearTransportRoute()`
- Emits `TransportRouteChanged` events for real-time visualization updates
- Includes programmatic fallback if JSON config is missing

### Configuration-Driven Architecture

The production system topology is entirely configurable through `production-config.json`:
- **Machine**: Single production source with position coordinates
- **Switches**: Multiple distribution hubs (currently 2) with individual positioning
- **Consumers**: Multiple endpoints (5 per switch) linked to specific switches
- **Positions**: All elements have X/Y coordinates for SVG placement

### Data Flow Architecture

```
JSON Config → ProductionService → Static Production Display
     ↓
Control Page → Transport Route Selection → Event → Live Animation Updates
```

### Transport Control System

**Two-Page Architecture**:
1. **Production System** (`/`): Fullscreen SVG visualization with selective line animation
2. **Transport Control** (`/control`): Radzen-based control interface with dropdown selectors

**Selective Animation Logic**:
- **Active Route**: Green, thick, animated dashed lines (machine → selected switch → selected consumer)
- **Inactive Routes**: Gray, thin, solid lines without animation
- **Real-time Updates**: Event-driven state changes between pages

### Domain Models (`Models/ProductionModels.cs`)

**Core Models**:
- **ProductionConfig**: JSON deserialization target matching config file structure
- **ProductionSystem**: Runtime system state with transport route information
- **TransportRoute**: Active route tracking (TargetSwitchId, TargetConsumerId, IsActive)
- **Position**: X/Y coordinate system for all visual elements

**Component Flow**: Machine → Switches → Consumers (configurable routing)

## Technology Stack

- **.NET 8** with C# 12
- **Blazor Server** - Server-side rendering only
- **Radzen.Blazor 4.32.0** - UI components for control interface
- **Serilog.AspNetCore 9.0.0** - Structured logging with file rotation
- **Custom SVG animations** with conditional rendering
- **System.Text.Json** for configuration loading

## Key Components

### Visualization Architecture
- **ProductionVisualization.razor**: Main SVG visualization component with position-based rendering
- **Fullscreen Display**: CSS with `overflow: hidden` and viewport-sized positioning
- **Conditional Animation**: Lines animate only when part of active transport route
- **Event Subscription**: Components subscribe to transport route changes via `IDisposable` pattern

### Configuration System
- **JSON-First Approach**: All topology defined in external configuration
- **Graceful Fallbacks**: Programmatic defaults when config file unavailable
- **Position-Based Layout**: SVG coordinates stored in configuration for flexible layouts

### Transport Control Interface
- **Cascading Selection**: Switch selection updates available consumer options
- **Real-time Feedback**: Active route display with visual confirmation
- **Service Integration**: Direct communication with ProductionService for route management

## Page Structure

1. **Production System** (`/`): Minimalist fullscreen SVG visualization
2. **Transport Control** (`/control`): Control interface with combo boxes and action buttons

## Logging Configuration

- Console logging for development
- File logging to `logs/blazor-YYYYMMDD.log` next to executable
- Daily rolling files with 7-day retention
- Real-time flushing every second

## Key Architectural Decisions

- **JSON Configuration**: Flexible topology definition without code changes
- **Event-Driven Transport Control**: Clean separation between control and visualization
- **Minimal Service Layer**: Essential functionality only for static visualization needs
- **Position-Based SVG Rendering**: All visual elements positioned via configuration coordinates