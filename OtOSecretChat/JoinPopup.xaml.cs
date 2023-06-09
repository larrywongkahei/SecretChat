﻿using System.Text.Json;
using CommunityToolkit.Maui.Views;
using SignalRServer.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading;
using System.Diagnostics;

namespace OtOSecretChat;

public partial class JoinPopup : Popup
{
	public int RoomNum { get; set; }

    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public JoinPopup()
	{
		InitializeComponent();
	}

	async void JoinRoom(Object sender, EventArgs e)
	{
		RoomNum = Convert.ToInt32(Input.Text);
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync($"http://localhost:5001/api/Rooms/{RoomNum}"))
            {
                string content = response.Content.ReadAsStringAsync().Result;
                if(content.Length > 0)
                {
                    var theRoom = JsonSerializer.Deserialize<Room>(content);
                    if (theRoom.UserTwo != null)
                    {
                        string text = "This room is Full";
                        ToastDuration duration = ToastDuration.Short;
                        double fontSize = 14;

                        var toast = Toast.Make(text, duration, fontSize);

                        await toast.Show(cancellationTokenSource.Token);

                    }
                    else
                    {
                        Close(Convert.ToInt32(RoomNum));
                    }
                }
                else
                {
                    string text = "This room does not exist";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;

                    var toast = Toast.Make(text, duration, fontSize);

                    await toast.Show(cancellationTokenSource.Token);
                }


            }
        }
                
	}
}
