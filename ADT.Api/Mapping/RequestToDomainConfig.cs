using ADT.Api.Models.Domain;
using ADT.Common.Models.Request;
using Mapster;

namespace ADT.Api.Mapping;

public class RequestToDomainConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<UserProfileRequestModel, UserProfile>();
    }
}