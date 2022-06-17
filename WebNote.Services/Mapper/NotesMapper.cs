using WebNote.Domain.Entities.Request;
using WebNote.Domain.Interfaces.Mapper;
using WebNote.Domain.Repository.Notes;

namespace WebNote.Services.Mapper
{
    public class NotesMapper : INotesMapper
    {
        public Notes Convert(CreateNoteRequest request)
        {
            return new Notes()
            {
                Id = System.Convert.ToBase64String((Guid.NewGuid()).ToByteArray()),
                IsPublic = request.IsPublic,
                Post = request.Post,
                PostedOn = DateTime.UtcNow,
                Username = request.Username,
            };
        }
    }
}
