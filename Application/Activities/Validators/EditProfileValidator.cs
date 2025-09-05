using System;
using Application.Profiles.Commands;
using FluentValidation;

namespace Application.Activities.Validators;

public class EditProfileValidator : AbstractValidator<EditProfile.Command>
{
    public EditProfileValidator()
    {
        RuleFor(x => x.DisplayName)
        .NotEmpty().WithMessage("The DisplayName property can not be empty");
    }
}
