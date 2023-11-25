namespace Puppy.Repository.IRepository
{
    public interface IFileRepository
    {
        Task<string> SaveFile(IFormFile file);
        Task<Stream> GetFile(string fileName);
    }
}