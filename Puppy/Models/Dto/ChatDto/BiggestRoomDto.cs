using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.ChatDto;

public class BiggestRoomDto
{
    public string? RoomId { get; set; }
    public ShortUserDto User1 { get; set; }
    public ShortUserDto User2 { get; set; }
    public List<ShortMessagesDto> Messages { get; set; }
}