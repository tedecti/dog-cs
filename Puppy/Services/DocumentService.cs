using Curs.Models;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Services.Interfaces;

namespace Puppy.Services;

public class DocumentService : IDocumentService
{
    private readonly AppDbContext _context;

    public DocumentService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Document>> GetDocumentsByPet(int petId)
    {
        var documents = await _context.Document.Where(x => x.PetId == petId).ToListAsync();
        return documents;
    }

    public async Task<Document> GetDocumentById(int documentId)
    {
        var document = await _context.Document.Where(x => x.Id == documentId).Include(x => x.Pet).FirstOrDefaultAsync();
        return document;
    }
}