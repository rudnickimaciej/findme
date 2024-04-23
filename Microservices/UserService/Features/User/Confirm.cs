using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Bus;
using UserService.Contracts;
using UserService.Entities;

namespace UserService.Features.User
{
    public static class Confirm
    {

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/auth/confirm",async (ISender sender, IEventBus bus, UserManager<AppUser> userManager, string userId, string token) =>
            {
                ;
                if (userId == null || token == null)
                {
                    return Results.BadRequest("User ID or token is null.");
                }

                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    // Handle the case where user is not found
                    return Results.BadRequest("Unable to find user with ID '{request.UserId}'.");

                }

                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    // Email successfully confirmed
                    // You can redirect to a success page or return a message
                    return Results.Ok("Email confirmed successfully.");
                }
                else
                {
                    // Failed to confirm email
                    // You can redirect to an error page or return a message
                    return Results.BadRequest("Failed to confirm email.");
                }
            });
        }
    }
}
