﻿namespace Puppy.Repository.Interfaces
{
    public interface IFileRepository
    {
        Task<string> SaveFile(IFormFile file);
        Task<Stream> GetFile(string fileName);
    }
}