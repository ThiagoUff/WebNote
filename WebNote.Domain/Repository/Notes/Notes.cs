namespace WebNote.Domain.Repository.Notes
{
    public class Notes
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Post { get; set; } = null!;
        public DateTime PostedOn { get; set; }
        public bool IsPublic { get; set; } 
    }

}
