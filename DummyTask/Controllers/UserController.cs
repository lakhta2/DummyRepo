using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DummyTask.Core.Abstractions;
using DummyTask.Core.ContractsAndDTO_s;
using DummyTask.Core.Models;

namespace DummyTask.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("~/api/GetAllActiveUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllActiveUsers()
        {
            var users = await _userService.GetAllActiveUsersAsync();

            return Ok(users);
        }
        [HttpGet("~/api/getUsersByLoginAndPassword")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllActiveUsers([FromBody] LoginPasswordUserRequest loginAndPassword)
        {
            var users = await _userService.GetUserByLoginAndPasswordAsync(loginAndPassword.login, loginAndPassword.password);

            return Ok(users);
        }
        [HttpPost("~/api/Create")]
        public async Task<ActionResult> Create([FromBody] CreateUser createUser)
        {
            await _userService.Create(createUser);
            return Ok();
        }
        [HttpPatch("~/api/updateLogin")]
        public async Task<ActionResult> Update([FromBody] UpdateLogin updateUser)
        {
            await _userService.Update(updateUser);
            return Ok();
        }
        [HttpPatch("~/api/updateUser")]
        public async Task<ActionResult> Update([FromBody] UpdateBirthday updateUser)
        {
            await _userService.Update(updateUser);
            return Ok();
        }
        [HttpPatch("~/api/updateGender")]
        public async Task<ActionResult> Update([FromBody] UpdateGender updateUser)
        {
            await _userService.Update(updateUser);
            return Ok();
        }
        [HttpPatch("~/api/updateName")]
        public async Task<ActionResult> Update([FromBody] UpdateName updateUser)
        {
            await _userService.Update(updateUser);
            return Ok();
        }
        [HttpPatch("~/api/updatePassword")]
        public async Task<ActionResult> Update([FromBody] UpdatePassword updateUser)
        {
            await _userService.Update(updateUser);
            return Ok();
        }
        [HttpDelete("~/api/softDelete")]
        public async Task<ActionResult> Delete([FromBody] DeleteSoft deleteUser)
        {
            await _userService.SoftDelete(deleteUser);
            return Ok();
        }
        [HttpPut("~/api/softDelete")]
        public async Task<ActionResult<User>> Restore([FromBody] Restore restore)
        {
            var restoredUser = await _userService.Restore(restore);
            return Ok(restoredUser);
        }
    }
}
