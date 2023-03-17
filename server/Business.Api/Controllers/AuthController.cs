using AutoMapper;
using Business.Domain.Interfaces.Services;
using Business.Domain.Model;
using Business.Domain.Model.DTO;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IValidator<User> _validator;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMemoryCache _cache;

        public AuthController(IMapper mapper,
                              IValidator<User> validator,
                              IUserService userService,
                              ITokenService tokenService,
                              IMemoryCache cache)
        {
            _mapper = mapper;
            _validator = validator;
            _userService = userService;
            _tokenService = tokenService;
            _cache = cache;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserDTO dto)
        {
            User user = _mapper.Map<User>(dto);
            ValidationResult resultModel = await _validator.ValidateAsync(user);

            if (!resultModel.IsValid) return BadRequest(Results.ValidationProblem(resultModel.ToDictionary()));

            if (!await _userService.VerifyIfUserExistsByEmail(dto.Email))
            {
                await _userService.PostUser(user);

                _cache.GetOrCreate(user.Id, item =>
                {
                    item.Value = user.Username;
                    return item;
                });

                return Ok(await _tokenService.GenerateJWT(user));
            }

            return Conflict("User already exists");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            User user = await _userService.GetUserByEmailPassword(dto.Email, dto.Password);

            if (user == null) return BadRequest("User or password invalid");
            
            return Ok(await _tokenService.GenerateJWT(user));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] UserLogoutDTO dto)
        {
            ClaimsPrincipal user = HttpContext.User;

            if (user != null)
            {
                User u = await _userService.GetUserByAccessToken(dto.AccessToken);
                await _userService.DeleteTokens(u.Id);

                return Ok();
            }

            return BadRequest("Invalid access token");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] UserRefreshtDTO dto)
        {
            if (string.IsNullOrEmpty(dto.RefreshToken)) return BadRequest("Invalid refresh token");

            User user = await _userService.GetUserByRefreshToken(dto.RefreshToken);

            if (user == null) return BadRequest("Invalid refresh token");
            
            return Ok(await _tokenService.GenerateJWT(user));
        }
    }
}
