using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Hubs;
using Puppy.Models;
using Puppy.Models.Dto.ChatDto;
using Puppy.Models.Dto.UserDtos;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class ChatRepository(AppDbContext context) : IChatRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<ChatMessage>> GetMessages(string roomId)
    {
        return await _context.ChatMessage.Where(m => m.RoomId == roomId).OrderByDescending(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<ChatMessage?> CreateMessage(string roomId, int userId, SendMessageDto sendMessageDto)
    {
        var newMessage = new ChatMessage()
        {
            RoomId = roomId,
            UserId = userId,
            Message = sendMessageDto.Message,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };
        _context.ChatMessage.Add(newMessage);
        await _context.SaveChangesAsync();
        return newMessage;
    }

    public async Task<ChatMessage?> EditMessage(EditMessageDto editMessageDto, int messageId)
    {
        var existingMessage = await GetMessageById(messageId);
        if (existingMessage != null) existingMessage.Message = editMessageDto.Message;
        await _context.SaveChangesAsync();
        return existingMessage;
    }

    public async Task<ChatMessage?> DeleteMessage(int messageId)
    {
        var message = await GetMessageById(messageId);
        _context.ChatMessage.Remove(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<ChatMessage?> GetMessageById(int messageId)
    {
        return await _context.ChatMessage.FirstOrDefaultAsync(m => m.Id == messageId);
    }

    public async Task<List<AllRoomsResponseDto>> GetAllRooms()
    {
        var rooms = await _context.ChatRoom
            .Include(c => c.ChatMessages)
            .Include(c => c.User2)
            .ToListAsync();

        var roomDtos = rooms.Select<ChatRoom, AllRoomsResponseDto>(room =>
        {
            var latestMessage = room.ChatMessages
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefault();

            var roomDto = new ShortRoomDtoU2
            {
                RoomId = room.RoomId,
                User2 = new ShortUserDto
                {
                    Id = room.User2.Id,
                    Avatar = room.User2.Avatar,
                    Username = room.User2.Username,
                    FirstName = room.User2.FirstName,
                    LastName = room.User2.LastName
                }
            };

            var messageDto = latestMessage == null
                ? null
                : new ShortMessagesDto
                {
                    Id = latestMessage.Id,
                    UserId = latestMessage.UserId,
                    Message = latestMessage.Message,
                    Timestamp = latestMessage.Timestamp
                };

            return new AllRoomsResponseDto
            {
                Room = roomDto,
                Message = messageDto
            };
        }).ToList();
        return roomDtos;
    }

    public async Task<ChatRoom> GetRoomById(string roomId)
    {
        return await _context.ChatRoom
            .Include(c => c.ChatMessages)
            .Include(c => c.User2)
            .Where(c => c.RoomId == roomId)
            .FirstOrDefaultAsync();
    }

    public async Task<ChatRoom?> CreateRoom(int user1Id, int user2Id)
    {
        
        var roomId = GenerateRoomId(user1Id, user2Id);
        var newRoom = new ChatRoom()
        {
            RoomId = roomId,
            User1Id = user1Id,
            User2Id = user2Id,
        };
        _context.ChatRoom.Add(newRoom);
        await _context.SaveChangesAsync();
        return newRoom;
    }

    public static string GenerateRoomId(int userId1, int userId2)
    {
        var ids = new int[] { userId1, userId2 };
        Array.Sort(ids);

        var randomPart = GenerateRandomString(8);

        var roomId = $"{ids[0]}_{ids[1]}_{randomPart}";

        return roomId;
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        using (var rng = new RNGCryptoServiceProvider())
        {
            var buffer = new byte[length];
            rng.GetBytes(buffer);
            var stringBuilder = new StringBuilder(length);
            foreach (var b in buffer)
            {
                stringBuilder.Append(chars[b % chars.Length]);
            }

            return stringBuilder.ToString();
        }
    }

    public async Task<ChatRoom?> GetRoomByUser(int userId)
    {
        
        var response = await context.ChatRoom
            .Where(c => c.User1Id == userId || c.User2Id==userId)
            .Include(c => c.ChatMessages)
            .Include(c => c.User1)
            .Include(c=>c.User2).FirstOrDefaultAsync();
        return response ?? null;
    }

    public async Task<bool> SetMessageRead(int messageId, int userId)
    {
        var message = await context.ChatMessage.FirstOrDefaultAsync(m => m.Id == messageId);
        if (message == null) return false;
        message.IsRead = true;
        await context.SaveChangesAsync();
        return true;
    }
}