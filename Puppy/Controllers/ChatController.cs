using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models;
using Puppy.Models.Dto.ChatDto;
using Puppy.Repositories.Interfaces;

namespace Puppy.Controllers;
[ApiController]
[Route("api/chat")]
public class ChatController(IChatRepository _chatRepository, IMapper _mapper) : Controller
{
    [Authorize]
    [HttpGet("{roomId}")]
    public async Task<ActionResult<IEnumerable<ShortMessagesDto>>> GetMessagesByRoom(string roomId)
    {
        var messages = await _chatRepository.GetMessages(roomId);
        var response = _mapper.Map<List<ShortMessagesDto>>(messages);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("{roomId}/message")]
    public async Task<IActionResult> SendMessage(string roomId, [FromBody]SendMessageDto sendMessageDto)
    {
        var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        await _chatRepository.CreateMessage(roomId, userId, sendMessageDto);
        return StatusCode(201);
    }

    [Authorize]
    [HttpPut("{roomId}/message/{messageId}")]
    public async Task<IActionResult> EditMessage(string roomId, int messageId, EditMessageDto editMessageDto)
    {
        var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        var chat = await _chatRepository.GetRoomById(roomId);
        var message = await _chatRepository.GetMessageById(messageId);

        if(message.UserId != userId)
        {
            return Unauthorized();
        }
        
        if (chat is null && chat.User1Id != userId)
        {
            return Unauthorized();
        }
            
        await _chatRepository.EditMessage(editMessageDto, messageId);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{roomId}/message/{messageId}")]
    public async Task<IActionResult> DeleteMessage(string roomId, int messageId)
    {
        var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        var chat = await _chatRepository.GetRoomById(roomId);
        if (chat is null && chat.User1Id != userId)
        {
            return Unauthorized();
        }
        
        var message = await _chatRepository.GetMessageById(messageId);

        if(message.UserId != userId)
        {
            return Unauthorized();
        }
        
        await _chatRepository.DeleteMessage(messageId);
        return NoContent();
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShortRoomDto>>> GetAllRooms()
    {
        var rooms = await _chatRepository.GetAllRooms();
        var response = _mapper.Map<IEnumerable<ShortRoomDto>>(rooms);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("room/{roomId}")]
    public async Task<ActionResult<List<GetRoomDto>>> GetRoomById(string roomId)
    {
        var room = await _chatRepository.GetRoomById(roomId);
        var response = _mapper.Map<GetRoomDto>(room);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("room/{userId2}")]
    public async Task<IActionResult> CreateRoom(int userId2)
    {
        var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        await _chatRepository.CreateRoom(userId, userId2);
        return StatusCode(201);
    }
}