using MediatR;
using OneOf;
using PetService.Application.Core.Exceptions;
using PetService.Infrastructure;

namespace PetService.Application.MissingPets
{
    public class Delete
    {
        public class Command : IRequest<OneOf<RecordNotFoundException, Unit>>
        {
            public Guid Id { get; set; }

        }
        public class Handler : IRequestHandler<Command, OneOf<RecordNotFoundException, Unit>>
        {
            public DataContext _context { get; }
            public Handler(DataContext context)
            {
                this._context = context;

            }
            public async Task<OneOf<RecordNotFoundException, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var movie = await _context.MissingPets.FindAsync(request.Id);

                if (movie == null)
                    return OneOf<RecordNotFoundException, Unit>.FromT0(new RecordNotFoundException($"Pet with {request.Id} id does not exist)"));
                _context.MissingPets.Remove(movie);
                await _context.SaveChangesAsync();
                return OneOf<RecordNotFoundException, Unit>.FromT1(Unit.Value);
            }
        }

    }
}