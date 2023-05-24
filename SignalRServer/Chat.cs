using System;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.Service;
using SignalRServer.Models;
using System.Text.Json;

namespace SignalRServer
{
	public class Chat : Hub
	{
        private readonly RoomsService _roomsService;

        public Chat(RoomsService roomservice)
        {
            _roomsService = roomservice;
        }
        public async Task SendMessage(string message, string roomnum)
        {
            await Clients.Group(roomnum).SendAsync("ReceiveMessage",message +"«"+ Context.ConnectionId);
        }

        public async Task CreateRoom(string RoomNum)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, RoomNum);
            var newRoom = new Room { RoomNumber = Convert.ToInt32(RoomNum), UserOne = Context.ConnectionId ,IsActive = "false"};
            await _roomsService.CreateAsync(newRoom);
        }

        public async Task JoinRoom(string RoomNum)
        {
            var response = await _roomsService.GetAsync(RoomNum);
            //Added response.UserTwo == null to prevent more than two user get in the chat.
            if (response != null && response.UserTwo == null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, RoomNum);
                Room newRoom = new Room { Id = response.Id, RoomNumber = response.RoomNumber, UserOne = response.UserOne, UserTwo = Context.ConnectionId, IsActive = "true"};
                await _roomsService.UpdateAsync(RoomNum, newRoom);
            }
        }

    }
}

