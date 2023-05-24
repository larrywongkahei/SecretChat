﻿using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Microsoft.AspNetCore.SignalR.Client;

namespace OtOSecretChat;

public partial class MainPage : ContentPage
{
    private readonly HubConnection _connection;

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



