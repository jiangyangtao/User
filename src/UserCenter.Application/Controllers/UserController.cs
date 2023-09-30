using Microsoft.AspNetCore.Mvc;
using UserCenter.Application.Dto;
using UserCenter.Core.Abstractions;
using Yangtao.Hosting.Controller;

namespace UserCenter.Application.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserProvider _userProvider;

        public UserController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        [HttpPost]
        public Task Add([FromBody] UserDto dto) => _userProvider.AddAsync(dto.Username, dto.Password);


        [HttpDelete]
        public Task Remove([FromBody] UserDtoBase dto) => _userProvider.RemoveAsync(dto.UserId);


        [HttpPatch]
        public Task ChangePassword([FromBody] ChangePasswordDto dto) => _userProvider.ChangePasswordAsync(dto.GetUserPassword());

        [HttpGet]
        public async Task<UserRolePaginationResult> List([FromBody] UserQueryDto dto)
        {
            var queryParams = dto.GetUserQueryParams();
            var list = await _userProvider.GetUsersAsync(queryParams);
            var count = await _userProvider.GetUserCountAsync(queryParams);

            return new UserRolePaginationResult(list, count);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userProvider.LoginAsync(dto.Username, dto.Password);
            if (user == null) return NotFound();

            var result = new LoginResult { UserId = user.UserId, Username = user.Username };
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] UserDtoBase dto)
        {
            var user = await _userProvider.GetByIdAsync(dto.UserId);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
