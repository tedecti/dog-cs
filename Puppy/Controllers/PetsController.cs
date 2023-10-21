using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Curs.Data;
using Curs.Models;
using Microsoft.AspNetCore.Authorization;
using Puppy.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PetsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPet()
        {
          if (_context.Pet == null)
          {
              return NotFound();
          }
            return await _context.Pet.ToListAsync();
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
            var pet = await _context.Pet.FindAsync(id);

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
        public async Task<ActionResult<Pet>> PostPet(AddPetRequestDto pet)
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

            var newPet = new Pet()
            {
                Name = pet.Name,
                PassportNumber = pet.PassportNumber,
                UserId = Convert.ToInt32(userId)
            };

            _context.Pet.Add(newPet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPet", pet);
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
