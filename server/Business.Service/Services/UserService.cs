using Business.Domain.Interfaces.Repositories;
using Business.Domain.Interfaces.Services;
using Business.Domain.Model;

namespace Business.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsers()
            => await _userRepository.GetUsers();

        public async Task<User> GetUserById(Guid id)
            => await _userRepository.GetUserById(id);

        public async Task<User> GetUserByUsernamePassword(string u, string p)
            => await _userRepository.GetUserByUsernamePassword(u, p);

        public async Task<User> GetUserByEmailPassword(string e, string p)
            => await _userRepository.GetUserByEmailPassword(e, p);

        public async Task<User> GetUserByUsername(string u)
            => await _userRepository.GetUserByUsername(u);

        public async Task<User> GetUserByRefreshToken(string refreshToken)
            => await _userRepository.GetUserByRefreshToken(refreshToken);

        public async Task<User> GetUserByAccessToken(string accessToken)
            => await _userRepository.GetUserByAccessToken(accessToken);

        public async Task<bool> VerifyIfUserExistsByEmail(string e)
            => await _userRepository.VerifyIfUserExistsByEmail(e);

        public async Task PostUser(User u)
        {
            u.UpdateId();
            u.UpdateIsActive(true);
            u.UpdateCreadtedDate();

            await _userRepository.PostUser(u);
        }

        public async Task SaveTokens(User u, string accessToken, string refreshToken)
        {
            u.UpdateAccessToken(accessToken);
            u.UpdateRefreshToken(refreshToken);

            await PutUser(u.Id, u);
        }

        public async Task<User> PutUser(Guid id, User u)
            => await _userRepository.PutUser(id, u);

        public async Task<bool> DeleteUser(Guid id)
            => await _userRepository.DeleteUser(id);

        public async Task<bool> DeleteTokens(Guid id)
        {
            var u = await _userRepository.GetUserById(id);

            if (u != null)
            {
                u.UpdateAccessToken(string.Empty);
                u.UpdateRefreshToken(string.Empty);

                try
                {
                    await PutUser(u.Id, u);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
