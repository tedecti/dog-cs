using Puppy.Models;

namespace Puppy.Services.Interfaces;

public interface IDocumentService
{
    Task<IEnumerable<Document>> GetDocumentsByPet(int petId);
    Task<Document> GetDocumentById(int documentId);
    
}