using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [Route("/")]
    [ApiController]
    public class PrincipalController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(new { api = "BiblioSystem", status = "up" });
        }
    }
}