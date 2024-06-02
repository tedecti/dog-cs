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
        
        if (chat is null && chat.User1.Id != userId)
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
        if (chat is null && chat.User1.Id != userId)
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
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _chatRepository.GetAllRooms();
        var response = _mapper.Map<List<AllRoomsResponseDto>>(rooms);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetRoomById(string roomId)
    {
        var room = await _chatRepository.GetRoomById(roomId);
        var response = _mapper.Map<BiggestRoomDto>(room);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetRoomByUser(int userId)
    {
        var room = await _chatRepository.GetRoomsByUser(userId);
        var response = _mapper.Map<List<FullRoomDto>>(room);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("room/{userId2}")]
    public async Task<IActionResult> CreateRoom(int userId2)
    {
        var userId1 = Convert.ToInt32(HttpContext.User.Identity?.Name);
        var existingRoom = await _chatRepository.GetAnyRoomBetweenUsers(userId1, userId2);
        if (existingRoom != null)
        {
            return Ok(new {RoomId = existingRoom});
        }
        var room = await _chatRepository.CreateRoom(userId1, userId2);
        if (room == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось создать комнату");
        }
        var newRoom = await _chatRepository.GetAnyRoomBetweenUsers(userId1, userId2);
        return StatusCode(StatusCodes.Status201Created, value: newRoom);
    }

    [Authorize]
    [HttpPut("message/{messageId}/read")]
    public async Task<IActionResult> ReadMessage(int messageId)
    {
        var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        var msg = await _chatRepository.SetMessageRead(messageId, userId);
        if (!msg)
        {
            return NotFound();
        }
        return NoContent();
    }

    [Authorize]
    [HttpGet("check/{userId2}")]
    public async Task<IActionResult> CheckIfChatExists(int userId2)
    {
        var userId1 = Convert.ToInt32(HttpContext.User.Identity?.Name);
        var existingRoom = await _chatRepository.GetAnyRoomBetweenUsers(userId1, userId2);
        if (existingRoom != null)
        {
            return Ok(new {RoomId = existingRoom});
        }
        return NotFound();
    }
}