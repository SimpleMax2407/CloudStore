using CS_DAL.Authentification;
using CS_BLL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using CS_BLL.Services;
using CS_BLL.Validation;

namespace CS_WebApplication.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : Controller
    {
        readonly FileService fileService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public FileController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, FileService fileService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.fileService = fileService;
        }

        // GET: FileController
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFileData([FromHeader] FilterFileDatumModel filter)
        {

            if (User.Identity is null)
            {
                return Forbid();
            }

            try
            {
                var models = await fileService.GetAllFileModelsByUserNameAsync(User.Identity.Name, filter);

                if (models is null || !models.Any())
                {
                    return NotFound();
                }

                return Ok(models);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("download/{fileName}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Download(string fileName)
        {

            if (User.Identity is null)
            {
                return Forbid();
            }

            try
            {
                var stream = await fileService.GetFileByModelAsync(new FileDatumModel { UserName = User.Identity.Name, FileName = fileName });
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                //return Ok(File(stream, "application/octet-stream"));
                return new FileStreamResult(stream, "application/octet-stream") { FileDownloadName = fileName };
            }
            catch (Exception ex)
            {
                if (ex is FileDoesntExistInFSException)
                {
                    return NotFound();
                }

                return Problem(ex.Message);
            }
        }

        // GET: FileController/Details/5
        [Authorize]
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddFile()
        {
            if (User.Identity is null)
            {
                return Forbid();
            }

            var file = Request.Form.Files[0];

            if (file is null)
            {
                return Problem("Uploaded file is empty");
            }

            try
            {
                await fileService.AddAsync(User.Identity.Name, file);
                return Ok(new FileResponse { Status="Success", Message ="File added successfully"});
            }
            catch (CloudStoreException ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{fileName}/{newFileName}")]
        public async Task<IActionResult> RenameFile(string fileName, string newFileName)
        {
            if (User.Identity is null)
            {
                return Forbid();
            }

            if (fileName == newFileName)
            {
                return BadRequest("Names are same");
            }

            try
            {
                await fileService.RenameAsync(User.Identity.Name, fileName, newFileName);
                return Ok(new FileResponse { Status = "Success", Message = "File renamed successfully" });
            }
            catch (CloudStoreException ex)
            {
                if (ex is FileDoesntExistInFSException || ex is FileDoesntExistException)
                {
                    return NotFound();
                }

                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditFile()
        {
            if (User.Identity is null)
            {
                return Forbid();
            }

            var file = Request.Form.Files[0];

            if (file is null)
            {
                return Problem("Uploaded file is empty");
            }

            try
            {
                await fileService.UpdateFileAsync(new FileDatumModel { UserName=User.Identity.Name, FileName = file.FileName}, file);
                return Ok(new FileResponse { Status = "Success", Message = "File edited successfully" });
            }
            catch (CloudStoreException ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{fileName}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> RemoveFile(string fileName)
        {
            if (User.Identity is null)
            {
                return Forbid();
            }

            try
            {
                await fileService.RemoveAsync(new FileDatumModel { UserName = User.Identity.Name, FileName = fileName });
                return Ok(new FileResponse { Status = "Success", Message = "File removed successfully" });
            }
            catch (CloudStoreException ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

