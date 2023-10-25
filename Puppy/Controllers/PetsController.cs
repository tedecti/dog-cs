using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Curs.Data;
using Curs.Models;
using Microsoft.AspNetCore.Authorization;
using Puppy.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Puppy.Repository.IRepository;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepo;

        public PetsController(AppDbContext context, IMapper mapper, IFileRepository fileRepo)
        {
            _context = context;
            _mapper = mapper;
            _fileRepo = fileRepo;
        }

        // GET: api/Pets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetPetDto>>> GetPet()
        {
            if (_context.Pet == null)
            {
                return NotFound();
            }

            var pets = await _context.Pet.Include(p => p.User).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<GetPetDto>>(pets);
            return Ok(dtos);
        }

        // GET: api/Pets/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            if (_context.Pet == null)
            {
                return NotFound();
            }

            var pet = await _context.Pet.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
            {
                return NotFound();
            }

            return pet;
        }

        // PUT: api/Pets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPet(int id, Pet pet)
        {
            if (id != pet.Id)
            {
                return BadRequest();
            }

            _context.Entry(pet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Pet>> PostPet([FromForm] AddPetRequestDto pet)
        {
            if (_context.Pet == null)
            {
                return Problem("Entity set 'AppDbContext.Pet'  is null.");
            }

            var userId = HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
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