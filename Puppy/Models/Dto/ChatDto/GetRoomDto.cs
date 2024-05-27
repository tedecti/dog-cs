namespace Puppy.Models.Dto.ChatDto;

public class GetRoomDto
{
    public string RoomId { get; set; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public List<ShortMessagesDto> ChatMessages { get; set; } 
}
