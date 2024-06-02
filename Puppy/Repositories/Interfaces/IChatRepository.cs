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
    Task<BiggestRoomDto> GetRoomById(string roomId);
    Task<List<FullRoomDto>> GetRoomsByUser(int userId);
    Task<ChatRoom?> CreateRoom(int user1Id, int user2Id);
    Task<bool> SetMessageRead(int messageId, int userId);
    Task<string?> GetAnyRoomBetweenUsers(int user1, int user2);
}