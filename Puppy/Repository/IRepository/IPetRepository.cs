using Curs.Models;
using Puppy.Models.Dto;

namespace Puppy.Repository.IRepository;

public interface IPetRepository
{
    Task<Pet> EditPet(UpdatePetDto updatePetDto, int petId);
}