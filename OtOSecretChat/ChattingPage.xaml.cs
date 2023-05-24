﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;

namespace OtOSecretChat;

public partial class ChattingPage : ContentPage
{

    public string messageToShow { get; set; }

    public string RoomNumber { get; set; }

    public HubConnection connection { get; set; }

    public ObservableCollection<Labelclass> labelList;

    public NavigationPage _mainPage;

    public ChattingPage(HubConnection connection, string RoomNum, NavigationPage mainpage)
    {
        InitializeComponent();
        _mainPage = mainpage;
        _mainPage.Popped += new EventHandler<NavigationEventArgs>(OnPopped);
        BindingContext = this;
        this.connection = connection;
        this.RoomNumber = RoomNum;
        labelList = new ObservableCollection<Labelclass>() { };

        Dispatcher.DispatchAsync(() =>
        {
            collection.ItemsSource = labelList;
        });

        connection.On<String>("ReceiveMessage", (message) =>
        {

            if (message.Split("«").Last().ToString() == connection.ConnectionId.Split(" ").Last())
            {
                labelList.Add(new Labelclass { Text = message.Split("«").First().ToString(), layout = LayoutOptions.End });

            }
            else
            {
                labelList.Add(new Labelclass { Text = message.Split("«").First().ToString(), layout = LayoutOptions.Start });
            }

        });
    }

    async void OnPopped(object sender, EventArgs e)
    {
        await connection.InvokeCoreAsync("LeaveRoom", args: new[] { RoomNumber });
    }

    async void Send(object sender, EventArgs e)
    {
        await connection.InvokeCoreAsync("SendMessage", args: new[] { Input.Text, RoomNumber });
        Input.Text = String.Empty;
        await _mainPage.PopAsync();
        await Navigation.PopAsync();

    }


}

    public class Labelclass
{
    public string Text { get; set; }

    public LayoutOptions layout { get; set; }

}