using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.DocumentDto;
using Puppy.Repository.Interfaces;
using Puppy.Services;
using Puppy.Services.Interfaces;

namespace Puppy.Repository;

public class DocumentRepository : IDocumentRepository
{
    private readonly AppDbContext _context;
    private readonly IFileRepository _fileRepository;
    private readonly IDocumentService _documentService;

    public DocumentRepository(AppDbContext context, IFileRepository fileRepository, IDocumentService documentService)
    {
        _context = context;
        _fileRepository = fileRepository;
        _documentService = documentService;
    }
    public async Task<Document> CreateDocument(UploadDocumentDto uploadDocumentDto, int petId)
    {
        List<string> imgs = new List<string>();
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
            UploadDate = DateTime.UtcNow
        };

        _context.Document.Add(newDocument);
        await _context.SaveChangesAsync();
        return newDocument;
    }

    public async Task<Document> EditDocument(UpdateDocumentDto updateDocumentDto, int documentId)
    {
        var existingDocument = await _documentService.GetDocumentById(documentId);
        List<string> imgs = new List<string>();
        foreach (var file in updateDocumentDto.Imgs)
        {
            imgs.Add(await _fileRepository.SaveFile(file));
        }
        existingDocument.Title = updateDocumentDto.Title;
        existingDocument.Description= updateDocumentDto.Description;
        existingDocument.Imgs = imgs.ToArray();
        if (existingDocument == null)
        {
            return null;
        }
        await _context.SaveChangesAsync();
        return existingDocument;
    }

    public async Task<Document> DeleteDocument(int documentId)
    {
        var document = await _documentService.GetDocumentById(documentId);
        if (document == null)
        {
            return null;
        }
        _context.Document.Remove(document);
        await _context.SaveChangesAsync();
        return document;
    }
}