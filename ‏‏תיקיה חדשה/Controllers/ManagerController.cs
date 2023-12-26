using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using System;
using System.Security.Claims;
using Services;

namespace Controllers
{
    using Models;

    [ApiController]
    [Route("[controller]")]
    public class ManagerController : ControllerBase
    {
        IUserService UserService;
        public ManagerController(IUserService UserService)
        {
            this.UserService = UserService;
        }

        [HttpPost]
        [Route("[Action]")]
        public ActionResult<String> Login([FromBody] User user)
        {
            var claims = new List<Claim>();
            var getUser = UserService.GetAll()?.FirstOrDefault(c => c.UserName == user.UserName && c.Password == user.Password);
            if (getUser == null)
                return Unauthorized();
            if (getUser.Manager)
                claims.Add(new Claim("type", "Manager"));
            claims.Add(new Claim("type", "User"));
            claims.Add(new Claim("Id", getUser.Id.ToString()));

            return new OkObjectResult(TaskTokenService.WriteToken(TaskTokenService.GetToken(claims)));
        }

    }

}




