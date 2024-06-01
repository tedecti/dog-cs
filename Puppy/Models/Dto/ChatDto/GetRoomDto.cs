using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.ChatDto;

public class GetRoomDto
{
    public string RoomId { get; set; }
    public ShortUserDto User2 { get; set; }
    public List<ShortMessagesDto> ChatMessages { get; set; } 
}
