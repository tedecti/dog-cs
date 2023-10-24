namespace Puppy.Repository.IRepository
{
    public interface IFileRepository
    {
        Task<string> SaveFile(IFormFile img);
    }
}