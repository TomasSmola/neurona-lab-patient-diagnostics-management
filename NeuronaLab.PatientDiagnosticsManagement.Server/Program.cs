using Microsoft.EntityFrameworkCore;
using NeuronaLab.PatientDiagnosticsManagement.Server.Api;
using NeuronaLab.PatientDiagnosticsManagement.Server.Api.GraphQL.Mutations;
using NeuronaLab.PatientDiagnosticsManagement.Server.Api.GraphQL.Queries;
using NeuronaLab.PatientDiagnosticsManagement.Server.Data.Contexts;
using NeuronaLab.PatientDiagnosticsManagement.Server.Data.Repositories;
using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;
using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<Patient>()
    .AddType<Diagnosis>()
    .AddDiagnosticEventListener<CustomGraphQLDiagnosticsListener>()
    .DisableIntrospection(false);

builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IPatientService, PatientService>();
builder.Services.AddTransient<IPatientRepository, PatientRepository>();

var app = builder.Build();

app.MapGraphQL();

app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    await using var dbContext = dbContextFactory.CreateDbContext();

    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();