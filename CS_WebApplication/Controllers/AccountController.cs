using CS_DAL.Authentification;
using CS_BLL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CS_WebApplication.Controllers
{


    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        async static Task<AccountModel> GetAccountModel(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            var roles = await userManager.GetRolesAsync(user);

            return new AccountModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Role = roles
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var r = Request.Query;

            if (r.Count == 0) // Get all users
            {
                var accounts = new List<AccountModel>();

                var users = new List<ApplicationUser>(_userManager.Users);
                
                foreach (var user in users)
                {
                    accounts.Add(await GetAccountModel(user, _userManager));
                }
                return Ok(accounts);
            }
            
            // Check correct query ------- //
            if (r.Count > 1)
            {
                return BadRequest();
            }

            var searchString = r["id"];
            if (searchString.Count == 1) // Get user by id
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == searchString.ToString());

                if (user is null)
                {
                    return NotFound();
                }

                return Ok(await GetAccountModel(user, _userManager));
            }

            searchString = r["username"];
            if (searchString.Count == 1) // Get user by user name
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == searchString.ToString());

                if (user is null)
                {
                    return NotFound();
                }

                return Ok(await GetAccountModel(user, _userManager));
            }

            return BadRequest();
        }

        // PUT api/<UserController>/
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ChangeUserData([FromBody] ChangeAccountModel value)
        {
            if (User.Identity is null)
            {
                return Unauthorized();
            }

            if (User.Identity.Name == value.UserName)
            {
                var user = _userManager.Users.SingleOrDefault(u => u.UserName == value.UserName);

                if (user is null)
                {
                    return NotFound();
                }

                var tasks = new List<Task>();

                if (!string.IsNullOrEmpty(value.NewPassword))
                {
                    tasks.Add(_userManager.ChangePasswordAsync(user, value.OldPassword, value.NewPassword));
                }

                if (!string.IsNullOrEmpty(value.NewEmail))
                {
                    tasks.Add(Task.Run(async () => await _userManager.ChangeEmailAsync(user, value.NewEmail, await _userManager.GenerateChangeEmailTokenAsync(user, value.NewEmail))));
                }

                Task.WaitAll(tasks.ToArray());

                return Ok(value);
            }

            return Forbid();
        }

        // DELETE api/<UserController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var r = Request.Query;

            if (r.Count > 1)
            {
                return BadRequest();
            }

            var searchString = r["id"];
            if (searchString.Count == 1) // Get user by id
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == searchString.ToString());

                if (user is null)
                {
                    return NotFound();
                }

                await _userManager.DeleteAsync(user);
                return Ok(new ErrorResponse { Error = "User was deleted successfully"});
            }

            searchString = r["username"];
            if (searchString.Count == 1) // Get user by user name
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == searchString.ToString());

                if (user is null)
                {
                    return NotFound();
                }

                await _userManager.DeleteAsync(user);
                return Ok(new ErrorResponse { Error = "User was deleted successfully" });
            }

            return BadRequest();
        }
    }
}
