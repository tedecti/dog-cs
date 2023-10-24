using AutoMapper;
using Curs.Data;
using Puppy.Repository.IRepository;

namespace Puppy.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;


        public FileRepository(AppDbContext context, IConfiguration configuration, IMapper mapper,
            IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            Guid myuuid = Guid.NewGuid();
            string fName = myuuid.ToString() + "." + file.ContentType.Split("/")[1].ToString();


            string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fName;

        }
    }
}
