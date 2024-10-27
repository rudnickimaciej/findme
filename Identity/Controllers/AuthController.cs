using System.Security.Claims;
using System.Text;
using Azure.Messaging.ServiceBus;
using Identity.DTOs;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Identity.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public UserManager<AppUser> _userManager { get; }
        public TokenService _tokenService { get; }

        //private readonly IEmailSender _emailSender;
        //private readonly IRabbitMqPublisher _rabbitPublisher;


        public AuthController(
            //UserManager<AppUser> userManager,
        TokenService tokenService
        //IRabbitMqPublisher rabbitPublisher
        )
        {
            this._tokenService = tokenService;
            //this._userManager = userManager;
            //this._emailSender = emailSender;
            //this._rabbitPublisher = rabbitPublisher;

            // ConnectionFactory factory = new();
            // factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            // factory.ClientProvidedName = "Rabbit Sender App";

            // var connection = factory.CreateConnection();
            // _channel = connection.CreateModel();
            // string exchangeName = "DemoExchange";
            // string routingKey = "demo-routing-key";
            // string queueName = "email_queue";
            // _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
            // _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            // _channel.QueueBind(queueName, exchangeName, routingKey);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            return CreateUserObject(new AppUser() { 
                DisplayName="Test User", 
                UserName = "Test User", 
                Email = "test@gmail.com", 
                Id = Guid.NewGuid().ToString()});

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
                return CreateUserObject(user);

            return Unauthorized();
        }

        [HttpPost("register")]
        [AllowAnonymous]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            return Ok("register sucessfull");

            return await RegisterWithEmailConfirmation(registerDto);
            // if (_apiOptions.VerifyEmail)
            // if (true)
            //     return await RegisterWithEmailConfirmation(registerDto);

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                return BadRequest("Username is already taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                RegistrationDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);


            if (result.Succeeded)
            {
                var userDto = CreateUserObject(user);
                object rabbitPayload = new
                {
                    Email = user.Email,
                    UserName = user.DisplayName
                };
                var message = JsonConvert.SerializeObject(rabbitPayload);
                //this._rabbitPublisher.PublishMessage(exchangeName: "DemoExchange", routingKey: "demo-routing-key", message: message, eventType: null);
                // _channel.BasicPublish(exchange: "DemoExchange", routingKey: "demo-routing-key", basicProperties: null, body: body);

                // var rabbitMQPublisher = new RabbitMQPublisher("your-rabbitmq-host", 5672, "guest", "guest");
                // rabbitMQPublisher.PublishMessage("DemoExchange", "demo-routing-key", "YourMessage", "EventType");


                return userDto;
            }
            return BadRequest(result.Errors);
        }
        private async Task<ActionResult> RegisterWithEmailConfirmation(RegisterDto registerDto)
        {

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                return BadRequest("Username is already taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = emailConfirmationToken }, Request.Scheme);

                //await _emailSender.SendEmailAsync(user.Email, "Confirm Your Email",
                //            $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.");
                await SendRegistrationEmail(user.Email, confirmationLink);
                return Ok("Registration successful. Please check your email for confirmation instructions.");

            }
            return BadRequest(result.Errors);


        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                // Handle the case where user ID or token is null
                return BadRequest("User ID or token is null.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Handle the case where user is not found
                return BadRequest($"Unable to find user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                // Email successfully confirmed
                // You can redirect to a success page or return a message
                return Ok("Email confirmed successfully.");
            }
            else
            {
                // Failed to confirm email
                // You can redirect to an error page or return a message
                return BadRequest("Failed to confirm email.");
            }
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<ActionResult<UserDto>> GetCurrentUser()
        //{
        //    var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
        //    return CreateUserObject(user);
        //}

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }

        private async Task SendRegistrationEmail(string registeredUserEmail, string confirmationLink)
        {
            string connectionString = "Endpoint=sb://findpet.servicebus.windows.net/;SharedAccessKeyName=testpolicy;SharedAccessKey=q4xXd4Oo1AH4uyx8vzTsEZUrLppkHTotp+ASbNV+fNY=;EntityPath=sendemailqueue";
            string queueName = "sendemailqueue";
            await using (var client = new ServiceBusClient(connectionString))
            {
                // Create a ServiceBusSender instance
                var sender = client.CreateSender(queueName);

                // Create a message
                var message = new EmailServiceBusMessage()
                {
                    EmailBody = $"Click this link {confirmationLink} to finish registration",
                    EmailSubject = "FindPet registration",
                    EmailSender = "mrrudnickiunity@gmail.com",
                    EmailReceiver = registeredUserEmail
                };
                string messageSerialized = JsonConvert.SerializeObject(message);


                // Send the message to the queue
                await sender.SendMessageAsync(new ServiceBusMessage(messageSerialized));

                Console.WriteLine("Message sent successfully.");
            }
        }
    }
}