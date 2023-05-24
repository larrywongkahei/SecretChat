using System.Diagnostics;
using System.Text.Json;
using CommunityToolkit.Maui.Views;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRServer.Models;
using CommunityToolkit.Maui.Alerts;
using System.Threading;
using CommunityToolkit.Maui.Core;

namespace OtOSecretChat;

public partial class CreatePopup : Popup
{
	public int RoomNumber { get; set; }

	private readonly HubConnection _connection;

	private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public CreatePopup(HubConnection connection)
	{
		_connection = connection;
		InitializeComponent();
		BindingContext = this;
		var randomNum = new Random();
		RoomNumber = randomNum.Next(1000, 9999);
		createRoom();
    }

	public async void createRoom()
	{
		await _connection.InvokeCoreAsync("CreateRoom", args: new[] { RoomNumber.ToString() });
	}

	async void StartChatting(Object sender, EventArgs e)
	{
		using(HttpClient client = new HttpClient())
		{
			using (HttpResponseMessage response = await client.GetAsync($"http://localhost:5001/api/Rooms/{RoomNumber}"))
			{
				string content = response.Content.ReadAsStringAsync().Result;
				var theRoom = JsonSerializer.Deserialize<Room>(content);
                if (theRoom.UserTwo != null)
				{
					Close(theRoom.RoomNumber);
				}
				else
				{
                    string text = "Awaiting user to join";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;

                    var toast = Toast.Make(text, duration, fontSize);

                    await toast.Show(cancellationTokenSource.Token);
                }
				}
			}
		}

	}


//public partial class CreatePopup : Popup
//{
//    public int RoomNumber { get; set; }
//    public CreatePopup()
//    {
//        InitializeComponent();
//        BindingContext = this;
//        var randomNum = new Random();
//        RoomNumber = randomNum.Next(1000, 9999);
//    }

//    void CreateRoom(Object sender, EventArgs e) => Close(RoomNumber);
//}

