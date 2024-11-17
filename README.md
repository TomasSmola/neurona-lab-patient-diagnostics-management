# Neurona Lab - Patient Diagnosis Management System

This project is a simple web application that allows users to manage patients and their diagnostics. It is built using ASP.NET and React.

## Prerequisites

- Docker
- Docker Compose

## Get and Run Project

In your terminal, run the following commands:

```bash
git clone https://github.com/TomasSmola/neurona-lab-patient-diagnostics-management.git
cd neurona-lab-patient-diagnostics-management
docker-compose build
docker-compose up
```

In your browser, navigate to `https://localhost:32771/index.html` to view the application.

## Solution Structure

### NeuronaLab.PatientDiagnosticsManagement.Server

It is an ASP.NET Core Web API project structured as follows:

- **API** - Contains the GrqphQL API mutations and queries
- **Data** - Contains the database context and models
- **Domain** - Contains the domain models and services

### neuronaLab.patientdiagnosticsmanagement.client

It is a simple React application, it doesn't any special library to call the GrapqQL API, it uses the built-in `fetch` function.

## Examples of GraphQL Queries and Mutations

### Crate Patient

```graphql
mutation Mutation{
    addPatient(name: "Karel Novak", age: 32) {
        id
        name
        age
    }
}
```

### Add Diagnostic Result to Patient

```graphql
mutation Mutation{
    addDiagnosis(patientId: 1, result: "Flu") {
        result
        date
    }
}
```

### Get All Patients

```graphql
query Query{
    patients {
        id
        name
        age
        diagnoses {
            date
            result
        }
    }
}
```

### Get Patient by ID

```graphql
query Query{
    patient(id: 1) {
        id
        name
        age
        diagnoses {
            date
            result
        }
    }
}
```

## What Could Be Improved

- Structure the solutions into separate projects instead of directories.
- Use database migrations instead of `await dbContext.Database.EnsureCreatedAsync()`.
- Write some API tests.
- Use Azure KeyVault for storing secrets (now they are available on GitHub).
- Introduce users and roles, secure the API with JWT tokens.
- Use a library like Apollo for calling the GraphQL API.