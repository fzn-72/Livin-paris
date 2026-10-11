# Liv'In Paris

**A C# application for meal-sharing management, order processing, and delivery-route planning in Paris.**

## Overview

Liv'In Paris is an academic team project that explores the development of a local meal-sharing platform. Customers can browse dishes, place orders, and access their order history, while cooks can manage their dishes and ingredients.

The application combines a database-backed management system with graph algorithms for route planning and the analysis of customer–cook relationships.

## Features

### User and platform management
- User authentication and profile management.
- Dedicated workflows for customers, cooks, and administrators.
- Role-specific access to platform features.

### Dish and order management
- Dish and ingredient management.
- Order creation, order-line management, and total calculation.
- Order-status tracking and order-history access.
- Cook evaluations.

### Statistics and data export
- Customer, cook, and platform-level statistics.
- Analysis of order activity, popular dishes, and cook performance.
- JSON and XML exports for selected data and statistics.

### Graph theory and route planning
- Implementation of generic graph structures using nodes and edges.
- Graph traversal and analysis, including BFS, DFS, connectivity, and cycle detection.
- Shortest-path algorithms: Dijkstra, Bellman–Ford, and Floyd–Warshall.
- Modelling and visualisation of the Paris Metro network.
- Analysis of customer–cook relationships using graph colouring and other graph properties.

## Technologies

- **Language:** C#
- **Framework:** .NET Framework
- **Database:** MySQL
- **Database connectivity:** MySQL Connector/NET (`MySql.Data`)
- **Data formats:** JSON and XML
- **JSON library:** Newtonsoft.Json
- **Graph visualisation:** `System.Drawing`
- **Development environment:** Visual Studio

## Project structure

The codebase is organised into several functional components:

| Component | Purpose |
|---|---|
| `GestionBDD` and `SessionUtilisateur` | Database helpers, session management, and application navigation |
| `ModuleProfil`, `ModuleClient`, `ModuleCuisinier`, `ModuleAdministrateur` | User profiles and role-specific functionality |
| `ModulePlat` and `ModuleCommande` | Dish, ingredient, and order management |
| `ModuleStats` | Statistics and performance indicators |
| `Graphe`, `Noeud`, and `Lien` | Generic graph structures and operations |
| `MetroGraphe`, `CalculDistance`, and `VisualisationMetro` | Metro network modelling, route calculations, and visualisation |
| `ClientCuisinierGraphe` | Customer–cook relationship analysis |
| `ExportJson`, `ExportXML`, and related classes | Data and statistics export |

## Getting started

### Prerequisites

- Windows
- Visual Studio with the appropriate .NET Framework targeting pack
- A local MySQL server
- The MySQL Connector/NET dependency and other required NuGet packages

### Setup

1. Clone or download the repository.
2. Open the appropriate solution in Visual Studio. The repository contains multiple iterations of the coursework, so make sure the selected solution, source files, and database schema belong to the same version.
3. Create the MySQL database using the corresponding SQL schema.
4. Configure the database connection for your local environment.
5. Update any machine-specific file paths used by the application.
6. Build and run the selected project in Visual Studio.

**Note:** Exact setup steps may vary depending on the version of the project being used. Database credentials and local configuration should be kept out of version control.

## Academic context

Developed as a team project at **ESILV**, Liv'In Paris provided an opportunity to apply software-engineering concepts in a practical setting, including:

- Object-oriented programming and modular application design.
- Relational database modelling and SQL operations.
- Graph theory and shortest-path algorithms.
- Data processing, serialisation, and file handling.
- Collaborative development and version control.

## Project status

Liv'In Paris is an academic prototype, not a production-ready service. The repository contains multiple iterations of the project, and the setup may require selecting and aligning the appropriate solution, source files, database schema, and local configuration.
