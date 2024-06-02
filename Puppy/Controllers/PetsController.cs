using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PetDtos;
using Puppy.Repositories.Interfaces;
using Puppy.Services;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPetRepository _petRepo;

        public PetsController(IMapper mapper, IPetRepository petRepo)
        {
            _mapper = mapper;
            _petRepo = petRepo;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            var pet = await _petRepo.GetPetById(id);

            if (pet == null)
            {
                return NotFound();
            }

            if (id != pet.Id)
            {
                return NotFound();
            }

            if (userId != pet.UserId)
            {
                return Forbid();
            }

            var responseDto = _mapper.Map<GetPetDto>(pet);
            return Ok(responseDto);
        }

        // GET: api/Pets/1
        [HttpGet("/api/user/{userId}/pets")]
        [Authorize]
        public async Task<ActionResult<Pet>> GetPets(int userId)
        {
            var pets = await _petRepo.GetPetsByUser(userId);

            var responseDto = _mapper.Map<IEnumerable<GetPetDto>>(pets);
            return Ok(responseDto);
        }

        // PUT: api/Pets/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPet(UpdatePetDto updatePetDto, int id)
        {
            var existingPet = await _petRepo.EditPet(updatePetDto, id);
            if (existingPet == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Pets
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Pet>> PostPet([FromForm] AddPetRequestDto pet)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            await _petRepo.PostPet(pet, userId);
            return StatusCode(201);
        }

        // DELETE: api/Pets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePet(int id)
        {
            await _petRepo.DeletePet(id);
            return NoContent();
        }
    }
}