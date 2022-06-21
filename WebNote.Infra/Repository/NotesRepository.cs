using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebNote.Domain.Interfaces.Repository;
using WebNote.Domain.Repository.Notes;

namespace WebNote.Infra.Repository
{
    public class NotesRepository : INotesRepository
    {
        private readonly IMongoCollection<Notes> _NotesCollection;
        public NotesRepository(
        IOptions<NotesDatabaseSettings> notesDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                notesDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                notesDatabaseSettings.Value.DatabaseName);

            _NotesCollection = mongoDatabase.GetCollection<Notes>(
                notesDatabaseSettings.Value.NotesCollectionName);
        }
        public async Task<List<Notes>> GetAsync() =>
        await _NotesCollection.Find(_ => true).ToListAsync();

        public async Task<Notes?> GetAsync(string id) =>
            await _NotesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<List<Notes>> GetAllByUserAsync(string username) =>
            await _NotesCollection.Find(x => x.Username == username).ToListAsync();

        public async Task CreateAsync(Notes newBook) =>
            await _NotesCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Notes updatedBook) =>
            await _NotesCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _NotesCollection.DeleteOneAsync(x => x.Id == id);
    }
}
