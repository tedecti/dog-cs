using Curs.Models;
using Curs.Models.Dto.DocumentDto;
using Puppy.Models.Dto.DocumentDto;

namespace Puppy.Repository.IRepository;

public interface IDocumentRepository
{
    Task<Document> CreateDocument(UploadDocumentDto uploadDocumentDto, int petId);
    Task<Document> EditDocument(UpdateDocumentDto updateDocumentDto, int documentId);
    Task<Document> DeleteDocument(int documentId);
}