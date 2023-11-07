using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Curs.Data;
using Curs.Models;
using Curs.Models.Dto.DocumentDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Repository;
using Puppy.Repository.IRepository;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepo;

        public DocumentController(AppDbContext context, IMapper mapper, IFileRepository fileRepo)
        {
            _context = context;
            _mapper = mapper;
            _fileRepo = fileRepo;
        }

        // GET: api/Document/Pet/1
        [HttpGet]
        [Route("Pet/{petId}")]
        [Authorize]
        public async Task<ActionResult<GetDocumentDto>> GetDocument(int petId)
        {
            if (petId == null)
            {
                return NotFound();
            }

            var documents = await _context.Document.Where(x => x.PetId == petId).ToListAsync();
            var documentDto = _mapper.Map<IEnumerable<ShortDocumentDto>>(documents);
            return Ok(documentDto);
        }

        // GET: api/Document/1
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<GetDocumentDto>> GetDocumentById(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            
            var document = await _context.Document.Where(x => x.Id == id).Include(x => x.Pet).FirstOrDefaultAsync();
            
            if (document == null)
            {
                return NotFound();
            }
            if (id != document.Id)
            {
                return NotFound();
            }

            if (document.Pet.UserId != userId)
            {
                return Forbid();
            }

            var documentDto = _mapper.Map<GetDocumentDto>(document);
            return Ok(documentDto);
        }

        // POST: api/Document/1
        [HttpPost]
        [Route("{petId}")]
        [Authorize]
        public async Task<ActionResult<Document>> PostDocument(int petId, [FromForm] UploadDocumentDto document)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            var pet = await _context.Pet.FindAsync(petId);

            if (pet == null)
            {
                return NotFound();
            }
            if (userId != Convert.ToInt32(pet.UserId))
            {
                return Forbid();
            }
            List<string> imgs = new List<string>();
            foreach (var file in document.Imgs)
            {
                imgs.Add(await _fileRepo.SaveFile(file));
            }
            

            var newDocument = new Document()
            {
                Title = document.Title,
                Description = document.Description,
                PetId = petId,
                Imgs = imgs.ToArray(),
                UploadDate = DateTime.UtcNow
            };

            _context.Document.Add(newDocument);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Document/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Document/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}