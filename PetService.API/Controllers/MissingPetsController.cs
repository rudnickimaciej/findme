using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetService.API.FileAPI;
using PetService.Application.Dtos;
using PetService.Application.MissingPets;
using PetService.Domain;
using PetService.Domain.Events;
using Shared.Bus;

namespace PetService.API.Controllers;

public class MissingPetsController : ApiBaseController
{
    private readonly IEventBus _bus;
    private readonly IMapper _mapper;

    private readonly IUploadFilesService _fileService;

    public MissingPetsController(IEventBus bus, IUploadFilesService fileService, IMapper mapper)
    {
        _bus = bus;
        _mapper = mapper;
        _fileService = fileService;
    }

    [HttpGet] //api/missingpets
    [AllowAnonymous]
    public async Task<ActionResult<List<MissingPet>>> GetPets()
    {
        return await Mediator.Send(new List.Query());
    }


    [HttpGet("{id}")] //api/missingpets/43fdsdf43
    public async Task<IActionResult> GetPet(Guid id)
    {
        var result = await Mediator.Send(new Details.Query { Id = id });
        return result.Match(
           pet => (IActionResult)Ok(pet),
           () => NotFound($"Pet with id {id} was not found.")
       );
    }

    [HttpPost] //api/missingpets
    [ProducesResponseType(typeof(ReportMissingPetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddMissingPet(ReportMissingPetDto pet)
    {
        try
        {
            // Attempt to upload photos using the file service
            var uploadResult = await _fileService.PostAsync(pet.Photos);
            if (uploadResult is null)
            {
                return Problem("Something went wrong when uploading images");
            }

            AddMissingPetDto addDto = new();
            addDto.Id = Guid.NewGuid();
            addDto.PhotoUrls = uploadResult.PhotoUrls;
            _mapper.Map(pet, addDto);

            var result = await Mediator.Send(new Create.Command { MissingPet = addDto });

            MissingPet pe = new();
            _mapper.Map(addDto, pe);
            return result.Match(
                p =>
                {
                    try
                    {
                        this._bus.Publish(new MissingPetAdded(pe));
                    }
                    catch (Exception e)
                    {
                        return Problem(e.Message);
                    }
                    return CreatedAtAction(nameof(GetPet), new { id = p.Id }, pet);
                },
                (e) => Problem(e.Message, statusCode: e.StateCode)
            );
        }
        catch (Exception ex)
        {
            // Handle any unexpected exceptions
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private void Log()
    {
        Console.WriteLine("Log");
    }
    [HttpPut("{id}")] //api/missingpets
    public async Task<IActionResult> EditPet(Guid id, MissingPet pet)
    {
        pet.Id = id;
        var result = await Mediator.Send(new Edit.Command()
        {
            MissingPet = pet
        });
        return result.Match(
                  (e) => Problem(e.Message, statusCode: StatusCodes.Status404NotFound),
                  (e) => Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError),
                  p => CreatedAtAction(nameof(GetPet), new { id = p.Id }, pet)
                );
    }

    [HttpDelete("{id}")] //api/missingpets
    public async Task<IActionResult> DeletePet(Guid id)
    {
        var result = await Mediator.Send(new Delete.Command() { Id = id });
        return result.Match(
            (e) => Problem(e.Message, statusCode: StatusCodes.Status404NotFound),
            p => (IActionResult)Ok()
        );
    }
}
