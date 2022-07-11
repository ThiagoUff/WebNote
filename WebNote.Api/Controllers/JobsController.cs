using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNote.Domain.Interfaces.Services;

namespace WebNote.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobsServices _jobsServices;

        public JobsController(IJobsServices jobsServices)
        {
            _jobsServices = jobsServices;
        }


        [HttpPost("notes")]
        public async Task<ActionResult> ProcessNotes()
        {
            await _jobsServices.ProcessNotes();
            return Ok();
        }
    }
}
