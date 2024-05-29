using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.ChatDto;

public class AllRoomsResponseDto
{
    public ShortRoomDtoU2 Room { get; set; } 
    public ShortMessagesDto Message { get; set; }
}