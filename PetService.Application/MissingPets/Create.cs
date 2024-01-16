using AutoMapper;
using FluentValidation;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetService.Application.Core.Exceptions;
using PetService.Application.Dtos;
using PetService.Application.MissingPets.Validators;
using PetService.Domain;
using PetService.Infrastructure;
using Shared.Commands;

namespace PetService.Application.MissingPets
{
    public class Create
    {

        public class Command : IRequest<Either<AppException, AddMissingPetDto>>
        {
            public AddMissingPetDto MissingPet { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.MissingPet).SetValidator(new MissingPetValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Either<AppException, AddMissingPetDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Either<AppException, AddMissingPetDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                MissingPet pet = new();
                _mapper.Map(request.MissingPet, pet);

                request.MissingPet.PhotoUrls?.ForEach(p => pet.AddPhoto(p));

                _context.MissingPets.Add(pet);
                var result = await _context.SaveChangesAsync() > 0;

                return result
                    ? Either<AppException, AddMissingPetDto>.Right(request.MissingPet)
                    : Either<AppException, AddMissingPetDto>.Left(AppException.Create(StatusCodes.Status500InternalServerError, "Error when adding new pet!"));
            }
        }
    }
}