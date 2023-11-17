using DatabaseApplication;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {

        private UserService.UserService _service;

        public UsersController()
        {
            _service = new();
        }

        [HttpGet(Name = "GetAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            return _service.GetAllUsers();
        }

        [HttpGet("getUser/{userId}", Name = "GetUserById")]
        public User GetUserById(int userId)
        {
            return _service.GetUserById(userId);
        }
        
        [HttpPost("addUser", Name = "AddUser")]
        public int AddUser([FromBody] User user)
        {
            return _service.AddUser(user);
        }

        [HttpDelete("removeUser/{userId}", Name = "RemoveUser")]
        public void RemoveUser(int userId)
        {
            _service.RemoveUser(userId);
        }

        [HttpPost("updateUser", Name = "UpdateUser")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            var res = _service.UpdateUser(user);
            if (res == -1)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
    }
}