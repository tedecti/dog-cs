using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.PetDtos;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class PetRepository : IPetRepository
{
    private readonly AppDbContext _context;
    private readonly IFileRepository _fileRepo;

    public PetRepository(AppDbContext context, IFileRepository fileRepo)
    {
        _context = context;
        _fileRepo = fileRepo;
    }

    public async Task<Pet?> GetPetById(int petId)
    {
        var pet = await _context.Pet.Include(x => x.Documents).Where(x => x.Id == petId).FirstOrDefaultAsync();
        return pet;
    }

    public async Task<IEnumerable<Pet>> GetPetsByUser(int userId)
    {
        var pets = await _context.Pet.Include(x => x.Documents).Where(x => x.UserId == userId).ToListAsync();
        return pets;
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

    public async Task<Pet?> EditPet(UpdatePetDto updatePetDto, int petId)
    {
        var existingPet = await GetPetById(petId);

        if (existingPet == null) return null;

        var oldPetPhotoList = existingPet.Imgs;

        var newPetPhotoList = new List<string>();
        foreach (var file in updatePetDto.Imgs)
        {
            newPetPhotoList.Add(await _fileRepo.SaveFile(file));
        }

        existingPet.Name = updatePetDto.Name;
        existingPet.PassportNumber = updatePetDto.PassportNumber;
        if (oldPetPhotoList.Length > 0)
        {
            foreach (var oldPhoto in oldPetPhotoList)
            {
                await _fileRepo.DeleteFileFromStorage(oldPhoto);
            }
        }

        existingPet.Imgs = newPetPhotoList.ToArray();

        await _context.SaveChangesAsync();
        return existingPet;
    }

    public async Task<Pet?> DeletePet(int petId)
    {
        var pet = await GetPetById(petId);
        if (pet == null)
        {
            return null;
        }

        _context.Pet.Remove(pet);
        await _context.SaveChangesAsync();
        return pet;
    }
}