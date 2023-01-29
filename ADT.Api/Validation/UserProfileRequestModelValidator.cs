using System.Text.RegularExpressions;
using ADT.Common.Models.Request;
using FluentValidation;

namespace ADT.Api.Validation;

public class UserProfileRequestModelValidator : AbstractValidator<UserProfileRequestModel>
{
    private static readonly Regex _phoneNumberRegex = new("\\(\\d{3}\\)\\d{3}-\\d{4}", RegexOptions.Compiled);
    
    public UserProfileRequestModelValidator()
    {
        RuleFor(x => x.FirstName).NotNull().MinimumLength(3).WithMessage("{PropertyName} must have at least {MinLength} characters");
        RuleFor(x => x.LastName).NotNull().MinimumLength(3).WithMessage("{PropertyName} must have at least {MinLength} characters");
        RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("{PropertyName} must not be empty");
        RuleFor(x => x.EmailAddress).NotNull().EmailAddress().WithMessage("Please specify a valid email address");
        RuleFor(x => x.PhoneNumber).NotNull().Must(BeFormatted).WithMessage("{PropertyName} must be in '(999)999-9999' format");
        RuleFor(x => x.Address).NotEmpty().WithMessage("{PropertyName} must not be empty");
    }

    private bool BeFormatted(string arg)
    {
        return arg != null && _phoneNumberRegex.IsMatch(arg);
    }
}
