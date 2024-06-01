using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.ChatDto;

public class FullRoomDto
{
    public string RoomId { get; set; }
    public ShortUserDto User1 { get; set; }
    public ShortUserDto User2 { get; set; }
    public ShortMessagesDto Message { get; set; }
}