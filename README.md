# Liv'In Paris

**A C# and MySQL academic project exploring neighbour-to-neighbour meal sharing, order management, and delivery-route planning on the Paris Metro network.**

## Overview

Liv'In Paris is a console-based prototype of a local meal-sharing platform. Home cooks can publish dishes, while customers can browse meals, place orders, and follow their order history. The application brings together object-oriented programming, relational database operations, data exports, statistics, and graph algorithms in one project.

The project was developed as a team assignment and combines two main areas: a database-backed management application and graph-based analysis for route planning and customer–cook relationships.

## Key features

### Profiles and role-based workflows
- Account creation, sign-in, and profile management.
- Separate workflows for customers, cooks, and administrators.
- Address management and administration of users and platform data.

### Dishes and orders
- Browse and search available dishes.
- Manage dishes and ingredients.
- Create orders with multiple order lines, calculate order totals, and manage order status.
- Access order history and manage cook evaluations.

### Statistics and data exports
- Customer, cook, and platform-level statistics.
- Indicators such as order activity over time, popular dishes, customer activity, and cook performance.
- Export selected data, including dishes, orders, and statistics, to JSON and XML.

### Graph algorithms and route planning
- Generic graph structures based on nodes and edges.
- Breadth-first search (BFS), depth-first search (DFS), connectivity checks, cycle detection, and degree-based analysis.
- Shortest-path implementations using Dijkstra, Bellman–Ford, and Floyd–Warshall.
- Paris Metro graph construction and visualisation to support route-planning experiments.
- Analysis of customer–cook relationships, including Welsh–Powell graph colouring, bipartite checks, planarity checks, and independent-group identification.

## Technology stack

- **C#** and **.NET Framework**
- **MySQL** with MySQL Connector/NET (`MySql.Data`)
- **Newtonsoft.Json** for JSON handling
- **XML** serialization and file handling
- **System.Drawing** for graph visualisation
- **Visual Studio** for development

> The repository contains multiple iterations of the coursework, with project files targeting different .NET Framework versions. Select the matching source files, solution, and database schema before building the application.

## Main components

| Component | Responsibility |
| --- | --- |
| `GestionBDD` and `SessionUtilisateur` | Application entry point, database helpers, session state, and navigation between modules. |
| `ModuleProfil`, `ModuleClient`, `ModuleCuisinier`, `ModuleAdministrateur` | Authentication, profile management, and role-specific operations. |
| `ModulePlat` and `ModuleCommande` | Dish and ingredient management, order creation, order lines, status tracking, and evaluations. |
| `ModuleStats` | Customer, cook, and global statistics. |
| `Graphe`, `Noeud`, and `Lien` | Generic graph data structures and core graph operations. |
| `MetroGraphe`, `CalculDistance`, and `VisualisationMetro` | Metro network modelling, route algorithms, distance utilities, and visualisation. |
| `ClientCuisinierGraphe` | Graph-based analysis of customer–cook relationships. |
| `ExportJson`, `ExportXML`, and related export classes | Data and statistics export. |

## Running locally

The application requires a compatible Windows development environment, Visual Studio with the required .NET Framework targeting pack, and a local MySQL instance.

1. Choose the intended integrated C# project and its matching solution; the repository includes earlier iterations and experimental projects.
2. Create the MySQL database using the schema that matches that version of the application.
3. Configure the database connection for your local environment. Do not commit database passwords or other credentials.
4. Update any machine-specific file paths used for input data or exports.
5. Build and run the selected project from Visual Studio.

Because the project evolved through several deliverables, a clean first-time setup may require aligning the selected solution, source files, database schema, and local configuration.

## Project context

This project was built for academic purposes to put several software-engineering concepts into practice: object-oriented design, database modelling and access, file handling, graph theory, shortest-path algorithms, and data visualisation.

**Note:** Liv'In Paris is an academic prototype rather than a production-ready service. Its setup and configuration should be reviewed before reuse or public deployment.
