using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Api.GraphQL.Types;

public class DiagnosisType : ObjectType<Diagnosis>
{
    protected override void Configure(IObjectTypeDescriptor<Diagnosis> descriptor)
    {
        descriptor.Field(d => d.Id).Type<NonNullType<IdType>>();
        descriptor.Field(d => d.Result).Type<StringType>();
        descriptor.Field(d => d.Date).Type<DateTimeType>();
        descriptor.Field(d => d.Patient).Type<PatientType>();
    }
}
