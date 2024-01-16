namespace PetService.Domain.ValueObjects
{
    public class MissingPetPhoto
    {
        public Guid Id { get; private set; }
        public string PhotoUrl { get; private set; }

        // Foreign Key property
        public Guid MissingPetId { get; private set; }

        // Navigation property for MissingPet
        public MissingPet MissingPet { get; private set; }

        private MissingPetPhoto() { } // Private constructor for EF Core

        public MissingPetPhoto(string photoUrl)
        {
            // Validate parameters if necessary

            PhotoUrl = photoUrl;
        }
    }

}