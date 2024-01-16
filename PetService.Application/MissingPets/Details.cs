using CSharpFunctionalExtensions;
using LanguageExt;
using MediatR;
using PetService.Domain;
using PetService.Infrastructure;

namespace PetService.Application.MissingPets
{
    public class Details
    {
        public class Query : IRequest<Option<MissingPet>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Option<MissingPet>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Option<MissingPet>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pet = await _context.MissingPets.FindAsync(request.Id);
                if (pet == null)
                    return Option<MissingPet>.None;
                return Option<MissingPet>.Some(pet);
            }
        }
    }
}