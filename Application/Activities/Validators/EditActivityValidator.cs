using System;
using Application.Activities.Commands;
using Application.Activities.DTOs;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Activities.Validators;

public class EditActivityValidator : BaseActivityValidator<EditActivity.Command, EditActivityDto>
{
    public EditActivityValidator() : base(x => x.ActivityDto)
    {
        RuleFor(x => x.ActivityDto.Id)
        .NotEmpty()
        .WithMessage("Identifier is required");

    }
}
