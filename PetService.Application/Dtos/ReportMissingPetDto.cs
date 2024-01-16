using Microsoft.AspNetCore.Http;

namespace PetService.Application.Dtos
{
    public class ReportMissingPetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime MissingDate { get; set; }
        public int Gender { get; set; }
        public Guid BreadId { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}