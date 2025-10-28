using A_Tecchnologies_Assignment.DTOs;
using FluentValidation;

namespace A_Tecchnologies_Assignment.Validators;

public class BlockCountryRequestValidator : AbstractValidator<BlockCountryRequest>
{
    public BlockCountryRequestValidator()
    {
        RuleFor(x => x.CountryCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("CountryCode is required.")
            .Length(2, 3).WithMessage("CountryCode must be 2 or 3 characters.")
            .Matches("^[A-Za-z]+$").WithMessage("CountryCode must contain only letters.");

        RuleFor(x => x.CountryName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("CountryName is required.")
            .MinimumLength(2).WithMessage("CountryName must be at least 2 characters long.");
    }
}
