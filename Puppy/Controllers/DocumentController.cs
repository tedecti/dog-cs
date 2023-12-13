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
using Puppy.Data;
using Puppy.Repository;
using Puppy.Repository.IRepository;
using Puppy.Services;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDocumentService _documentService;
        private readonly IPetService _petService;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepo;

        public DocumentController(AppDbContext context, IMapper mapper, IFileRepository fileRepo, IPetService petService, IDocumentService documentService)
        {
            _context = context;
            _mapper = mapper;
            _fileRepo = fileRepo;
            _petService = petService;
            _documentService = documentService;
        }

        // GET: api/Document/Pet/1
        [HttpGet]
        [Route("Pet/{petId}")]
        [Authorize]
        public async Task<ActionResult<GetDocumentDto>> GetDocument(int petId)
        {
            var pet = await _petService.GetPetById(petId);
            if (pet == null)
            {
                return NotFound();
            }
            var documents = await _documentService.GetDocumentsByPet(petId);
            var documentDto = _mapper.Map<IEnumerable<ShortDocumentDto>>(documents);
            return Ok(documentDto);
        }

        // GET: api/Document/1
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<GetDocumentDto>> GetDocumentById(int id)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            var document = await _documentService.GetDocumentById(id);
            
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
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            var pet = await _petService.GetPetById(petId);

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