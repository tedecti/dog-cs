using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Hubs;
using Puppy.Models;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class ChatRepository(AppDbContext context) : IChatRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ChatMessage>> GetMessages(string roomId)
    {
        return await _context.ChatMessage.Where(m => m.RoomId == roomId).OrderByDescending(m => m.Timestamp)
            .ToListAsync();
    }
}