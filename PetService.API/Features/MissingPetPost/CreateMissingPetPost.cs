using FluentValidation;
using MediatR;
using PetService.Domain;
using PetService.Infrastructure;

namespace PetService.API.Features.MissingPetPost
{
    public static class CreateMissingPetPost
    {
        public class Command : IRequest<Guid>
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime MissingDate { get; set; }
            public int Gender { get; set; }
            public Guid BreadId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Name).NotEmpty();
                RuleFor(c => c.Description).NotEmpty();
                RuleFor(c => c.MissingDate).NotEmpty();
                RuleFor(c => c.MissingDate).LessThanOrEqualTo(DateTime.Now);
                RuleFor(c => c.Gender).NotEmpty();
                RuleFor(c => c.BreadId).NotEmpty();
            }
        }
            internal sealed class Handler : IRequestHandler<Command, Guid>
        {
            private readonly DataContext _dbContext;

            public Handler(DataContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var post = new MissingPet
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    BreadId = request.BreadId,
                    Gender = request.Gender,
                    MissingDate = request.MissingDate
                };

                _dbContext.Add(post);
                await _dbContext.SaveChangesAsync();

                return post.Id;
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/missingpetposts", async (CreateMissingPetPost.Command command, ISender sender) =>
            {
                var postId = await sender.Send(command);

                return Results.Ok(postId);
            });
        }
    }
}
