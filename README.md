# Infor ION API Demo Application

This is a demo application built with Blazor WebAssembly to showcase how to interact with the Infor ION API. The application retrieves data for Items, Jobs, and Lots from the Infor ION API and displays it in a modern, responsive UI. If the API calls fail or no data is returned, the application gracefully falls back to sample data.

## Features

- **Items Page**: Displays a list of items pulled from the Infor ION API with search functionality.
- **Jobs Page**: Displays a list of jobs pulled from the Infor ION API with search functionality.
- **Lots Page**: Displays a list of lots pulled from the Infor ION API with search functionality.
- **Settings Page**: Provides a user interface for configuring the connection settings required to access the API.
- **Responsive UI**: Built with MudBlazor to ensure a modern, mobile-friendly design.

## Technologies Used

- **Blazor WebAssembly**: For building a client-side web application.
- **MudBlazor**: For Material Design-inspired UI components.
- **.NET 9**: as the application framework.
- **Infor ION API**: Demonstrates how to retrieve enterprise data via Infor’s API.
- **JSON Configuration**: Reads connection and application settings from JSON files.
