using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Models;
using Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly int userId;
        IUserService userService;
        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value);
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Policy = "Manager")]
        public ActionResult<List<User>> GetAll() => userService.GetAll();

        [HttpGet]
        [Route("GetMyUser")]
        [Authorize(Policy = "User")]
        public ActionResult<User> GetMyUser()
        {
            var user = userService.Get(userId);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public ActionResult Post([FromBody] User user)
        {
            userService.Post(user);
            return CreatedAtAction(nameof(Post), new { Id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Manager")]
        public ActionResult Delete(int id)
        {           
            var user = userService.Get(id);
            if (user is null)
                return NotFound();
            userService.Delete(id);
            return NoContent();
        }
    }
}