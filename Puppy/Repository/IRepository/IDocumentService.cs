using Curs.Models;
using Curs.Models.Dto.DocumentDto;

namespace Puppy.Repository.IRepository;

public interface IDocumentService
{
    Task<Document> GetDocumentByPet(int petId);
    Task<Document> GetDocumentById(int documentId);
    Task<Document> CreateDocument(UploadDocumentDto uploadDocumentDto);
    Task<Document> EditDocument();
    Task<Document> DeleteDocument(int documentId);
    
}