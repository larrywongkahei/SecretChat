using System;
using Microsoft.AspNetCore.SignalR;
namespace SignalRServer
{
	public class Chat : Hub
	{
        public async Task SendMessage(string message, string roomnum)
        {
            await Clients.Group(roomnum).SendAsync("ReceiveMessage",message +"«"+ Context.ConnectionId);
        }

        public async Task CreateRoom(string RoomNum)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, RoomNum);
        }

        public async Task JoinRoom(string RoomNum)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, RoomNum);
        }
    }
}

