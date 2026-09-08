.NET Clean Architecture Workspace

A reusable ASP.NET Core Web API starter workspace built with Clean Architecture principles.

The goal of this repository is to provide a solid and extensible foundation for building scalable .NET applications with a clear separation of concerns, testability, infrastructure integrations, authentication, messaging, caching, logging, and observability.

Architecture

The solution is organized around the following layers:

┌─────────────────────────────────────────────┐
│                  API Layer                   │
│              Template.Api                   │
│     Controllers / Middleware / Filters      │
└──────────────────────┬──────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────┐
│              Application Layer              │
│           Template.Application              │
│   Services / DTOs / Interfaces / Managers   │
└──────────────────────┬──────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────┐
│                Domain Layer                 │
│              Template.Domain                │
│        Entities / Business Rules            │
└─────────────────────────────────────────────┘
                       ▲
                       │
        ┌──────────────┼──────────────┐
        │              │              │
        ▼              ▼              ▼
┌──────────────┐ ┌──────────────┐ ┌──────────────┐
│ Persistence  │ │Infrastructure│ │   Identity   │
│              │ │              │ │              │
│ EF / Data    │ │ Kafka /      │ │ Auth /       │
│ Access       │ │ Integrations │ │ Authorization│
└──────────────┘ └──────────────┘ └──────────────┘


The architecture keeps business logic independent from infrastructure and framework-specific concerns.

Project Structure
.
├── src
│   ├── Template.Api
│   ├── Template.Application
│   ├── Template.Config
│   ├── Template.Domain
│   ├── Template.Identity
│   ├── Template.Infrastructure
│   ├── Template.Persistence
│   └── Template.Shared
│
├── tests
│   └── Template.Tests
│
├── logstash
│   └── logstash.conf
│
├── docker-compose.yml
├── docker-compose.override.yml
├── docker-compose.dcproj
├── dotnet-template.sln
├── launchSettings.json
├── LICENSE
└── README.md


The repository currently separates API, Application, Domain, Configuration, Identity, Infrastructure, Persistence and Shared concerns into individual projects. {"fallbackMarkdown":"(GitHub
)","reference":{"matched_text":"","prefix":null,"start_idx":2762,"end_idx":2779,"safe_urls":["https://github.com/erdincdegirmenci/dotnet-clean-architecture-workspace"],"refs":[],"alt":"(GitHub
)","prompt_text":null,"type":"grouped_webpages","items":[{"title":"GitHub - erdincdegirmenci/dotnet-clean-architecture-workspace · GitHub","url":"https://github.com/erdincdegirmenci/dotnet-clean-architecture-workspace","attribution":"GitHub","pub_date":null,"snippet":null,"thumbnail_url":"https://images.openai.com/static-rsc-1/xUIjbRFPwgSf03TaDaSJUSISbzPg2og-FM0sKVG3FIGt5Ijjisw2GkfuI0bseVw6uRzufbtvF40fMqQh8WAJe3JTY8gekqurhCqkcTMQ4C6eMrXtukgrDN8PH-zqqvMFgk_U-aez3Um4o_lOkqJJTvP5xMgvHLmug2o-9Xh-PQA0BLzgSNvNdvLGM-rcz7LJ7XRFqD0-NcvQ8TlAPMbllqVWT2eIFs7xESSlwTaQoLA","attribution_segments":null,"supporting_websites":[],"refs":[{"turn_index":0,"ref_type":"view","ref_index":0}],"hue":null,"attributions":null}],"error":null,"style":null,"fallback_items":null,"status":"done"},"showLoginRequiredCard":false}

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


The repository includes Docker Compose configuration and a dedicated Logstash configuration. {"fallbackMarkdown":"(GitHub
)","reference":{"matched_text":"","prefix":null,"start_idx":6172,"end_idx":6189,"safe_urls":["https://github.com/erdincdegirmenci/dotnet-clean-architecture-workspace"],"refs":[],"alt":"(GitHub
)","prompt_text":null,"type":"grouped_webpages","items":[{"title":"GitHub - erdincdegirmenci/dotnet-clean-architecture-workspace · GitHub","url":"https://github.com/erdincdegirmenci/dotnet-clean-architecture-workspace","attribution":"GitHub","pub_date":null,"snippet":null,"thumbnail_url":"https://images.openai.com/static-rsc-1/xUIjbRFPwgSf03TaDaSJUSISbzPg2og-FM0sKVG3FIGt5Ijjisw2GkfuI0bseVw6uRzufbtvF40fMqQh8WAJe3JTY8gekqurhCqkcTMQ4C6eMrXtukgrDN8PH-zqqvMFgk_U-aez3Um4o_lOkqJJTvP5xMgvHLmug2o-9Xh-PQA0BLzgSNvNdvLGM-rcz7LJ7XRFqD0-NcvQ8TlAPMbllqVWT2eIFs7xESSlwTaQoLA","attribution_segments":null,"supporting_websites":[],"refs":[{"turn_index":0,"ref_type":"view","ref_index":0}],"hue":null,"attributions":null}],"error":null,"style":null,"fallback_items":null,"status":"done"},"showLoginRequiredCard":false}

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

Domain
  ├── Entities
  ├── Value Objects
  ├── Domain Rules
  └── Domain Abstractions

Application

Application logic should depend on abstractions rather than infrastructure implementations.

Application
  ├── DTOs
  ├── Interfaces
  ├── Services
  ├── Managers
  ├── Repositories
  └── Mapping

Infrastructure

Infrastructure-specific implementations should remain outside the Domain layer.

Infrastructure
  ├── Kafka
  ├── External Services
  └── Infrastructure Implementations

API

The API layer should focus primarily on HTTP concerns.

Api
  ├── Controllers
  ├── Middleware
  ├── Filters
  └── HTTP Configuration

Why Clean Architecture?

This workspace follows Clean Architecture to make the application:

Easier to test
Easier to maintain
Easier to extend
Less coupled to infrastructure
More suitable for large applications
Easier to evolve over time

The most important principle is keeping business logic independent from external technologies.

        Frameworks & Infrastructure
                  │
                  ▼
        ┌────────────────────┐
        │    Application     │
        └─────────┬──────────┘
                  │
                  ▼
        ┌────────────────────┐
        │      Domain        │
        └────────────────────┘

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
