using Business.Domain.Model;

namespace Business.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByUsernamePassword(string u, string p);
        Task<User> GetUserByEmailPassword(string e, string p);
        Task<User> GetUserByUsername(string u);
        Task<User> GetUserByRefreshToken(string refreshToken);
        Task<User> GetUserByAccessToken(string accessToken);
        Task<bool> VerifyIfUserExistsByEmail(string e);
        Task PostUser(User user);
        Task<User> PutUser(Guid id, User user);
        Task<bool> DeleteUser(Guid id);
    }
}
