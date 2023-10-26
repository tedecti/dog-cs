using AutoMapper;
using Curs.Data;
using Curs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileRepository _fileRepo;
        private readonly IMapper _mapper;

        public PostController(AppDbContext context, IWebHostEnvironment environment, IFileRepository fileRepo, IMapper mapper)
        {
            _context = context;
            _environment = environment;
            _fileRepo = fileRepo;
            _mapper = mapper;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPost()
        {
            if (_context.Post == null)
            {
                return NotFound();
            }
            
            var posts = await _context.Post.Include(p => p.User).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<GetPostDto>>(posts);

            return Ok(dtos);
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPostDto>> GetPost(int id)
        {
            if (_context.Post == null)
            {
                return NotFound();
            }
            var post = await _context.Post.Include(p => p.User).FirstAsync(p=> p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            
            var dtos = _mapper.Map<GetPostDto>(post);
            
            return Ok(dtos);
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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


        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> PostPost([FromForm] UploadPostRequestDto post)
        {
            var userId = HttpContext.User.Identity.Name;
            int userIdInt = Convert.ToInt32(userId);
            if (userIdInt == null)
            {
                return Unauthorized();
            }

            if (_context.Post == null)
            {
                return Problem("Entity set 'AppDbContext.Post'  is null.");
            }

            List<string> imgs = new List<string>();
            foreach (var file in post.Imgs)
            {
                imgs.Add(await _fileRepo.SaveFile(file));
            }
            

            var newPost = new Post()
            {
                Title = post.Title,
                Description = post.Description,
                UserId = userIdInt,
                Imgs = imgs.ToArray(),
                UploadDate = DateTime.UtcNow
            };
            _context.Post.Add(newPost);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return (_context.Post?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}