using NeuronaLab.PatientDiagnosticsManagement.Server.Domain.Entities;

namespace NeuronaLab.PatientDiagnosticsManagement.Server.Api.GraphQL.Types;

public class PatientType : ObjectType<Patient>
{
    protected override void Configure(IObjectTypeDescriptor<Patient> descriptor)
    {
        descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
        descriptor.Field(p => p.Name).Type<StringType>();
        descriptor.Field(p => p.Age).Type<IntType>();
        descriptor.Field(p => p.Diagnoses).Type<ListType<DiagnosisType>>();
    }
}
