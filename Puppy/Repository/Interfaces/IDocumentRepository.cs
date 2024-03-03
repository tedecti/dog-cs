using Puppy.Models;
using Puppy.Models.Dto.DocumentDto;

namespace Puppy.Repository.Interfaces;

public interface IDocumentRepository
{
    Task<Document> CreateDocument(UploadDocumentDto uploadDocumentDto, int petId);
    Task<Document> EditDocument(UpdateDocumentDto updateDocumentDto, int documentId);
    Task<Document> DeleteDocument(int documentId);
}