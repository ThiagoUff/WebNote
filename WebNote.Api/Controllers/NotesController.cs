using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNote.Domain.Entities.Request;
using WebNote.Domain.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebNote.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesServices _notesServices;

        public NotesController(INotesServices notesServices)
        {
            _notesServices = notesServices;
        }
      
        [HttpPost]
        public async Task<ActionResult> CreateNote([FromBody] CreateNoteRequest request)
        {
            await _notesServices.CreateNote(request);
            return CreatedAtAction(nameof(CreateNote), new { Username = request.Username }, request);
        }
        [HttpGet("username/{username}")]
        public async Task<ActionResult> CreateNote(string username)
        {
            return Ok(await _notesServices.GetAllNotesFromUser(username));
        }


    }
}
