using Curs.Models;
using Curs.Models.Dto.DocumentDto;

namespace Puppy.Repository.IRepository;

public interface IDocumentRepository
{
    Task<Document> CreateDocument(UploadDocumentDto uploadDocumentDto);
    Task<Document> EditDocument();
    Task<Document> DeleteDocument(int documentId);
}