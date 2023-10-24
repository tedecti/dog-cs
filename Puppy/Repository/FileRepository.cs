using AutoMapper;
using Curs.Data;
using Puppy.Repository.IRepository;

namespace Puppy.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FileRepository(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<string> SaveFile(IFormFile img)
        {
            throw new NotImplementedException();
        }
    }
}