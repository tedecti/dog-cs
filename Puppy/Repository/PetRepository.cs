using Curs.Data;
using Curs.Models;
using Puppy.Data;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PetDtos;
using Puppy.Repository.IRepository;
using Puppy.Services;

namespace Puppy.Repository;

public class PetRepository : IPetRepository
{
    private readonly IPetService _petService;
    private readonly AppDbContext _context;
    private readonly IFileRepository _fileRepo;

    public PetRepository(IPetService petService, AppDbContext context, IFileRepository fileRepo)
    {
        _petService = petService;
        _context = context;
        _fileRepo = fileRepo;
    }

    public async Task<Pet> PostPet(AddPetRequestDto addPetRequestDto, int userId)
    {
        var imgs = new List<string>();
        foreach (var file in addPetRequestDto.Imgs)
        {
            imgs.Add(await _fileRepo.SaveFile(file));
        }
            
        var newPet = new Pet()
        {
            Name = addPetRequestDto.Name,
            PassportNumber = addPetRequestDto.PassportNumber,
            UserId = Convert.ToInt32(userId),
            Imgs = imgs.ToArray()
        };

        _context.Pet.Add(newPet);
        await _context.SaveChangesAsync();
        return newPet;
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

    public async Task<Pet> DeletePet(int petId)
    {
        var pet = await _petService.GetPetById(petId);
        if (pet == null)
        {
            return null;
        }
        _context.Pet.Remove(pet);
        await _context.SaveChangesAsync();
        return pet;
    }
}