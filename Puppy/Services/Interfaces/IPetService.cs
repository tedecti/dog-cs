using Curs.Models;

namespace Puppy.Services;

public interface IPetService
{
    Task<Pet> GetPetById(int petId);
    Task<IEnumerable<Pet>> GetPetsByUser(int userId);
}