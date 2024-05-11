using Puppy.Models;
using Puppy.Models.Dto.PetDtos;

namespace Puppy.Repositories.Interfaces;

public interface IPetRepository
{
    Task<IEnumerable<Pet>> GetPetsByUser(int userId);
    Task<Pet?> EditPet(UpdatePetDto updatePetDto, int petId);
    Task<Pet> PostPet(AddPetRequestDto addPetRequestDto, int userId);
    Task<Pet?> DeletePet(int petId);
    Task<Pet?> GetPetById(int petId);
}