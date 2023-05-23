using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Microsoft.AspNetCore.SignalR.Client;

namespace OtOSecretChat;

public partial class MainPage : ContentPage
{
    private readonly HubConnection _connection;

    public MainPage()
    {
        InitializeComponent();
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5001/chat")
            .Build();

        //_connection.On<string>("ReceiveMessage", (message) =>
        //{
        //    Debug.WriteLine(message);
        //});

        Task.Run(() =>
        {
            Dispatcher.Dispatch(async () => await _connection.StartAsync());
        });
    }


    private async void OnButtonClicked(object sender, EventArgs e)
    {
        await DisplayPopup();


    }

    //public async Task DisplayPopup()
    //{
    //    var popup = new MainPagePopup();

    //    var result = await this.ShowPopupAsync(popup);

    //    if (result is bool returnVal)
    //    {
    //        if (returnVal)
    //        {
    //            await waitForSeconds();
    //            var decision = await this.ShowPopupAsync(new CreatePopup());
    //            if (decision is int value)
    //            {
    //                await _connection.InvokeCoreAsync("CreateRoom", args: new[] { decision.ToString() });
    //                await Navigation.PushAsync(new ChattingPage(_connection, decision.ToString()));
    //            }
    //        }
    //        else
    //        {
    //            await waitForSeconds();
    //            var decision = await this.ShowPopupAsync(new JoinPopup());
    //            if (decision is int value)
    //            {
    //                await _connection.InvokeCoreAsync("JoinRoom", args: new[] { decision.ToString() });
    //                await Navigation.PushAsync(new ChattingPage(_connection, decision.ToString()));
    //            }
    //        }
    //    }
    //}
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
                    await Navigation.PushAsync(new ChattingPage(_connection, decision.ToString()));
                }
            }
            else
            {
                await waitForSeconds();
                var decision = await this.ShowPopupAsync(new JoinPopup());
                if (decision is int value)
                {
                    await _connection.InvokeCoreAsync("JoinRoom", args: new[] { decision.ToString() });
                }
            }
        }
    }


    async Task waitForSeconds()
    {
        await Task.Delay(500);
    }

    //public async void sendmessage(string message)
    //{
    //    await _connection.InvokeCoreAsync("SendMessage", args: new[] {"hi"});
    //}

}



