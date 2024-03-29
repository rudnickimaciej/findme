using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostService.Contracts;
using PostService.Database;
using PostService.Entities;
using PostService.Shared;


namespace PetService.API.Features.MissingPetPost
{
    public static class GetMissingPetPost
    {
        public class Query : IRequest<Result<MissingPetPostResponse>>
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Query, Result<MissingPetPostResponse>>
        {
            private readonly DataContext _dbContext;

            public Handler(DataContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Result<MissingPetPostResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var postResponse = await _dbContext.MissingPets
                    .Where(post => post.Id == request.Id)
                    .Select(post => new MissingPetPostResponse()
                    {
                        Name = post.Name,
                        Description = post.Description,
                        Gender = post.Gender,
                        MissingDate = post.MissingDate,
                        BreadId = post.BreadId
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if(postResponse is null)
                {
                    return Result.Failure<MissingPetPostResponse>(new Error(
                        "GetMissingPetPost.Null",
                        "The post with specified ID was not found"
                        ));
                }

                return postResponse;
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/missingpets/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetMissingPetPost.Query { Id = id };
                var result = await sender.Send(query);

                if (result.IsFailure)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.Ok(result.Value);
            });
        }
    }
}
