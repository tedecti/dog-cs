using Curs.Data;
using Curs.Models;
using Microsoft.EntityFrameworkCore;

namespace Puppy.Services;

public class PetService : IPetService
{
    private readonly AppDbContext _context;

    public PetService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Pet>> GetPetsByUser(int userId)
    {
        var pets = await _context.Pet.Include(x => x.Documents).Where(x => x.UserId == userId).ToListAsync();
        return pets;
    }

    public async Task<Pet> GetPetById(int petId)
    {
        var pet = await _context.Pet.Include(x => x.Documents).Where(x => x.Id == petId).FirstOrDefaultAsync();
        return pet;
    }
}