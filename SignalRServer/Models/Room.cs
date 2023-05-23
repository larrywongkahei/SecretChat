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

		public string? UserOne { get; set; }

		public string? UserTwo { get; set; }
    }
}

