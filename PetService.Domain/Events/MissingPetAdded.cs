using Shared.Events;
namespace PetService.Domain.Events
{
    public class MissingPetAdded : Event
    {
        public MissingPet AddedPet { get; private set; }

        public MissingPetAdded(MissingPet pet)
        {
            this.AddedPet = pet;
        }
    }
}