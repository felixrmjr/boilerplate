using Business.Database;
using Business.Domain.Interfaces.Repositories;
using Business.Domain.Model;
using Business.Domain.Models.Others;
using Microsoft.Extensions.Options;

namespace Business.Repository.Repositories
{
    public class LogRequestRepository : ILogRequestRepository
    {
        private readonly AppMongoDbContext _context;

        public LogRequestRepository(IOptions<MongoConnection> settings)
        {
            _context = new AppMongoDbContext(settings);
        }

        public async Task Post(LogRequest l)
            => await _context.Logs.InsertOneAsync(l);
    }
}
