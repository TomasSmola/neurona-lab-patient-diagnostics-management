# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM node:20 AS client-build
WORKDIR /client
COPY neuronalab.patientdiagnosticsmanagement.client/package*.json ./
RUN npm install
COPY neuronalab.patientdiagnosticsmanagement.client/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet-sdk-with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN mkdir -p /root/.aspnet/https

# This stage is used to build the service project
FROM dotnet-sdk-with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["neuronalab.patientdiagnosticsmanagement.client/nuget.config", "neuronalab.patientdiagnosticsmanagement.client/"]
COPY ["NeuronaLab.PatientDiagnosticsManagement.Server/NeuronaLab.PatientDiagnosticsManagement.Server.csproj", "NeuronaLab.PatientDiagnosticsManagement.Server/"]
COPY ["neuronalab.patientdiagnosticsmanagement.client/neuronalab.patientdiagnosticsmanagement.client.esproj", "neuronalab.patientdiagnosticsmanagement.client/"]
RUN dotnet restore "./NeuronaLab.PatientDiagnosticsManagement.Server/NeuronaLab.PatientDiagnosticsManagement.Server.csproj"
COPY . .
WORKDIR "/src/NeuronaLab.PatientDiagnosticsManagement.Server"
RUN dotnet build "./NeuronaLab.PatientDiagnosticsManagement.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NeuronaLab.PatientDiagnosticsManagement.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client-build /client/dist ../wwwroot
ENTRYPOINT ["dotnet", "NeuronaLab.PatientDiagnosticsManagement.Server.dll"]