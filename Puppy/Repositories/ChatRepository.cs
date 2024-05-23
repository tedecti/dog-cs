using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Hubs;
using Puppy.Models;

namespace Puppy.Repositories;

public class ChatRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ChatMessage>> GetMessages(string roomId)
    {
        return await _context.ChatMessage.Where(m => m.RoomId == roomId).OrderByDescending(m => m.Timestamp)
            .ToListAsync();
    }
}