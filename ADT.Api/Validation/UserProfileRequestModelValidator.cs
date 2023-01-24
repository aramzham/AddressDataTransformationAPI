using System.Text.RegularExpressions;
using ADT.Api.Models.Request;
using FluentValidation;

namespace ADT.Api.Validation;

public class UserProfileRequestModelValidator : AbstractValidator<UserProfileRequestModel>
{
    private static readonly Regex _phoneNumberRegex = new("\\(\\d{3}\\)\\d{3}-\\d{4}", RegexOptions.Compiled);
    
    public UserProfileRequestModelValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3);
        RuleFor(x => x.LastName).MinimumLength(3);
        RuleFor(x => x.EmailAddress).EmailAddress();
        RuleFor(x => x.PhoneNumber).Must(BeFormatted);
    }

    private bool BeFormatted(string arg)
    {
        return _phoneNumberRegex.IsMatch(arg);
    }
}