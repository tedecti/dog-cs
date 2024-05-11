using Puppy.Models;
using Puppy.Models.Dto.DocumentDto;

namespace Puppy.Repositories.Interfaces;

public interface IDocumentRepository
{
    Task<Document> CreateDocument(UploadDocumentDto uploadDocumentDto, int petId);
    Task<Document?> EditDocument(UpdateDocumentDto updateDocumentDto, int documentId);
    Task<Document?> DeleteDocument(int documentId);
    Task<IEnumerable<Document>> GetDocumentsByPet(int petId);
    Task<Document?> GetDocumentById(int documentId);
}