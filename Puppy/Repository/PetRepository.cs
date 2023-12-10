using Curs.Data;
using Curs.Models;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;
using Puppy.Services;

namespace Puppy.Repository;

public class PetRepository : IPetRepository
{
    private readonly IPetService _petService;
    private readonly AppDbContext _context;

    public PetRepository(IPetService petService, AppDbContext context)
    {
        _petService = petService;
        _context = context;
    }
    public async Task<Pet> EditPet(UpdatePetDto updatePetDto, int petId)
    {
        var existingPet = await _petService.GetPetById(petId);
        existingPet.Name = updatePetDto.Name;
        existingPet.PassportNumber = updatePetDto.PassportNumber;
        if (existingPet == null)
        {
            return null;
        }

        await _context.SaveChangesAsync();
        return existingPet;
    }
}