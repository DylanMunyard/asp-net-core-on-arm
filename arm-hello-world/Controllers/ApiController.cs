using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arm_hello_world.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace arm_hello_world.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly ArmContext _armContext;

        public ApiController(ILogger<ApiController> logger,
            ArmContext armContext)
        {
            _logger = logger;
            _armContext = armContext;
        }

        [HttpGet]
        public JsonResult Get()
        {
            _logger.LogInformation("Received a request at {Time}", DateTime.Now);

            return new JsonResult(new {Hello = "World"});
        }

        [HttpGet("host")]
        public JsonResult Host()
        {
            _logger.LogInformation("/api/host called");

            return new JsonResult(new {HttpContext.Request.Host});
        }

        [HttpGet("db")]
        public JsonResult Db()
        {
            _logger.LogInformation("Testing the DB");
            var armItems = _armContext.ArmItem.Select(a => a.Description);

            return new JsonResult(new {Descriptions = string.Join(", ", armItems.Select(a => a))});
        }
    }
}