using Business.Database;
using Business.Domain.Interfaces.Repositories;
using Business.Domain.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Business.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppMongoDbContext _context;

        public UserRepository(IOptions<MongoConnection> settings)
        {
            _context = new AppMongoDbContext(settings);
        }

        public async Task<IEnumerable<User>> GetUsers()
            => await _context.Users.Find(_ => true).ToListAsync();

        public async Task<User> GetUserById(Guid id)
            => await _context.Users.Find(doc => doc.Id == id).FirstOrDefaultAsync();

        public async Task<User> GetUserByUsernamePassword(string u, string p)
            => await _context.Users.Find(doc => doc.Username == u && doc.Password == p).FirstOrDefaultAsync();

        public async Task<User> GetUserByEmailPassword(string e, string p)
            => await _context.Users.Find(doc => doc.Email == e && doc.Password == p).FirstOrDefaultAsync();

        public async Task<User> GetUserByUsername(string u)
            => await _context.Users.Find(doc => doc.Username == u).FirstOrDefaultAsync();

        public async Task<User> GetUserByRefreshToken(string refreshToken)
            => await _context.Users.Find(doc => doc.RefreshToken == refreshToken).FirstOrDefaultAsync();

        public async Task<User> GetUserByAccessToken(string accessToken)
            => await _context.Users.Find(doc => doc.AccessToken == accessToken).FirstOrDefaultAsync();

        public async Task<bool> VerifyIfUserExistsByEmail(string e)
            => await _context.Users.Find(doc => doc.Email == e).AnyAsync();

        public async Task PostUser(User u)
            => await _context.Users.InsertOneAsync(u);

        public async Task<User> PutUser(Guid id, User u)
        {
            IMongoCollection<User> users = _context.Users;

            Expression<Func<User, bool>> filter = x => x.Id.Equals(id);

            User user = await users.Find(filter).FirstOrDefaultAsync();

            if (user != null)
            {
                user.UpdateUsername(u.Username);
                user.UpdatePassword(u.Password);
                user.UpdateAccessToken(u.AccessToken);
                user.UpdateRefreshToken(u.RefreshToken);

                ReplaceOneResult result = users.ReplaceOne(filter, user);

                return user;
            }
            else
                throw new KeyNotFoundException();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            DeleteResult actionResult = await _context.Users.DeleteManyAsync(n => n.Id.Equals(id));

            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }
    }
}
