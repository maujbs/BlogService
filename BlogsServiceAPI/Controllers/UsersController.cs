using BlogAdapter.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BlogAdapter.Adapters;
using System;
using Microsoft.AspNetCore.Cors;

namespace BlogServiceAPI.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerRequest
    {

        [HttpPost]
        [Route("CreateUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<object> CreateUser(string username, string password, int roleLevel)
        {
            try
            {
                UserAdapter databaseAdapter = new UserAdapter();
                databaseAdapter.AddUser(username,password, roleLevel);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return true;
        }

        [HttpPost]
        [Route("LogIn")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<User> LogIn(string username, string password)
        {
            User user = new User();
            try
            {
                UserAdapter databaseAdapter = new UserAdapter();
                user = databaseAdapter.LogIn(username, password);
            }
            catch (Exception ex)
            {
                HandleRequestException(ex);
            }
            return user;
        }

        [HttpPost]
        [Route("LogOut")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<object> LogOut(string username, int token)
        {
            try
            {
                UserAdapter databaseAdapter = new UserAdapter();
                databaseAdapter.LogOut(username, token);
            }
            catch (Exception ex)
            {
                HandleRequestException(ex);
            }
            return true;
        }

    }
}