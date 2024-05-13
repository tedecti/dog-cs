using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.DocumentDto;
using Puppy.Repositories.Interfaces;
using Puppy.Services;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _documentRepository;
        private readonly IPetRepository _petRepository;

        public DocumentController(IDocumentRepository documentRepository, IMapper mapper, IPetRepository petRepository)
        {
            _mapper = mapper;
            _petRepository = petRepository;
            _documentRepository = documentRepository;
        }

        // GET: api/Document/Pet/1
        [HttpGet]
        [Route("Pet/{petId}")]
        [Authorize]
        public async Task<ActionResult<GetDocumentDto>> GetDocument(int petId)
        {
            var pet = await _petRepository.GetPetById(petId);
            if (pet == null)
            {
                return NotFound();
            }

            var documents = await _documentRepository.GetDocumentsByPet(petId);
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

            var document = await _documentRepository.GetDocumentById(id);

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
            var pet = await _petRepository.GetPetById(petId);

            if (pet == null)
            {
                return NotFound();
            }

            if (userId != Convert.ToInt32(pet.UserId))
            {
                return Forbid();
            }

            await _documentRepository.CreateDocument(document, petId);
            return Ok();
        }

        // PUT: api/Document/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDocument(UpdateDocumentDto updateDocumentDto, int id)
        {
            await _documentRepository.EditDocument(updateDocumentDto, id);
            return NoContent();
        }

        // DELETE: api/Document/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _documentRepository.DeleteDocument(id);
            return NoContent();
        }
    }
}