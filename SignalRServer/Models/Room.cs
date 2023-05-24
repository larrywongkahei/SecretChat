using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SignalRServer.Models
{
	public class Room
	{

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }

		[BsonElement("RoomNum")]
		public int RoomNumber { get; set; }

        [BsonElement("userOne")]
        public string? UserOne { get; set; }

        [BsonElement("userTwo")]
        public string? UserTwo { get; set; }

        [BsonElement("isActive")]
        public string? IsActive { get; set; }
	}
}

