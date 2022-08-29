using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadUserDto>>> GetUserss()
        {

            IEnumerable<ReadUserDto> users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ReadUserDto>> GetUser([FromRoute] long id)
        {
            ReadUserDto user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/User/Comments/5       
        [HttpGet]
        [Route("{id:long}/Comments")]
        public async Task<ActionResult<ReadUserCommentsDto>> GetUserComments([FromRoute] long id)
        {
            ReadUserCommentsDto user = await _userService.GetUserCommentsAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/User/5/PostLikes      
        [HttpGet]
        [Route("{id:long}/PostLikes")]
        public async Task<ActionResult<ReadUserPostLikesDto>> GetUserPostLikes([FromRoute] long id)
        {
            ReadUserPostLikesDto user = await _userService.GetUserPostLikesAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/User/5/CommentLikes     
        [HttpGet]
        [Route("{id:long}/CommentLikes")]
        public async Task<ActionResult<ReadUserCommentLikesDto>> GetUserCommentLikes([FromRoute] long id)
        {
            ReadUserCommentLikesDto user = await _userService.GetUserCommentLikesAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/User/5
        // To protect from overusering attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:long}")]
        public async Task<IActionResult> PutUser([FromRoute] long id, [FromBody] CreateUserDto user)
        {
            ReadUserDto userToModify = await _userService.GetUserAsync(id);
            if (userToModify == null)
            {
                return BadRequest();
            }
            else
            {
                CreateUserDto newUser = await _userService.UpdateUserAsync(id, user);
                return Ok(newUser);
            }

            /*
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/
        }

        // POST: api/Users
        // To protect from overusering attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReadUserDto>> PostUser([FromBody] CreateUserDto user)
        {
            ReadUserDto newUser = await _userService.CreateUserAsync(user);
            return Ok(newUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {
            //if (_context.Users == null)
            //{
            //    return NotFound();
            //}
            ReadUserDto userToDelete = await _userService.GetUserAsync(id);
            if (userToDelete == null)
            {
                return NotFound();
            }
            else
            {
                await _userService.DeleteUserAsync(id);
            }

            return NoContent();
        }

        // UPDATE :api/ChangeUserState/1
        //[HttpPut("{id:long}")]
        //public async Task<IActionResult> ChangeUserState([FromRoute] long id, ChangeStateDto state)
        //{
        //    ReadUserDto userToModify = await _userService.GetUserAsync(id);
        //    if (userToModify == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        CreateUserDto modifiedUser = await _userService.ChangeUserStateAsync(id, state);
        //        return Ok(modifiedUser);
        //    }
        //}
    }
}
