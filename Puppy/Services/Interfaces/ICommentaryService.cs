using Curs.Models;

namespace Puppy.Services.Interfaces;

public interface ICommentaryService
{
    Task<IEnumerable<Commentary>> GetCommentaries(int postId);
    Task<int> GetTotal(int postId);
}