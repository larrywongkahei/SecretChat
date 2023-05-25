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
                Room newRoom = new Room { Id = response.Id, RoomNumber = response.RoomNumber, UserOne = response.UserOne, UserTwo = Context.ConnectionId, IsActive = response?.IsActive};
                await _roomsService.UpdateAsync(RoomNum, newRoom);
            }
        }

        public async Task LeaveRoom(string RoomNum)
        {
            await CountDown(RoomNum);
            await RemoveEverything(RoomNum);

        }

        public async Task StartChatting(string RoomNum)
        {
            var response = await _roomsService.GetAsync(RoomNum);
            Room newRoom = new Room { Id = response?.Id, RoomNumber = Convert.ToInt32(RoomNum), UserOne = response?.UserOne, UserTwo = Context.ConnectionId, IsActive = "true" };
            await _roomsService.UpdateAsync(RoomNum, newRoom);
        }

        private async Task CountDown(string RoomNum)
        {
            await Clients.Group(RoomNum).SendAsync("ReceiveMessage", "This chat will be closed after 5 seconds, users would not be able to type.«");
            await Task.Delay(1000);
            for (var i = 0; i <= 4; i++)
            {
                await Clients.Group(RoomNum).SendAsync("ReceiveMessage", $"{4-i}«");
                await Task.Delay(1000);
            }
        }

        private async Task RemoveEverything(string RoomNum)
        {
            var theRoom = await _roomsService.GetAsync(RoomNum);
            var userOne = theRoom?.UserOne;
            var userTwo = theRoom?.UserTwo;
            if (userOne != null && userTwo != null)
            {
                await Groups.RemoveFromGroupAsync(userOne, RoomNum);
                await Groups.RemoveFromGroupAsync(userTwo, RoomNum);
                await _roomsService.RemoveAsync(Convert.ToInt32(RoomNum));
            }
        }
    }
}

