using FluentValidation;
using Mapster;
using MediatR;
using PostService.Contracts;
using PostService.Database;
using PostService.Entities;
using PostService.Shared;
using Shared.Bus;
using Shared.Events;
using static PetService.API.Features.MissingPetPost.AddMissingPetPost.Handler;


namespace PetService.API.Features.MissingPetPost
{
    public static class AddMissingPetPost
    {
        public class Command : IRequest<Result<AddMissingPetPostResponse>>
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
            internal sealed class Handler : IRequestHandler<Command, Result<AddMissingPetPostResponse>>
        {
            private readonly DataContext _dbContext;
            private readonly IValidator<Command> _validator;
            private readonly IEventBus _bus;


            public Handler(DataContext dbContext, IValidator<Command> validator)
            {
                _dbContext = dbContext;
                _validator = validator;
            }

            public async Task<Result<AddMissingPetPostResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    Result.Failure<Guid>(
                        new Error("AddMissingPost.Validation",
                        validationResult.ToString()));
                };

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

                return post.Adapt<AddMissingPetPostResponse>();

            }
            public class NewMissingPetPostAdded : Event
            {
                public AddMissingPetPostResponse Post{ get; private set; }

                public NewMissingPetPostAdded(AddMissingPetPostResponse post)
                {
                    Post = post;
                }
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/missingpets", async (AddMissingPetPostRequest request, ISender sender, IEventBus bus) =>
            {
                var command = request.Adapt<AddMissingPetPost.Command>();

                var result = await sender.Send(command);

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                await bus.Publish(new NewMissingPetPostAdded(result.Value),"missingpostaddedqueue");
                var createdUri = $"api/missingpets/{result.Value.Id}"; // Assuming you have an endpoint to retrieve the created resource

                //return Results.Ok(result.Value);
                return Results.Created(createdUri, result.Value);

            });
        }
    }
}
