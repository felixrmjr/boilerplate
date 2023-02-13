using AutoMapper;
using Business.Domain.Interfaces.Services;
using Business.Domain.Model;
using Business.Domain.Model.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IValidator<User> _validator;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        public UserController(IMapper mapper,
                              IValidator<User> validator,
                              IUserService userService,
                              IMemoryCache cache,
                              ILogger<UserController> logger)

        {
            _mapper = mapper;
            _validator = validator;
            _userService = userService;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetUsers();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var user = await _userService.GetUserById(id);

            return user == null ? NotFound() : Ok(user);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(408)]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UserDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            var result = await _validator.ValidateAsync(user);

            if (!result.IsValid) return BadRequest(Results.ValidationProblem(result.ToDictionary()));

            try
            {
                var u = await _userService.PutUser(id, user);

                _cache.GetOrCreate(u.Id, item =>
                {
                    item.Value = u.Username;
                    return item;
                });

                return Ok(new { id = u.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: {HttpContext.Request.Path} -> {ex}");
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _userService.DeleteUser(id);

            return NoContent();
        }
    }
}
