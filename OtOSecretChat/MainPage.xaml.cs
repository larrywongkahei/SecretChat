using System.Diagnostics;
using System.Text.Json;
using CommunityToolkit.Maui.Views;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRServer.Models;

namespace OtOSecretChat;

public partial class MainPage : ContentPage
{
    private readonly HubConnection _connection;

    public string roomNum { get; set; }

    public NavigationPage mainPage;

    public MainPage()
    {
        InitializeComponent();
        mainPage = new NavigationPage(this);
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5001/chat")
            .Build();
        Task.Run(() =>
        {
            Dispatcher.Dispatch(async () => await _connection.StartAsync());
        });
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        await DisplayPopup();
    }

    private async void CheckIfInvokeLeaveRoom()
    {
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync("http://localhost:5001/api/Rooms"))
            {
                var result = response.Content.ReadAsStringAsync().Result;
                if(result.Length > 0)
                {
                    var data = JsonSerializer.Deserialize<Room[]>(result);
                    var theGroup = data?.FirstOrDefault(x => x.UserOne == _connection.ConnectionId);
                    roomNum = theGroup?.RoomNumber.ToString();
                    Debug.Write(theGroup?.RoomNumber);
                    Debug.Write(theGroup?.IsActive);
                    if (theGroup?.IsActive == "true")
                    {
                        Debug.WriteLine("Processing to invoke leave room");
                        await _connection.InvokeCoreAsync("LeaveRoom", args: new[] { roomNum });
                    }
                    else
                    {
                        Debug.WriteLine("This is either the first time or user didnt start a chat.");
                    }
                }

            }
        }
    }

    protected override void OnAppearing()
    {
        CheckIfInvokeLeaveRoom();
    }
    public async Task DisplayPopup()
    {
        var popup = new MainPagePopup();

        var result = await this.ShowPopupAsync(popup);

        if (result is bool returnVal)
        {
            if (returnVal)
            {
                await waitForSeconds();
                var decision = await this.ShowPopupAsync(new CreatePopup(_connection));
                if (decision is int value)
                {
                    await _connection.InvokeCoreAsync("StartChatting", args: new[] { decision.ToString() });
                    await mainPage.PushAsync(new ChattingPage(_connection, decision.ToString(), mainPage));
                    await Navigation.PushAsync(new ChattingPage(_connection, decision.ToString(), mainPage));
                }
            }
            else
            {
                await waitForSeconds();
                var decision = await this.ShowPopupAsync(new JoinPopup());
                if (decision is int value)
                {
                    await _connection.InvokeCoreAsync("JoinRoom", args: new[] { decision.ToString() });
                    await waitForSeconds();
                    var WaitingRoom = await this.ShowPopupAsync(new WaitingRoom(decision.ToString()));
                    if (WaitingRoom is bool)
                    {
                        await mainPage.PushAsync(new ChattingPage(_connection, decision.ToString(), mainPage));
                        await Navigation.PushAsync(new ChattingPage(_connection, decision.ToString(), mainPage));
                    }
                }
            }
        }
    }
    async Task waitForSeconds()
    {
        await Task.Delay(500);
    }
}



