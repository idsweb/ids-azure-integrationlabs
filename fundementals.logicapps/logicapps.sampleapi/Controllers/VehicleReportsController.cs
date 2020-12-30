using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace logicapps.sampleapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleReportsController : Controller
    {
        private readonly ILogger<VehicleReportsController> _logger;

        public VehicleReportsController(ILogger<VehicleReportsController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Route("CheckReg/{reg}")]
        public IActionResult CheckReg(string reg)
        {
            if(reg.Contains("Q")){
                return BadRequest();
            }
            if (reg.Contains("NL"))
            {
                VehicleReport report = new VehicleReport();
                report.ReportRef = "12345";
                report.Registration = "NLO3 UAY";
                return Ok(report);
            }
            else{
                return NotFound();
            }
        }
    }

    public class VehicleReport
    {

        public string ReportRef { get; set; }
        public string Registration { get; set; }
    }
}
