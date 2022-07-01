using CS_DAL.Authentification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CS_WebApplication.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
