using Email;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Bus;
using Shared.Events;
using UserService.Contracts;
using UserService.Entities;
using UserService.Shared;
using static UserService.Features.User.Register.Handler;

namespace UserService.Features.User
{
    public static class Register
    {
        public class Command : IRequest<Result<RegisterUserResponse>>
        {
            public string UserName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string ConfirmPassword { get; set; } = string.Empty;

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.UserName)
                     .NotEmpty().WithMessage("Username is required.")
                     .MinimumLength(5).WithMessage("Username must be at least 5 characters long.");

                RuleFor(c => c.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email address.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

                RuleFor(x => x.ConfirmPassword)
                  .Equal(x => x.Password).WithMessage("Passwords do not match.");

            }
        }
        internal sealed class Handler : IRequestHandler<Command, Result<RegisterUserResponse>>
        {
            private readonly IValidator<Command> _validator;
            private readonly IEventBus _bus;
            private readonly UserManager<AppUser> _userManager;
            private readonly IEmailClient _emailClient;

            public Handler(IValidator<Command> validator, UserManager<AppUser> userManager, IEventBus bus, IEmailClient emailClient)
            {
                _validator = validator;
                _userManager = userManager;
                _bus = bus;
                _emailClient = emailClient;
            }

            public async Task<Result<RegisterUserResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<RegisterUserResponse>(
                        new Error("RegisterUser.Validation",
                        validationResult.ToString()));
                };

                if (_userManager.Users.Any(x => x.UserName == request.UserName))
                {
                    return Result.Failure<RegisterUserResponse>(
                        new Error("RegisterUser.Validation",
                        "Username is already taken"));
                }

                if (_userManager.Users.Any(x => x.Email == request.Email))
                {
                    return Result.Failure<RegisterUserResponse>(
                     new Error("RegisterUser.Validation",
                     "Email is already taken"));
                }

                var user = new AppUser
                {
                    Email = request.Email,
                    UserName = request.UserName
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    //var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    //string baseUrl = "https://localhost:44353"; 
                    //var confirmationLink = $"{baseUrl}/api/auth/confirm?userId={user.Id}&token={emailConfirmationToken}";
                    //})
                    return Result.Success(new RegisterUserResponse()
                    {
                        UserName = user.UserName,
                        UserEmail = user.Email
                    });
                }

                return Result.Failure<RegisterUserResponse>(new Error("RegisterUser.Error", result.Errors.ToList().First().Description));

            }
            public class NewUserRegistered : Event
            {
                public string userName { get; private set; }
                public string userEmail { get; private set; }


                public NewUserRegistered(string userName, string userEmail)
                {
                  this.userEmail = userEmail;
                  this.userName = userName;
                }
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/auth", async (RegisterUserRequest request, ISender sender, IEventBus bus) =>
            {
                var command = request.Adapt<Register.Command>();

                var result = await sender.Send(command);

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                await bus.Publish(new NewUserRegistered(result.Value.UserName, result.Value.UserEmail), "userregisteredqueue");

                return Results.Ok(result.Value);
            });
        }
    }
}
