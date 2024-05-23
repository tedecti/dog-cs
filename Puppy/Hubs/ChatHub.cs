using Microsoft.AspNetCore.SignalR;
using Puppy.Data;
using Puppy.Models;
using static System.DateTime;

namespace Puppy.Hubs;

public class ChatHub(AppDbContext context) : Hub
{
    private readonly AppDbContext _context = context;
    public async Task SendMessage(string roomId, string msg)
    {
        var user = Convert.ToInt32(Context.User?.Identity?.Name);
        var chatMessage = new ChatMessage
        {
            RoomId = roomId,
            UserId = user,
            Message = msg,
            Timestamp = UtcNow
        };

        _context.ChatMessage.Add(chatMessage);
        await _context.SaveChangesAsync();
        await Clients.Group(roomId).SendAsync("ReceiveMessage", user, msg);
    }

    public async Task JoinRoom(int user2)
    {
        var user1 = Convert.ToInt32(Context.User?.Identity?.Name);
        {
            var roomId = GenerateRoomId(user1, user2);
            var room = await _context.ChatRoom.FindAsync(roomId);

            if (room == null)
            {
                room = new ChatRoom
                {
                    RoomId = roomId,
                    User1Id = user1,
                    User2Id = user2
                };
                _context.ChatRoom.Add(room);
                await _context.SaveChangesAsync();
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
    }
    private static string GenerateRoomId(int user1, int user2)
    {
        var users = new[] { user1, user2 };
        Array.Sort(users);
        return string.Join("-", users);
    }
}

