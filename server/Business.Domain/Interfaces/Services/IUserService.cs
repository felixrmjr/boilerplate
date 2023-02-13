using Business.Domain.Model;

namespace Business.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByUsernamePassword(string u, string p);
        Task<User> GetUserByEmailPassword(string e, string p);
        Task<User> GetUserByUsername(string u);
        Task<User> GetUserByRefreshToken(string refreshToken);
        Task<User> GetUserByAccessToken(string accessToken);
        Task<bool> VerifyIfUserExistsByEmail(string e);
        Task PostUser(User u);
        Task SaveTokens(User u, string accessToken, string refreshToken);
        Task<User> PutUser(Guid id, User u);
        Task<bool> DeleteUser(Guid id);
        Task<bool> DeleteTokens(Guid id);
    }
}
