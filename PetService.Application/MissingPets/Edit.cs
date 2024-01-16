using AutoMapper;
using LanguageExt;
using MediatR;
using OneOf;
using PetService.Application.Core.Exceptions;
using PetService.Domain;
using PetService.Infrastructure;

namespace PetService.Application.MissingPets
{
    public class Edit
    {
        public class Command : IRequest<OneOf<RecordNotFoundException, ServerErrorException, MissingPet>>
        {
            public MissingPet MissingPet { get; set; }
        }

        public class Handler : IRequestHandler<Command, OneOf<RecordNotFoundException, ServerErrorException, MissingPet>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                this._mapper = mapper;
                _context = context;
            }
            public async Task<OneOf<RecordNotFoundException, ServerErrorException, MissingPet>> Handle(Command request, CancellationToken cancellationToken)
            {
                var pet = await _context.MissingPets.FindAsync(request.MissingPet.Id);
                if (pet == null)
                    return OneOf<RecordNotFoundException, ServerErrorException, MissingPet>.FromT0(new RecordNotFoundException($"Pet with {request.MissingPet.Id} id does not exist)"));
                _mapper.Map(request.MissingPet, pet);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                    return OneOf<RecordNotFoundException, ServerErrorException, MissingPet>.FromT1(new ServerErrorException($"Error when editing Pet"));

                return OneOf<RecordNotFoundException, ServerErrorException, MissingPet>.FromT2(request.MissingPet);
            }
        }
    }

}