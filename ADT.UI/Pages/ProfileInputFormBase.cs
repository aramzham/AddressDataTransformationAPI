using System.Text.RegularExpressions;
using ADT.Common.Models.Request;
using ADT.UI.Services.Contracts;
using Blazorise;
using Microsoft.AspNetCore.Components;

namespace ADT.UI.Pages;

public class ProfileInputFormBase : ComponentBase
{
    protected DatePicker<DateOnly?> _datePicker;
    protected bool _isLoading;
    protected UserProfileRequestModel _requestModel = new(null, null, DateOnly.MaxValue, null, null, null);
    private readonly Regex _emailValidationRegex = new("^[a-z0-9]+[\\._]?[a-z0-9]+[@]\\w+[.]\\w{2,3}$", RegexOptions.Compiled);
    private readonly Regex _phoneValidationRegex = new(@"^\(\d{3}\)\d{3}-\d{4}$", RegexOptions.Compiled);

    [Inject] public IUserProfileService UserProfileService { get; set; }

    static void Validate(ValidatorEventArgs e, Regex regex)
    {
        var value = Convert.ToString(e.Value);

        e.Status = string.IsNullOrEmpty(value)
            ? ValidationStatus.None :
            regex.IsMatch(value)
                ? ValidationStatus.Success
                : ValidationStatus.Error;
    }

    protected void ValidateEmail(ValidatorEventArgs e) => Validate(e, _emailValidationRegex);

    protected void ValidatePhone(ValidatorEventArgs e) => Validate(e, _phoneValidationRegex);

    protected async Task OnSubmitClicked()
    {
        _isLoading = true;

        await UserProfileService.Add(_requestModel);

        _isLoading = false;
    }
}