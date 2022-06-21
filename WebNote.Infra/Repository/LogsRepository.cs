using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebNote.Domain.Interfaces.Repository;
using WebNote.Domain.Repository.Logs;

namespace WebNote.Infra.Repository
{
    public class LogsRepository : ILogsRepository
    {
        private readonly IMongoCollection<Logs> _LogsCollection;
        public LogsRepository(IOptions<LogsDatabaseSettings> logsDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                logsDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                logsDatabaseSettings.Value.DatabaseName);

            _LogsCollection = mongoDatabase.GetCollection<Logs>(
                logsDatabaseSettings.Value.LogsCollectionName);
        }
        public async Task<List<Logs>> GetAsync() =>
        await _LogsCollection.Find(_ => true).ToListAsync();

        public async Task<Logs?> GetAsync(string id) =>
            await _LogsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Logs>> GetAllByUserAsync(string username) =>
            await _LogsCollection.Find(x => x.Username == username).ToListAsync();

        public async Task CreateAsync(Logs newLog) =>
            await _LogsCollection.InsertOneAsync(newLog);

        public async Task UpdateAsync(string id, Logs updatedLog) =>
            await _LogsCollection.ReplaceOneAsync(x => x.Id == id, updatedLog);

        public async Task RemoveAsync(string id) =>
            await _LogsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
