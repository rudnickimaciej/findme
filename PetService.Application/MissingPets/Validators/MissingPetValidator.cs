using FluentValidation;
using PetService.Application.Dtos;
using PetService.Domain;

namespace PetService.Application.MissingPets.Validators
{
    public class MissingPetValidator : AbstractValidator<AddMissingPetDto>
    {
        public MissingPetValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
