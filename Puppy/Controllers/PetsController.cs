using AutoMapper;
using Curs.Data;
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
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepo;
        private readonly IPetService _petService;
        private readonly IPetRepository _petRepo;

        public PetsController(AppDbContext context, IMapper mapper, IFileRepository fileRepo, IPetService petService, IPetRepository petRepo)
        {
            _context = context;
            _mapper = mapper;
            _fileRepo = fileRepo;
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
            List<string> imgs = new List<string>();
            foreach (var file in pet.Imgs)
            {
                imgs.Add(await _fileRepo.SaveFile(file));
            }
            
            var newPet = new Pet()
            {
                Name = pet.Name,
                PassportNumber = pet.PassportNumber,
                UserId = Convert.ToInt32(userId),
                Imgs = imgs.ToArray()
            };

            _context.Pet.Add(newPet);
            await _context.SaveChangesAsync();
            
            return StatusCode(201);
        }

        // DELETE: api/Pets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePet(int id)
        {
            if (_context.Pet == null)
            {
                return NotFound();
            }

            var pet = await _context.Pet.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            _context.Pet.Remove(pet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PetExists(int id)
        {
            return (_context.Pet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}