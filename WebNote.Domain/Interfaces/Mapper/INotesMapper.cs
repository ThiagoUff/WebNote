using WebNote.Domain.Entities.Request;
using WebNote.Domain.Repository.Notes;

namespace WebNote.Domain.Interfaces.Mapper
{
    public interface INotesMapper
    {
        Notes Convert(CreateNoteRequest request);
    }
}
