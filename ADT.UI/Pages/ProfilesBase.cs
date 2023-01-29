using ADT.Common.Models.Response;
using ADT.UI.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace ADT.UI.Pages;

public class ProfilesBase : ComponentBase
{
    protected IEnumerable<UserProfileResponseModel> _profiles;

    [Inject] public IUserProfileService UserProfileService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _profiles = await UserProfileService.GetAll();
    }
}