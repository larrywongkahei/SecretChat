using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRServer.Models;
using SignalRServer.Service;
namespace SignalRServer
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomsService _roomService;

        public RoomsController(RoomsService roomservice)
        {
            _roomService = roomservice;
        }
        // GET: api/Rooms
        [HttpGet]
        public async Task<List<Room>>  Get() => await _roomService.GetAsync();
        // GET: api/Rooms/5
        [HttpGet("{roomNum}", Name = "Get")]
        public async Task<Room?> Get(string RoomNum)
        {
            var room = await _roomService.GetAsync(RoomNum);
            return room;
        }

        // POST: api/Rooms
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}


