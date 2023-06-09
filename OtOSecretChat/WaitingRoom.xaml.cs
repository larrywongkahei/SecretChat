﻿namespace OtOSecretChat;

using System.Diagnostics;
using System.Text.Json;
using CommunityToolkit.Maui.Views;
using SignalRServer.Models;

public partial class WaitingRoom : Popup
{
	public string RoomNum { get; set; }

	public bool IsActive { get; set; } = false;

	public WaitingRoom(string RoomNumber)
	{
		InitializeComponent();
		RoomNum = RoomNumber;
		WaitingToJoin();
	}

	async void WaitingToJoin()
	{
		while (!IsActive)
		{
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync($"http://localhost:5001/api/Rooms/{RoomNum}"))
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                        var theRoom = JsonSerializer.Deserialize<Room>(content);
                        if (theRoom.IsActive == "false")
                        {
                            await Task.Delay(1000);
                        }
                        else
                        {
                            IsActive = !IsActive;
                            Close(true);
                        }

                }
            }
        }

	}
}
