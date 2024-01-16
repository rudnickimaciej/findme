using Microsoft.AspNetCore.Http;

namespace PetService.Application.Dtos
{
    public class AddMissingPetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime MissingDate { get; set; }
        public int Gender { get; set; }
        public Guid BreadId { get; set; }
        public List<string> PhotoUrls { get; set; }
    }
}