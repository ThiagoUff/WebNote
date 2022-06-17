namespace WebNote.Domain.Entities.Request
{
    public class CreateNoteRequest
    {
        public string Username { get; set; } = null!;
        public string Post { get; set; } = null!;
        public bool IsPublic { get; set; }
    }
}
