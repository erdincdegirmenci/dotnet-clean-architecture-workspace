.NET Clean Architecture Workspace

A reusable ASP.NET Core Web API starter workspace built with Clean Architecture principles.

The goal of this repository is to provide a solid and extensible foundation for building scalable .NET applications with a clear separation of concerns, testability, infrastructure integrations, authentication, messaging, caching, logging, and observability.

Architecture

The solution is organized around the following layers:

<img width="309" height="537" alt="image" src="https://github.com/user-attachments/assets/5353f141-bf58-4cac-935f-00603ed778f0" />


The architecture keeps business logic independent from infrastructure and framework-specific concerns.

Project Structure
<img width="180" height="391" alt="image" src="https://github.com/user-attachments/assets/d32f96be-8240-4a22-915b-97a7ac21530d" />


Projects
Template.Api

The entry point of the application.

Responsible for:

HTTP API
Controllers
Middleware
Filters
API configuration
Dependency injection composition
Authentication configuration
Swagger/OpenAPI
Template.Application

Contains application-level business orchestration.

Responsible for:

Application services
DTOs
Interfaces
Managers
Repositories abstractions
Mapping
Application use cases
Template.Domain

Contains the core domain model and business rules.

The Domain layer should remain independent from infrastructure and external frameworks whenever possible.

Template.Persistence

Responsible for data access and persistence-related implementations.

Typical responsibilities include:

Entity Framework Core
Repository implementations
Database access
Persistence configuration
Template.Infrastructure

Contains integrations with external infrastructure and services.

Examples include:

Kafka
External service integrations
Infrastructure-specific implementations
Template.Identity

Contains authentication and identity-related functionality.

Template.Config

Contains application configuration models and feature configuration.

Template.Shared

Contains functionality shared between different layers of the application.

Template.Tests

Contains automated tests for the application.

Technology Stack

The workspace is designed around the following technologies and concepts:

.NET
ASP.NET Core Web API
C#
Clean Architecture
Dependency Injection
Entity Framework Core
MediatR
AutoMapper
JWT Authentication
ASP.NET Core Identity
Redis
Apache Kafka
RabbitMQ
Polly
Serilog
Elasticsearch
Logstash
Kibana
OpenTelemetry
Swagger / OpenAPI
Docker
Docker Compose
Key Features
Clean Architecture
Separation of concerns
Dependency Injection
DTO-based application layer
Repository abstractions
Service layer
Authentication and authorization
JWT Bearer authentication
Identity integration
API versioning
Swagger/OpenAPI
Rate limiting
Feature flags
Redis caching
Kafka messaging
RabbitMQ support
Resilience policies with Polly
Structured logging with Serilog
Elasticsearch integration
Logstash pipeline
Kibana visualization
OpenTelemetry support
Dockerized development environment
Automated tests
Getting Started
Prerequisites

Make sure you have the following installed:

.NET SDK
Docker Desktop
Git
Clone the Repository
git clone https://github.com/erdincdegirmenci/dotnet-clean-architecture-workspace.git

cd dotnet-clean-architecture-workspace

Restore Dependencies
dotnet restore

Build
dotnet build

Run Tests
dotnet test

Run the API
dotnet run --project src/Template.Api

Docker

The repository contains Docker Compose configuration for running the application together with supporting infrastructure.

Start the environment:

docker compose up --build


Stop the environment:

docker compose down


To remove containers and volumes:

docker compose down -v

Infrastructure

The workspace is designed to support a distributed application environment.

                    ┌───────────────┐
                    │  ASP.NET API  │
                    └───────┬───────┘
                            │
              ┌─────────────┼─────────────┐
              │             │             │
              ▼             ▼             ▼
           Redis          Kafka        RabbitMQ
              │
              │
              ▼
        Application Cache


              Logging / Observability
                       │
                       ▼
                  ┌─────────┐
                  │ Serilog │
                  └────┬────┘
                       │
              ┌────────┴────────┐
              ▼                 ▼
         Elasticsearch         Seq
              │
              ▼
           Logstash
              │
              ▼
            Kibana

Configuration

Application configuration can be managed through standard ASP.NET Core configuration sources:

appsettings.json
appsettings.Development.json
Environment variables
Docker Compose environment variables
User Secrets

Do not commit sensitive information such as:

Passwords
Connection strings containing credentials
JWT signing keys
API keys
Access tokens
Private certificates
Testing

Tests are located under:

tests/Template.Tests


Run the complete test suite:

dotnet test


For local development, it is recommended to run tests before opening a pull request.

Development Guidelines

When extending the workspace, try to preserve the architectural boundaries.

Domain

Keep business rules in the Domain layer.

<img width="195" height="118" alt="image" src="https://github.com/user-attachments/assets/8d67486c-8dea-45c4-b67d-6f00e36d4618" />

Application

Application logic should depend on abstractions rather than infrastructure implementations.

<img width="143" height="155" alt="image" src="https://github.com/user-attachments/assets/9af8f407-ec34-4841-b310-d27442767e1f" />


Infrastructure

Infrastructure-specific implementations should remain outside the Domain layer.

<img width="290" height="101" alt="image" src="https://github.com/user-attachments/assets/3f0ee7c3-82e9-4dd6-952f-4db07ecba315" />

API

The API layer should focus primarily on HTTP concerns.

<img width="226" height="135" alt="image" src="https://github.com/user-attachments/assets/bc5c7090-b3a2-4307-a92b-5e8e07f6a7e6" />


Why Clean Architecture?

This workspace follows Clean Architecture to make the application:

Easier to test
Easier to maintain
Easier to extend
Less coupled to infrastructure
More suitable for large applications
Easier to evolve over time

The most important principle is keeping business logic independent from external technologies.

<img width="292" height="253" alt="image" src="https://github.com/user-attachments/assets/706c4b60-d649-4894-be82-19c90dc500c7" />

When to Use This Repository

This workspace can be used as a starting point for:

REST APIs
Microservices
Enterprise applications
Distributed systems
Backend services
Event-driven applications
Projects requiring centralized logging and observability

It is intended to be adapted to the requirements of the application rather than used as a rigid framework.

Production Considerations

Before using this workspace in production, review:

.NET target framework and package versions
Authentication configuration
JWT key management
Database configuration
Redis configuration
Kafka/RabbitMQ configuration
Logging configuration
Health checks
Rate limiting policies
CORS configuration
Secrets management
Docker image security
Observability configuration
Automated CI/CD pipeline
Roadmap

Potential future improvements:

 Health Checks
 Global exception handling improvements
 Result pattern
 CQRS improvements
 Integration test infrastructure
 Testcontainers support
 Database migration examples
 GitHub Actions CI/CD
 Container security scanning
 Distributed tracing examples
 OpenTelemetry dashboards
 Kubernetes deployment examples
License

This project is licensed under the MIT License.

See the LICENSE file for details.
