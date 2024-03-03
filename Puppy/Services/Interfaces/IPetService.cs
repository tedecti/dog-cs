using Puppy.Models;

namespace Puppy.Services.Interfaces;

public interface IPetService
{
    Task<Pet> GetPetById(int petId);
    Task<IEnumerable<Pet>> GetPetsByUser(int userId);
}