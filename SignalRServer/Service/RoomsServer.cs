using System;
using SignalRServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SignalRServer.Service
{
	public class RoomsService
	{
		private readonly IMongoCollection<Room> _roomsCollection;

		public RoomsService()
		{
            const string connectionUri = "mongodb+srv://makemak123:makecak123@rooms.rmoptuv.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var mongoClient = new MongoClient(settings);
            var mongoDatabase = mongoClient.GetDatabase("RoomNum");

            _roomsCollection = mongoDatabase.GetCollection<Room>("Rooms");
        }

        public async Task<List<Room>> GetAsync() =>
            await _roomsCollection.Find(_ => true).ToListAsync();

        public async Task<Room?> GetAsync(string roomNum) =>
            await _roomsCollection.Find(x => x.RoomNumber.ToString() == roomNum).FirstOrDefaultAsync();

        public async Task CreateAsync(Room newRoom) =>
            await _roomsCollection.InsertOneAsync(newRoom);

        public async Task UpdateAsync(string roomNum, Room updatedRoom) =>
            await _roomsCollection.ReplaceOneAsync(x => x.RoomNumber.ToString() == roomNum, updatedRoom);

        public async Task RemoveAsync(string id) =>
            await _roomsCollection.DeleteOneAsync(x => x.Id == id);
    }
}

