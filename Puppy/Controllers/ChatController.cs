using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models;
using Puppy.Repositories.Interfaces;

namespace Puppy.Controllers;

public class ChatController(IChatRepository chatRepository) : Controller
{
    [Authorize]
    [HttpGet("{roomId}")]
    public async Task<IEnumerable<ChatMessage>> GetMessages(string roomId)
    {
        var messages = await chatRepository.GetMessages(roomId);
        return messages;
    }
}