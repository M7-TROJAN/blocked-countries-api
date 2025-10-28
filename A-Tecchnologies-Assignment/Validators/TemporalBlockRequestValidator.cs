using A_Tecchnologies_Assignment.DTOs;
using FluentValidation;

namespace A_Tecchnologies_Assignment.Validators;

public class TemporalBlockRequestValidator : AbstractValidator<TemporalBlockRequest>
{
    public TemporalBlockRequestValidator()
    {
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("CountryCode is required.")
            .Length(2, 3).WithMessage("CountryCode must be 2 or 3 characters.");

        RuleFor(x => x.CountryName)
            .NotEmpty().WithMessage("CountryName is required.")
            .MinimumLength(2).WithMessage("CountryName must be at least 2 characters long.");

        RuleFor(x => x.DurationMinutes)
            .InclusiveBetween(1, 1440)
            .WithMessage("Duration must be between 1 and 1440 minutes.");
    }
}
