using Puppy.Models;
using Puppy.Models.Dto.ChatDto;

namespace Puppy.Repositories.Interfaces;

public interface IChatRepository
{
    Task<List<ChatMessage>> GetMessages(string roomId);
    Task<ChatMessage?> CreateMessage(string roomId, int userId, SendMessageDto sendMessageDto);
    Task<ChatMessage?> EditMessage(EditMessageDto editMessageDto, int messageId);
    Task<ChatMessage?> DeleteMessage(int messageId);
    Task<ChatMessage?> GetMessageById(int messageId);
    Task<List<AllRoomsResponseDto>> GetAllRooms();
    Task<ChatRoom> GetRoomById(string roomId);
    Task<List<ChatRoom>> GetRoomByUser(int userId);
    Task<ChatRoom?> CreateRoom(int user1Id, int user2Id);
    Task<bool> SetMessageRead(int messageId, int userId);
}