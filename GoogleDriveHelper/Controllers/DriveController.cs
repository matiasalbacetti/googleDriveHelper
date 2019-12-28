using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriveController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var driveService = new GoogleDriveService();

            var folder = GoogleDriveService.getFolderInfo("root", "root");

            return new JsonResult(new { folder = folder });
        }
    }
}
