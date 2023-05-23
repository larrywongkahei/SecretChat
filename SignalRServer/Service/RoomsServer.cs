using System;
using SignalRServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SignalRServer.Service
{
	public class RoomsService
	{
		private readonly IMongoCollection<Room> _roomsCollection;

		public RoomsService(
			IOptions<RoomNumberDatabaseSettings> roomNumberDatabaseSettings)
		{
            var mongoClient = new MongoClient(
            roomNumberDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                roomNumberDatabaseSettings.Value.DatabaseName);

            _roomsCollection = mongoDatabase.GetCollection<Room>(
                roomNumberDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<Room>> GetAsync() =>
            await _roomsCollection.Find(_ => true).ToListAsync();

        public async Task<Room?> GetAsync(string roomNum) =>
            await _roomsCollection.Find(x => x.RoomNumber.ToString() == roomNum).FirstOrDefaultAsync();

        public async Task CreateAsync(Room newRoom) =>
            await _roomsCollection.InsertOneAsync(newRoom);

        public async Task UpdateAsync(string id, Room updatedBook) =>
            await _roomsCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _roomsCollection.DeleteOneAsync(x => x.Id == id);
    }
}

