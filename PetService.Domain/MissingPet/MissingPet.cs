using PetService.Domain.ValueObjects;

namespace PetService.Domain
{
    public class MissingPet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime MissingDate { get; set; }
        public int Gender { get; set; }
        // Foreign Key to Bread
        public Guid BreadId { get; set; }
        public Bread Bread { get; set; }
        // Navigation property for photos
        private List<MissingPetPhoto> _photos = new List<MissingPetPhoto>();
        public IReadOnlyList<MissingPetPhoto> Photos => _photos;

        public void AddPhoto(string photoUrl)
        {
            // Validate parameters if necessary

            // Add logic for adding a photo
            var photo = new MissingPetPhoto(photoUrl);
            _photos.Add(photo);

            // Raise domain events if needed
        }
    }
}