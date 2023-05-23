using System;
using System.Text.Json.Serialization;

namespace SignalRServer.Models
{
	public class Room
	{
        [JsonPropertyName("id")]
		public string Id { get; set; }

        [JsonPropertyName("roomNumber")]
        public int RoomNumber { get; set; }

        [JsonPropertyName("userOne")]
        public string UserOne { get; set; }

        [JsonPropertyName("userTwo")]
        public string UserTwo { get; set; }

        public string IsActive { get; set; }

    }
}

