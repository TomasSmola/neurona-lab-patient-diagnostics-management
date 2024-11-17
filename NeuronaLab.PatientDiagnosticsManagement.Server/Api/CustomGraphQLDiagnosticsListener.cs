using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution;
using HotChocolate.Resolvers;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Api;

public class CustomGraphQLDiagnosticsListener(ILogger<CustomGraphQLDiagnosticsListener> logger) : ExecutionDiagnosticEventListener
{
    public override void RequestError(IRequestContext context, Exception exception)
    {
        logger.LogError(exception, "GraphQL Request Error: {Query}", context.Request.ToString());
        base.RequestError(context, exception);
    }

    public override void ResolverError(IMiddlewareContext context, IError error)
    {
        logger.LogError(error.Exception, "Resolver Error: {Field}", context.Selection.Field.Name);
        base.ResolverError(context, error);
    }
}
