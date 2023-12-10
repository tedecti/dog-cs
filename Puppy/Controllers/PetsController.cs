using AutoMapper;
using Curs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;
using Puppy.Services;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPetService _petService;
        private readonly IPetRepository _petRepo;

        public PetsController(IMapper mapper, IPetService petService, IPetRepository petRepo)
        {
            _mapper = mapper;
            _petService = petService;
            _petRepo = petRepo;
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {

            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        
            var pet = await _petService.GetPetById(id);

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
        [HttpGet("/api/User/{userId}/Pets")]
        [Authorize]
        public async Task<ActionResult<Pet>> GetPets(int userId)
        {
            var pets = await _petService.GetPetsByUser(userId);

            var responseDto = _mapper.Map<IEnumerable<GetPetDto>>(pets);
            return Ok(responseDto);
        }

        // PUT: api/Pets/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPet(UpdatePetDto updatePetDto, int petId)
        {
            var existingPet = await _petRepo.EditPet(updatePetDto, petId);
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
            var pet = await _petRepo.DeletePet(id);
            if (pet == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}