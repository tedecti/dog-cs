using Puppy.Models;

namespace Puppy.Repositories.Interfaces;

public interface IChatRepository
{
    Task<IEnumerable<ChatMessage>> GetMessages(string roomId);
}