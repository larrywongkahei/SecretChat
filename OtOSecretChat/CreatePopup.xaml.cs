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

    public bool CouldStart { get; set; } = false;

    private readonly HubConnection _connection;

    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public CreatePopup(HubConnection connection)
    {
        _connection = connection;
        InitializeComponent();
        getNonDuplicateNumber();
        BindingContext = this;
    }

    public async void createRoom(int roomNum)
    {
        await _connection.InvokeCoreAsync("CreateRoom", args: new[] { roomNum.ToString() });
    }

    public async void ConfirmToStart()
    {
        while (!CouldStart)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync($"http://localhost:5001/api/Rooms/{RoomNumber}"))
                {
                    string content = response?.Content?.ReadAsStringAsync().Result;
                    if(Content != null)
                    {
                        var theRoom = JsonSerializer.Deserialize<Room>(content);
                        if (theRoom.UserTwo != null)
                        {
                            CouldStart = !CouldStart;
                            userlabel.Text = "User2";
                        }
                        else
                        {
                            await Task.Delay(2000);
                        }
                    }

                }
            }
        }
    }

    async void StartChatting(Object sender, EventArgs e)
    {
        if (CouldStart)
        {
            Close(RoomNumber);
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
    public async void getNonDuplicateNumber()
    {
        using (HttpClient httpClient = new HttpClient())
        {
            using (HttpResponseMessage response = await httpClient.GetAsync("http://localhost:5001/api/Rooms"))
            {
                string content = response?.Content?.ReadAsStringAsync().Result;
                if(content != null)
                {
                    var data = JsonSerializer.Deserialize<Room[]>(content);
                    var randomNum = new Random();
                    bool readyToReturn = false;
                    while (!readyToReturn)
                    {
                        int roomNum = randomNum.Next(1000, 9999);
                        var result = data.FirstOrDefault(x => x.RoomNumber == roomNum);
                        if (result == null)
                        {
                            RoomNumber = roomNum;
                            createRoom(roomNum);
                            roomlabel.Text = roomNum.ToString();
                            break;
                        }
                    }
                }
                else
                {
                    var randomNum = new Random();
                    int roomNum = randomNum.Next(1000, 9999);
                    RoomNumber = roomNum;
                    createRoom(roomNum);
                    roomlabel.Text = roomNum.ToString();

                }

                ConfirmToStart();


            }

        }
    }

}
