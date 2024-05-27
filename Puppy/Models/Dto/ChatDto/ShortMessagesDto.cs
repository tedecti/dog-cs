namespace Puppy.Models.Dto.ChatDto;

public class ShortMessagesDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}