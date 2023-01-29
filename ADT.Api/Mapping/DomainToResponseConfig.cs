using ADT.Api.Models.Domain;
using ADT.Common.Models.Response;
using Mapster;

namespace ADT.Api.Mapping;

public class DomainToResponseConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<UserProfile, UserProfileResponseModel>()
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
    }
}