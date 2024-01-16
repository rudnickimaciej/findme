using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetService.Domain;
using PetService.Infrastructure;

namespace PetService.Application.MissingPets
{
    public class List
    {
        public class Query : IRequest<List<MissingPet>>
        {
        }

        public class Handler : IRequestHandler<Query, List<MissingPet>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<MissingPet>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.MissingPets.ToListAsync();
            }
        }
    }
}