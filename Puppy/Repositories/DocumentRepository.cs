using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.DocumentDto;
using Puppy.Repositories.Interfaces;
using static System.DateTime;

namespace Puppy.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly AppDbContext _context;
    private readonly IFileRepository _fileRepository;

    public DocumentRepository(AppDbContext context, IFileRepository fileRepository)
    {
        _context = context;
        _fileRepository = fileRepository;
    }

    public async Task<Document> CreateDocument(UploadDocumentDto uploadDocumentDto, int petId)
    {
        var imgs = new List<string>();
        foreach (var file in uploadDocumentDto.Imgs)
        {
            imgs.Add(await _fileRepository.SaveFile(file));
        }


        var newDocument = new Document()
        {
            Title = uploadDocumentDto.Title,
            Description = uploadDocumentDto.Description,
            PetId = petId,
            Imgs = imgs.ToArray(),
            UploadDate = UtcNow
        };

        _context.Document.Add(newDocument);
        await _context.SaveChangesAsync();
        return newDocument;
    }

    public async Task<Document?> EditDocument(UpdateDocumentDto updateDocumentDto, int documentId)
    {
        var existingDocument = await GetDocumentById(documentId);
        var imgs = new List<string>();
        foreach (var file in updateDocumentDto.Imgs)
        {
            imgs.Add(await _fileRepository.SaveFile(file));
        }

        if (existingDocument != null)
        {
            existingDocument.Title = updateDocumentDto.Title;
            existingDocument.Description = updateDocumentDto.Description;
            existingDocument.Imgs = imgs.ToArray();

            await _context.SaveChangesAsync();
            return existingDocument;
        }

        return null;
    }

    public async Task<Document?> DeleteDocument(int documentId)
    {
        var document = await GetDocumentById(documentId);
        if (document == null)
        {
            return null;
        }

        _context.Document.Remove(document);
        await _context.SaveChangesAsync();
        return document;
    }

    public async Task<IEnumerable<Document>> GetDocumentsByPet(int petId)
    {
        var documents = await _context.Document.Where(x => x.PetId == petId).ToListAsync();
        return documents;
    }

    public async Task<Document?> GetDocumentById(int documentId)
    {
        var document = await _context.Document.Where(x => x.Id == documentId).Include(x => x.Pet).FirstOrDefaultAsync();
        return document;
    }
}